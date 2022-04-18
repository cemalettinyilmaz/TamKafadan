using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using TamKafadan.Filters;
using TamKafadan.Models;
using TamKafadan.ViewModels;

namespace TamKafadan.Controllers
{
    public class YazarController : Controller
    {
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;

        public YazarController(AppDbContext db, IWebHostEnvironment env)
        {
            this._db = db;
            this._env = env;
        }
        public IActionResult Index(string guid)
        {
            if (guid != null)
            {
                Yazar yazar = _db.Yazarlar.FirstOrDefault(x => x.Guid.ToString() == guid);
                if (yazar == null)
                {
                    return NotFound();
                }
                HttpContext.Session.SetString("kullaniciAdi", yazar.KullaniciAdi);
                if (yazar.IlkMi)
                {
                    TempData["mesaj"] = "Merhaba Tam Kafadan Blog Sayfasına Hoşgeldin.";
                    return RedirectToAction("Ayarlar");
                }
                else
                {
                    TempData["mesaj"] = "Başarıyla Giriş Yaptınız.";
                    return RedirectToAction("Index", "Home");
                }
            }
            return View();
        }

        [Route("/{kullaniciAdi}")]
        public IActionResult Profil(string kullaniciAdi)
        {
            if (kullaniciAdi == null)
            {
                return NotFound();
            }
            Yazar yazar = _db.Yazarlar.Include(x => x.Makaleleri).Include(x => x.Konulari).FirstOrDefault(x => x.KullaniciAdi == kullaniciAdi);
            if (yazar == null)
            {
                return NotFound();
            }

            return View(yazar);
        }



        [Login]
        public IActionResult Ayarlar()
        {
            var kadi = HttpContext.Session.GetString("kullaniciAdi");
            Yazar girisYapan = _db.Yazarlar.Include(x => x.Konulari).FirstOrDefault(x => x.KullaniciAdi == kadi);

            if (girisYapan == null)
            {
                return NotFound();
            }

            AyarlarViewModel vm = new AyarlarViewModel();

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
            if (vm == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {           //girisYapan   
                Yazar gy = _db.Yazarlar.Include(x => x.Konulari).FirstOrDefault(x => x.KullaniciAdi == vm.KullaniciAdi);
                gy.Konulari.Clear();
                if (vm.SecilenKonular != null)
                {
                    foreach (var konuId in vm.SecilenKonular)
                    {
                        Konu ek = _db.Konular.Find(Convert.ToInt32(konuId));
                        gy.Konulari.Add(ek);
                    }

                }
                if (gy.Email != vm.Email)
                {
                    gy.Email = vm.Email;
                    gy.Guid = Guid.NewGuid();
                }
                gy.KullaniciAdi = vm.KullaniciAdi;
                gy.Biografi = vm.Biografi;
                if (vm.Resim != null)
                {
                    gy.ResimYolu = resimKaydet(vm.Resim);
                }
                gy.YazarAd = vm.YazarAd;
                gy.IlkMi = false;
                _db.Update(gy);
                _db.SaveChanges();
                TempData["mesaj"] = "Ayarlarınız başarıyla kayıt edildi.";
                return RedirectToAction("Ayarlar", "Yazar");

            }
            return View(vm);
        }

        private string resimKaydet(IFormFile resim)
        {
            string resimAdi = Guid.NewGuid() + Path.GetExtension(resim.FileName);

            string kaydetmeYolu = Path.Combine(_env.WebRootPath, "images", resimAdi);


            using (FileStream fs = new FileStream(kaydetmeYolu, FileMode.Create))
            {
                resim.CopyTo(fs);
            }

            return resimAdi;
        }


        public IActionResult Cikis()
        {
            HttpContext.Session.Remove("kullaniciAdi");
            TempData["mesaj"] = "Başarıyla Çıkış Yaptınız.";
            return RedirectToAction("Index", "Home");
        }
    }
}
