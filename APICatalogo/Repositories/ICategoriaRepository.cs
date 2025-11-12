using APICatalogo.Models;
using APICatalogo.Pagination;

namespace APICatalogo.Repositories
{
    public interface ICategoriaRepository : IRepository<Categoria>
    {
        PagedList<Categoria> ObterCategorias(CategoriasParameters categoriasParameters);
        PagedList<Categoria> ObterCategoriasFiltroNome(CategoriasFiltroNome categoriasFiltroNome);
    }
}
