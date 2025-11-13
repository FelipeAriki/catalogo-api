using APICatalogo.Context;
using APICatalogo.Models;
using APICatalogo.Pagination;

namespace APICatalogo.Repositories
{
    public class ProdutoRepository(AppDbContext appDbContext) : Repository<Produto>(appDbContext), IProdutoRepository
    {
        public async Task<PagedList<Produto>> ObterProdutosAsync(ProdutosParameters produtosParameters)
        {
            var produtos = await ObterTodosAsync();
            return PagedList<Produto>.ToPagedList(produtos.OrderBy(p => p.Id).AsQueryable(), produtosParameters.PageNumber, produtosParameters.PageSize);

        }

        public async Task<PagedList<Produto>> ObterProdutosFiltroPrecoAsync(ProdutosFiltroPreco produtosFiltroPreco)
        {
            var produtos = await ObterTodosAsync();
            if(produtosFiltroPreco.Preco.HasValue && !string.IsNullOrEmpty(produtosFiltroPreco.PrecoCriterio))
            {
                if (produtosFiltroPreco.PrecoCriterio.Equals("maior", StringComparison.OrdinalIgnoreCase))
                    produtos = produtos.Where(p => p.Preco > produtosFiltroPreco.Preco.Value).OrderBy(p => p.Preco);
                else if (produtosFiltroPreco.PrecoCriterio.Equals("menor", StringComparison.OrdinalIgnoreCase))
                    produtos = produtos.Where(p => p.Preco < produtosFiltroPreco.Preco.Value).OrderBy(p => p.Preco);
                else if (produtosFiltroPreco.PrecoCriterio.Equals("igual", StringComparison.OrdinalIgnoreCase))
                    produtos = produtos.Where(p => p.Preco == produtosFiltroPreco.Preco.Value).OrderBy(p => p.Preco);
            }
            return PagedList<Produto>.ToPagedList(produtos.AsQueryable(), produtosFiltroPreco.PageNumber, produtosFiltroPreco.PageSize);
        }

        /*public IEnumerable<Produto> ObterProdutos(ProdutosParameters produtosParams)
        {
           return ObterTodos()
               .OrderBy(p => p.Nome)
               .Skip((produtosParams.PageNumber - 1) * produtosParams.PageSize)
               .Take(produtosParams.PageSize).ToList();
        }*/

        public async Task<IEnumerable<Produto>> ObterProdutosPorCategoriaAsync(int id)
        {
            var produtosPorCategoria = await ObterTodosAsync();
            return produtosPorCategoria.Where(c => c.CategoriaId == id);
        }
    }
}
