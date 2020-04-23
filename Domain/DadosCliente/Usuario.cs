using System.Collections.Generic;

namespace Domain.DadosCliente
{
    public class Usuario : EntidadeDominio
    {
        public string NomeCompleto { get; set; }
        public byte Sexo { get; set; }
        public string DataNascimento { get; set; }
        public string Cpf { get; set; }
        public byte TelefoneTipo { get; set; }
        public string TelefoneDdd { get; set; }
        public string TelefoneNumero { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public CartaoDeCredito Cartao { get; set; }
        public List<Endereco> Enderecos { get; set; }
    }
}
