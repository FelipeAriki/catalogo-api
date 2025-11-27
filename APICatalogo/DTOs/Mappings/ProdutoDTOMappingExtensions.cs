using APICatalogo.Models;

namespace APICatalogo.DTOs.Mappings
{
    public static class ProdutoDTOMappingExtensions
    {
        public static ProdutoDTO? ToProdutoDTO(this Produto produto)
        {
            if (produto == null) return null;

            return new ProdutoDTO()
            {
                Id = produto.Id,
                Nome = produto.Nome,
                Descricao = produto.Descricao,
                Preco = produto.Preco,
                ImagemUrl = produto.ImagemUrl,
                CategoriaId = produto.CategoriaId,
            };
        }

        public static Produto? ToProduto(this ProdutoDTO produtoDTO)
        {
            if (produtoDTO == null) return null;

            return new Produto()
            {
                Id = produtoDTO.Id,
                Nome = produtoDTO.Nome,
                Descricao = produtoDTO.Descricao,
                Preco = produtoDTO.Preco,
                ImagemUrl = produtoDTO.ImagemUrl,
                CategoriaId = produtoDTO.CategoriaId,
            };
        }

        public static IEnumerable<ProdutoDTO> ToProdutoDTOList(this IEnumerable<Produto> produtos)
        {
            if (produtos == null || !produtos.Any()) return new List<ProdutoDTO>();

            return produtos.Select(produto => new ProdutoDTO
            {
                Id = produto.Id,
                Nome = produto.Nome,
                Descricao = produto.Descricao,
                Preco = produto.Preco,
                ImagemUrl = produto.ImagemUrl,
                CategoriaId = produto.CategoriaId,
            }).ToList();
        }
    }
}
