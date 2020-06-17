namespace Domain.DadosCliente
{
    public class CartaoDeCredito : EntidadeDominio
    {
        public CartaoDeCredito()
        {
            Valor = null;
            Numeracao = "";
            NomeImpresso = "";
            Validade = "";
            Apelido = "";
            Ativo = 1;
        }
        public int Bandeira { get; set; }
        public string BandeiraDescricao { get; set; }
        public string Numeracao { get; set; }
        public string NomeImpresso { get; set; }
        public string Validade { get; set; }
        public string Apelido { get; set; }
        public int? CodigoSeguranca { get; set; }
        public int UsuarioId { get; set; }
        public int QtdeParcelas { get; set; }
        public double? Valor { get; set; }
        public byte Ativo { get; set; }
    }
}
