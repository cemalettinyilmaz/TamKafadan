using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace TamKafadan.Attributes
{
    public class GecerliResimAttribute : ValidationAttribute
    {
        public int ResimMaxBoyutuMB { get; set; } = 1;

        public override bool IsValid(object value)
        {
            if (value != null)
            {
                IFormFile resim = (IFormFile)value;


                if (!resim.ContentType.StartsWith("image/"))
                {
                    ErrorMessage = "Geçersiz resim dosyası eklediniz..";
                    return false;
                }
                else if (resim.Length > ResimMaxBoyutuMB * 1024 * 1024)
                {
                    ErrorMessage = "Resim boyutu " + ResimMaxBoyutuMB.ToString() + "Mb'dan büyük olamaz.";
                    return false;
                }
            }
            else
            {
                return true;
            }
            return true;
        }
    }
}
