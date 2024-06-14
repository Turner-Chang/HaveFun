using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HaveFun.Migrations
{
    /// <inheritdoc />
    public partial class add_userreview_complaintcategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "complaintCategoryId",
                table: "UserReviews",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_UserReviews_complaintCategoryId",
                table: "UserReviews",
                column: "complaintCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserReviews_ComplaintCategories_complaintCategoryId",
                table: "UserReviews",
                column: "complaintCategoryId",
                principalTable: "ComplaintCategories",
                principalColumn: "ComplaintCategoryId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserReviews_ComplaintCategories_complaintCategoryId",
                table: "UserReviews");

            migrationBuilder.DropIndex(
                name: "IX_UserReviews_complaintCategoryId",
                table: "UserReviews");

            migrationBuilder.DropColumn(
                name: "complaintCategoryId",
                table: "UserReviews");
        }
    }
}
