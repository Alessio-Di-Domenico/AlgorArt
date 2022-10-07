using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AlgorArt.Data.Migrations
{
    public partial class HalfwayN : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FundImage",
                table: "RequestFunds");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "FundImage",
                table: "RequestFunds",
                type: "varbinary(max)",
                nullable: true);
        }
    }
}
