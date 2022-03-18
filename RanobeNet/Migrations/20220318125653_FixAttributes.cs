using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RanobeNet.Migrations
{
    public partial class FixAttributes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NovelAttribute_Episodes_EpisodeId",
                table: "NovelAttribute");

            migrationBuilder.DropForeignKey(
                name: "FK_NovelAttribute_Novels_NovelId",
                table: "NovelAttribute");

            migrationBuilder.DropPrimaryKey(
                name: "PK_NovelAttribute",
                table: "NovelAttribute");

            migrationBuilder.RenameTable(
                name: "NovelAttribute",
                newName: "NovelAttributes");

            migrationBuilder.RenameIndex(
                name: "IX_NovelAttribute_NovelId",
                table: "NovelAttributes",
                newName: "IX_NovelAttributes_NovelId");

            migrationBuilder.RenameIndex(
                name: "IX_NovelAttribute_EpisodeId",
                table: "NovelAttributes",
                newName: "IX_NovelAttributes_EpisodeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_NovelAttributes",
                table: "NovelAttributes",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_NovelAttributes_Episodes_EpisodeId",
                table: "NovelAttributes",
                column: "EpisodeId",
                principalTable: "Episodes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_NovelAttributes_Novels_NovelId",
                table: "NovelAttributes",
                column: "NovelId",
                principalTable: "Novels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NovelAttributes_Episodes_EpisodeId",
                table: "NovelAttributes");

            migrationBuilder.DropForeignKey(
                name: "FK_NovelAttributes_Novels_NovelId",
                table: "NovelAttributes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_NovelAttributes",
                table: "NovelAttributes");

            migrationBuilder.RenameTable(
                name: "NovelAttributes",
                newName: "NovelAttribute");

            migrationBuilder.RenameIndex(
                name: "IX_NovelAttributes_NovelId",
                table: "NovelAttribute",
                newName: "IX_NovelAttribute_NovelId");

            migrationBuilder.RenameIndex(
                name: "IX_NovelAttributes_EpisodeId",
                table: "NovelAttribute",
                newName: "IX_NovelAttribute_EpisodeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_NovelAttribute",
                table: "NovelAttribute",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_NovelAttribute_Episodes_EpisodeId",
                table: "NovelAttribute",
                column: "EpisodeId",
                principalTable: "Episodes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_NovelAttribute_Novels_NovelId",
                table: "NovelAttribute",
                column: "NovelId",
                principalTable: "Novels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
