using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TamKafadan.Models
{
    public class Yazar
    {
        public Yazar()
        {
            Makaleleri=new HashSet<Makale>();
            Konulari = new HashSet<Konu>();
        }
        [Key]
        public int YazarId { get; set; }
        [Required]
        public string Email { get; set; }
        [Required,MaxLength(30)]
        public string YazarAd { get; set; }
        public string Biografi { get; set; }
        [Required]
        public string KullaniciAdi { get; set; }
        public string ResimYolu { get; set; } = "defaultprofil.png";
        public bool IlkMi { get; set; } = true;
        public Guid Guid { get; set; } = Guid.NewGuid();
        public ICollection<Makale> Makaleleri { get; set; }
        public ICollection<Konu> Konulari { get; set; }
    }
}
