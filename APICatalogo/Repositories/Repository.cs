using APICatalogo.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace APICatalogo.Repositories
{
    public class Repository<T>(AppDbContext appDbContext) : IRepository<T> where T : class
    {
        protected readonly AppDbContext _appDbContext = appDbContext;

        public IEnumerable<T> ObterTodos()
        {
            return _appDbContext.Set<T>().ToList();
        }

        public T? Obter(Expression<Func<T, bool>> predicate)
        {
            return _appDbContext.Set<T>().FirstOrDefault(predicate);
        }

        public T Inserir(T entidade)
        {
            _appDbContext.Set<T>().Add(entidade);
            _appDbContext.SaveChanges();
            return entidade;
        }

        public T Alterar(T entidade)
        {
            _appDbContext.Set<T>().Update(entidade);
            _appDbContext.SaveChanges();
            return entidade;
        }

        public T Remover(T entidade)
        {
            _appDbContext.Set<T>().Remove(entidade);
            _appDbContext.SaveChanges();
            return entidade;
        }
    }
}
