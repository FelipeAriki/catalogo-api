using APICatalogo.Context;
using APICatalogo.Models;

namespace APICatalogo.Repositories
{
    public class ProdutoRepository(AppDbContext appDbContext) : IProdutoRepository
    {
        private readonly AppDbContext _appDbContext = appDbContext;

        public IQueryable<Produto> ObterProdutos()
        {
            return _appDbContext.Produto;
        }

        public Produto ObterProduto(int id)
        {
            var produto = _appDbContext.Produto.FirstOrDefault(x => x.Id == id);
            return produto ?? throw new InvalidOperationException("Produto não encontrado!");
        }

        public Produto InserirProduto(Produto produto)
        {
            if(produto is null) throw new InvalidOperationException("Produto não encontrado!");

            _appDbContext.Produto.Add(produto);
            _appDbContext.SaveChanges();
            return produto;
        }

        public bool AlterarProduto(Produto produto)
        {
            if (produto is null) throw new InvalidOperationException("Produto não encontrado!");

            if (_appDbContext.Produto.Any(p => p.Id == produto.Id))
            {
                _appDbContext.Produto.Update(produto);
                _appDbContext.SaveChanges();
                return true;
            }
            return false;
        }

        public bool RemoverProduto(int id)
        {
            var produto = _appDbContext.Produto.Find(id);
            if(produto is not null)
            {
                _appDbContext.Produto.Remove(produto);
                _appDbContext.SaveChanges();
                return true;
            }
            return false;
        }
    }
}
