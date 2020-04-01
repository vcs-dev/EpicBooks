using System;

namespace Domain.Negocio
{
    public class ItemBloqueado : EntidadeDominio
    {
        public int Qtde { get; set; }
        public Guid SessaoGuid { get; set; }
    }
}
