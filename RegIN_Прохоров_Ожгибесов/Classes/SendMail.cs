using System.Net;
using System.Net.Mail;

namespace RegIN_Прохоров_Ожгибесов.Classes
{
    public class SendMail
    {
        public static void SendMessage(string Message, string To)
        {
            var smptClient = new SmtpClient("smtp.yandex.ru")
            {
                Port = 587,
                Credentials = new NetworkCredential("brenovc@yandex.ru", "jezhogmxcyeokfcy"),
                EnableSsl = true,
            };
            smptClient.Send("brenovc@yandex.ru", To, "Проект RegIn",Message);
        }
    }
}
