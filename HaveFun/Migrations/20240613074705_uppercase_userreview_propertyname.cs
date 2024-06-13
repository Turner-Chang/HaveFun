using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HaveFun.Migrations
{
    /// <inheritdoc />
    public partial class uppercase_userreview_propertyname : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserReviews_ComplaintCategories_complaintCategoryId",
                table: "UserReviews");

            migrationBuilder.DropForeignKey(
                name: "FK_UserReviews_UserInfos_beReportedUserId",
                table: "UserReviews");

            migrationBuilder.DropForeignKey(
                name: "FK_UserReviews_UserInfos_reportUserId",
                table: "UserReviews");

            migrationBuilder.RenameColumn(
                name: "reportUserId",
                table: "UserReviews",
                newName: "ReportUserId");

            migrationBuilder.RenameColumn(
                name: "reportTime",
                table: "UserReviews",
                newName: "ReportTime");

            migrationBuilder.RenameColumn(
                name: "complaintCategoryId",
                table: "UserReviews",
                newName: "ComplaintCategoryId");

            migrationBuilder.RenameColumn(
                name: "beReportedUserId",
                table: "UserReviews",
                newName: "BeReportedUserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserReviews_reportUserId",
                table: "UserReviews",
                newName: "IX_UserReviews_ReportUserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserReviews_complaintCategoryId",
                table: "UserReviews",
                newName: "IX_UserReviews_ComplaintCategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_UserReviews_beReportedUserId",
                table: "UserReviews",
                newName: "IX_UserReviews_BeReportedUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserReviews_ComplaintCategories_ComplaintCategoryId",
                table: "UserReviews",
                column: "ComplaintCategoryId",
                principalTable: "ComplaintCategories",
                principalColumn: "ComplaintCategoryId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserReviews_UserInfos_BeReportedUserId",
                table: "UserReviews",
                column: "BeReportedUserId",
                principalTable: "UserInfos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserReviews_UserInfos_ReportUserId",
                table: "UserReviews",
                column: "ReportUserId",
                principalTable: "UserInfos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserReviews_ComplaintCategories_ComplaintCategoryId",
                table: "UserReviews");

            migrationBuilder.DropForeignKey(
                name: "FK_UserReviews_UserInfos_BeReportedUserId",
                table: "UserReviews");

            migrationBuilder.DropForeignKey(
                name: "FK_UserReviews_UserInfos_ReportUserId",
                table: "UserReviews");

            migrationBuilder.RenameColumn(
                name: "ReportUserId",
                table: "UserReviews",
                newName: "reportUserId");

            migrationBuilder.RenameColumn(
                name: "ReportTime",
                table: "UserReviews",
                newName: "reportTime");

            migrationBuilder.RenameColumn(
                name: "ComplaintCategoryId",
                table: "UserReviews",
                newName: "complaintCategoryId");

            migrationBuilder.RenameColumn(
                name: "BeReportedUserId",
                table: "UserReviews",
                newName: "beReportedUserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserReviews_ReportUserId",
                table: "UserReviews",
                newName: "IX_UserReviews_reportUserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserReviews_ComplaintCategoryId",
                table: "UserReviews",
                newName: "IX_UserReviews_complaintCategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_UserReviews_BeReportedUserId",
                table: "UserReviews",
                newName: "IX_UserReviews_beReportedUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserReviews_ComplaintCategories_complaintCategoryId",
                table: "UserReviews",
                column: "complaintCategoryId",
                principalTable: "ComplaintCategories",
                principalColumn: "ComplaintCategoryId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserReviews_UserInfos_beReportedUserId",
                table: "UserReviews",
                column: "beReportedUserId",
                principalTable: "UserInfos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserReviews_UserInfos_reportUserId",
                table: "UserReviews",
                column: "reportUserId",
                principalTable: "UserInfos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
