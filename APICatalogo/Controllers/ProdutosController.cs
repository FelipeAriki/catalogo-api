using APICatalogo.Models;
using APICatalogo.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace APICatalogo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProdutosController(IProdutoRepository produtoRepository) : ControllerBase
    {
        private readonly IProdutoRepository _repository = produtoRepository;

        [HttpGet]
        public ActionResult<IEnumerable<Produto>> ObterProdutos()
        {
                var produtos = _repository.ObterProdutos().ToList();
                if (produtos is null)
                    return NotFound();
                return Ok(produtos);
        }

        [HttpGet("{id:int:min(1)}", Name = "ObterProduto")]
        public ActionResult<Produto> ObterProduto(int id)
        {
                var produtos = _repository.ObterProduto(id);
                if (produtos is null)
                    return NotFound("Produto não encontrado!");
                return Ok(produtos);
        }

        [HttpPost]
        public ActionResult InserirProduto(Produto produto)
        {
                if (produto == null)
                    return BadRequest();

            var novoProduto = _repository.InserirProduto(produto);
            return new CreatedAtRouteResult("ObterProduto", new { id = novoProduto.Id }, novoProduto);
        }

        /*
         Antes de existir o [ApiController] era necessário fazer o endpoint dessa maneira
         [HttpPost]
        public ActionResult Post([FromBody] Produto produto)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
        }
         */

        [HttpPut("{id:int:min(1)}")]
        public ActionResult AlterarProduto(int id, Produto produto)
        {
                if (id != produto.Id)
                    return BadRequest();

            bool sucesso = _repository.AlterarProduto(produto);
            if (sucesso) return Ok(produto);
            return StatusCode(500, $"Falha ao atualizar o produto de id: {id}");
        }

        [HttpDelete("{id:int:min(1)}")]
        public ActionResult RemoverProduto(int id)
        {
            bool sucesso = _repository.RemoverProduto(id);
            if (sucesso) return Ok($"Produto de id: {id} foi excluído com sucesso!");
            return StatusCode(500, $"Falha ao remover o produto de id: {id}");
        }
    }
}
