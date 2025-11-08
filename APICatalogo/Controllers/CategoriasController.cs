using APICatalogo.DTOs;
using APICatalogo.Models;
using APICatalogo.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace APICatalogo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriasController(IUnitOfWork unitOfWork, ILogger<CategoriasController> logger) : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ILogger<CategoriasController> _logger = logger;

        [HttpGet]
        public ActionResult<IEnumerable<CategoriaDTO>> ObterCategorias()
        {
            var categorias = _unitOfWork.CategoriaRepository.ObterTodos();
            if(categorias == null) return NotFound("Nenhuma categoria encontrada.");

            var categoriasDTO = new List<CategoriaDTO>();
            foreach (var categoria in categorias)
            {
                var categoriaDTO = new CategoriaDTO()
                {
                    Id = categoria.Id,
                    Nome = categoria.Nome,
                    ImagemUrl = categoria.ImagemUrl,
                };
                categoriasDTO.Add(categoriaDTO);
            }
            return Ok(categoriasDTO);
        }

        [HttpGet("{id:int:min(1)}", Name = "ObterCategoria")]
        public ActionResult<CategoriaDTO> ObterCategoria(int id)
        {
            var categoria = _unitOfWork.CategoriaRepository.Obter(c => c.Id == id);
            if (categoria == null)
            {
                _logger.LogWarning($"Categoria com id: {id} não encontradaS");
                return NotFound("Categoria não encontrada...");
            }

            return Ok(new CategoriaDTO()
            {
                Id = categoria.Id,
                Nome = categoria.Nome,
                ImagemUrl = categoria.ImagemUrl,
            });
        }

        [HttpPost]
        public ActionResult<CategoriaDTO> InserirCategoria(CategoriaDTO dto)
        {
            if (dto == null)
            {
                _logger.LogWarning($"Request inválido: {nameof(CategoriaDTO)}");
                return BadRequest("Informações inválidas.");
            }

            var categoria = new Categoria()
            {
                Id = dto.Id,
                Nome = dto.Nome,
                ImagemUrl = dto.ImagemUrl,
            };

            var categoriaCriada = _unitOfWork.CategoriaRepository.Inserir(categoria);
            _unitOfWork.Commit();

            var categoriaCriadaDTO = new CategoriaDTO()
            {
                Id = categoriaCriada.Id,
                Nome = categoriaCriada.Nome,
                ImagemUrl = categoriaCriada.ImagemUrl,
            };

            return new CreatedAtRouteResult("ObterCategoria", new { id = categoriaCriadaDTO.Id }, categoriaCriadaDTO);
        }

        [HttpPut("{id:int:min(1)}")]
        public ActionResult<CategoriaDTO> AlterarCategoria(int id, CategoriaDTO dto)
        {
            if (id != dto.Id)
            {
                _logger.LogWarning("Dados inválidos...");
                return BadRequest("Categoria não encontrada.");
            }

            var categoria = new Categoria()
            {
                Id = dto.Id,
                Nome = dto.Nome,
                ImagemUrl = dto.ImagemUrl,
            };

            var categoriaAtualizadaDTO = _unitOfWork.CategoriaRepository.Alterar(categoria);
            _unitOfWork.Commit();

            var categoriaDTO = new CategoriaDTO()
            {
                Id = categoriaAtualizadaDTO.Id,
                Nome = categoriaAtualizadaDTO.Nome,
                ImagemUrl = categoriaAtualizadaDTO.ImagemUrl,
            };
            return Ok(categoriaDTO);
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
