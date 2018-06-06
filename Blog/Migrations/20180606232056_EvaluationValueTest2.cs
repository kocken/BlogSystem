using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Blog.Migrations
{
    public partial class EvaluationValueTest2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EvaluationValueId",
                table: "Evaluation",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "EvaluationValue",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EvaluationValue", x => x.Id);
                });

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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Evaluation_EvaluationValue_EvaluationValueId",
                table: "Evaluation");

            migrationBuilder.DropTable(
                name: "EvaluationValue");

            migrationBuilder.DropIndex(
                name: "IX_Evaluation_EvaluationValueId",
                table: "Evaluation");

            migrationBuilder.DropColumn(
                name: "EvaluationValueId",
                table: "Evaluation");
        }
    }
}
