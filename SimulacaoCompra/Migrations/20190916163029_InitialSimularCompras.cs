using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace SimulacaoCompra.Migrations
{
    public partial class InitialSimularCompras : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Compras",
                columns: table => new
                {
                    Idcompra = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Datacompra = table.Column<DateTime>(nullable: false),
                    Qtdparcelas = table.Column<decimal>(type: "Decimal(10,5)", nullable: false),
                    TipoCalculo = table.Column<string>(nullable: true),
                    Valorjuros = table.Column<decimal>(type: "decimal(10, 4)", nullable: false),
                    Valorparcela = table.Column<decimal>(type: "decimal(10, 2)", nullable: true),
                    Valortotal = table.Column<decimal>(type: "decimal(10, 2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Compras", x => x.Idcompra);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Compras");
        }
    }
}
