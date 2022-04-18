using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TamKafadan.Filters;
using TamKafadan.Models;
using TamKafadan.ViewModels;

namespace TamKafadan.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _db;

        public HomeController(ILogger<HomeController> logger, AppDbContext db)
        {
            _logger = logger;
            this._db = db;
        }

        public IActionResult Index()
        {
            var kadi = HttpContext.Session.GetString("kullaniciAdi");
            Yazar girisYapan = _db.Yazarlar.Include(x => x.Konulari).Include(x => x.Makaleleri).FirstOrDefault(x => x.KullaniciAdi == kadi);

            HomeMakaleViewModel vm = new HomeMakaleViewModel();

            vm.EnCokOkunanBesMakale = _db.Makaleler.Include(x => x.Yazar).Take(6).Where(x => x.OlusuturulmaZamani.Month == DateTime.Now.Month && x.OnayliMi == true).OrderByDescending(x => x.GoruntulenmeSayisi).ToList();

            if (girisYapan != null)
            {
                List<Konu> konular = new List<Konu>();
                foreach (var item in girisYapan.Konulari)
                {
                    konular.Add(_db.Konular.Include(x => x.Yazarlar).Include(x => x.Makaleler.Where(x => x.OnayliMi == true)).FirstOrDefault(x => x.KonuAdi == item.KonuAdi));
                }
                List<Makale> gonderilecekMakaleler = new List<Makale>();
                foreach (var item in konular)
                {
                    foreach (var makale in item.Makaleler)
                    {
                        gonderilecekMakaleler.Add(makale);
                    }
                }
                vm.KullaniciKonuMakaleleri = gonderilecekMakaleler.Distinct().ToList();
            }
            vm.AnaSayfaMakaleleri = _db.Konular.Include(x => x.Makaleler.Where(x => x.OnayliMi == true)).Include(x => x.Yazarlar).ToList();
            return View(vm);
        }

        public IActionResult Privacy()
        {
            return View();
        }
        [Login]
        public IActionResult Takip()
        {
            var kadi = HttpContext.Session.GetString("kullaniciAdi");
            Yazar girisYapan = _db.Yazarlar.Include(x => x.Konulari).Include(x => x.Makaleleri).FirstOrDefault(x => x.KullaniciAdi == kadi);

            HomeMakaleViewModel vm = new HomeMakaleViewModel();

            if (girisYapan != null)
            {
                List<Konu> konular = new List<Konu>();
                foreach (var item in girisYapan.Konulari)
                {
                    konular.Add(_db.Konular.Include(x => x.Yazarlar).Include(x => x.Makaleler.Where(x => x.OnayliMi == true)).FirstOrDefault(x => x.KonuAdi == item.KonuAdi));
                }
                List<Makale> gonderilecekMakaleler = new List<Makale>();
                foreach (var item in konular)
                {
                    foreach (var makale in item.Makaleler)
                    {
                        gonderilecekMakaleler.Add(makale);
                    }
                }
                vm.KullaniciKonuMakaleleri = gonderilecekMakaleler.Distinct().ToList();
                ViewBag.KonuSayisi = girisYapan.Konulari.Count();
            }
            return View(vm);
        }




        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
