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

            migrationBuilder.CreateIndex(
                name: "IX_Friend_FriendListId",
                table: "Friend",
                column: "FriendListId");

            migrationBuilder.CreateIndex(
                name: "IX_Friend_FriendListId1",
                table: "Friend",
                column: "FriendListId1");

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
