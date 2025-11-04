using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APICatalogo.Migrations
{
    /// <inheritdoc />
    public partial class PopulaProdutos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder mb)
        {
            mb.Sql("INSERT INTO produtos(Nome, Descricao, Preco, ImagemUrl, Estoque, DataCadastro, CategoriaId) VALUES('Coca-cola diet', 'Refrigerante de cola 350ml', 5.45, 'cocacola.jpg', 50, now(), 1)");
            mb.Sql("INSERT INTO produtos(Nome, Descricao, Preco, ImagemUrl, Estoque, DataCadastro, CategoriaId) VALUES('Guaraná diet', 'Refrigerante 350ml', 5.45, 'guaraná.jpg', 50, now(), 1)");
            mb.Sql("INSERT INTO produtos(Nome, Descricao, Preco, ImagemUrl, Estoque, DataCadastro, CategoriaId) VALUES('Pudim', 'docinho bom', 5.45, 'pudim.jpg', 50, now(), 3)");
            mb.Sql("INSERT INTO produtos(Nome, Descricao, Preco, ImagemUrl, Estoque, DataCadastro, CategoriaId) VALUES('x-tudo', 'lanche saboroso', 5.45, 'xtudo.jpg', 50, now(), 2)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder mb)
        {
            mb.Sql("DELE FROM produtos");
        }
    }
}
