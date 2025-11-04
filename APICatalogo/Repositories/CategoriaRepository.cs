using APICatalogo.Context;
using APICatalogo.Models;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Repositories
{
    public class CategoriaRepository(AppDbContext appDbContext) : ICategoriaRepository
    {
        private readonly AppDbContext _appDbContext = appDbContext;

        public IEnumerable<Categoria> ObterCategoriasAsync()
        {
            return _appDbContext.Categorias.ToList();
        }

        public Categoria ObterCategoria(int id)
        {
            return _appDbContext.Categorias?.FirstOrDefault(c => c.Id == id);
        }

        public Categoria InserirCategoria(Categoria categoria)
        {
            if (categoria == null) throw new ArgumentNullException(nameof(categoria));

            _appDbContext.Categorias?.Add(categoria);
            _appDbContext.SaveChanges();
            return categoria;
        }

        public Categoria AlterarCategoria(Categoria categoria)
        {
            if (categoria == null) throw new ArgumentNullException(nameof(categoria));

            _appDbContext.Entry(categoria).State = EntityState.Modified;
            _appDbContext.SaveChanges();
            return categoria;
        }

        public Categoria RemoverCategoria(int id)
        {
            var categoria = _appDbContext.Categorias?.Find(id);
            if(categoria == null) throw new ArgumentNullException(nameof(categoria));

            _appDbContext.Categorias?.Remove(categoria);
            _appDbContext.SaveChanges();
            return categoria;
        }
    }
}
