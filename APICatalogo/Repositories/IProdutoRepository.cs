using APICatalogo.Models;
using APICatalogo.Pagination;

namespace APICatalogo.Repositories
{
    public interface IProdutoRepository : IRepository<Produto>
    {
        Task<PagedList<Produto>> ObterProdutosAsync(ProdutosParameters produtosParameters);
        Task<PagedList<Produto>> ObterProdutosFiltroPrecoAsync(ProdutosFiltroPreco produtosFiltroPreco);
        Task<IEnumerable<Produto>> ObterProdutosPorCategoriaAsync(int id);
    }
}
