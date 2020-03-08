using Core.Application;
using Domain;

namespace Core.Interfaces
{
    public interface IFacade
    {
        Result Salvar(EntidadeDominio entidade);
        Result Alterar(EntidadeDominio entidade);
        Result Excluir(EntidadeDominio entidade);
        Result Consultar(EntidadeDominio entidade);
        Result Visualizar(EntidadeDominio entidade);
    }
}
