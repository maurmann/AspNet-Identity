using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace AspNet_Identity.Controllers
{
    public class ProtegidoController : ApiController
    {

        [HttpGet]
        [Authorize]
        public async Task<IHttpActionResult> Consultar(int id)
        {
            return Ok(id);
        }

    }
}
