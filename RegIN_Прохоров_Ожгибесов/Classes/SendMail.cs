using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace RegIN_Прохоров_Ожгибесов.Classes
{
    public class SendMail
    {
        public static void SendMessage(string Message,string To)
        {
            var smptClient = new SmtpClient("smtp.yandex.ru")
            {
                Port = 587,
                Credentials = new NetworkCredential("brenovc@yandex.ru", "pqstsmgvacsuqqxk"),
                EnableSsl = true,
            };
            smptClient.Send("landaxer.yandex.ru",To, "Проект RegIn",Message);
        }
    }
}
