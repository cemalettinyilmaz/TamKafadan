using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TamKafadan.Models;

namespace TamKafadan.ViewModels
{
    public class MakaleYazViewModel
    {
        public int MakaleId { get; set; }
        [Required(ErrorMessage ="Makalenin başlığı olmazsa olmaz.")]
        [Display(Name = "Makale Başlığı")]
        
        public string Baslik { get; set; }
        [Required(ErrorMessage = "Makalenin içeriği olmazsa olmaz.")]
        [Display(Name = "Makale İçeriği")]
        public string Icerik { get; set; }
        [Display(Name = "Yayınlanma Zamanı")]
        public int YazarId { get; set; }
        public SelectList Konular { get; set; }
        
        public List<string> SecilenKonular { get; set; } = new List<string>();
    }
}
