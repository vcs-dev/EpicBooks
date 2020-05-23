﻿using Core.Interfaces;
using Domain;
using Domain.DadosCliente;
using System.Text.RegularExpressions;

namespace Core.Impl.Business
{
    public class ValidadorSenha : IStrategy
    {
        public string Processar(EntidadeDominio entidade)
        {
            if (entidade.GetType().Name.Equals("Usuario"))
            {
                Usuario usuario = (Usuario)entidade;
                Regex regex;

                if (!usuario.Senha.Equals(usuario.ConfirmacaoSenha))
                    return "Confirmação de senha diferente da senha";

                if (string.IsNullOrWhiteSpace(usuario.Senha))
                {
                    return "A senha não pode ter espaços em branco";
                }
                if(usuario.Senha.Length < 8)
                    return "A senha deve conter pelo menos 8 caracteres";

                regex = new Regex("@\b[0-9a-zA-Z!@$%&*()]");
                MatchCollection mc = regex.Matches(usuario.Senha);
                if(mc.Count < 4)
                    return "A senha deve conter letras maiúsculas, minúsculas e caracteres especiais";
            }
            else
            {
                return "Deve ser registrado um usuário";
            }
            return null;
        }
    }
}
