using System.ComponentModel.DataAnnotations;

namespace TamKafadan.Models
{
    public class Admin
    {
        [Key]
        public int AdminId { get; set; }
        public string name { get; set; }
        public string sifre { get; set; }
    }
}
