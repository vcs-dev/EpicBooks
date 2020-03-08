using Domain;

namespace Core.Interfaces
{
    public interface IStrategy
    {
        string Processar(EntidadeDominio entidade);
    }
}
