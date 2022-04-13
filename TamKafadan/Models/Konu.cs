using System.Collections;
using System.Collections.Generic;

namespace TamKafadan.Models
{
    public class Konu
    {
        public Konu()
        {
            Yazarlar = new HashSet<Yazar>();
            Makaleler=new HashSet<Makale>();
        }
        public int KonuId { get; set; }
        public string KonuAdi { get; set; }
        public ICollection<Yazar> Yazarlar { get; set; }
        public ICollection<Makale> Makaleler { get; set; }

    }
}
