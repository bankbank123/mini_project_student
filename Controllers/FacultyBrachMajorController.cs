using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Mini_Project.Models;
using Mini_Project.ViewModels;
using NuGet.Protocol.Plugins;

namespace Mini_Project.Controllers
{
    public class FacultyBrachMajorController : Controller
    {

        private readonly MiniProjectDbContext _context;

        public FacultyBrachMajorController(MiniProjectDbContext context)
        {
            _context = context;
        }
        // GET: FacultyBrachMajorController
        public async Task<IActionResult> Index(string searchTerm, int pageNumber = 1, int pageSize = 10)
        {
            var query = from faculty in _context.Faculties
                        join branch in _context.Branches on faculty.FacultyId equals branch.FacultyId
                        join major in _context.Majors on branch.BranchId equals major.BranchId into branchMajors
                        from major in branchMajors.DefaultIfEmpty()
                        select new FacultyBranchMajorViewModel
                        {
                            FacultyId = faculty.FacultyId,
                            FacultyName = faculty.FacultyName,
                            BranchId = branch.BranchId,
                            BranchName = branch.BranchName,
                            MajorId = major.MajorId,
                            MajorName = major.MajorName
                        };

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(x => x.FacultyName.Contains(searchTerm) || x.BranchName.Contains(searchTerm) || x.MajorName.Contains(searchTerm));
            }

            var totalItems = await query.CountAsync();
            var totalPages = (int)System.Math.Ceiling(totalItems / (double)pageSize);
            var results = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            var viewModel = new SearchViewModel
            {
                SearchTerm = searchTerm,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalPages = totalPages,
                Results = results
            };

            return View(viewModel);
        }

        // GET: FacultyBrachMajorController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: FacultyBrachMajorController/Create
        public ActionResult CreateFaculty()
        {
            return View();
        }

        // POST: FacultyBrachMajorController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateFaculty(Faculty faculty)
        {
            if (ModelState.IsValid)
            {
                // Find the faculty in the database
                var faculty_db = await _context.Faculties.FirstOrDefaultAsync(f => f.FacultyName == faculty.FacultyName);

                // Check if the faculty already exists
                if (faculty_db != null)
                {
                    ModelState.AddModelError("", "ชื่อคณะมีในระบบเรียบร้อยแล้ว");
                    return View(faculty);
                }

                try
                {
                    // Faculty does not exist, so add it to the database
                    _context.Faculties.Add(faculty);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception)
                {
                    // Handle any database-related exceptions
                    throw;
                }
            }
            return View(faculty);
        }


        public ActionResult CreateBrach()
        {
            ViewData["FacultyID"] = new SelectList(_context.Faculties, "FacultyId", "FacultyName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateBrach([Bind("BranchId, BranchName, FacultyId")] Branch branch,
            Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary modelState)
        {

            if (modelState.IsValid)
            {
                var BrachNamedb = await _context.Branches.FirstOrDefaultAsync(b => b.BranchName == branch.BranchName && b.FacultyId == branch.FacultyId);
                if (BrachNamedb != null)
                {
                    return NotFound();
                }

                _context.Branches.Add(branch);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["FacultyID"] = new SelectList(_context.Faculties, "FacultyId", "FacultyName");
            return View(branch);

        }

        public ActionResult CreateMajor()
        {
            ViewData["FacultyID"] = new SelectList(_context.Faculties, "FacultyId", "FacultyName");
            ViewData["BranchID"] = new SelectList(_context.Branches, "BranchId", "BranchName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateMajor([Bind("MajorId, MajorName, BranchId")] Major major,
            Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary modelState)
        {
            if (modelState.IsValid)
            {
                var BrachNamedb = _context.Majors.FirstOrDefault(m => m.MajorName == major.MajorName && m.BranchId == major.BranchId);
                if (BrachNamedb != null)
                {
                    return NotFound();
                }

                _context.Majors.Add(major);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
                
            }

            ViewData["BranchID"] = new SelectList(_context.Branches, "BranchId", "BranchName");
            return View(major);
        }

        // POST: FacultyBrachMajorController/Edit/5
        public async Task<ActionResult> EditFaculty()
        {

            var faculty = await _context.Faculties.ToListAsync();
            return View(faculty);
        }

        [HttpPost]
        public async Task<ActionResult> EditFaculty(int facultyId, string facultyName)
        {
            if (facultyId == 0)
            {
                return NotFound();
            }

            var faculty = await _context.Faculties.FindAsync(facultyId);
            if (faculty == null)
            {
                return NotFound();
            }

            faculty.FacultyName = facultyName;

            if (faculty != null)
            {
                try
                {
                    _context.Faculties.Update(faculty);
                    await _context.SaveChangesAsync();
                }
                catch (Exception)
                {
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }

            return View(faculty);

        }
        public ActionResult EditBranch()
        {
            ViewData["BranchId"] = new SelectList(_context.Branches, "BranchId", "BranchName");
            ViewData["FacultyId"] = new SelectList(_context.Faculties, "FacultyId", "FacultyName");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditBranch(int FacultyId, int BranchId, string BranchName)
        {

            if (BranchId == 0)
            {
                return NotFound();
            }
            var Branch = await _context.Branches.FindAsync(BranchId);
            if (Branch == null)
            {
                return NotFound();
            }
            Branch.BranchName = BranchName;
            try
            {
                _context.Branches.Update(Branch);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                throw;
            }
        }
        public ActionResult EditMajor()
        {
            ViewData["BranchId"] = new SelectList(_context.Branches, "BranchId", "BranchName");
            ViewData["FacultyId"] = new SelectList(_context.Faculties, "FacultyId", "FacultyName");
            ViewData["MajorId"] = new SelectList(_context.Majors, "MajorId", "MajorName");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditMajor(int BranchId, int MajorId, string MajorName)
        {
            if (MajorId == 0)
            {
                return NotFound();
            }
            var Major = await _context.Majors.FindAsync(MajorId);
            if (Major == null)
            {
                return NotFound();
            }
            Major.MajorName = MajorName;
            try
            {
                _context.Majors.Update(Major);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                throw;
            }
        }

        // GET: FacultyBrachMajorController/Delete/5
        public ActionResult Delete()
        {
            ViewData["BranchId"] = new SelectList(_context.Branches, "BranchId", "BranchName");
            ViewData["FacultyId"] = new SelectList(_context.Faculties, "FacultyId", "FacultyName");
            ViewData["MajorId"] = new SelectList(_context.Majors, "MajorId", "MajorName");
            return View();
        }

        // POST: FacultyBrachMajorController/Delete/5
        [HttpPost]
        public async Task<ActionResult> DeleteFaculty(int facultyId)
        {
            if (facultyId == 0)
            {
                return NotFound();
            }

            var faculty = await _context.Faculties
                .Include(f => f.Branches)
                .ThenInclude(b => b.Majors)
                .FirstOrDefaultAsync(f => f.FacultyId == facultyId);

            if (faculty == null)
            {
                return NotFound();
            }

            // Check if there are any branches associated with this faculty
            if (faculty.Branches != null && faculty.Branches.Any())
            {
                foreach (var branch in faculty.Branches)
                {
                    _context.Majors.RemoveRange(branch.Majors);
                }

                _context.Branches.RemoveRange(faculty.Branches);
            }

            _context.Faculties.Remove(faculty);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<ActionResult> DeleteBranch(int branchId)
        {
            if (branchId == 0)
            {
                return NotFound();
            }

            var branch = await _context.Branches
                .Include(b => b.Majors)
                .FirstOrDefaultAsync(b => b.BranchId == branchId);

            if (branch == null)
            {
                return NotFound();
            }

            // Remove associated majors
            _context.Majors.RemoveRange(branch.Majors);

            // Remove the branch itself
            _context.Branches.Remove(branch);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public async Task<ActionResult> DeleteMajor(int majorId)
        {
            if (majorId == 0)
            {
                return NotFound();
            }

            var major = await _context.Majors.FindAsync(majorId);
            if (major == null)
            {
                return NotFound();
            }
            _context.Majors.Remove(major);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public JsonResult GetFacultyName(int facultyId)
        {
            var faculty = _context.Faculties
                .Where(f => f.FacultyId == facultyId)
                .Select(f => new { f.FacultyName })
                .FirstOrDefault();

            if (faculty == null)
            {
                return Json(new { facultyName = "" });
            }

            return Json(faculty);
        }

        public JsonResult GetBranchName(int branchId)
        {
            var branch = _context.Branches
                .Where(b => b.BranchId == branchId)
                .Select(b => new { b.BranchName })
                .FirstOrDefault();

            if (branch == null)
            {
                return Json(new { BranchName = "" });
            }

            return Json(branch);
        }

        public JsonResult GetMajorName(int majorId)
        {
            var major = _context.Majors
                .Where(m => m.MajorId == majorId)
                .Select(m => new { m.MajorName })
                .FirstOrDefault();
            if (major == null)
            {
                return Json(new { MajorName = "" });
            }
            return Json(major);
        }

        public JsonResult GetBranchesByFacultyId(int facultyId)
        {
            var branches = _context.Branches
                .Where(b => b.FacultyId == facultyId)
                .Select(b => new { b.BranchId, b.BranchName })
                .ToList();

            return Json(branches);
        }

        public JsonResult GetMajorNameByBranch(int branchId)
        {
            var major = _context.Majors
                .Where(m => m.BranchId == branchId)
                .Select(m => new { m.MajorId, m.MajorName })
                .ToList();

            return Json(major);
        }
    }
}
