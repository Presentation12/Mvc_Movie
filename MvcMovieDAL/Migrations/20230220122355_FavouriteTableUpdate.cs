using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MvcMovieDAL.Migrations
{
    /// <inheritdoc />
    public partial class FavouriteTableUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Active",
                table: "Favourite",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                table: "Favourite",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Favourite",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "UpdatedBy",
                table: "Favourite",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "Favourite",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Active",
                table: "Favourite");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Favourite");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Favourite");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Favourite");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "Favourite");
        }
    }
}
