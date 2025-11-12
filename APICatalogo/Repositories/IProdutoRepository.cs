using APICatalogo.Models;
using APICatalogo.Pagination;

namespace APICatalogo.Repositories
{
    public interface IProdutoRepository : IRepository<Produto>
    {
        //IEnumerable<Produto> ObterProdutos(ProdutosParameters produtosParams);
        PagedList<Produto> ObterProdutos(ProdutosParameters produtosParameters);
        IEnumerable<Produto> ObterProdutosPorCategoria(int id);
    }
}
