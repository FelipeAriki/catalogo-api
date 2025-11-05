using APICatalogo.Models;
using APICatalogo.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace APICatalogo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProdutosController(/*IRepository<Produto> repository,*/ IProdutoRepository produtoRepository) : ControllerBase
    {
        //private readonly IRepository<Produto> _repository = repository;
        private readonly IProdutoRepository _repository = produtoRepository;

        [HttpGet("produtos/{id:int}")]
        public ActionResult<IEnumerable<Produto>> ObterProdutosPorCategoria(int id)
        {
            var produtos = _repository.ObterProdutosPorCategoria(id);
            if (produtos == null) return NotFound("Produtos não encontrados.");
            return Ok(produtos);
        }

        [HttpGet]
        public ActionResult<IEnumerable<Produto>> ObterProdutos()
        {
                var produtos = _repository.ObterTodos();
                if (produtos is null)
                    return NotFound();
                return Ok(produtos);
        }

        [HttpGet("{id:int:min(1)}", Name = "ObterProduto")]
        public ActionResult<Produto> ObterProduto(int id)
        {
                var produtos = _repository.Obter(c => c.Id == id);
                if (produtos is null)
                    return NotFound("Produto não encontrado!");
                return Ok(produtos);
        }

        [HttpPost]
        public ActionResult InserirProduto(Produto produto)
        {
                if (produto == null)
                    return BadRequest();

            var novoProduto = _repository.Inserir(produto);
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

            var produtoAtualizado = _repository.Alterar(produto);
            return Ok(produtoAtualizado);
        }

        [HttpDelete("{id:int:min(1)}")]
        public ActionResult RemoverProduto(int id)
        {
            var produto = _repository.Obter(p => p.Id == id);
            if (produto is null) return NotFound("Produto não encontrado.");

            return Ok(_repository.Remover(produto));
        }
    }
}
