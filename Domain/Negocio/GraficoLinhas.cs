using System.Collections.Generic;

namespace Domain.Negocio
{
    public class GraficoLinhas
    {
        public GraficoLinhas()
        {
            labels = new List<string>();
            datasets = new List<DataSetGraficoLinhas>();           
        }
        public List<string> labels { get; set; }
        public List<DataSetGraficoLinhas> datasets { get; set; }
        public string mensagemErro { get; set; }
    }
}
