using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Common.Migrations
{
    public partial class GenreKeyFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MovieGenre_Genres_MovieId",
                table: "MovieGenre");

            migrationBuilder.CreateIndex(
                name: "IX_MovieGenre_GenreId",
                table: "MovieGenre",
                column: "GenreId");

            migrationBuilder.AddForeignKey(
                name: "FK_MovieGenre_Genres_GenreId",
                table: "MovieGenre",
                column: "GenreId",
                principalTable: "Genres",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MovieGenre_Genres_GenreId",
                table: "MovieGenre");

            migrationBuilder.DropIndex(
                name: "IX_MovieGenre_GenreId",
                table: "MovieGenre");

            migrationBuilder.AddForeignKey(
                name: "FK_MovieGenre_Genres_MovieId",
                table: "MovieGenre",
                column: "MovieId",
                principalTable: "Genres",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
