using Domain;
using System.Collections.Generic;

namespace Core.Application
{
    public class Result
    {
        public string Msg { get; set; }
        public List<EntidadeDominio> Entidades { get; set; }
    }
}
