namespace Domain.Negocio
{
    public class Troca : EntidadeDominio
    {
        public int UsuarioId { get; set; }
        public int PedidoId { get; set; }
        public int ItemId { get; set; }
        public string NomeItem { get; set; }
        public char Status { get; set; }
        public int MyProperty { get; set; }
        public int Qtde { get; set; }
        public bool VoltaParEstoque { get; set; }
        public Cupom CupomTroca { get; set; }
        public string EstoqueObservacao { get; set; }
    }
}
