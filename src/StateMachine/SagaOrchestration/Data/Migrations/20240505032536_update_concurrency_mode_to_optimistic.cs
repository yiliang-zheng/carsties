using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SagaOrchestration.Data.Migrations
{
    /// <inheritdoc />
    public partial class update_concurrency_mode_to_optimistic : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<uint>(
                name: "xmin",
                table: "FinishAuctionStateInstance",
                type: "xid",
                rowVersion: true,
                nullable: false,
                defaultValue: 0u);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "xmin",
                table: "FinishAuctionStateInstance");
        }
    }
}
