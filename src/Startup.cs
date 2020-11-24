using AspNet_Identity.Identity;
using AspNet_Identity.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Owin;
using System.Data.Entity;

[assembly: OwinStartup(typeof(AspNet_Identity.Startup))]
namespace AspNet_Identity
{
    public class Startup
    {
        public void Configuration(IAppBuilder builder)
        {
            builder.CreatePerOwinContext<DbContext>(() =>
                            new IdentityDbContext<Usuario>("DefaultConnection"));

            builder.CreatePerOwinContext<IUserStore<Usuario>>(
                (opcoes, contextoOwin) =>
                {
                    var dbContext = contextoOwin.Get<DbContext>();
                    return new UserStore<Usuario>(dbContext);
                });

            builder.CreatePerOwinContext<UserManager<Usuario>>(
                (opcoes, contextoOwin) =>
                {
                    var userStore = contextoOwin.Get<IUserStore<Usuario>>();
                    var userManager = new UserManager<Usuario>(userStore);

                    userManager.UserValidator = new UserValidator<Usuario>(userManager) { RequireUniqueEmail = true };
                    userManager.PasswordValidator = new CustomPasswordValidator();

                    userManager.EmailService = new AccountEmailConfirmation();


                    // Configuração do provider que será utilizado para gerar o token
                    var dataProtectionProvider = opcoes.DataProtectionProvider;
                    var dataProtectionProviderCreated = dataProtectionProvider.Create("teste curso alura");

                    userManager.UserTokenProvider = new DataProtectorTokenProvider<Usuario>(dataProtectionProviderCreated);

                    return userManager;
                });

        }
    }
}