using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PaymentGateway_Task.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Admin",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    Passwords = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "TransactionTypes",
                columns: table => new
                {
                    TransactionType_ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TransactionType_Name = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionType_ID", x => x.TransactionType_ID);
                });

            migrationBuilder.CreateTable(
                name: "UsersType",
                columns: table => new
                {
                    UserType_ID = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersType", x => x.UserType_ID);
                });

            migrationBuilder.CreateTable(
                name: "Transaction",
                columns: table => new
                {
                    Transaction_ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TransactionName = table.Column<string>(maxLength: 50, nullable: false),
                    TransactionAmount = table.Column<decimal>(type: "decimal(19, 4)", nullable: false),
                    TransactionType_ID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transaction", x => x.Transaction_ID);
                    table.ForeignKey(
                        name: "FK_TransactionType_ID",
                        column: x => x.TransactionType_ID,
                        principalTable: "TransactionTypes",
                        principalColumn: "TransactionType_ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(maxLength: 50, nullable: false),
                    Password = table.Column<string>(maxLength: 100, nullable: false),
                    UserTypeID = table.Column<int>(nullable: false),
                    PDF = table.Column<string>(unicode: false, nullable: true),
                    PDF_Name = table.Column<string>(maxLength: 255, nullable: true),
                    AdminApproval = table.Column<bool>(nullable: false),
                    CreditBalance = table.Column<decimal>(type: "decimal(19, 4)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.ID);
                    table.ForeignKey(
                        name: "FK_UsersType",
                        column: x => x.UserTypeID,
                        principalTable: "UsersType",
                        principalColumn: "UserType_ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LoginTokens",
                columns: table => new
                {
                    Token = table.Column<string>(unicode: false, maxLength: 255, nullable: false),
                    UserID = table.Column<int>(nullable: true),
                    TokenTime = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    TokenExpiration = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Token", x => x.Token);
                    table.ForeignKey(
                        name: "FK_User_ID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LoginTokens_UserID",
                table: "LoginTokens",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_TransactionType_ID",
                table: "Transaction",
                column: "TransactionType_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserTypeID",
                table: "Users",
                column: "UserTypeID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Admin");

            migrationBuilder.DropTable(
                name: "LoginTokens");

            migrationBuilder.DropTable(
                name: "Transaction");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "TransactionTypes");

            migrationBuilder.DropTable(
                name: "UsersType");
        }
    }
}
