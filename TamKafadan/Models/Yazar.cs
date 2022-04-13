using System.Collections.Generic;

namespace TamKafadan.Models
{
    public class Yazar
    {
        public Yazar()
        {
            Makaleleri=new HashSet<Makale>();
            Konulari = new HashSet<Konu>();
        }
        public int YazarId { get; set; }
        public string Email { get; set; }
        public string YazarAd { get; set; }
        public string Biografi { get; set; }
        public string KullaniciAdi { get; set; }
        public bool SilindiMi { get; set; }
        public ICollection<Makale> Makaleleri { get; set; }
        public ICollection<Konu> Konulari { get; set; }
    }
}
