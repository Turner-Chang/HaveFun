using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HaveFun.Migrations
{
    /// <inheritdoc />
    public partial class add_userreview : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Friend",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsBlocked = table.Column<bool>(type: "bit", nullable: false),
                    FriendListId = table.Column<int>(type: "int", nullable: true),
                    FriendListId1 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Friend", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Friend_FriendLists_FriendListId",
                        column: x => x.FriendListId,
                        principalTable: "FriendLists",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Friend_FriendLists_FriendListId1",
                        column: x => x.FriendListId1,
                        principalTable: "FriendLists",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UserReviews",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    reportTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    reportUserId = table.Column<int>(type: "int", nullable: false),
                    beReportedUserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserReviews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserReviews_UserInfos_beReportedUserId",
                        column: x => x.beReportedUserId,
                        principalTable: "UserInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserReviews_UserInfos_reportUserId",
                        column: x => x.reportUserId,
                        principalTable: "UserInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Friend_FriendListId",
                table: "Friend",
                column: "FriendListId");

            migrationBuilder.CreateIndex(
                name: "IX_Friend_FriendListId1",
                table: "Friend",
                column: "FriendListId1");

            migrationBuilder.CreateIndex(
                name: "IX_UserReviews_beReportedUserId",
                table: "UserReviews",
                column: "beReportedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserReviews_reportUserId",
                table: "UserReviews",
                column: "reportUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Friend");

            migrationBuilder.DropTable(
                name: "UserReviews");
        }
    }
}
