using APICatalogo.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace APICatalogo.Repositories
{
    public class Repository<T>(AppDbContext appDbContext) : IRepository<T> where T : class
    {
        protected readonly AppDbContext _appDbContext = appDbContext;

        public async Task<IEnumerable<T>> ObterTodosAsync()
        {
            return await _appDbContext.Set<T>().AsNoTracking().ToListAsync();
        }

        public async Task<T?> ObterAsync(Expression<Func<T, bool>> predicate)
        {
            return await _appDbContext.Set<T>().FirstOrDefaultAsync(predicate);
        }

        public T Inserir(T entidade)
        {
            _appDbContext.Set<T>().Add(entidade);
            //_appDbContext.SaveChanges();
            return entidade;
        }

        public T Alterar(T entidade)
        {
            _appDbContext.Set<T>().Update(entidade);
            //_appDbContext.SaveChanges();
            return entidade;
        }

        public T Remover(T entidade)
        {
            _appDbContext.Set<T>().Remove(entidade);
            //_appDbContext.SaveChanges();
            return entidade;
        }
    }
}
