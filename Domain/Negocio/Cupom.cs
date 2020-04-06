using System;

namespace Domain.Negocio
{
    public class Cupom : EntidadeDominio
    {
        public string Codigo { get; set; }
        public char Tipo { get; set; }
        public double Valor { get; set; }
        public DateTime DataExpiracao { get; set; }
        public byte? Usado { get; set; }
        public int? UsuarioId { get; set; }
    }
}
