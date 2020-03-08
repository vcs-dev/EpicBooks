using Domain;
using System.Collections.Generic;

namespace Core.Interfaces
{
    public interface IDAO
    {
        void Salvar(EntidadeDominio entidade);
        void Alterar(EntidadeDominio entidade);
        void Excluir(EntidadeDominio entidade);
        List<EntidadeDominio> Consultar(EntidadeDominio entidade);
    }
}
