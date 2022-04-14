using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TamKafadan.Models
{
    public class Makale
    {
        public Makale()
        {
            Konulari=new HashSet<Konu>();
        }
        [Key]
        public int MakaleId { get; set; }
        [Required]
        [Display(Name ="Makale Başlığı")]
        public string Baslik { get; set; }
        [Required]
        [Display(Name = "Makale İçeriği")]
        public string Icerik { get; set; }
        [Display(Name ="Yayınlanma Zamanı")]
        public DateTime OlusuturulmaZamani { get; set; }= DateTime.Now;
        [Display(Name ="Görüntüleme Sayısı")]
        public int GoruntulenmeSayisi { get; set; } = 0;
        public bool OnayliMi { get; set; } = false;
        public int YazarId { get; set; }
        public Yazar Yazar { get; set; }
        public ICollection<Konu> Konulari { get; set; }
    }
}
