using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Linq;
using TamKafadan.Filters;
using TamKafadan.Models;
using TamKafadan.ViewModels;

namespace TamKafadan.Controllers
{
    public class MakaleController : Controller
    {
        private readonly AppDbContext _db;

        public MakaleController(AppDbContext db)
        {
            this._db = db;
        }
        public IActionResult Index()
        {
            return View();
        }
        [Login]
        public IActionResult MakaleYaz()
        {
            MakaleYazViewModel vm = new MakaleYazViewModel();
            vm.Konular = new SelectList(_db.Konular.ToList(), "KonuId", "KonuAdi");
            return View(vm);
        }
        [Login]
        [HttpPost]
        public IActionResult MakaleYaz(MakaleYazViewModel vm)
        {
            vm.Konular = new SelectList(_db.Konular.ToList(), "KonuId", "KonuAdi");//Hatalı bilgi gelirse boş olarak geliyor tekrar gönderiyorum.

            var kadi = HttpContext.Session.GetString("kullaniciAdi");
            Yazar girisYapan = _db.Yazarlar.FirstOrDefault(x => x.KullaniciAdi == kadi);

            if (girisYapan == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                if (vm.SecilenKonular.Count == 0)
                {
                    ModelState.AddModelError("SecilenKonular", "Lütfen en az bir konu seçiniz.");
                }
                return View(vm);
            }

            //Eğer sorun yoksa kayıt yapıcam

            Makale ym=new Makale(); //ym Yeni Makale
            ym.Baslik = vm.Baslik;
            ym.Icerik=vm.Icerik;
            ym.YazarId = girisYapan.YazarId;

            foreach (var konuId in vm.SecilenKonular)
            {
                Konu ek = _db.Konular.Find(Convert.ToInt32(konuId));
                ym.Konulari.Add(ek);
            }

            _db.Makaleler.Add(ym);
            _db.SaveChanges();
            TempData["mesaj"] = "Makale başarıyla kayıt edildi.";

            return RedirectToAction("Index", "Home");
        }
    }
}
