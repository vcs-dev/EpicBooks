using Domain;

namespace Domain.Negocio
{
    public class Cupom : EntidadeDominio
    {
        public string CodigoCupom { get; set; }
        public char TipoCupom { get; set; }
        public double ValorCupom { get; set; }
        public byte Status { get; set; }
        public int? UsuarioId { get; set; }
    }
}
