using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EvaExchangePlatform.Repository.Migrations
{
    public partial class transactionLogs_date_format_update : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "TransactionDate",
                table: "TransactionLogs",
                type: "datetime(6)",
                nullable: false,
                defaultValueSql: "current_timestamp",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldDefaultValueSql: "current_timestamp")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "TransactionDate",
                table: "TransactionLogs",
                type: "longtext",
                nullable: false,
                defaultValueSql: "current_timestamp",
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValueSql: "current_timestamp")
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
