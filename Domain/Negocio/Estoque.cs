namespace Domain.Negocio
{
    public class Estoque : EntidadeDominio
    {
        public string CaminhoImagem { get; set; }
        public int ProdutoId { get; set; }
        public string NomeProduto { get; set; }
        public int Qtde { get; set; }
        public double ValorCusto { get; set; }
        public string Observacao { get; set; }
    }
}
