using APICatalogo.DTOs;
using APICatalogo.DTOs.Mappings;
using APICatalogo.Pagination;
using APICatalogo.Repositories;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace APICatalogo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriasController(IUnitOfWork unitOfWork, ILogger<CategoriasController> logger) : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ILogger<CategoriasController> _logger = logger;

        [HttpGet("pagination")]
        public ActionResult<IEnumerable<CategoriaDTO>> ObterCategoriasPaginado([FromQuery] CategoriasParameters categoriasParameters)
        {
            var categorias = _unitOfWork.CategoriaRepository.ObterCategorias(categoriasParameters);
            var metadata = new
            {
                categorias.TotalCount,
                categorias.PageSize,
                categorias.CurrentPage,
                categorias.TotalPages,
                categorias.HasNext,
                categorias.HasPrevious
            };

            Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));
            return Ok(categorias.ToCategoriaDTOList());
        }

        [HttpGet]
        public ActionResult<IEnumerable<CategoriaDTO>> ObterCategorias()
        {
            var categorias = _unitOfWork.CategoriaRepository.ObterTodos();
            if(categorias == null) return NotFound("Nenhuma categoria encontrada.");

            return Ok(categorias.ToCategoriaDTOList());
        }

        [HttpGet("{id:int:min(1)}", Name = "ObterCategoria")]
        public ActionResult<CategoriaDTO> ObterCategoria(int id)
        {
            var categoria = _unitOfWork.CategoriaRepository.Obter(c => c.Id == id);
            if (categoria == null)
            {
                _logger.LogWarning($"Categoria com id: {id} não encontradaS");
                return NotFound("Categoria não encontrada.");
            }

            return Ok(categoria.ToCategoriaDTO());
        }

        [HttpPost]
        public ActionResult<CategoriaDTO> InserirCategoria(CategoriaDTO dto)
        {
            if (dto == null)
            {
                _logger.LogWarning($"Request inválido: {nameof(CategoriaDTO)}");
                return BadRequest("Informações inválidas.");
            }

            var categoria = dto.ToCategoria();

            var categoriaCriada = _unitOfWork.CategoriaRepository.Inserir(categoria);
            _unitOfWork.Commit();

            var categoriaCriadaDTO = categoriaCriada.ToCategoriaDTO();

            return new CreatedAtRouteResult("ObterCategoria", new { id = categoriaCriadaDTO?.Id }, categoriaCriadaDTO);
        }

        [HttpPut("{id:int:min(1)}")]
        public ActionResult<CategoriaDTO> AlterarCategoria(int id, CategoriaDTO dto)
        {
            if (id != dto.Id)
            {
                _logger.LogWarning("Dados inválidos...");
                return BadRequest("Categoria não encontrada.");
            }

            var categoria = dto.ToCategoria();

            var categoriaAtualizadaDTO = _unitOfWork.CategoriaRepository.Alterar(categoria);
            _unitOfWork.Commit();

            return Ok(categoriaAtualizadaDTO.ToCategoriaDTO());
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
