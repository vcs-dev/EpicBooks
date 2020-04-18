using System.Collections.Generic;

namespace Domain.Negocio
{
    public class ConjuntoDados
    {
        public ConjuntoDados()
        {
            fill = false;
            data = new List<int>();
            label = "Como fazer amigos e influenciar pessoas";
            data.Add(1533);
            data.Add(2134);
            data.Add(1848);
            data.Add(2400);
            data.Add(3430);
            data.Add(4044);
            data.Add(2093);
            data.Add(4949);
            data.Add(3949);
            data.Add(3493);
            data.Add(7495);
            data.Add(6499);
            data.Add(5940);
            lineTension = 0;
            backgroundColor = "trasparent";
            borderColor = "#00b9ae";
            borderWidth = 4;
            pointBackgroundColor = "#00b9ae";
        }
        public string label { get; set; }
        public bool fill { get; set; }
        public List<int> data { get; set; }
        public double lineTension { get; set; }
        public string backgroundColor { get; set; }
        public string borderColor { get; set; }
        public int borderWidth { get; set; }
        public string pointBackgroundColor { get; set; }
    }
}
