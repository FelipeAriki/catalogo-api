using APICatalogo.Context;
using APICatalogo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProdutosController(AppDbContext appDbContext) : ControllerBase
    {
        private readonly AppDbContext _appDbContext = appDbContext;

        [HttpGet]
        public ActionResult<IEnumerable<Produto>> ObterProdutos()
        {
                var produtos = _appDbContext.Produto?.AsNoTracking().ToList();
                if (produtos is null)
                    return NotFound();
                return Ok(produtos);
        }

        [HttpGet("{id:int:min(1)}", Name = "ObterProduto")]
        public ActionResult<Produto> ObterProduto(int id)
        {
                var produtos = _appDbContext.Produto?.AsNoTracking().FirstOrDefault(p => p.Id == id);
                if (produtos is null)
                    return NotFound("Produto não encontrado!");
                return Ok(produtos);
        }

        [HttpPost]
        public ActionResult InserirProduto(Produto produto)
        {
                if (produto == null)
                    return BadRequest();

                _appDbContext.Add(produto);
                _appDbContext.SaveChanges();
                return new CreatedAtRouteResult("ObterProduto", new { id = produto.Id }, produto);
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

                _appDbContext.Entry(produto).State = EntityState.Modified;
                _appDbContext.SaveChanges();

                return Ok(produto);
        }

        [HttpDelete("{id:int:min(1)}")]
        public ActionResult RemoverProduto(int id)
        {
                var produto = _appDbContext.Produto?.FirstOrDefault(p => p.Id == id);
                //var produto = _appDbContext.Produto.Find(id);

                if (produto == null)
                    return NotFound("Produto não encontrado.");

                _appDbContext.Remove(produto);
                _appDbContext.SaveChanges();

                return Ok();
        }
    }
}
