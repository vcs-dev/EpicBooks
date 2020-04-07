namespace Domain.Negocio
{
    public class Estoque : EntidadeDominio
    {
        public int ProdutoId { get; set; }
        public int Qtde { get; set; }
        public double ValorCusto { get; set; }
    }
}
