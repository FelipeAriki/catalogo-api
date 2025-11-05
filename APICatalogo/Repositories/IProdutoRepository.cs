using APICatalogo.Models;

namespace APICatalogo.Repositories
{
    public interface IProdutoRepository : IRepository<Produto>
    {
        IEnumerable<Produto> ObterProdutosPorCategoria(int id);
    }
}
