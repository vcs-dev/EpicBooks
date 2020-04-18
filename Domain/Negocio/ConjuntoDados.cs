using System.Collections.Generic;

namespace Domain.Negocio
{
    public class ConjuntoDados
    {
        public ConjuntoDados()
        {
            Fill = false;
            Data = new List<int>();
            Label = "Como fazer amigos e influenciar pessoas";
            Data.Add(1533);
            Data.Add(2134);
            Data.Add(1848);
            Data.Add(2400);
            Data.Add(3430);
            Data.Add(4044);
            Data.Add(2093);
            Data.Add(4949);
            Data.Add(3949);
            Data.Add(3493);
            Data.Add(7495);
            Data.Add(6499);
            Data.Add(5940);
            LineTension = 0;
            BackgroundColor = "trasparent";
            BorderColor = "#00b9ae";
            BorderWidth = 4;
            PointBackgroundColor = "#00b9ae";
        }
        public string Label { get; set; }
        public bool Fill { get; set; }
        public List<int> Data { get; set; }
        public double LineTension { get; set; }
        public string BackgroundColor { get; set; }
        public string BorderColor { get; set; }
        public int BorderWidth { get; set; }
        public string PointBackgroundColor { get; set; }
    }
}
