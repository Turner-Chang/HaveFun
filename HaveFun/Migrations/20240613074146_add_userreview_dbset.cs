using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HaveFun.Migrations
{
    /// <inheritdoc />
    public partial class add_userreview_dbset : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Friend");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Friend",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FriendListId = table.Column<int>(type: "int", nullable: true),
                    FriendListId1 = table.Column<int>(type: "int", nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsBlocked = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
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
        }
    }
}
