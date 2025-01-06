using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Makrowave_Type_Backend.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "user",
                columns: table => new
                {
                    user_id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    username = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    password_hash = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_id", x => x.user_id);
                });

            migrationBuilder.CreateTable(
                name: "daily_record",
                columns: table => new
                {
                    daily_record_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    date = table.Column<DateTime>(type: "date", nullable: false),
                    time = table.Column<int>(type: "integer", nullable: false),
                    score = table.Column<int>(type: "integer", nullable: false),
                    accuracy = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_daily_record_id", x => x.daily_record_id);
                    table.ForeignKey(
                        name: "FK_daily_record_user_user_id",
                        column: x => x.user_id,
                        principalTable: "user",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "session",
                columns: table => new
                {
                    session_id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    ExpirationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_session_id", x => x.session_id);
                    table.ForeignKey(
                        name: "FK_session_user_user_id",
                        column: x => x.user_id,
                        principalTable: "user",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_theme",
                columns: table => new
                {
                    user_theme_id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    ui_text = table.Column<string>(type: "character varying(7)", maxLength: 7, nullable: false),
                    ui_background = table.Column<string>(type: "character varying(7)", maxLength: 7, nullable: false),
                    text_incomplete = table.Column<string>(type: "character varying(7)", maxLength: 7, nullable: false),
                    text_complete = table.Column<string>(type: "character varying(7)", maxLength: 7, nullable: false),
                    text_incorrect = table.Column<string>(type: "character varying(7)", maxLength: 7, nullable: false),
                    inactive_key = table.Column<string>(type: "character varying(7)", maxLength: 7, nullable: false),
                    inactive_text = table.Column<string>(type: "character varying(7)", maxLength: 7, nullable: false),
                    active_text = table.Column<string>(type: "character varying(7)", maxLength: 7, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_theme_id", x => x.user_theme_id);
                    table.ForeignKey(
                        name: "FK_user_theme_user_user_theme_id",
                        column: x => x.user_theme_id,
                        principalTable: "user",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "gradient_color",
                columns: table => new
                {
                    gradient_color_id = table.Column<int>(type: "integer", nullable: false),
                    user_theme_id = table.Column<Guid>(type: "uuid", nullable: false),
                    color = table.Column<string>(type: "character varying(7)", maxLength: 7, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_gradient_color_id", x => new { x.gradient_color_id, x.user_theme_id });
                    table.ForeignKey(
                        name: "FK_gradient_color_user_theme_user_theme_id",
                        column: x => x.user_theme_id,
                        principalTable: "user_theme",
                        principalColumn: "user_theme_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_daily_record_user_id",
                table: "daily_record",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_gradient_color_user_theme_id",
                table: "gradient_color",
                column: "user_theme_id");

            migrationBuilder.CreateIndex(
                name: "IX_session_user_id",
                table: "session",
                column: "user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "daily_record");

            migrationBuilder.DropTable(
                name: "gradient_color");

            migrationBuilder.DropTable(
                name: "session");

            migrationBuilder.DropTable(
                name: "user_theme");

            migrationBuilder.DropTable(
                name: "user");
        }
    }
}
