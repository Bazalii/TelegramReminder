using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace NothingToForgetBot.Data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "chats_with_language",
                columns: table => new
                {
                    chat_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    language = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_chat_with_language", x => x.chat_id);
                });

            migrationBuilder.CreateTable(
                name: "notes",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    chat_id = table.Column<long>(type: "bigint", nullable: false),
                    content = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_note", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "repeated_via_minutes_messages",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    interval = table.Column<int>(type: "integer", nullable: false),
                    end_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    chat_id = table.Column<long>(type: "bigint", nullable: false),
                    content = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_repeated_via_minutes_message", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "repeated_via_seconds_messages",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    interval = table.Column<int>(type: "integer", nullable: false),
                    end_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    chat_id = table.Column<long>(type: "bigint", nullable: false),
                    content = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_repeated_via_seconds_message", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "scheduled_messages",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    publishing_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    chat_id = table.Column<long>(type: "bigint", nullable: false),
                    content = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_scheduled_message", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "supported_languages",
                columns: table => new
                {
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "chats_with_language");

            migrationBuilder.DropTable(
                name: "notes");

            migrationBuilder.DropTable(
                name: "repeated_via_minutes_messages");

            migrationBuilder.DropTable(
                name: "repeated_via_seconds_messages");

            migrationBuilder.DropTable(
                name: "scheduled_messages");

            migrationBuilder.DropTable(
                name: "supported_languages");
        }
    }
}
