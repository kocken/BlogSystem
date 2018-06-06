using Microsoft.EntityFrameworkCore.Migrations;

namespace Blog.Migrations
{
    public partial class BetterAnnotations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PostEvaluation_EvaluationId",
                table: "PostEvaluation");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_PostEvaluation_EvaluationId_PostId",
                table: "PostEvaluation",
                columns: new[] { "EvaluationId", "PostId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_PostEvaluation_EvaluationId_PostId",
                table: "PostEvaluation");

            migrationBuilder.CreateIndex(
                name: "IX_PostEvaluation_EvaluationId",
                table: "PostEvaluation",
                column: "EvaluationId");
        }
    }
}
