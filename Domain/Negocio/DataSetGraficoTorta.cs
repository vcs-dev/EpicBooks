using System.Collections.Generic;

namespace Domain.Negocio
{
    public class DataSetGraficoTorta
    {
        public DataSetGraficoTorta()
        {
            data = new List<int>();
            backgroundColor = new List<string>();
        }
        public List<int> data { get; set; }
        public List<string> backgroundColor { get; set; }
        public string label { get; set; }
    }
}
