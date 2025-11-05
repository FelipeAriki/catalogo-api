using APICatalogo.Context;
using APICatalogo.Models;

namespace APICatalogo.Repositories
{
    public class ProdutoRepository(AppDbContext appDbContext) : Repository<Produto>(appDbContext), IProdutoRepository
    {
        public IEnumerable<Produto> ObterProdutosPorCategoria(int id)
        {
            return ObterTodos().Where(c => c.CategoriaId == id);
        }
    }
}
