using APICatalogo.Models;

namespace APICatalogo.Repositories
{
    public interface IProdutoRepository
    {
        IQueryable<Produto> ObterProdutos();
        Produto ObterProduto(int id);
        Produto InserirProduto(Produto produto);
        bool AlterarProduto(Produto produto);
        bool RemoverProduto(int id);
    }
}
