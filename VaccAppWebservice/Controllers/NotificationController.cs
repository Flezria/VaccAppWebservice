using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace VaccAppWebservice.Controllers
{
    public class NotificationController : ApiController
    {
        VaccineContext db = new VaccineContext();

        [Route("SendNotification/{parentEmail}/{parentName}/{childName}/{dateForVac}/{vacNavn}")]
        [HttpGet]
        public async Task<IHttpActionResult> APISendMail(String parentEmail, String parentName, String childName, String dateForVac, String vacNavn)
        {
            SendEmail(parentEmail, parentName, childName, dateForVac, vacNavn);

            return Ok();
        }

        private void SendEmail(String parentEmail, String parentName, String childName, String dateForVac, String vacNavn)
        {
            using (MailMessage mm = new MailMessage("vaccinationsbogen@gmail.com", parentEmail))
            {
                mm.Subject = $"!VACCINATIONS PÅMINDELSE - {childName.ToUpper()}!";
                mm.Body = $"Kære {parentName}\nDet er blevet tid til at du skal bestille tid hos lægen til dit barn {childName}.\n" +
                          $"Der skal foretages en {vacNavn} vaccination inden d. {dateForVac}.\nDer kan læses meget mere " +
                          $"om denne vaccination i appen.\n\n- Vaccinations bogen";

                mm.IsBodyHtml = false;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.EnableSsl = true;
                NetworkCredential NetworkCred = new NetworkCredential("vaccinationsbogen@gmail.com", "roskilde_4000");
                smtp.UseDefaultCredentials = true;
                smtp.Credentials = NetworkCred;
                smtp.Port = 587;
                smtp.Send(mm);
            }
        }

    }
}