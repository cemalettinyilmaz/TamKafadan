using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using TamKafadan.Models;

namespace TamKafadan.Areas.Admin.Controllers
{

    [Area("Admin")] //bu önemli
    public class DashboardController : Controller
    {
        private readonly AppDbContext _db;

        public DashboardController(AppDbContext db)
        {
            this._db = db;
        }
        public IActionResult Index()
        {
            TamKafadan.Models.Admin girisYapan = new TamKafadan.Models.Admin();

            return View(girisYapan);
        }
        [HttpPost]
        public IActionResult Index(Models.Admin admin)
        {

            Models.Admin girisYapan = _db.Admins.FirstOrDefault(x => x.name == admin.name && x.sifre == admin.sifre);
            if (girisYapan == null)
            {
                return NotFound();
            }
            HttpContext.Session.SetString("admin", girisYapan.name);
            return RedirectToAction("Index","Konu","Admin");
        }
        public IActionResult Cikis()
        {
            HttpContext.Session.Remove("admin");
            TempData["mesaj"] = "Başarıyla Çıkış Yaptınız.";
            return RedirectToAction("Index", "Dashboard","Admin");
        }
    }
}
