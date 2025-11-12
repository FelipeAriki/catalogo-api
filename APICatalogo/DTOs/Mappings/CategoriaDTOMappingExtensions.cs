using APICatalogo.Models;

namespace APICatalogo.DTOs.Mappings
{
    public static class CategoriaDTOMappingExtensions
    {
        public static CategoriaDTO? ToCategoriaDTO(this Categoria categoria)
        {
            if (categoria == null) return null;

            return new CategoriaDTO()
            {
                Id = categoria.Id,
                Nome = categoria.Nome,
                ImagemUrl = categoria.ImagemUrl,
            };
        }

        public static Categoria? ToCategoria(this CategoriaDTO categoriaDTO)
        {
            if (categoriaDTO == null) return null;

            return new Categoria()
            {
                Id = categoriaDTO.Id,
                Nome = categoriaDTO.Nome,
                ImagemUrl = categoriaDTO.ImagemUrl,
            };
        }

        public static IEnumerable<CategoriaDTO> ToCategoriaDTOList(this IEnumerable<Categoria> categorias)
        {
            if(categorias == null || !categorias.Any()) return new List<CategoriaDTO>();

            return categorias.Select(categoria => new CategoriaDTO
            {
                Id = categoria.Id,
                Nome = categoria.Nome,
                ImagemUrl = categoria.ImagemUrl,
            }).ToList();
        }
    }
}
