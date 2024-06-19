using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class Update_Primary_Key_For_Grant_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Grants",
                table: "Grants");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "Grants",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Grants",
                table: "Grants",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Grants",
                table: "Grants");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Grants");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Grants",
                table: "Grants",
                column: "Key");
        }
    }
}
