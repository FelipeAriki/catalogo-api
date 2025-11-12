using APICatalogo.Context;
using APICatalogo.Models;
using APICatalogo.Pagination;

namespace APICatalogo.Repositories
{
    public class ProdutoRepository(AppDbContext appDbContext) : Repository<Produto>(appDbContext), IProdutoRepository
    {
        public PagedList<Produto> ObterProdutos(ProdutosParameters produtosParameters)
        {
            var produtos = ObterTodos().OrderBy(p => p.Id).AsQueryable();
            return PagedList<Produto>.ToPagedList(produtos, produtosParameters.PageNumber, produtosParameters.PageSize);

        }

        /*public IEnumerable<Produto> ObterProdutos(ProdutosParameters produtosParams)
        {
           return ObterTodos()
               .OrderBy(p => p.Nome)
               .Skip((produtosParams.PageNumber - 1) * produtosParams.PageSize)
               .Take(produtosParams.PageSize).ToList();
        }*/

        public IEnumerable<Produto> ObterProdutosPorCategoria(int id)
        {
            return ObterTodos().Where(c => c.CategoriaId == id);
        }
    }
}
