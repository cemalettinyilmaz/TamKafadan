using System;
using System.Collections.Generic;

namespace TamKafadan.Models
{
    public class Makale
    {
        public Makale()
        {
            Konulari=new HashSet<Konu>();
        }
        public int MakaleId { get; set; }
        public string Baslik { get; set; }
        public string Icerik { get; set; }
        public DateTime OlusuturulmaZamani { get; set; }= DateTime.Now;
        public int GoruntulenmeSayisi { get; set; } = 0;
        public int YazarId { get; set; }
        public Yazar Yazar { get; set; }
        public ICollection<Konu> Konulari { get; set; }
    }
}
