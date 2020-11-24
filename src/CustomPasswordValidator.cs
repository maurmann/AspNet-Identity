using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace AspNet_Identity
{
    public class CustomPasswordValidator : IIdentityValidator<string>
    {
        private const int tamanhoMinimo = 8;


        public async Task<IdentityResult> ValidateAsync(string item)
        {
            var erros = new List<string>();


            if (!VerificaTamanhoRequerido(item))
                erros.Add($"A senha deve possuir pelo menos {tamanhoMinimo} caracteres");

            if (!VerificaLowercase(item))
                erros.Add("A senha deve possuir pelo menos um caracter minúsculo");

            if (!VerificaUppercase(item))
                erros.Add("A senha deve possuir pelo menos um caracter maiúsculo");

            if (!VerificaDigito(item))
                erros.Add("A senha deve possuir pelo menos um digito");

            if (!VerificaCaracteresEspeciais(item))
                erros.Add("nao tem caracteres especiais");

            if (erros.Any())
                return IdentityResult.Failed(erros.ToArray());
            else
                return IdentityResult.Success;

        }

        private bool VerificaTamanhoRequerido(string senha) => senha?.Length >= tamanhoMinimo;

        private bool VerificaLowercase(string senha) => senha.Any(char.IsLower);


        private bool VerificaUppercase(string senha) => senha.Any(char.IsUpper);


        private bool VerificaDigito(string senha) => senha.Any(char.IsDigit);


        private bool VerificaCaracteresEspeciais(string senha) => Regex.IsMatch(senha, @"[~`!@#$%^&*()+=|\\{}':;.,<>/?[\]""_-]");


    }
}