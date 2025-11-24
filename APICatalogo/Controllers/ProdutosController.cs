using APICatalogo.DTOs;
using APICatalogo.DTOs.Mappings;
using APICatalogo.Models;
using APICatalogo.Pagination;
using APICatalogo.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace APICatalogo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[EnableRateLimiting("fixedwindow")]
    public class ProdutosController(IUnitOfWork unitOfWork) : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        [HttpGet("produtos/{id:int}")]
        public async Task<ActionResult<IEnumerable<ProdutoDTO>>> ObterProdutosPorCategoriaAsync(int id)
        {
            var produtos = await _unitOfWork.ProdutoRepository.ObterProdutosPorCategoriaAsync(id);
            if (produtos == null || !produtos.Any()) return NotFound("Produtos não encontrados.");
            return Ok(produtos.ToCategoriaDTOList());
        }

        [HttpGet("pagination")]
        public async Task<ActionResult<IEnumerable<ProdutoDTO>>> ObterProdutosPaginadoAsync([FromQuery] ProdutosParameters produtosParameters)
        {
            var produtos = await _unitOfWork.ProdutoRepository.ObterProdutosAsync(produtosParameters);
            if (produtos == null || !produtos.Any()) return NotFound("Produtos não encontrados.");
            
            return ObterProdutosPaginacao(produtos);
        }

        [HttpGet("filter/preco/pagination")]
        public async Task<ActionResult<IEnumerable<ProdutoDTO>>> ObterProdutosFiltroPrecoPaginado([FromQuery] ProdutosFiltroPreco produtosFiltroPreco)
        {
            var produtos = await _unitOfWork.ProdutoRepository.ObterProdutosFiltroPrecoAsync(produtosFiltroPreco);
            if (produtos == null || !produtos.Any()) return NotFound("Produtos não encontrados.");

            return ObterProdutosPaginacao(produtos);
        }

        [HttpGet]
        [Authorize(Policy = "UserOnly")]
        public async Task<ActionResult<IEnumerable<ProdutoDTO>>> ObterProdutos()
        {
            var produtos = await _unitOfWork.ProdutoRepository.ObterTodosAsync();
            if (produtos is null) return NotFound("Nenhum produto encontrado.");

            return Ok(produtos.ToCategoriaDTOList());
        }

        [HttpGet("{id:int:min(1)}", Name = "ObterProduto")]
        public async Task<ActionResult<ProdutoDTO>> ObterProduto(int id)
        {
                var produtos = await _unitOfWork.CategoriaRepository.ObterAsync(c => c.Id == id);
                if (produtos is null)
                    return NotFound("Produto não encontrado.");

                return Ok(produtos.ToCategoriaDTO());
        }

        [HttpPost]
        public async Task<ActionResult<ProdutoDTO>> InserirProduto(ProdutoDTO produtoDTO)
        {
            if (produtoDTO == null)
                return BadRequest("Informações inválidas.");

            var produto = produtoDTO.ToCategoria();

            var produtoCriado = _unitOfWork.ProdutoRepository.Inserir(produto);
            await _unitOfWork.CommitAsync();

            var produtoCriadoDTO = produtoCriado.ToCategoriaDTO();

            return new CreatedAtRouteResult("ObterProduto", new { id = produtoCriadoDTO?.Id }, produtoCriadoDTO);
        }

        [HttpPut("{id:int:min(1)}")]
        public async Task<ActionResult<ProdutoDTO>> AlterarProduto(int id, ProdutoDTO produtoDTO)
        {
                if (id != produtoDTO.Id)
                    return BadRequest();

            var produto = produtoDTO.ToCategoria();

            var produtoAtualizadoDTO = _unitOfWork.ProdutoRepository.Alterar(produto);
            await _unitOfWork.CommitAsync();

            return Ok(produtoAtualizadoDTO.ToCategoriaDTO());
        }

        [HttpDelete("{id:int:min(1)}")]
        public async Task<ActionResult<int>> RemoverProduto(int id)
        {
            var produto = await _unitOfWork.CategoriaRepository.ObterAsync(p => p.Id == id);
            if (produto is null) return NotFound("Produto não encontrado.");

            var produtoRemovido = _unitOfWork.CategoriaRepository.Remover(produto);
            await _unitOfWork.CommitAsync();
            return Ok(produtoRemovido.Id);
        }

        private ActionResult<IEnumerable<ProdutoDTO>> ObterProdutosPaginacao(PagedList<Produto> produtos)
        {
            var metadata = new
            {
                produtos.TotalCount,
                produtos.PageSize,
                produtos.CurrentPage,
                produtos.TotalPages,
                produtos.HasNext,
                produtos.HasPrevious
            };

            Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));
            return Ok(produtos.ToCategoriaDTOList());
        }
    }
}
