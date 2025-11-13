using APICatalogo.Models;
using APICatalogo.Pagination;

namespace APICatalogo.Repositories
{
    public interface ICategoriaRepository : IRepository<Categoria>
    {
        Task<PagedList<Categoria>> ObterCategoriasAsync(CategoriasParameters categoriasParameters);
        Task<PagedList<Categoria>> ObterCategoriasFiltroNomeAsync(CategoriasFiltroNome categoriasFiltroNome);
    }
}
