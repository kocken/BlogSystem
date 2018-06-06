using Microsoft.EntityFrameworkCore.Migrations;

namespace Blog.Migrations
{
    public partial class EvaluationValueTest3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "EvaluationValueId",
                table: "Evaluation",
                nullable: true,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "EvaluationValueId",
                table: "Evaluation",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);
        }
    }
}
