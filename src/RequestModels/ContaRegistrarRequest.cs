using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AspNet_Identity.RequestModels
{
    public class ContaRegistrarRequest
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        [MinLength(5)]
        [MaxLength(50)]
        public string NomeCompleto { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Senha { get; set; }

    }
}