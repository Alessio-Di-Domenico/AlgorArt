using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AlgorArt.Data.Migrations
{
    public partial class HalfWay : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "FundImage",
                table: "RequestFunds",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FundImage",
                table: "RequestFunds");
        }
    }
}
