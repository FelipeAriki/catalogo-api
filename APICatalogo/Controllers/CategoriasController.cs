using APICatalogo.Models;
using APICatalogo.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriasController(ICategoriaRepository repository,/*IRepository<Categoria> repository,*/ ILogger<CategoriasController> logger) : ControllerBase
    {
        private readonly ICategoriaRepository _repository = repository;
        //private readonly IRepository<Categoria> _repository = repository;
        private readonly ILogger<CategoriasController> _logger = logger;

        [HttpGet]
        public ActionResult<IEnumerable<Categoria>> ObterCategorias()
        {
            var categoria = _repository.ObterTodos();
            return Ok(categoria);
        }

        [HttpGet("{id:int:min(1)}", Name = "ObterCategoria")]
        public ActionResult<Categoria> ObterCategoria(int id)
        {
            var categoria = _repository.Obter(c => c.Id == id);
            if (categoria == null)
            {
                _logger.LogWarning($"Categoria com id: {id} não encontradaS");
                return NotFound("Categoria não encontrada...");
            }
            return Ok(categoria);
        }

        [HttpPost]
        public ActionResult InserirCategoria(Categoria request)
        {
            if (request == null)
            {
                _logger.LogWarning($"Request inválido: {nameof(Categoria)}");
                return BadRequest("Informações inválidas.");
            }

                var categoriaCriada = _repository.Inserir(request);

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

            _repository.Alterar(request);
            return Ok(request);
        }

        [HttpDelete("{id:int:min(1)}")]
        public ActionResult<int> RemoverCategoria(int id)
        {
            var categoria = _repository.Obter(c => c.Id == id);

            if (categoria == null)
            {
                _logger.LogWarning($"Categoria com id: {id} não encontradaS");
                return NotFound("Categoria não encontrada");
            }
            var categoriaExcluida = _repository.Remover(categoria);
            return Ok(categoriaExcluida.Id);
        }

    }
}
