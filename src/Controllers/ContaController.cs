using AspNet_Identity.Models;
using AspNet_Identity.RequestModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Web.Http;

namespace AspNet_Identity.Controllers
{
    [RoutePrefix("contas")]
    public class ContaController : ApiController
    {

        [HttpPost]
        [Route("registrar")]
        public IHttpActionResult Registrar(ContaRegistrarRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var dbContext = new IdentityDbContext<Usuario>("DefaultConnection");
            var userStore = new UserStore<Usuario>(dbContext);
            var userManager = new UserManager<Usuario>(userStore);

            var novoUsuario = new Usuario();
            novoUsuario.Email = request.Email;
            novoUsuario.UserName = request.UserName;
            novoUsuario.NomeCompleto = request.NomeCompleto;

            userManager.Create(novoUsuario, request.Senha);

            return Ok();
        }

    }
}
