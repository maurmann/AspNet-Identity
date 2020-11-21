using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AspNet_Identity.Models
{
    public class Usuario : IdentityUser 
    {
        public string NomeCompleto { get; set; }
    }
}