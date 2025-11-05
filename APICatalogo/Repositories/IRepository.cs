using System.Linq.Expressions;

namespace APICatalogo.Repositories
{
    public interface IRepository<T>
    {
        IEnumerable<T> ObterTodos();
        T? Obter(Expression<Func<T, bool>> predicate);
        T Inserir(T entidade);
        T Alterar(T entidade);
        T Remover(T entidade);
    }
}
