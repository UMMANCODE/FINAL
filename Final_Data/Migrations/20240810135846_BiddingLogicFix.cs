using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Final_Data.Migrations
{
    /// <inheritdoc />
    public partial class BiddingLogicFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "No",
                table: "Bids");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "No",
                table: "Bids",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
