using APICatalogo.Context;
using APICatalogo.Models;
using APICatalogo.Pagination;

namespace APICatalogo.Repositories
{
    public class CategoriaRepository(AppDbContext appDbContext) : Repository<Categoria>(appDbContext), ICategoriaRepository
    {
        public PagedList<Categoria> ObterCategorias(CategoriasParameters categoriasParameters)
        {
            var categorias = ObterTodos().OrderBy(c => c.Id).AsQueryable();
            return PagedList<Categoria>.ToPagedList(categorias, categoriasParameters.PageNumber, categoriasParameters.PageSize);
        }

        public PagedList<Categoria> ObterCategoriasFiltroNome(CategoriasFiltroNome categoriasFiltroNome)
        {
            var categorias = ObterTodos().AsQueryable();
            if (!string.IsNullOrEmpty(categoriasFiltroNome.Nome))
                categorias = categorias.Where(c => c.Nome.Contains(categoriasFiltroNome.Nome));
            return PagedList<Categoria>.ToPagedList(categorias, categoriasFiltroNome.PageNumber, categoriasFiltroNome.PageSize);
        }
    }
}
