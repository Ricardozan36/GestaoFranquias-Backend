using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Franquias.Api.Migrations
{
    /// <inheritdoc />
    public partial class AdicionaChamadosERoyalties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Estoques_ProdutosServicos_ProdutoServicoId",
                table: "Estoques");

            migrationBuilder.DropForeignKey(
                name: "FK_ItensVenda_ProdutosServicos_ProdutoServicoId",
                table: "ItensVenda");

            migrationBuilder.DropForeignKey(
                name: "FK_ItensVenda_Vendas_VendaId",
                table: "ItensVenda");

            migrationBuilder.DropIndex(
                name: "IX_Usuarios_Email",
                table: "Usuarios");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProdutosServicos",
                table: "ProdutosServicos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ItensVenda",
                table: "ItensVenda");

            migrationBuilder.DropColumn(
                name: "AnoRef",
                table: "Royalties");

            migrationBuilder.RenameTable(
                name: "ProdutosServicos",
                newName: "Produtos");

            migrationBuilder.RenameTable(
                name: "ItensVenda",
                newName: "ItemVenda");

            migrationBuilder.RenameColumn(
                name: "Pago",
                table: "Royalties",
                newName: "Mes");

            migrationBuilder.RenameColumn(
                name: "MesRef",
                table: "Royalties",
                newName: "Ano");

            migrationBuilder.RenameColumn(
                name: "FaturamentoBase",
                table: "Royalties",
                newName: "ValorFaturamento");

            migrationBuilder.RenameColumn(
                name: "DataFechamento",
                table: "Chamados",
                newName: "RespostaFranqueadora");

            migrationBuilder.RenameIndex(
                name: "IX_ItensVenda_VendaId",
                table: "ItemVenda",
                newName: "IX_ItemVenda_VendaId");

            migrationBuilder.RenameIndex(
                name: "IX_ItensVenda_ProdutoServicoId",
                table: "ItemVenda",
                newName: "IX_ItemVenda_ProdutoServicoId");

            migrationBuilder.AddColumn<DateTime>(
                name: "DataGeracao",
                table: "Royalties",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DataPagamento",
                table: "Royalties",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DataVencimento",
                table: "Royalties",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<decimal>(
                name: "PercentualAplicado",
                table: "Royalties",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "StatusPagamento",
                table: "Royalties",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Chamados",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddColumn<DateTime>(
                name: "DataEncerramento",
                table: "Chamados",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Produtos",
                table: "Produtos",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ItemVenda",
                table: "ItemVenda",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Estoques_Produtos_ProdutoServicoId",
                table: "Estoques",
                column: "ProdutoServicoId",
                principalTable: "Produtos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ItemVenda_Produtos_ProdutoServicoId",
                table: "ItemVenda",
                column: "ProdutoServicoId",
                principalTable: "Produtos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ItemVenda_Vendas_VendaId",
                table: "ItemVenda",
                column: "VendaId",
                principalTable: "Vendas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Estoques_Produtos_ProdutoServicoId",
                table: "Estoques");

            migrationBuilder.DropForeignKey(
                name: "FK_ItemVenda_Produtos_ProdutoServicoId",
                table: "ItemVenda");

            migrationBuilder.DropForeignKey(
                name: "FK_ItemVenda_Vendas_VendaId",
                table: "ItemVenda");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Produtos",
                table: "Produtos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ItemVenda",
                table: "ItemVenda");

            migrationBuilder.DropColumn(
                name: "DataGeracao",
                table: "Royalties");

            migrationBuilder.DropColumn(
                name: "DataPagamento",
                table: "Royalties");

            migrationBuilder.DropColumn(
                name: "DataVencimento",
                table: "Royalties");

            migrationBuilder.DropColumn(
                name: "PercentualAplicado",
                table: "Royalties");

            migrationBuilder.DropColumn(
                name: "StatusPagamento",
                table: "Royalties");

            migrationBuilder.DropColumn(
                name: "DataEncerramento",
                table: "Chamados");

            migrationBuilder.RenameTable(
                name: "Produtos",
                newName: "ProdutosServicos");

            migrationBuilder.RenameTable(
                name: "ItemVenda",
                newName: "ItensVenda");

            migrationBuilder.RenameColumn(
                name: "ValorFaturamento",
                table: "Royalties",
                newName: "FaturamentoBase");

            migrationBuilder.RenameColumn(
                name: "Mes",
                table: "Royalties",
                newName: "Pago");

            migrationBuilder.RenameColumn(
                name: "Ano",
                table: "Royalties",
                newName: "MesRef");

            migrationBuilder.RenameColumn(
                name: "RespostaFranqueadora",
                table: "Chamados",
                newName: "DataFechamento");

            migrationBuilder.RenameIndex(
                name: "IX_ItemVenda_VendaId",
                table: "ItensVenda",
                newName: "IX_ItensVenda_VendaId");

            migrationBuilder.RenameIndex(
                name: "IX_ItemVenda_ProdutoServicoId",
                table: "ItensVenda",
                newName: "IX_ItensVenda_ProdutoServicoId");

            migrationBuilder.AddColumn<int>(
                name: "AnoRef",
                table: "Royalties",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "Chamados",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProdutosServicos",
                table: "ProdutosServicos",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ItensVenda",
                table: "ItensVenda",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_Email",
                table: "Usuarios",
                column: "Email",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Estoques_ProdutosServicos_ProdutoServicoId",
                table: "Estoques",
                column: "ProdutoServicoId",
                principalTable: "ProdutosServicos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ItensVenda_ProdutosServicos_ProdutoServicoId",
                table: "ItensVenda",
                column: "ProdutoServicoId",
                principalTable: "ProdutosServicos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ItensVenda_Vendas_VendaId",
                table: "ItensVenda",
                column: "VendaId",
                principalTable: "Vendas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
