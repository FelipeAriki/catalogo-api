using APICatalogo.Models;
using APICatalogo.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriasController(ICategoriaRepository categoriaRepository, ILogger<CategoriasController> logger) : ControllerBase
    {
        private readonly ICategoriaRepository _categoriaRepository = categoriaRepository;
        private readonly ILogger<CategoriasController> _logger = logger;

        [HttpGet]
        public ActionResult<IEnumerable<Categoria>> ObterCategorias()
        {
            var categoria = _categoriaRepository.ObterCategoriasAsync();
            return Ok(categoria);
        }

        [HttpGet("{id:int:min(1)}", Name = "ObterCategoria")]
        public ActionResult<Categoria> ObterCategoria(int id)
        {
            var categoria = _categoriaRepository.ObterCategoria(id);
            if (categoria == null)
            {
                _logger.LogWarning($"Categoria com id: {id} não encontradaS");
                return NotFound("Categoria não encontrada...");
            }
            return Ok(categoria);
        }

        /*[HttpGet("produtos")]
        public ActionResult<IEnumerable<Categoria>> ObterCategoriasProdutos()
        {
            return _appDbContext.Categorias.AsNoTracking().Include(p => p.Produtos).ToList();
        }*/

        [HttpPost]
        public ActionResult InserirCategoria(Categoria request)
        {
            if (request == null)
            {
                _logger.LogWarning($"Request inválido: {nameof(Categoria)}");
                return BadRequest("Informações inválidas.");
            }

                var categoriaCriada = _categoriaRepository.InserirCategoria(request);

                return new CreatedAtRouteResult("ObterCategoria", new { id = categoriaCriada.Id }, categoriaCriada);
        }

        [HttpPut("{id:int:min(1)}")]
        public ActionResult AlterarCategoria(int id, Categoria request)
        {
            if (id != request.Id)
            {
                _logger.LogWarning("Dados inválidos...");
                return BadRequest("Categoria não encontrada.");
            }
            
            _categoriaRepository.AlterarCategoria(request);
            return Ok(request);
        }

        [HttpDelete("{id:int:min(1)}")]
        public ActionResult<int> RemoverCategoria(int id)
        {
            var categoria = _categoriaRepository.ObterCategoria(id);

            if (categoria == null)
            {
                _logger.LogWarning($"Categoria com id: {id} não encontradaS");
                return NotFound("Categoria não encontrada");
            }
            var categoriaExcluida = _categoriaRepository.RemoverCategoria(id);
            return Ok(categoriaExcluida.Id);
        }

    }
}
