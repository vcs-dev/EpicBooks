using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Negocio
{
    public class GraficoTorta
    {
        public GraficoTorta()
        {
            datasets = new List<DataSetGraficoTorta>();
            labels = new List<string>();
        }
        public List<DataSetGraficoTorta> datasets { get; set; }
        public List<string> labels { get; set; }
    }
}
