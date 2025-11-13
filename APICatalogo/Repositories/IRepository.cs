using System.Linq.Expressions;

namespace APICatalogo.Repositories
{
    public interface IRepository<T>
    {
        Task<IEnumerable<T>> ObterTodosAsync();
        Task<T?> ObterAsync(Expression<Func<T, bool>> predicate);
        T Inserir(T entidade);
        T Alterar(T entidade);
        T Remover(T entidade);
    }
}
