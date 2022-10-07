using Microsoft.EntityFrameworkCore.Migrations;

namespace AlgorArt.Data.Migrations
{
    public partial class Halfway2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FundType",
                table: "RequestFunds",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FundType",
                table: "RequestFunds");
        }
    }
}
