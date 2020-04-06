using Domain;

namespace Domain.Negocio
{
    public class Cupom : EntidadeDominio
    {
        public Cupom()
        {
            Valor = 00.00;
        }
        public string Codigo { get; set; }
        public char Tipo { get; set; }
        public double Valor { get; set; }
        public byte Status { get; set; }
        public int? UsuarioId { get; set; }
    }
}
