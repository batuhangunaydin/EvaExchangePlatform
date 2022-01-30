using Microsoft.EntityFrameworkCore.Migrations;

namespace EvaExchangePlatform.Repository.Migrations
{
    public partial class Logs_table_updated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SymbolCode",
                table: "TransactionLogs",
                newName: "ShareCode");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ShareCode",
                table: "TransactionLogs",
                newName: "SymbolCode");
        }
    }
}
