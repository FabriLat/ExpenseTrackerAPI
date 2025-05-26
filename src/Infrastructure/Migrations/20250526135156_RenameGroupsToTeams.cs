using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RenameGroupsToTeams : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id_user = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    last_name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    email = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    password = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    phone_number = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id_user);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "teams",
                columns: table => new
                {
                    id_team = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    team_name = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    created_date = table.Column<DateOnly>(type: "date", nullable: false),
                    owner_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id_team);
                    table.ForeignKey(
                        name: "owner_id",
                        column: x => x.owner_id,
                        principalTable: "users",
                        principalColumn: "id_user");
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "expenses",
                columns: table => new
                {
                    id_expense = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    amount = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    expense_name = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    description = table.Column<string>(type: "varchar(155)", maxLength: 155, nullable: true, defaultValueSql: "'\"No description added.\"'", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    expense_date = table.Column<DateOnly>(type: "date", nullable: false),
                    team_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id_expense);
                    table.ForeignKey(
                        name: "team_Id",
                        column: x => x.team_id,
                        principalTable: "teams",
                        principalColumn: "id_team");
                    table.ForeignKey(
                        name: "user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id_user",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "invitations",
                columns: table => new
                {
                    id_invitation = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    owner_user_id = table.Column<int>(type: "int", nullable: false),
                    invited_user_id = table.Column<int>(type: "int", nullable: false),
                    team_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id_invitation);
                    table.ForeignKey(
                        name: "fk_invitations_teams",
                        column: x => x.team_id,
                        principalTable: "teams",
                        principalColumn: "id_team");
                    table.ForeignKey(
                        name: "invited_user_id",
                        column: x => x.invited_user_id,
                        principalTable: "users",
                        principalColumn: "id_user");
                    table.ForeignKey(
                        name: "owner_user_id",
                        column: x => x.owner_user_id,
                        principalTable: "users",
                        principalColumn: "id_user");
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "users_teams",
                columns: table => new
                {
                    user_id = table.Column<int>(type: "int", nullable: false),
                    team_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => new { x.user_id, x.team_id })
                        .Annotation("MySql:IndexPrefixLength", new[] { 0, 0 });
                    table.ForeignKey(
                        name: "TeamId",
                        column: x => x.team_id,
                        principalTable: "teams",
                        principalColumn: "id_team");
                    table.ForeignKey(
                        name: "userId",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id_user");
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateIndex(
                name: "team_Id_idx",
                table: "expenses",
                column: "team_id");

            migrationBuilder.CreateIndex(
                name: "user_id_idx",
                table: "expenses",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "fk_invitations_teams",
                table: "invitations",
                column: "team_id");

            migrationBuilder.CreateIndex(
                name: "id_invited_idx",
                table: "invitations",
                column: "invited_user_id");

            migrationBuilder.CreateIndex(
                name: "owner_user_id_idx",
                table: "invitations",
                column: "owner_user_id");

            migrationBuilder.CreateIndex(
                name: "owner_id_idx",
                table: "teams",
                column: "owner_id");

            migrationBuilder.CreateIndex(
                name: "email_UNIQUE",
                table: "users",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "phone_number_UNIQUE",
                table: "users",
                column: "phone_number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "teamId_idx",
                table: "users_teams",
                column: "team_id");

            migrationBuilder.CreateIndex(
                name: "userId_idx",
                table: "users_teams",
                column: "user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "expenses");

            migrationBuilder.DropTable(
                name: "invitations");

            migrationBuilder.DropTable(
                name: "users_teams");

            migrationBuilder.DropTable(
                name: "teams");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
