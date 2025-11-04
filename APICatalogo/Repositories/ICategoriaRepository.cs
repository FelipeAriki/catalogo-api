using APICatalogo.Models;

namespace APICatalogo.Repositories
{
    public interface ICategoriaRepository
    {
        IEnumerable<Categoria> ObterCategoriasAsync();
        Categoria ObterCategoria(int id);
        Categoria InserirCategoria(Categoria categoria);
        Categoria AlterarCategoria(Categoria categoria);
        Categoria RemoverCategoria(int id);
    }
}
