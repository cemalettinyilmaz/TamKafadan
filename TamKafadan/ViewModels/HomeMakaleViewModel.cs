using System.Collections.Generic;
using TamKafadan.Models;

namespace TamKafadan.ViewModels
{
    public class HomeMakaleViewModel
    {
        public List<Makale> EnCokOkunanBesMakale { get; set; }
        public List<Makale> KullaniciKonuMakaleleri { get; set; }
        public List<Makale> ZiyaretciRastgeleMakaleleri { get; set; }
    }
}
