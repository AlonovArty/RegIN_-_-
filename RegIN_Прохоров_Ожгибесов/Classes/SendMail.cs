using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RegIN_Прохоров_Ожгибесов.Classes
{
    public class SendMail
    {
        public static void SendMessage(string Message,string To)
        {
            var smptClient = new smptClient("smtp.yandex.ru")
            {
                Port = 587,
                Credential = new NetworkCredential("yandex@yandex.ru", "password"),
                EnableSsl = true,
            };
            smptClient.Send("landaxer.yandex.ru",To, "Проект RegIn",Message);
        }
    }
}
