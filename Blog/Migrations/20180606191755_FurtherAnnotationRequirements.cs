using Microsoft.EntityFrameworkCore.Migrations;

namespace Blog.Migrations
{
    public partial class FurtherAnnotationRequirements : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Evaluation_EvaluationValue_ValueId",
                table: "Evaluation");

            migrationBuilder.DropForeignKey(
                name: "FK_PostEvaluation_Evaluation_EvaluationId",
                table: "PostEvaluation");

            migrationBuilder.DropForeignKey(
                name: "FK_PostEvaluation_Post_PostId",
                table: "PostEvaluation");

            migrationBuilder.DropIndex(
                name: "IX_Evaluation_ValueId",
                table: "Evaluation");

            migrationBuilder.DropColumn(
                name: "ValueId",
                table: "Evaluation");

            migrationBuilder.AlterColumn<int>(
                name: "RankId",
                table: "User",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "AuthorId",
                table: "Post",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "PostId",
                table: "Evaluation",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "EvaluatedOnId",
                table: "Evaluation",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "EvaluatedById",
                table: "Evaluation",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EvaluationValueId",
                table: "Evaluation",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Evaluation_EvaluationValueId",
                table: "Evaluation",
                column: "EvaluationValueId");

            migrationBuilder.AddForeignKey(
                name: "FK_Evaluation_EvaluationValue_EvaluationValueId",
                table: "Evaluation",
                column: "EvaluationValueId",
                principalTable: "EvaluationValue",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PostEvaluation_Evaluation_EvaluationId",
                table: "PostEvaluation",
                column: "EvaluationId",
                principalTable: "Evaluation",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PostEvaluation_Post_PostId",
                table: "PostEvaluation",
                column: "PostId",
                principalTable: "Post",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Evaluation_EvaluationValue_EvaluationValueId",
                table: "Evaluation");

            migrationBuilder.DropForeignKey(
                name: "FK_PostEvaluation_Evaluation_EvaluationId",
                table: "PostEvaluation");

            migrationBuilder.DropForeignKey(
                name: "FK_PostEvaluation_Post_PostId",
                table: "PostEvaluation");

            migrationBuilder.DropIndex(
                name: "IX_Evaluation_EvaluationValueId",
                table: "Evaluation");

            migrationBuilder.DropColumn(
                name: "EvaluationValueId",
                table: "Evaluation");

            migrationBuilder.AlterColumn<int>(
                name: "RankId",
                table: "User",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "AuthorId",
                table: "Post",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "PostId",
                table: "Evaluation",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "EvaluatedOnId",
                table: "Evaluation",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "EvaluatedById",
                table: "Evaluation",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "ValueId",
                table: "Evaluation",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Evaluation_ValueId",
                table: "Evaluation",
                column: "ValueId");

            migrationBuilder.AddForeignKey(
                name: "FK_Evaluation_EvaluationValue_ValueId",
                table: "Evaluation",
                column: "ValueId",
                principalTable: "EvaluationValue",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PostEvaluation_Evaluation_EvaluationId",
                table: "PostEvaluation",
                column: "EvaluationId",
                principalTable: "Evaluation",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PostEvaluation_Post_PostId",
                table: "PostEvaluation",
                column: "PostId",
                principalTable: "Post",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
