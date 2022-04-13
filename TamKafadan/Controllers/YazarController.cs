using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using TamKafadan.Filters;
using TamKafadan.Models;
using TamKafadan.ViewModels;

namespace TamKafadan.Controllers
{
    public class YazarController : Controller
    {
        private readonly AppDbContext _db;

        public YazarController(AppDbContext db)
        {
            this._db = db;
        }
        public IActionResult Index(string guid)
        {
            if(guid!=null)
            {
                Yazar yazar = _db.Yazarlar.FirstOrDefault(x => x.Guid.ToString() == guid);
                if(yazar==null)
                {
                    return NotFound();
                }
                HttpContext.Session.SetString("kullaniciAdi", yazar.KullaniciAdi);
                if(yazar.IlkMi)
                {
                    return RedirectToAction("Ayarlar");
                }
                else
                {
                    TempData["mesaj"] = "Başarıyla Giriş Yaptınız.";
                    return RedirectToAction("Index","Home");
                }
            }
            return View();
        }
        [Login]
        public IActionResult Ayarlar()
        {
            var kadi = HttpContext.Session.GetString("kullaniciAdi");
            Yazar girisYapan = _db.Yazarlar.Include(x => x.Konulari).FirstOrDefault(x => x.KullaniciAdi == kadi);

            if (girisYapan==null)
            {
                return NotFound();
            }
            AyarlarViewModel vm=new AyarlarViewModel();
            vm.Email = girisYapan.Email;
            vm.KullaniciAdi = girisYapan.KullaniciAdi;
            vm.YazarAd = girisYapan.YazarAd;
            vm.YazarId = girisYapan.YazarId;
            vm.Biografi = girisYapan.Biografi;
            vm.ResimYolu = girisYapan.ResimYolu;
            SelectList selectLists = new SelectList(_db.Konular.ToList(), "KonuId", "KonuAdi");
            foreach (var item in selectLists)
            {
                if (girisYapan.Konulari.Select(x => x.KonuId).ToList().Contains(Convert.ToInt32(item.Value)))
                {
                    item.Selected = true;
                }
            }

            vm.Konular = selectLists;
            

            return View(vm);
        }
        [HttpPost]
        public IActionResult Ayarlar(AyarlarViewModel vm)
        {
            if(vm==null)
            {
                return NotFound();
            }
            if(ModelState.IsValid)
            {

            }
            return View(vm);
        }


        public IActionResult Cikis()
        {
            HttpContext.Session.Remove("kullaniciAdi");
            //Clear kullanırsak Sessionın cookkesini temizlemeliyiz.
            TempData["mesaj"] = "Başarıyla Çıkış Yaptınız.";
            return RedirectToAction("Index", "Home");
        }
    }
}
