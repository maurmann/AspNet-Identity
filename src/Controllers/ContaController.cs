using AspNet_Identity.Models;
using AspNet_Identity.RequestModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Web;
using System.Web.Http;

namespace AspNet_Identity.Controllers
{
    [RoutePrefix("contas")]
    public class ContaController : ApiController
    {

        private UserManager<Usuario> userManager;

        public UserManager<Usuario> UserManager
        {
            get
            {
                if (userManager == null)
                {
                    var contextOwin = HttpContext.Current.GetOwinContext();
                    userManager = contextOwin.GetUserManager<UserManager<Usuario>>();
                }
                return userManager;
            }

            set
            {
                userManager = value;
            }

        }


        [HttpPost]
        [Route("registrar")]
        public IHttpActionResult Registrar(ContaRegistrarRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var novoUsuario = new Usuario
            {
                Email = request.Email,
                UserName = request.UserName,
                NomeCompleto = request.NomeCompleto
            };

            UserManager.Create(novoUsuario, request.Senha);

            return Ok();
        }

    }
}
