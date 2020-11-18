using AspNet_Identity.RequestModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AspNet_Identity.Controllers
{
    public class ContaController : ApiController
    {

        [HttpPost]
        public IHttpActionResult Registrar(ContaRegistrarRequest request)
        {
            return Ok();
        }

    }
}
