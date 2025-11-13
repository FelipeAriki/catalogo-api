using APICatalogo.Context;
using APICatalogo.Models;
using APICatalogo.Pagination;

namespace APICatalogo.Repositories
{
    public class CategoriaRepository(AppDbContext appDbContext) : Repository<Categoria>(appDbContext), ICategoriaRepository
    {
        public async Task<PagedList<Categoria>> ObterCategoriasAsync(CategoriasParameters categoriasParameters)
        {
            var categorias = await ObterTodosAsync();

            return PagedList<Categoria>.ToPagedList(categorias.OrderBy(c => c.Id).AsQueryable(), categoriasParameters.PageNumber, categoriasParameters.PageSize);
        }

        public async Task<PagedList<Categoria>> ObterCategoriasFiltroNomeAsync(CategoriasFiltroNome categoriasFiltroNome)
        {
            var categorias = await ObterTodosAsync();
            if (!string.IsNullOrEmpty(categoriasFiltroNome.Nome))
                categorias = categorias.Where(c => c.Nome.Contains(categoriasFiltroNome.Nome));

            return PagedList<Categoria>.ToPagedList(categorias.AsQueryable(), categoriasFiltroNome.PageNumber, categoriasFiltroNome.PageSize);
        }
    }
}
