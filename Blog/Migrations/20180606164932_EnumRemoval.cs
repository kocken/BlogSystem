using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Blog.Migrations
{
    public partial class EnumRemoval : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rank",
                table: "User");

            migrationBuilder.RenameColumn(
                name: "Value",
                table: "Evaluation",
                newName: "ValueId");

            migrationBuilder.AddColumn<int>(
                name: "RankId",
                table: "User",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "EvaluationValue",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EvaluationValue", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Rank",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rank", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_User_RankId",
                table: "User",
                column: "RankId");

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
                name: "FK_User_Rank_RankId",
                table: "User",
                column: "RankId",
                principalTable: "Rank",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Evaluation_EvaluationValue_ValueId",
                table: "Evaluation");

            migrationBuilder.DropForeignKey(
                name: "FK_User_Rank_RankId",
                table: "User");

            migrationBuilder.DropTable(
                name: "EvaluationValue");

            migrationBuilder.DropTable(
                name: "Rank");

            migrationBuilder.DropIndex(
                name: "IX_User_RankId",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_Evaluation_ValueId",
                table: "Evaluation");

            migrationBuilder.DropColumn(
                name: "RankId",
                table: "User");

            migrationBuilder.RenameColumn(
                name: "ValueId",
                table: "Evaluation",
                newName: "Value");

            migrationBuilder.AddColumn<int>(
                name: "Rank",
                table: "User",
                nullable: false,
                defaultValue: 0);
        }
    }
}
