using APICatalogo.Context;
using APICatalogo.Models;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Repositories
{
    public class CategoriaRepository(AppDbContext appDbContext) : Repository<Categoria>(appDbContext), ICategoriaRepository
    {
    }
}
