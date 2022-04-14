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
        [HttpPost]
        [Login]
        [ValidateAntiForgeryToken]
        public IActionResult MakaleYaz(MakaleYazViewModel vm)
        {
            vm.Konular = new SelectList(_db.Konular.ToList(), "KonuId", "KonuAdi");//Hatalı bilgi gelirse boş olarak geliyor tekrar gönderiyorum.

            var kadi = HttpContext.Session.GetString("kullaniciAdi");
            Yazar girisYapan = _db.Yazarlar.FirstOrDefault(x => x.KullaniciAdi == kadi);

            if (girisYapan == null)
            {
                return NotFound();
            }

            if (vm.SecilenKonular.Count == 0)
            {
                ModelState.AddModelError("SecilenKonular", "Lütfen en az bir konu seçiniz.");
                return View(vm);
            }
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            //Eğer sorun yoksa kayıt yapıcam

            Makale ym = new Makale(); //ym Yeni Makale
            ym.Baslik = vm.Baslik;
            ym.Icerik = vm.Icerik;
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

        public IActionResult MakaleOku(int makaleId)
        {
            Makale makale = _db.Makaleler.Include(x => x.Yazar).Include(x => x.Konulari).FirstOrDefault(x => x.MakaleId == makaleId);
            if (makale == null)
            {
                return NotFound();
            }

            makale.GoruntulenmeSayisi++;
            _db.Update(makale);
            _db.SaveChanges();

            return View(makale);
        }

        [Login]
        public IActionResult MakaleDuzenle(int makaleId)
        {
            MakaleYazViewModel vm = new MakaleYazViewModel();
            //duzenlenenMakale
            Makale dM = _db.Makaleler.Include(x => x.Konulari).FirstOrDefault(x => x.MakaleId == makaleId);
            vm.MakaleId = dM.MakaleId;
            vm.Icerik = dM.Icerik;
            vm.Baslik = dM.Baslik;
            vm.YazarId = dM.YazarId;            

            SelectList selectLists = new SelectList(_db.Konular.ToList(), "KonuId", "KonuAdi");
            foreach (var item in selectLists)
            {
                if (dM.Konulari.Select(x => x.KonuId).ToList().Contains(Convert.ToInt32(item.Value)))
                {
                    item.Selected = true;
                }
            }
            vm.Konular = selectLists;

            return View(vm);
        }

        [HttpPost]
        [Login]
        [ValidateAntiForgeryToken]
        public IActionResult MakaleDuzenle(MakaleYazViewModel vm)
        {

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
                    vm.Konular = new SelectList(_db.Konular.ToList(), "KonuId", "KonuAdi");//Hatalı bilgi gelirse boş olarak geliyor tekrar gönderiyorum.

                    ModelState.AddModelError("SecilenKonular", "Lütfen en az bir konu seçiniz.");
                }
                return View(vm);
            }

            //Eğer sorun yoksa kayıt yapıcam

            Makale gm = _db.Makaleler.Include(x => x.Konulari).FirstOrDefault(x => x.MakaleId == vm.MakaleId);//Guncelenen Makale
            gm.Baslik = vm.Baslik;
            gm.Icerik = vm.Icerik;
            gm.YazarId = girisYapan.YazarId;
            gm.MakaleId = vm.MakaleId;
            gm.OnayliMi = false;
            gm.OlusuturulmaZamani = DateTime.Now;
            gm.Konulari.Clear();
            foreach (var konuId in vm.SecilenKonular)
            {
                Konu ek = _db.Konular.Find(Convert.ToInt32(konuId));//Eklenen Konu
                gm.Konulari.Add(ek);
            }

            _db.Update(gm);
            _db.SaveChanges();
            TempData["mesaj"] = "Makale başarıyla kayıt edildi.";

            return RedirectToAction("MakaleOku", "Makale", new { makaleId = gm.MakaleId });
        }
        [Login]     
      
        public IActionResult MakaleSil(int makaleId)
        {
            Makale silinecekMakale = _db.Makaleler.Include(x=>x.Yazar).FirstOrDefault(x=>x.MakaleId==makaleId);

            return View(silinecekMakale);
        }
        [Login]
        [HttpPost]
     
        public IActionResult MakaleSil(Makale makale)
        {
            Yazar yazar = _db.Yazarlar.Find(makale.YazarId);
            _db.Makaleler.Remove(makale);
            _db.SaveChanges();
            TempData["mesaj"] = "Makale Başarıyla Silindi.";

            return RedirectToAction("Profil", "Yazar", new { kullaniciAdi = yazar.KullaniciAdi });
        }


    }
}
