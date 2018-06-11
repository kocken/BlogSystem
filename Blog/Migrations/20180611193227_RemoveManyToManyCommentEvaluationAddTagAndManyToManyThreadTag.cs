using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Blog.Migrations
{
    public partial class RemoveManyToManyCommentEvaluationAddTagAndManyToManyThreadTag : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CommentEvaluation");

            migrationBuilder.CreateTable(
                name: "Tag",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tag", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ThreadTag",
                columns: table => new
                {
                    ThreadId = table.Column<int>(nullable: false),
                    TagId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThreadTag", x => new { x.ThreadId, x.TagId });
                    table.UniqueConstraint("AK_ThreadTag_TagId_ThreadId", x => new { x.TagId, x.ThreadId });
                    table.ForeignKey(
                        name: "FK_ThreadTag_Tag_TagId",
                        column: x => x.TagId,
                        principalTable: "Tag",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ThreadTag_Thread_ThreadId",
                        column: x => x.ThreadId,
                        principalTable: "Thread",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ThreadTag");

            migrationBuilder.DropTable(
                name: "Tag");

            migrationBuilder.CreateTable(
                name: "CommentEvaluation",
                columns: table => new
                {
                    CommentId = table.Column<int>(nullable: false),
                    EvaluationId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommentEvaluation", x => new { x.CommentId, x.EvaluationId });
                    table.ForeignKey(
                        name: "FK_CommentEvaluation_Comment_CommentId",
                        column: x => x.CommentId,
                        principalTable: "Comment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CommentEvaluation_Evaluation_EvaluationId",
                        column: x => x.EvaluationId,
                        principalTable: "Evaluation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CommentEvaluation_EvaluationId",
                table: "CommentEvaluation",
                column: "EvaluationId");
        }
    }
}
