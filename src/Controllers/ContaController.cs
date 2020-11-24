using AspNet_Identity.Models;
using AspNet_Identity.RequestModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using System.Linq;

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
        public async Task<IHttpActionResult> Registrar(ContaRegistrarRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            var usuario = await UserManager.FindByEmailAsync(request.Email);
            if (usuario != null)
                return BadRequest("Usuário já cadastrado!");


            var novoUsuario = new Usuario
            {
                Email = request.Email,
                UserName = request.UserName,
                NomeCompleto = request.NomeCompleto
            };

            var result = await UserManager.CreateAsync(novoUsuario, request.Senha);
            if (result.Succeeded)
                return Ok();


            AddToModelState(result);
            return BadRequest(ModelState);
        }

        private void AddToModelState(IdentityResult result)
        {
            foreach (var item in result.Errors)
            {
                ModelState.AddModelError("Identity Error", item);
            }
        }
    }
}
