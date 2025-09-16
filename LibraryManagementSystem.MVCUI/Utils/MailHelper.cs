using LibraryManagementSystem.DAL;
using System;
using System.Net;
using System.Net.Mail;

namespace LibraryManagementSystem.MVCUI.Utils
{
    public class MailHelper
    {
        public static bool SendMail(Elaqe elaqe) //, string to, string subject, string body
        {
            try
            {
                // Burada mail göndərmə loqikasını əlavə edin.
                // Məsələn, SMTP server istifadə edərək mail göndərə bilərsiniz.
                // Bu nümunədə sadəcə true qaytarırıq ki, mail uğurla göndərilib.

                SmtpClient smtpClient = new SmtpClient("mail.saytinadi.com", 587);
                smtpClient.Credentials = new NetworkCredential("mail istifadechi adi bura", "mail shifresi bura");
                //smtpClient.EnableSsl = true; // Əgər mail transferi üçün SSL istifadə olunursa, bu sətri aktiv etmək lazım gələcək.

                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(elaqe.Email, $"{elaqe.Adi} {elaqe.Soyadi}"); // Mesaj göndərənin email ünvanı və adı
                mailMessage.To.Add("hamidhamidovaga@gmail.com"); // Əlaqə forması üçün təyin olunmuş (göndərilən mesajların gedəcəyi) email ünvanı (to)
                mailMessage.Subject = "Saytan mesaj gəlib!"; // Mesajın mövzusu (subject)
                mailMessage.Body = $"Adı: {elaqe.Adi}\nSoyadı: {elaqe.Soyadi}\nEmail: {elaqe.Email}\nMesaj: {elaqe.Mesaj}"; // Mesajın məzmunu (body)
                mailMessage.IsBodyHtml = true; // Əgər mesaj HTML formatında da ola bilər.

                smtpClient.Send(mailMessage);

                return true;
            }
            catch (Exception)
            {
                // Əgər mail göndərmə zamanı xəta baş verərsə, false qaytarırıq.
                return false;
            }
        }
    }
}
