using Microsoft.EntityFrameworkCore.Migrations;

namespace Blog.Migrations
{
    public partial class RemoveEvaluatedOn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Evaluation_User_EvaluatedOnId",
                table: "Evaluation");

            migrationBuilder.DropIndex(
                name: "IX_Evaluation_EvaluatedOnId",
                table: "Evaluation");

            migrationBuilder.DropColumn(
                name: "EvaluatedOnId",
                table: "Evaluation");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EvaluatedOnId",
                table: "Evaluation",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Evaluation_EvaluatedOnId",
                table: "Evaluation",
                column: "EvaluatedOnId");

            migrationBuilder.AddForeignKey(
                name: "FK_Evaluation_User_EvaluatedOnId",
                table: "Evaluation",
                column: "EvaluatedOnId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
