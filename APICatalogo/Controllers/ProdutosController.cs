using APICatalogo.DTOs;
using APICatalogo.Models;
using APICatalogo.Pagination;
using APICatalogo.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;

namespace APICatalogo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    //[EnableRateLimiting("fixedwindow")]
    public class ProdutosController(IUnitOfWork unitOfWork, IMapper mapper, IMemoryCache memoryCache) : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly IMemoryCache _memoryCache = memoryCache;
        private const string CacheProdutosKey = "CacheProdutos";
        private const string CacheProdutosCategoriaPrefixo = "CacheProdutosCategoria_";

        [HttpGet("produtos/{id:int}")]
        public async Task<ActionResult<IEnumerable<ProdutoDTO>>> ObterProdutosPorCategoriaAsync(int id)
        {
            var cacheKey = ObterProdutoCacheKey(id);
            if (!_memoryCache.TryGetValue(cacheKey, out IEnumerable<Produto>? produtos))
            {
                produtos = await _unitOfWork.ProdutoRepository.ObterProdutosPorCategoriaAsync(id);
                if (produtos == null || !produtos.Any()) return NotFound("Produtos não encontrados.");
                SetCache(cacheKey, produtos);
            }
            return Ok(_mapper.Map<IEnumerable<ProdutoDTO>>(produtos));
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

        /// <summary>
        /// Obtém uma lista de objetos Produto
        /// </summary>
        /// <returns>Uma lista de objetos Produto</returns>
        [HttpGet]
        [Authorize(Policy = "UserOnly")]
        public async Task<ActionResult<IEnumerable<ProdutoDTO>>> ObterProdutos()
        {
            if (!_memoryCache.TryGetValue(CacheProdutosKey, out IEnumerable<Produto>? produtos))
            {
                produtos = await _unitOfWork.ProdutoRepository.ObterTodosAsync();
                if (produtos is null || !produtos.Any()) return NotFound("Nenhum produto encontrado.");
                SetCache(CacheProdutosKey, produtos);
            }

            return Ok(_mapper.Map<IEnumerable<ProdutoDTO>>(produtos));
        }

        /// <summary>
        /// Obtém um Produto pelo seu ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Objeto Produto</returns>
        [HttpGet("{id:int:min(1)}", Name = "ObterProduto")]
        public async Task<ActionResult<ProdutoDTO>> ObterProduto(int id)
        {
            var cacheKey = ObterProdutoCacheKey(id);
            if (!_memoryCache.TryGetValue(cacheKey, out Produto? produto))
            {
                produto = await _unitOfWork.ProdutoRepository.ObterAsync(c => c.Id == id);
                if (produto is null)
                    return NotFound("Produto não encontrado.");
            }
            return Ok(_mapper.Map<IEnumerable<ProdutoDTO>>(produto));
        }

        [HttpPost]
        public async Task<ActionResult<ProdutoDTO>> InserirProduto(ProdutoDTO produtoDTO)
        {
            if (produtoDTO == null)
                return BadRequest("Informações inválidas.");

            var produto = _mapper.Map<Produto>(produtoDTO);

            var produtoCriado = _unitOfWork.ProdutoRepository.Inserir(produto);
            await _unitOfWork.CommitAsync();

            InvalidateCacheAfterChange(produtoCriado.Id, produtoCriado);

            var produtoCriadoDTO = _mapper.Map<ProdutoDTO>(produtoCriado);

            return new CreatedAtRouteResult("ObterProduto", new { id = produtoCriadoDTO?.Id }, produtoCriadoDTO);
        }

        [HttpPut("{id:int:min(1)}")]
        public async Task<ActionResult<ProdutoDTO>> AlterarProduto(int id, ProdutoDTO produtoDTO)
        {
                if (id != produtoDTO.Id)
                    return BadRequest();

            var produto = _mapper.Map<Produto>(produtoDTO);

            var produtoAtualizadoDTO = _unitOfWork.ProdutoRepository.Alterar(produto);
            await _unitOfWork.CommitAsync();

            InvalidateCacheAfterChange(id, produtoAtualizadoDTO);

            return Ok(_mapper.Map<ProdutoDTO>(produtoAtualizadoDTO));
        }

        [HttpDelete("{id:int:min(1)}")]
        public async Task<ActionResult<int>> RemoverProduto(int id)
        {
            var produto = await _unitOfWork.ProdutoRepository.ObterAsync(p => p.Id == id);
            if (produto is null) return NotFound("Produto não encontrado.");

            var produtoRemovido = _unitOfWork.ProdutoRepository.Remover(produto);
            await _unitOfWork.CommitAsync();

            InvalidateCacheAfterChange(id);

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
            return Ok(_mapper.Map<ProdutoDTO>(produtos));
        }

        private string ObterProdutoCacheKey(int id) => $"CacheProduto_{id}";
        private string ObterProdutosCategoriaCacheKey(int categoriaId) => $"{CacheProdutosCategoriaPrefixo}{categoriaId}";

        private void SetCache<T>(string key, T data)
        {
            var cacheOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30),
                SlidingExpiration = TimeSpan.FromSeconds(15),
                Priority = CacheItemPriority.High
            };
            _memoryCache.Set(key, data, cacheOptions);
        }

        private void InvalidateCacheAfterChange(int id, Produto? produto = null)
        {
            _memoryCache.Remove(CacheProdutosKey);
            _memoryCache.Remove(ObterProdutoCacheKey(id));

            if (produto != null)
            {
                _memoryCache.Remove(ObterProdutosCategoriaCacheKey(produto.CategoriaId));
                SetCache(ObterProdutoCacheKey(id), produto);
            }
        }
    }
}
