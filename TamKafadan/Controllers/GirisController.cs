using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Net;
using System.Net.Mail;
using TamKafadan.Models;

namespace TamKafadan.Controllers
{
    public class GirisController : Controller
    {
        private readonly AppDbContext _db;

        public GirisController(AppDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {

            return View();
        }
        [HttpPost]
        public IActionResult Index(string email)
        {
            Yazar girisYapan = _db.Yazarlar.FirstOrDefault(x => x.Email == email);
            if(girisYapan==null)
            {
                girisYapan=new Yazar();
                girisYapan.Email=email;
                girisYapan.KullaniciAdi= (email).Split('@')[0];
                girisYapan.YazarAd = girisYapan.KullaniciAdi;
                MailGonder(girisYapan.Email,girisYapan.Guid.ToString());
                _db.Yazarlar.Add(girisYapan);
                _db.SaveChanges();
            }
            else
            {
                MailGonder(girisYapan.Email, girisYapan.Guid.ToString());
            }
         
            return RedirectToAction("Index","Home");
           
        }
        public void MailGonder(string email,string guid)
        {

            SmtpClient smtp = new SmtpClient(); //
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.UseDefaultCredentials = false;      
            smtp.Credentials = new NetworkCredential("tamkafadanblog@gmail.com", "!!Cemo1991");
            smtp.EnableSsl = true; 
            smtp.Timeout = 10000;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("tamkafadanblog@gmail.com", "Tam Kafadan Blog");
            mail.To.Add(email);           

            mail.Subject = "Tam Kafadan Blog Giriş";
            mail.Body = $"<h1><a href='https://localhost:44302/Yazar/Index?guid={guid}'>Giriş Yapmak için lütfen tıklayın.<a></h1><p>Eğer yukarıdaki link çalışmıyorsa lütfen https://localhost:44302/Yazar/Index?guid={guid} kopyalayıp tarayıcınıza yapıştırın.<p>";

            mail.IsBodyHtml = true;
            mail.SubjectEncoding = System.Text.Encoding.UTF8;
            mail.BodyEncoding = System.Text.Encoding.UTF8;          

            try
            {
                smtp.Send(mail);
                mail.Dispose();
                ViewBag.Mesaj = "Mail gönderildi,lütfen gelen kutusunu kontrol ediniz.";
            }
            catch (Exception ex)
            {
                ViewBag.Mesaj = "Hata oluştu: " + ex.ToString();
                throw;
            }            

           
        }
    }
}
