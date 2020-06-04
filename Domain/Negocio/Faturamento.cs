namespace Domain.Negocio
{
    public class Faturamento : EntidadeDominio
    {
        public string DataInicial { get; set; }
        public string DataFinal { get; set; }
        public double Valor { get; set; }
        public string Data { get; set; }
        public string TipoGrafico { get; set; }
    }
}
