
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TamKafadan.Attributes;

namespace TamKafadan.ViewModels
{
    public class AyarlarViewModel
    {
        public int YazarId { get; set; }
        [Required]
        public string Email { get; set; }
        [Required(ErrorMessage ="Yazar adı zorunludur."), MaxLength(30),Display(Name ="Yazar Adı")]
        public string YazarAd { get; set; }
        public string Biografi { get; set; }
        [Required(ErrorMessage = "Kullanıcı adı zorunludur."), MaxLength(30), Display(Name = "Kullanıcı Adı")]
        public string KullaniciAdi { get; set; }
        [GecerliResim(ResimMaxBoyutuMB = 2)]
        public IFormFile Resim { get; set; }
        public string ResimYolu { get; set; }
        public SelectList Konular { get; set; }
        public List<string> SecilenKonular { get; set; }
    }
}
