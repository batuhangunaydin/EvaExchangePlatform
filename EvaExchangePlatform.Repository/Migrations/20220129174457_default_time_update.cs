using Microsoft.EntityFrameworkCore.Migrations;

namespace EvaExchangePlatform.Repository.Migrations
{
    public partial class default_time_update : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "TransactionDate",
                table: "TransactionLogs",
                type: "longtext",
                nullable: false,
                defaultValueSql: "current_timestamp",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldDefaultValue: "01/29/2022 20:43:39")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "TransactionDate",
                table: "TransactionLogs",
                type: "longtext",
                nullable: false,
                defaultValue: "01/29/2022 20:43:39",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldDefaultValueSql: "current_timestamp")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }
    }
}
