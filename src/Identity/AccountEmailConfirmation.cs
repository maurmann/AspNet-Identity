using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace AspNet_Identity.Identity
{
    public class AccountEmailConfirmation : IIdentityMessageService
    {
        public async Task SendAsync(IdentityMessage message)
        {


            // Enviar o e-mail
            string assunto = message.Subject;

            string destinatario = message.Destination;

            string boy = message.Body;



        }
    }
}