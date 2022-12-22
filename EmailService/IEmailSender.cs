using System;
using System.Collections.Generic;
using System.Text;

namespace EmailService
{
    interface IEmailSender
    {
        void SendEmail(Message message);
    }
}
