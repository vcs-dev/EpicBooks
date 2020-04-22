using System;
using System.Collections.Generic;

namespace Domain.Negocio
{
    public class DataSetGraficoLinhas : EntidadeDominio
    {
        public DataSetGraficoLinhas()
        {
            fill = false;
            data = new List<double>();
            lineTension = 0;
            backgroundColor = "trasparent";
            Random random = new Random();
            borderColor = string.Format("rgba({0}, {1}, {2}, 0.9)", random.Next(0, 255), random.Next(0, 255), random.Next(0, 255));
            borderWidth = 4;
            pointBackgroundColor = borderColor;
        }
        public string DataInicial { get; set; }
        public string DataFinal { get; set; }
        public string label { get; set; }
        public bool fill { get; set; }
        public List<double> data { get; set; }
        public double lineTension { get; set; }
        public string backgroundColor { get; set; }
        public string borderColor { get; set; }
        public int borderWidth { get; set; }
        public string pointBackgroundColor { get; set; }
    }
}
