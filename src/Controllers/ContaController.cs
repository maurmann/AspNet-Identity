using AspNet_Identity.Models;
using AspNet_Identity.RequestModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Threading.Tasks;
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



        private SignInManager<Usuario, string> signInManager;

        public SignInManager<Usuario, string> SignInManager
        {
            get
            {
                if (signInManager == null)
                {
                    var contextOwin = HttpContext.Current.GetOwinContext();
                    signInManager = contextOwin.GetUserManager<SignInManager<Usuario, string>>();
                }
                return signInManager;
            }

            set
            {
                signInManager = value;
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
            {
                IdETokenDeConfirmacao retornoFake = await EnviarEmailDeConfirmacaoAsync(novoUsuario).ConfigureAwait(false);
                return Ok(retornoFake);
            }


            AddToModelState(result);
            return BadRequest(ModelState);
        }

        [HttpPost]
        [Route("confirmar")]
        public async Task<IHttpActionResult> Confirmar(IdETokenDeConfirmacao idETokenDeConfirmacao)
        {

            if (idETokenDeConfirmacao == null || idETokenDeConfirmacao.Id == null || idETokenDeConfirmacao.Token == null)
                return BadRequest();

            var resultado = await UserManager.ConfirmEmailAsync(idETokenDeConfirmacao.Id, idETokenDeConfirmacao.Token);
            if (resultado.Succeeded)
                return Ok("Usuario conformado");


            return BadRequest();
        }

        [HttpPost]
        [Route("login")]
        public async Task<IHttpActionResult> Login(LoginRequest loginRequest)
        {

            if (ModelState.IsValid)
            {
                var usuario = await UserManager.FindByEmailAsync(loginRequest.Email);
                if (usuario == null)
                    return BadRequest("Usuário inválido");



                var signInResult = await SignInManager.PasswordSignInAsync(usuario.UserName, loginRequest.Password, isPersistent: false, shouldLockout: false);


                // Idealmente não retornar informações nos status lockedout, requiresverification
                // Alternativa enviar e-mail para o usuário informando o ocorrido

                switch (signInResult)
                {
                    case SignInStatus.Success:
                        return Ok();
                        break;
                    case SignInStatus.LockedOut:
                        return BadRequest("Usuário bloqueado");
                        break;
                    case SignInStatus.RequiresVerification:
                        return BadRequest("Conta não confirmada");
                        break;
                    case SignInStatus.Failure:
                        return BadRequest("Não foi possível autenticar com o usuário e senha informados");
                        break;
                    default:
                        throw new ArgumentOutOfRangeException("");
                        break;
                }


            }

            return BadRequest();

        }



        private async Task<IdETokenDeConfirmacao> EnviarEmailDeConfirmacaoAsync(Usuario usuario)
        {
            var token = await UserManager.GenerateEmailConfirmationTokenAsync(usuario.Id);

            // Em um uso real enviar um link além do código                        
            await UserManager.SendEmailAsync(usuario.Id, "Confirmação de e-mail", $"Confirme seu e-mail para poder utilizar a aplicação. Use esse token {token}");

            return new IdETokenDeConfirmacao() { Id = usuario.Id, Token = token };
        }

        private void AddToModelState(IdentityResult result)
        {
            foreach (var item in result.Errors)
            {
                ModelState.AddModelError("Identity Error", item);
            }
        }
    }


    public class IdETokenDeConfirmacao
    {
        public string Id { get; set; }
        public string Token { get; set; }
    }




}
