using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class update_bid_table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BidDateTime",
                table: "Bids",
                newName: "BidTime");

            migrationBuilder.AddColumn<string>(
                name: "Bidder",
                table: "Bids",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Bidder",
                table: "Bids");

            migrationBuilder.RenameColumn(
                name: "BidTime",
                table: "Bids",
                newName: "BidDateTime");
        }
    }
}
