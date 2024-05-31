using Microsoft.AspNetCore.Mvc;

namespace Mini_Project.Controllers
{
    public class AddressController : Controller
    {
        private readonly MiniProjectDbContext _context;
        public AddressController(MiniProjectDbContext context)
        {
            _context = context;
        }

        public JsonResult GetProvince(int ProvinceId){
            var province = _context.ThaiProvinces
                .Where(p => p.Id == ProvinceId)
                .Select(p => new {p.Id ,p.NameTh})
                .FirstOrDefault();
            return Json(province);
        }

        public JsonResult GetAmphure(int AmphureId){
            var amphures = _context.ThaiAmphures
                .Where(a => a.Id == AmphureId)
                .Select(a => new { a.Id, a.NameTh })
                .FirstOrDefault();

            return Json(amphures);
        }

        public JsonResult GetTambon(int TambonId){
            var tambons = _context.ThaiTambons
                .Where(t => t.Id == TambonId)
                .Select(t => new { t.Id, t.NameTh })
                .FirstOrDefault();

            return Json(tambons);
        }

        public JsonResult GetAmphureByProvince(int ProvinceId)
        {
            var amphures = _context.ThaiAmphures
                .Where(a => a.ProvinceId == ProvinceId)
                .Select(a => new { a.Id, a.NameTh })
                .OrderBy(a => a.NameTh)
                .ToList();

            return Json(amphures);
        }

        public JsonResult GetTambonByAmphure(int AmphureId)
        {
            var tambons = _context.ThaiTambons
                .Where(t => t.AmphureId == AmphureId)
                .Select(t => new { t.Id, t.NameTh })
                .OrderBy(a => a.NameTh)
                .ToList();

            return Json(tambons);
        }

        public JsonResult GetZipCode(int TambonId){
            var zipcode = _context.ThaiTambons
                .Where(z => z.Id == TambonId)
                .Select(z => new {z.ZipCode})
                .FirstOrDefault();

            return Json(zipcode);
        }

    }
}
