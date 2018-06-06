using Microsoft.EntityFrameworkCore.Migrations;

namespace Blog.Migrations
{
    public partial class SmallStructureChange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Evaluation_Comment_PostId",
                table: "Evaluation");

            migrationBuilder.RenameColumn(
                name: "PostId",
                table: "Evaluation",
                newName: "CommentId");

            migrationBuilder.RenameIndex(
                name: "IX_Evaluation_PostId",
                table: "Evaluation",
                newName: "IX_Evaluation_CommentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Evaluation_Comment_CommentId",
                table: "Evaluation",
                column: "CommentId",
                principalTable: "Comment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Evaluation_Comment_CommentId",
                table: "Evaluation");

            migrationBuilder.RenameColumn(
                name: "CommentId",
                table: "Evaluation",
                newName: "PostId");

            migrationBuilder.RenameIndex(
                name: "IX_Evaluation_CommentId",
                table: "Evaluation",
                newName: "IX_Evaluation_PostId");

            migrationBuilder.AddForeignKey(
                name: "FK_Evaluation_Comment_PostId",
                table: "Evaluation",
                column: "PostId",
                principalTable: "Comment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
