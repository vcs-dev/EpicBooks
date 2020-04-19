using System.Collections.Generic;

namespace Domain.Negocio
{
    public class GraficoLinhas
    {
        public GraficoLinhas()
        {
            labels = new List<string>();
            //ConjuntoDados DataSet = new ConjuntoDados();
            datasets = new List<ConjuntoDados>();
            //datasets.Add(DataSet);
            //labels.Add("Fevereiro 2019");
            //labels.Add("Março 2019");
            //labels.Add("Abril 2019");
            //labels.Add("Maio 2019");
            //labels.Add("Junho 2019");
            //labels.Add("Julho 2019");
            //labels.Add("Agosto 2019");
            //labels.Add("Setembro 2019");
            //labels.Add("Outubro 2019");
            //labels.Add("Novembro 2019");
            //labels.Add("Dezembro 2019");
            //labels.Add("Janeiro 2020");
            //labels.Add("Fevereiro 2020");                
        }
        public List<string> labels { get; set; }
        public List<ConjuntoDados> datasets { get; set; }
    }
}
