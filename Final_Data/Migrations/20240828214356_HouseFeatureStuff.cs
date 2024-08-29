using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Final_Data.Migrations
{
    /// <inheritdoc />
    public partial class HouseFeatureStuff : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FeatureName",
                table: "HouseFeatures",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FeatureName",
                table: "HouseFeatures");
        }
    }
}
