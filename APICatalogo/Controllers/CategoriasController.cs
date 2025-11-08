using APICatalogo.Models;
using APICatalogo.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriasController(IUnitOfWork unitOfWork, ILogger<CategoriasController> logger) : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ILogger<CategoriasController> _logger = logger;

        [HttpGet]
        public ActionResult<IEnumerable<Categoria>> ObterCategorias()
        {
            var categoria = _unitOfWork.CategoriaRepository.ObterTodos();
            return Ok(categoria);
        }

        [HttpGet("{id:int:min(1)}", Name = "ObterCategoria")]
        public ActionResult<Categoria> ObterCategoria(int id)
        {
            var categoria = _unitOfWork.CategoriaRepository.Obter(c => c.Id == id);
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

                var categoriaCriada = _unitOfWork.CategoriaRepository.Inserir(request);
                _unitOfWork.Commit();

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

            _unitOfWork.CategoriaRepository.Alterar(request);
            _unitOfWork.Commit();
            return Ok(request);
        }

        [HttpDelete("{id:int:min(1)}")]
        public ActionResult<int> RemoverCategoria(int id)
        {
            var categoria = _unitOfWork.CategoriaRepository.Obter(c => c.Id == id);

            if (categoria == null)
            {
                _logger.LogWarning($"Categoria com id: {id} não encontradaS");
                return NotFound("Categoria não encontrada");
            }
            var categoriaExcluida = _unitOfWork.CategoriaRepository.Remover(categoria);
            _unitOfWork.Commit();
            return Ok(categoriaExcluida.Id);
        }

    }
}
