using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HaveFun.Migrations
{
    /// <inheritdoc />
    public partial class AddUserInfoColumnPost : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_UserInfos_UserId",
                table: "Posts");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_UserInfos_UserId",
                table: "Posts",
                column: "UserId",
                principalTable: "UserInfos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_UserInfos_UserId",
                table: "Posts");

            migrationBuilder.DropTable(
                name: "Friend");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_UserInfos_UserId",
                table: "Posts",
                column: "UserId",
                principalTable: "UserInfos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
