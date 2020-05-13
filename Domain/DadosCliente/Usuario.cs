﻿namespace Domain.DadosCliente
{
    public class Usuario : EntidadeDominio
    {
        public Usuario()
        {
            EnderecoEntrega.TipoEndereco = 1;
            EnderecoCobranca.TipoEndereco = 2;
        }
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
        public Endereco EnderecoEntrega { get; set; }
        public Endereco EnderecoCobranca { get; set; }
        public byte EndEntregaECobranca { get; set; }
    }
}
