﻿namespace Domain.DadosCliente
{
    public class Endereco : EntidadeDominio
    {
        public int TipoEndereco { get; set; }
        public int TipoResidencia { get; set; }
        public int TipoLogradouro { get; set; }
        public string Logradouro { get; set; }
        public int Numero { get; set; }
        public string Cep { get; set; }
        public string Bairro { get; set; }
        public int Cidade { get; set; }
        public int Estado { get; set; }
        public int UsuarioId { get; set; }
    }
}
