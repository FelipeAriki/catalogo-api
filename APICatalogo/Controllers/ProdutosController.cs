using APICatalogo.DTOs;
using APICatalogo.DTOs.Mappings;
using APICatalogo.Models;
using APICatalogo.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace APICatalogo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProdutosController(IUnitOfWork unitOfWork) : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        [HttpGet("produtos/{id:int}")]
        public ActionResult<IEnumerable<ProdutoDTO>> ObterProdutosPorCategoria(int id)
        {
            var produtos = _unitOfWork.ProdutoRepository.ObterProdutosPorCategoria(id);
            if (produtos == null || !produtos.Any()) return NotFound("Produtos não encontrados.");
            return Ok(produtos.ToCategoriaDTOList());
        }

        [HttpGet]
        public ActionResult<IEnumerable<ProdutoDTO>> ObterProdutos()
        {
            var produtos = _unitOfWork.ProdutoRepository.ObterTodos();
            if (produtos is null) return NotFound("Nenhum produto encontrado.");

            return Ok(produtos.ToCategoriaDTOList());
        }

        [HttpGet("{id:int:min(1)}", Name = "ObterProduto")]
        public ActionResult<ProdutoDTO> ObterProduto(int id)
        {
                var produtos = _unitOfWork.CategoriaRepository.Obter(c => c.Id == id);
                if (produtos is null)
                    return NotFound("Produto não encontrado.");

                return Ok(produtos.ToCategoriaDTO());
        }

        [HttpPost]
        public ActionResult InserirProduto(ProdutoDTO produtoDTO)
        {
            if (produtoDTO == null)
                return BadRequest("Informações inválidas.");

            var produto = produtoDTO.ToCategoria();

            var produtoCriado = _unitOfWork.ProdutoRepository.Inserir(produto);
            _unitOfWork.Commit();

            var produtoCriadoDTO = produtoCriado.ToCategoriaDTO();

            return new CreatedAtRouteResult("ObterProduto", new { id = produtoCriadoDTO.Id }, produtoCriadoDTO);
        }

        [HttpPut("{id:int:min(1)}")]
        public ActionResult<ProdutoDTO> AlterarProduto(int id, ProdutoDTO produtoDTO)
        {
                if (id != produtoDTO.Id)
                    return BadRequest();

            var produto = produtoDTO.ToCategoria();

            var produtoAtualizadoDTO = _unitOfWork.ProdutoRepository.Alterar(produto);
            _unitOfWork.Commit();

            return Ok(produtoAtualizadoDTO.ToCategoriaDTO());
        }

        [HttpDelete("{id:int:min(1)}")]
        public ActionResult<int> RemoverProduto(int id)
        {
            var produto = _unitOfWork.CategoriaRepository.Obter(p => p.Id == id);
            if (produto is null) return NotFound("Produto não encontrado.");

            var produtoRemovido = _unitOfWork.CategoriaRepository.Remover(produto);
            _unitOfWork.Commit();
            return Ok(produtoRemovido.Id);
        }
    }
}
