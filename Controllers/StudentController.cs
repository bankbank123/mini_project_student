using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Mini_Project.Controllers
{
    public class StudentController : Controller
    {
        private readonly MiniProjectDbContext _context;
        public StudentController(MiniProjectDbContext context)
        {
            _context = context;
        }
        // GET: StudentController
        public async Task<ActionResult> Index(int? pageIndex)
        {
            int pageSize = 10; // Number of items per page

            var studentsQuery = _context.Students
                .Include(s => s.StudentAddressHomeland)
                .Include(s => s.StudentAddressPresent)
                .Include(s => s.StudentBranch)
                .Include(s => s.StudentElementarySchool)
                .Include(s => s.StudentFaculty)
                .Include(s => s.StudentGender)
                .Include(s => s.StudentJhighSchool)
                .Include(s => s.StudentPrefix)
                .Include(s => s.StudentShighSchool)
                .Include(s => s.StudentMajor); // Include StudentMajor

            // Calculate total number of pages
            var count = await studentsQuery.CountAsync();
            var totalPages = (int)Math.Ceiling(count / (double)pageSize);

            // Determine current page index
            int currentPageIndex = pageIndex ?? 1;
            ViewBag.PageIndex = currentPageIndex;
            ViewBag.TotalPages = totalPages;

            var students = await studentsQuery
                .OrderBy(s => s.StudentId)
                .Skip((currentPageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return View(students);
        }


        // GET: StudentController/Details/5
        public async Task<IActionResult> Details(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            var student = await _context.Students
                .Include(s => s.StudentAddressHomeland)
                    .ThenInclude(a => a.AddressHomelandTambon)
                .Include(s => s.StudentAddressPresent)
                    .ThenInclude(a => a.AddressPresentTambon)
                .Include(s => s.StudentBranch)
                .Include(s => s.StudentElementarySchool)
                .Include(s => s.StudentFaculty)
                .Include(s => s.StudentGender)
                .Include(s => s.StudentJhighSchool)
                .Include(s => s.StudentPrefix)
                .Include(s => s.StudentShighSchool)
                .Include(s => s.StudentMajor)
                .Include(s => s.Families)
                .FirstOrDefaultAsync(s => s.StudentId == id);

            if (student == null)
            {
                return NotFound();
            }

            var addressHomelandQuery = await _context.AddressHomelands
                .Include(t => t.AddressHomelandTambon)
                .Include(a => a.AddressHomelandAmphure)
                .Include(p => p.AddressHomelandProvince)
                .FirstOrDefaultAsync(p => p.AddressHomelandProvinceId == student.StudentAddressHomeland.AddressHomelandProvinceId
                                          && p.AddressHomelandAmphureId == student.StudentAddressHomeland.AddressHomelandAmphureId
                                          && p.AddressHomelandTambonId == student.StudentAddressHomeland.AddressHomelandTambonId);

            var addressPresentQuery = await _context.AddressPresents
                .Include(t => t.AddressPresentTambon)
                .Include(a => a.AddressPresentAmphure)
                .Include(p => p.AddressPresentProvince)
                .FirstOrDefaultAsync(p => p.AddressPresentProvinceId == student.StudentAddressPresent.AddressPresentProvinceId
                                          && p.AddressPresentAmphureId == student.StudentAddressPresent.AddressPresentAmphureId
                                          && p.AddressPresentTambonId == student.StudentAddressPresent.AddressPresentTambonId);

            if (addressHomelandQuery == null || addressPresentQuery == null)
            {
                return NotFound();
            }

            var viewModel = new StudentDetailsViewModel
            {
                Student = student,
                AddressHomeland = student.StudentAddressHomeland,
                AddressHomelandTambon = addressHomelandQuery.AddressHomelandTambon.NameTh,
                AddressHomelandAmphure = addressHomelandQuery.AddressHomelandAmphure.NameTh,
                AddressHomelandProvince = addressHomelandQuery.AddressHomelandProvince.NameTh,
                AddressHomelandZipCode = addressHomelandQuery.AddressHomelandTambon.ZipCode,
                AddressPresentTambon = addressPresentQuery.AddressPresentTambon.NameTh,
                AddressPresentAmphure = addressPresentQuery.AddressPresentAmphure.NameTh,
                AddressPresentProvince = addressPresentQuery.AddressPresentProvince.NameTh,
                AddressPresentZipCode = addressPresentQuery.AddressPresentTambon.ZipCode,
                AddressPresent = student.StudentAddressPresent,
                ElementarySchool = student.StudentElementarySchool,
                JhighSchool = student.StudentJhighSchool,
                ShighSchool = student.StudentShighSchool,
                Families = student.Families.ToList()
            };

            return View(viewModel);
        }


        // GET: StudentController/Create
        [HttpGet]
        public ActionResult Create()
        {
            ViewData["StudentPrefixId"] = new SelectList(_context.StudentPrefixes.OrderBy(sp => sp.StudentPrefixName), "StudentPrefixId", "StudentPrefixName");
            ViewData["GenderId"] = new SelectList(_context.Genders.OrderBy(g => g.GenderName), "GenderId", "GenderName");
            ViewData["ThaiProvinceId"] = new SelectList(_context.ThaiProvinces.OrderBy(tp => tp.NameTh), "Id", "NameTh");
            ViewData["BranchId"] = new SelectList(_context.Branches.OrderBy(b => b.BranchName), "BranchId", "BranchName");
            ViewData["FacultyId"] = new SelectList(_context.Faculties.OrderBy(f => f.FacultyName), "FacultyId", "FacultyName");
            ViewData["MajorId"] = new SelectList(_context.Majors.OrderBy(m => m.MajorName), "MajorId", "MajorName");
            return View();
        }

        // POST: StudentController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind("Student, AddressPresent, AddressHomeland, ElementarySchool, JhighSchool, ShighSchool ,Families, SameAddress")] StudentFamilyViewModel viewModel, Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary modelState)
        {


            if (viewModel.SameAddress)
            {
                viewModel.AddressPresent.AddressPresentProvinceId = viewModel.AddressHomeland.AddressHomelandProvinceId;
                viewModel.AddressPresent.AddressPresentAmphureId = viewModel.AddressHomeland.AddressHomelandAmphureId;
                viewModel.AddressPresent.AddressPresentTambonId = viewModel.AddressHomeland.AddressHomelandTambonId;
                viewModel.AddressPresent.AddressPresent1 = viewModel.AddressHomeland.AddressHomeland1;
            }

            if (modelState.IsValid)
            {
                try
                {

                    var addressPresent = new AddressPresent
                    {
                        AddressPresent1 = viewModel.AddressPresent.AddressPresent1,
                        AddressPresentProvinceId = viewModel.AddressPresent.AddressPresentProvinceId,
                        AddressPresentAmphureId = viewModel.AddressPresent.AddressPresentAmphureId,
                        AddressPresentTambonId = viewModel.AddressPresent.AddressPresentTambonId
                    };
                    _context.AddressPresents.Add(addressPresent);
                    await _context.SaveChangesAsync();
                    viewModel.Student.StudentAddressPresentId = addressPresent.AddressPresentId;



                    var addressHomeland = new AddressHomeland
                    {
                        AddressHomeland1 = viewModel.AddressHomeland.AddressHomeland1,
                        AddressHomelandProvinceId = viewModel.AddressHomeland.AddressHomelandProvinceId,
                        AddressHomelandAmphureId = viewModel.AddressHomeland.AddressHomelandAmphureId,
                        AddressHomelandTambonId = viewModel.AddressHomeland.AddressHomelandTambonId
                    };
                    _context.AddressHomelands.Add(addressHomeland);
                    await _context.SaveChangesAsync();
                    viewModel.Student.StudentAddressHomelandId = addressHomeland.AddressHomelandId;



                    var elementarySchool = new ElementarySchool
                    {
                        ElementarySchoolName = viewModel.ElementarySchool.ElementarySchoolName,
                        ElementarySchoolGrade = viewModel.ElementarySchool.ElementarySchoolGrade
                    };
                    _context.ElementarySchools.Add(elementarySchool);
                    await _context.SaveChangesAsync();
                    viewModel.Student.StudentElementarySchoolId = elementarySchool.ElementarySchoolId;

                    var jhighSchool = new JhighSchool
                    {
                        JhighSchoolName = viewModel.JhighSchool.JhighSchoolName,
                        JhighSchoolGrade = viewModel.JhighSchool.JhighSchoolGrade
                    };
                    _context.JhighSchools.Add(jhighSchool);
                    await _context.SaveChangesAsync();
                    viewModel.Student.StudentJhighSchoolId = jhighSchool.JhighSchoolId;

                    var shighSchool = new ShighSchool
                    {
                        ShighSchoolName = viewModel.ShighSchool.ShighSchoolName,
                        ShighSchoolGrade = viewModel.ShighSchool.ShighSchoolGrade
                    };
                    _context.ShighSchools.Add(shighSchool);
                    await _context.SaveChangesAsync();
                    viewModel.Student.StudentShighSchoolId = shighSchool.ShighSchoolId;

                    using (var transaction = await _context.Database.BeginTransactionAsync())
                    {
                        try
                        {
                            var student = new Student
                            {
                                StudentFirstName = viewModel.Student.StudentFirstName,
                                StudentMiddleName = viewModel.Student.StudentMiddleName,
                                StudentLastName = viewModel.Student.StudentLastName,
                                StudentAge = viewModel.Student.StudentAge,
                                StudentPrefixId = viewModel.Student.StudentPrefixId,
                                StudentGenderId = viewModel.Student.StudentGenderId,
                                StudentAddressPresentId = viewModel.Student.StudentAddressPresentId,
                                StudentAddressHomelandId = viewModel.Student.StudentAddressHomelandId,
                                StudentElementarySchoolId = viewModel.Student.StudentElementarySchoolId,
                                StudentJhighSchoolId = viewModel.Student.StudentJhighSchoolId,
                                StudentShighSchoolId = viewModel.Student.StudentShighSchoolId,
                                StudentFacultyId = viewModel.Student.StudentFacultyId,
                                StudentBranchId = viewModel.Student.StudentBranchId,
                                StudentMajorId = viewModel.Student.StudentMajorId,
                            };
                            _context.Students.Add(student);
                            await _context.SaveChangesAsync();

                            foreach (var familyVM in viewModel.Families)
                            {
                                var family = new Family
                                {
                                    FamilyRole = familyVM.FamilyRole,
                                    StudentId = student.StudentId
                                };
                                _context.Families.Add(family);
                            }
                            await _context.SaveChangesAsync();

                            await transaction.CommitAsync();
                            return RedirectToAction(nameof(Index));
                        }
                        catch (Exception)
                        {
                            await transaction.RollbackAsync();
                            ModelState.AddModelError(string.Empty, "Error occurred while saving data.");
                        }
                    }

                }
                catch (Exception)
                {
                    ModelState.AddModelError(string.Empty, "Error occurred while processing the request.");
                }
            }
            PopulateDropdowns(); // You need to define this method
            return View(viewModel);
        }

        private void PopulateDropdowns()
        {
            ViewData["StudentPrefixId"] = new SelectList(_context.StudentPrefixes, "StudentPrefixId", "StudentPrefixName");
            ViewData["GenderId"] = new SelectList(_context.Genders, "GenderId", "GenderName");
            ViewData["ThaiProvinceId"] = new SelectList(_context.ThaiProvinces, "Id", "NameTh");
            ViewData["ThaiAmphuresId"] = new SelectList(_context.ThaiAmphures, "Id", "NameTh");
            ViewData["ThaiTambonsId"] = new SelectList(_context.ThaiTambons, "Id", "NameTh");
            ViewData["BranchId"] = new SelectList(_context.Branches, "BranchId", "BranchName");
            ViewData["FacultyId"] = new SelectList(_context.Faculties, "FacultyId", "FacultyName");
            ViewData["MajorId"] = new SelectList(_context.Majors, "MajorId", "MajorName");
        }

        // GET: StudentController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var student = await _context.Students
                .Include(s => s.StudentAddressHomeland)
                .Include(s => s.StudentAddressPresent)
                .Include(s => s.StudentElementarySchool)
                .Include(s => s.StudentJhighSchool)
                .Include(s => s.StudentShighSchool)
                .Include(s => s.Families)
                .FirstOrDefaultAsync(s => s.StudentId == id);

            if (student == null)
            {
                return NotFound();
            }

            var ZipCodePresentSearch = GetZipCode(student.StudentAddressPresent.AddressPresentTambonId);
            var ZipCodeHomelandsSearch = GetZipCode(student.StudentAddressHomeland.AddressHomelandTambonId);
            var familyCount = student.Families.Count;
            var fCheck = student.Families.ToList();
            var viewModel = new StudentFamilyViewModel
            {
                Student = student,
                AddressHomeland = student.StudentAddressHomeland,
                AddressPresent = student.StudentAddressPresent,
                ElementarySchool = student.StudentElementarySchool,
                JhighSchool = student.StudentJhighSchool,
                ShighSchool = student.StudentShighSchool,
                ZipCodePresent = ZipCodePresentSearch,
                ZipCodeHomeland = ZipCodeHomelandsSearch,
                FamilyNumber = familyCount,
                Families = student.Families.ToList()
            };

            PopulateDropdowns(); // Make sure you have this method to populate dropdown data
            return View(viewModel);
        }

        // POST: StudentController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind("Student, AddressPresent, AddressHomeland, ElementarySchool, JhighSchool, ShighSchool ,Families, SameAddress")] StudentFamilyViewModel viewModel, Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary modelState)
        {
            if (viewModel.SameAddress)
            {
                viewModel.AddressPresent.AddressPresentProvinceId = viewModel.AddressHomeland.AddressHomelandProvinceId;
                viewModel.AddressPresent.AddressPresentAmphureId = viewModel.AddressHomeland.AddressHomelandAmphureId;
                viewModel.AddressPresent.AddressPresentTambonId = viewModel.AddressHomeland.AddressHomelandTambonId;
                viewModel.AddressPresent.AddressPresent1 = viewModel.AddressHomeland.AddressHomeland1;
            }

            if (modelState.IsValid)
            {
                var student = await _context.Students
                    .Include(s => s.StudentAddressHomeland)
                    .Include(s => s.StudentAddressPresent)
                    .Include(s => s.StudentElementarySchool)
                    .Include(s => s.StudentJhighSchool)
                    .Include(s => s.StudentShighSchool)
                    .Include(s => s.Families)
                    .FirstOrDefaultAsync(s => s.StudentId == viewModel.Student.StudentId);

                if (student == null)
                {
                    return NotFound();
                }

                // Update student fields
                student.StudentFirstName = viewModel.Student.StudentFirstName;
                student.StudentMiddleName = viewModel.Student.StudentMiddleName;
                student.StudentLastName = viewModel.Student.StudentLastName;
                student.StudentAge = viewModel.Student.StudentAge;
                student.StudentPrefixId = viewModel.Student.StudentPrefixId;
                student.StudentGenderId = viewModel.Student.StudentGenderId;
                student.StudentMajorId = viewModel.Student.StudentMajorId;
                student.StudentBranchId = viewModel.Student.StudentBranchId;
                student.StudentFacultyId = viewModel.Student.StudentFacultyId;

                // Update educational fields
                student.StudentElementarySchool.ElementarySchoolName = viewModel.ElementarySchool.ElementarySchoolName;
                student.StudentElementarySchool.ElementarySchoolGrade = viewModel.ElementarySchool.ElementarySchoolGrade;
                student.StudentJhighSchool.JhighSchoolName = viewModel.JhighSchool.JhighSchoolName;
                student.StudentJhighSchool.JhighSchoolGrade = viewModel.JhighSchool.JhighSchoolGrade;
                student.StudentShighSchool.ShighSchoolName = viewModel.ShighSchool.ShighSchoolName;
                student.StudentShighSchool.ShighSchoolGrade = viewModel.ShighSchool.ShighSchoolGrade;

                // Update AddressPresent
                student.StudentAddressPresent.AddressPresent1 = viewModel.AddressPresent.AddressPresent1;
                student.StudentAddressPresent.AddressPresentProvinceId = viewModel.AddressPresent.AddressPresentProvinceId;
                student.StudentAddressPresent.AddressPresentAmphureId = viewModel.AddressPresent.AddressPresentAmphureId;
                student.StudentAddressPresent.AddressPresentTambonId = viewModel.AddressPresent.AddressPresentTambonId;

                // Update AddressHomelands
                student.StudentAddressHomeland.AddressHomeland1 = viewModel.AddressHomeland.AddressHomeland1;
                student.StudentAddressHomeland.AddressHomelandProvinceId = viewModel.AddressHomeland.AddressHomelandProvinceId;
                student.StudentAddressHomeland.AddressHomelandAmphureId = viewModel.AddressHomeland.AddressHomelandAmphureId;
                student.StudentAddressHomeland.AddressHomelandTambonId = viewModel.AddressHomeland.AddressHomelandTambonId;

                // Remove existing families
                _context.Families.RemoveRange(student.Families);
                await _context.SaveChangesAsync();

                // Add new families
                foreach (var family in viewModel.Families)
                {
                    student.Families.Add(new Family
                    {
                        FamilyRole = family.FamilyRole,
                        StudentId = student.StudentId
                    });
                }

                _context.Update(student);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            // If ModelState is invalid, repopulate the ViewBag with dropdown data
            PopulateDropdowns();
            return View(viewModel);
        }

        // GET: StudentController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            var student = await _context.Students
                .Include(s => s.StudentAddressHomeland)
                    .ThenInclude(a => a.AddressHomelandTambon)
                .Include(s => s.StudentAddressPresent)
                    .ThenInclude(a => a.AddressPresentTambon)
                .Include(s => s.StudentBranch)
                .Include(s => s.StudentElementarySchool)
                .Include(s => s.StudentFaculty)
                .Include(s => s.StudentGender)
                .Include(s => s.StudentJhighSchool)
                .Include(s => s.StudentPrefix)
                .Include(s => s.StudentShighSchool)
                .Include(s => s.StudentMajor)
                .Include(s => s.Families)
                .FirstOrDefaultAsync(s => s.StudentId == id);

            if (student == null)
            {
                return NotFound();
            }

            var addressHomelandQuery = await _context.AddressHomelands
                .Include(t => t.AddressHomelandTambon)
                .Include(a => a.AddressHomelandAmphure)
                .Include(p => p.AddressHomelandProvince)
                .FirstOrDefaultAsync(p => p.AddressHomelandProvinceId == student.StudentAddressHomeland.AddressHomelandProvinceId
                                          && p.AddressHomelandAmphureId == student.StudentAddressHomeland.AddressHomelandAmphureId
                                          && p.AddressHomelandTambonId == student.StudentAddressHomeland.AddressHomelandTambonId);

            var addressPresentQuery = await _context.AddressPresents
                .Include(t => t.AddressPresentTambon)
                .Include(a => a.AddressPresentAmphure)
                .Include(p => p.AddressPresentProvince)
                .FirstOrDefaultAsync(p => p.AddressPresentProvinceId == student.StudentAddressPresent.AddressPresentProvinceId
                                          && p.AddressPresentAmphureId == student.StudentAddressPresent.AddressPresentAmphureId
                                          && p.AddressPresentTambonId == student.StudentAddressPresent.AddressPresentTambonId);

            if (addressHomelandQuery == null || addressPresentQuery == null)
            {
                return NotFound();
            }

            var viewModel = new StudentDetailsViewModel
            {
                Student = student,
                AddressHomeland = student.StudentAddressHomeland,
                AddressHomelandTambon = addressHomelandQuery.AddressHomelandTambon.NameTh,
                AddressHomelandAmphure = addressHomelandQuery.AddressHomelandAmphure.NameTh,
                AddressHomelandProvince = addressHomelandQuery.AddressHomelandProvince.NameTh,
                AddressHomelandZipCode = addressHomelandQuery.AddressHomelandTambon.ZipCode,
                AddressPresentTambon = addressPresentQuery.AddressPresentTambon.NameTh,
                AddressPresentAmphure = addressPresentQuery.AddressPresentAmphure.NameTh,
                AddressPresentProvince = addressPresentQuery.AddressPresentProvince.NameTh,
                AddressPresentZipCode = addressPresentQuery.AddressPresentTambon.ZipCode,
                AddressPresent = student.StudentAddressPresent,
                ElementarySchool = student.StudentElementarySchool,
                JhighSchool = student.StudentJhighSchool,
                ShighSchool = student.StudentShighSchool,
                Families = student.Families.ToList()
            };

            return View(viewModel);
        }

        // POST: StudentController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var student = await _context.Students
                .Include(s => s.StudentAddressHomeland)
                    .ThenInclude(a => a.AddressHomelandTambon)
                .Include(s => s.StudentAddressPresent)
                    .ThenInclude(a => a.AddressPresentTambon)
                .Include(s => s.StudentBranch)
                .Include(s => s.StudentElementarySchool)
                .Include(s => s.StudentFaculty)
                .Include(s => s.StudentGender)
                .Include(s => s.StudentJhighSchool)
                .Include(s => s.StudentPrefix)
                .Include(s => s.StudentShighSchool)
                .Include(s => s.StudentMajor)
                .Include(s => s.Families)
                .FirstOrDefaultAsync(s => s.StudentId == id);

            if (student == null)
            {
                return NotFound();
            }

            var addressHomelandQuery = await _context.AddressHomelands
                .FirstOrDefaultAsync(i => i.AddressHomelandId == student.StudentAddressHomelandId);
            var addressPresentQuery = await _context.AddressPresents
                .FirstOrDefaultAsync(i => i.AddressPresentId == student.StudentAddressPresentId);

            var ElementarySchool = await _context.ElementarySchools.FirstOrDefaultAsync(i => i.ElementarySchoolId == student.StudentElementarySchoolId);
            var JHighSchool = await _context.JhighSchools.FirstOrDefaultAsync(i => i.JhighSchoolId == student.StudentJhighSchoolId);
            var SHighSchool = await _context.ShighSchools.FirstOrDefaultAsync(i => i.ShighSchoolId == student.StudentShighSchoolId);

            if (addressHomelandQuery == null || addressPresentQuery == null || SHighSchool == null || JHighSchool == null)
            {
                return NotFound();
            }
            _context.Families.RemoveRange(student.Families);
            _context.Students.Remove(student);
            _context.AddressHomelands.Remove(addressHomelandQuery);
            _context.AddressPresents.Remove(addressPresentQuery);
            _context.ElementarySchools.Remove(ElementarySchool);
            _context.JhighSchools.Remove(JHighSchool);
            _context.ShighSchools.Remove(SHighSchool);
            await _context.SaveChangesAsync();


            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<ActionResult> Search(string searchString)
        {
            var student = await _context.Students
               .Include(s => s.StudentAddressHomeland)
                    .ThenInclude(a => a.AddressHomelandTambon)
               .Include(s => s.StudentAddressPresent)
                    .ThenInclude(a => a.AddressPresentTambon)
               .Include(s => s.StudentBranch)
               .Include(s => s.StudentElementarySchool)
               .Include(s => s.StudentFaculty)
               .Include(s => s.StudentGender)
               .Include(s => s.StudentJhighSchool)
               .Include(s => s.StudentPrefix)
               .Include(s => s.StudentShighSchool)
               .Include(s => s.StudentMajor)
               .FirstOrDefaultAsync(s => s.StudentFirstName.Contains(searchString) || s.StudentMiddleName.Contains(searchString) || s.StudentLastName.Contains(searchString));

            if (student == null)
            {
                return NotFound();
            }

            var studentFamily = await _context.Students.Include(s => s.Families).FirstOrDefaultAsync(s => s.StudentId == student.StudentId);

            var addressHomelandQuery = await _context.AddressHomelands
                .Include(t => t.AddressHomelandTambon)
                .Include(a => a.AddressHomelandAmphure)
                .Include(p => p.AddressHomelandProvince)
                .FirstOrDefaultAsync(p => p.AddressHomelandProvinceId == student.StudentAddressHomeland.AddressHomelandProvinceId
                                          && p.AddressHomelandAmphureId == student.StudentAddressHomeland.AddressHomelandAmphureId
                                          && p.AddressHomelandTambonId == student.StudentAddressHomeland.AddressHomelandTambonId);

            var addressPresentQuery = await _context.AddressPresents
                .Include(t => t.AddressPresentTambon)
                .Include(a => a.AddressPresentAmphure)
                .Include(p => p.AddressPresentProvince)
                .FirstOrDefaultAsync(p => p.AddressPresentProvinceId == student.StudentAddressPresent.AddressPresentProvinceId
                                          && p.AddressPresentAmphureId == student.StudentAddressPresent.AddressPresentAmphureId
                                          && p.AddressPresentTambonId == student.StudentAddressPresent.AddressPresentTambonId);

            if (addressHomelandQuery == null || addressPresentQuery == null)
            {
                return NotFound();
            }

            var viewModel = new StudentDetailsViewModel
            {
                Student = student,
                AddressHomeland = student.StudentAddressHomeland,
                AddressPresent = student.StudentAddressPresent,
                ElementarySchool = student.StudentElementarySchool,
                JhighSchool = student.StudentJhighSchool,
                ShighSchool = student.StudentShighSchool,
                Families = studentFamily.Families.ToList(),
                AddressHomelandTambon = addressHomelandQuery.AddressHomelandTambon.NameTh,
                AddressHomelandAmphure = addressHomelandQuery.AddressHomelandAmphure.NameTh,
                AddressHomelandProvince = addressHomelandQuery.AddressHomelandProvince.NameTh,
                AddressHomelandZipCode = addressHomelandQuery.AddressHomelandTambon.ZipCode,
                AddressPresentTambon = addressPresentQuery.AddressPresentTambon.NameTh,
                AddressPresentAmphure = addressPresentQuery.AddressPresentAmphure.NameTh,
                AddressPresentProvince = addressPresentQuery.AddressPresentProvince.NameTh,
                AddressPresentZipCode = addressPresentQuery.AddressPresentTambon.ZipCode
            };

            Console.WriteLine(viewModel);

            return View(viewModel);
        }


        public int GetZipCode(int TambonId)
        {
            var zipcode = _context.ThaiTambons
                .Where(z => z.Id == TambonId)
                .Select(z => z.ZipCode)
                .FirstOrDefault();

            return zipcode;
        }
    }
}
