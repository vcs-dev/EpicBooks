using System.Collections.Generic;

namespace Domain.Negocio
{
    public class GraficoLinhas
    {
        public GraficoLinhas()
        {
            Labels = new List<string>();
            ConjuntoDados DataSet = new ConjuntoDados();
            DataSets = new List<ConjuntoDados>();
            DataSets.Add(DataSet);
            Labels.Add("Fevereiro 2019");
            Labels.Add("Março 2019");
            Labels.Add("Abril 2019");
            Labels.Add("Maio 2019");
            Labels.Add("Junho 2019");
            Labels.Add("Julho 2019");
            Labels.Add("Agosto 2019");
            Labels.Add("Setembro 2019");
            Labels.Add("Outubro 2019");
            Labels.Add("Novembro 2019");
            Labels.Add("Dezembro 2019");
            Labels.Add("Janeiro 2020");
            Labels.Add("Fevereiro 2020");                
        }
        public List<string> Labels { get; set; }
        public List<ConjuntoDados> DataSets { get; set; }
    }
}
