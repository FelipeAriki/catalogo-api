using APICatalogo.DTOs;
using APICatalogo.DTOs.Mappings;
using APICatalogo.Models;
using APICatalogo.Pagination;
using APICatalogo.Repositories;
using Microsoft.AspNetCore.Authorization;
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
        public async Task<ActionResult<IEnumerable<CategoriaDTO>>> ObterCategoriasPaginado([FromQuery] CategoriasParameters categoriasParameters)
        {
            var categorias = await _unitOfWork.CategoriaRepository.ObterCategoriasAsync(categoriasParameters);
            if (categorias == null || !categorias.Any()) return NotFound("Categorias não encontradas.");
            return ObterCategoriasPaginacao(categorias);
        }
        [HttpGet("filter/nome/pagination")]
        public async Task<ActionResult<IEnumerable<CategoriaDTO>>> ObterProdutosFiltroPrecoPaginado([FromQuery] CategoriasFiltroNome categoriasFiltroNome)
        {
            var categorias = await _unitOfWork.CategoriaRepository.ObterCategoriasFiltroNomeAsync(categoriasFiltroNome);
            if (categorias == null || !categorias.Any()) return NotFound("Categorias não encontradas.");

            return ObterCategoriasPaginacao(categorias);
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoriaDTO>>> ObterCategorias()
        {
            var categorias = await _unitOfWork.CategoriaRepository.ObterTodosAsync();
            if(categorias == null) return NotFound("Nenhuma categoria encontrada.");

            return Ok(categorias.ToCategoriaDTOList());
        }

        [HttpGet("{id:int:min(1)}", Name = "ObterCategoria")]
        public async Task<ActionResult<CategoriaDTO>> ObterCategoria(int id)
        {
            var categoria = await _unitOfWork.CategoriaRepository.ObterAsync(c => c.Id == id);
            if (categoria == null)
            {
                _logger.LogWarning($"Categoria com id: {id} não encontradaS");
                return NotFound("Categoria não encontrada.");
            }

            return Ok(categoria.ToCategoriaDTO());
        }

        [HttpPost]
        public async Task<ActionResult<CategoriaDTO>> InserirCategoria(CategoriaDTO dto)
        {
            if (dto == null)
            {
                _logger.LogWarning($"Request inválido: {nameof(CategoriaDTO)}");
                return BadRequest("Informações inválidas.");
            }

            var categoria = dto.ToCategoria();

            var categoriaCriada = _unitOfWork.CategoriaRepository.Inserir(categoria);
            await _unitOfWork.CommitAsync();

            var categoriaCriadaDTO = categoriaCriada.ToCategoriaDTO();

            return new CreatedAtRouteResult("ObterCategoria", new { id = categoriaCriadaDTO?.Id }, categoriaCriadaDTO);
        }

        [HttpPut("{id:int:min(1)}")]
        public async Task<ActionResult<CategoriaDTO>> AlterarCategoria(int id, CategoriaDTO dto)
        {
            if (id != dto.Id)
            {
                _logger.LogWarning("Dados inválidos...");
                return BadRequest("Categoria não encontrada.");
            }

            var categoria = dto.ToCategoria();

            var categoriaAtualizadaDTO = _unitOfWork.CategoriaRepository.Alterar(categoria);
            await _unitOfWork.CommitAsync();

            return Ok(categoriaAtualizadaDTO.ToCategoriaDTO());
        }

        [HttpDelete("{id:int:min(1)}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<ActionResult<int>> RemoverCategoria(int id)
        {
            var categoria = await _unitOfWork.CategoriaRepository.ObterAsync(c => c.Id == id);

            if (categoria == null)
            {
                _logger.LogWarning($"Categoria com id: {id} não encontradaS");
                return NotFound("Categoria não encontrada");
            }
            var categoriaExcluida = _unitOfWork.CategoriaRepository.Remover(categoria);
            await _unitOfWork.CommitAsync();
            return Ok(categoriaExcluida.Id);
        }

        private ActionResult<IEnumerable<CategoriaDTO>> ObterCategoriasPaginacao(PagedList<Categoria> categorias)
        {
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

    }
}
