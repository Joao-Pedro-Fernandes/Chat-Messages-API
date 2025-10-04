using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChatMessages.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddChatUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Chats_Users_ReceiverUserId",
                table: "Chats");

            migrationBuilder.DropForeignKey(
                name: "FK_Chats_Users_SenderUserId",
                table: "Chats");

            migrationBuilder.DropIndex(
                name: "IX_Chats_ReceiverUserId",
                table: "Chats");

            migrationBuilder.DropColumn(
                name: "ReceiverUserId",
                table: "Chats");

            migrationBuilder.RenameColumn(
                name: "SenderUserId",
                table: "Chats",
                newName: "CreatorUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Chats_SenderUserId",
                table: "Chats",
                newName: "IX_Chats_CreatorUserId");

            migrationBuilder.AddColumn<Guid>(
                name: "GroupConnectionId",
                table: "Chats",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci");

            migrationBuilder.CreateTable(
                name: "ChatUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ChatId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Accepted = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChatUsers_Chats_ChatId",
                        column: x => x.ChatId,
                        principalTable: "Chats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChatUsers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_ChatUsers_ChatId",
                table: "ChatUsers",
                column: "ChatId");

            migrationBuilder.CreateIndex(
                name: "IX_ChatUsers_UserId",
                table: "ChatUsers",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Chats_Users_CreatorUserId",
                table: "Chats",
                column: "CreatorUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Chats_Users_CreatorUserId",
                table: "Chats");

            migrationBuilder.DropTable(
                name: "ChatUsers");

            migrationBuilder.DropColumn(
                name: "GroupConnectionId",
                table: "Chats");

            migrationBuilder.RenameColumn(
                name: "CreatorUserId",
                table: "Chats",
                newName: "SenderUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Chats_CreatorUserId",
                table: "Chats",
                newName: "IX_Chats_SenderUserId");

            migrationBuilder.AddColumn<int>(
                name: "ReceiverUserId",
                table: "Chats",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Chats_ReceiverUserId",
                table: "Chats",
                column: "ReceiverUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Chats_Users_ReceiverUserId",
                table: "Chats",
                column: "ReceiverUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Chats_Users_SenderUserId",
                table: "Chats",
                column: "SenderUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
