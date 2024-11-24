using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LunaVerde.Migrations
{
    /// <inheritdoc />
    public partial class InitSync : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderItems");

            migrationBuilder.DropTable(
                name: "Menu");

            migrationBuilder.DropTable(
                name: "Orders");
        }
    }
}
