using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace UploadProject.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    UserName = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "INTEGER", nullable: false),
                    PasswordHash = table.Column<string>(type: "TEXT", nullable: true),
                    SecurityStamp = table.Column<string>(type: "TEXT", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: true),
                    PhoneNumber = table.Column<string>(type: "TEXT", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "INTEGER", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CompetitionSessions",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    DayNumber = table.Column<int>(type: "INTEGER", nullable: false),
                    StartDateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EndDateTime = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompetitionSessions", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RoleId = table.Column<string>(type: "TEXT", nullable: false),
                    ClaimType = table.Column<string>(type: "TEXT", nullable: true),
                    ClaimValue = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    ClaimType = table.Column<string>(type: "TEXT", nullable: true),
                    ClaimValue = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    ProviderKey = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "TEXT", nullable: true),
                    UserId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    RoleId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    LoginProvider = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    Value = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "TEXT", nullable: false),
                    IsAdmin = table.Column<bool>(type: "INTEGER", nullable: false),
                    Username = table.Column<string>(type: "TEXT", nullable: false),
                    Password = table.Column<string>(type: "TEXT", nullable: false),
                    CompetitionSessionID = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Users_CompetitionSessions_CompetitionSessionID",
                        column: x => x.CompetitionSessionID,
                        principalTable: "CompetitionSessions",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "AdminUploadedFiles",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "TEXT", nullable: false),
                    UserID = table.Column<Guid>(type: "TEXT", nullable: false),
                    CompetitionSessionID = table.Column<Guid>(type: "TEXT", nullable: false),
                    FileName = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdminUploadedFiles", x => x.ID);
                    table.ForeignKey(
                        name: "FK_AdminUploadedFiles_CompetitionSessions_CompetitionSessionID",
                        column: x => x.CompetitionSessionID,
                        principalTable: "CompetitionSessions",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AdminUploadedFiles_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CompetitorUploadedFiles",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "TEXT", nullable: false),
                    UserID = table.Column<Guid>(type: "TEXT", nullable: false),
                    CompetitionSessionID = table.Column<Guid>(type: "TEXT", nullable: false),
                    FileName = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompetitorUploadedFiles", x => x.ID);
                    table.ForeignKey(
                        name: "FK_CompetitorUploadedFiles_CompetitionSessions_CompetitionSessionID",
                        column: x => x.CompetitionSessionID,
                        principalTable: "CompetitionSessions",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CompetitorUploadedFiles_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "CompetitionSessions",
                columns: new[] { "ID", "DayNumber", "EndDateTime", "Name", "StartDateTime" },
                values: new object[,]
                {
                    { new Guid("87392a53-922a-416d-aa8d-546e36d3b84c"), 1, new DateTime(2023, 5, 23, 15, 30, 0, 0, DateTimeKind.Unspecified), "Desktop 1", new DateTime(2023, 5, 23, 12, 30, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("9284d533-bdd5-4553-bcfd-5f099870fce2"), 2, new DateTime(2023, 5, 24, 11, 0, 0, 0, DateTimeKind.Unspecified), "Desktop 2", new DateTime(2023, 5, 24, 8, 30, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("a749079e-b5fe-4ade-83ae-be472c2715c8"), 2, new DateTime(2023, 5, 24, 16, 0, 0, 0, DateTimeKind.Unspecified), "Android", new DateTime(2023, 5, 24, 12, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "ID", "CompetitionSessionID", "IsAdmin", "Password", "Username" },
                values: new object[,]
                {
                    { new Guid("01f62ae4-1f5b-4711-822d-9115c097d0ec"), null, false, "Q20YPETN", "PC36" },
                    { new Guid("054bdf2e-f5fc-4419-a32a-87ba7177115b"), null, false, "7XYUSPHQ", "PC13" },
                    { new Guid("0de489f9-c450-4ac6-9e21-617e063ed347"), null, false, "U65Z9RTN", "PC40" },
                    { new Guid("0f965ebb-2cde-4b6c-9713-6a376cd58826"), null, false, "TI3H0GBY", "PC34" },
                    { new Guid("1440304e-7858-4d24-bf9a-3509783b3960"), null, false, "IBWLF3O8", "PC41" },
                    { new Guid("14dbb9f6-b790-41ea-b897-5b729e0fc2d9"), null, false, "AMUP204N", "PC16" },
                    { new Guid("1e215475-1af5-4260-987b-190591001599"), null, false, "8OQ2TLX3", "PC19" },
                    { new Guid("1ef81564-a13d-4186-8ddb-413fd7cec588"), null, false, "H79ZX4AJ", "PC35" },
                    { new Guid("29aa894c-a723-4301-ae39-b2164993fa10"), null, false, "2VSYQ6OR", "PC25" },
                    { new Guid("398fc3e2-1aa6-4cd7-b9da-c1de0402e649"), null, false, "WUN6XHB0", "PC11" },
                    { new Guid("3d8f92b6-1fc8-4ba5-bcd2-1d75a866c061"), null, true, "WeLoveITSSBIndonesia", "admin2" },
                    { new Guid("45adbb25-083d-4648-b784-201b602a3c59"), null, false, "OFMVWPSJ", "PC26" },
                    { new Guid("4d48bb92-52c0-4bc8-aeea-9c11ae31816e"), null, false, "EDHWLKFR", "PC17" },
                    { new Guid("4e3710ff-c7ae-44af-bb56-fa72037fe347"), null, false, "K3U4NVTE", "PC24" },
                    { new Guid("4e88380a-f6c8-42c3-b02d-858e5d19191a"), null, false, "LM4E7A0U", "PC07" },
                    { new Guid("587b636b-94d4-4700-9f2a-038174352663"), null, false, "BSTYZKAV", "PC27" },
                    { new Guid("58dce682-5a37-4c59-a6d4-c9a206aaf77e"), null, false, "C3HYVEN8", "PC21" },
                    { new Guid("60b7f2b1-ff1c-48f2-84d4-3a04f9a296a2"), null, false, "J1UK3L98", "PC33" },
                    { new Guid("6376076a-ea6b-4f3a-b24c-a91d0ef71420"), null, false, "EGMB1C3N", "PC47" },
                    { new Guid("6896aef6-78e2-449e-9711-7154631c7625"), null, false, "V1LC78K9", "PC28" },
                    { new Guid("6b9002c5-2141-483b-8470-694647f1ab3a"), null, false, "FHDRIXVL", "PC04" },
                    { new Guid("6c4f21aa-c3e6-469e-b154-366e96be9351"), null, false, "5XJFAKLB", "PC02" },
                    { new Guid("72cbfdb5-89e6-41b7-9f9c-4fe348aad03e"), null, false, "O4R2JXU1", "PC09" },
                    { new Guid("75c83891-dc00-4f11-abf7-00ad306baea8"), null, false, "RY6TICPM", "PC14" },
                    { new Guid("7a6ac97d-eece-463a-911f-4476018914e6"), null, false, "5OXN6Z9W", "PC43" },
                    { new Guid("7b199a4b-7aa3-49e7-adb2-a3de00a2b219"), null, false, "0O63J98R", "PC37" },
                    { new Guid("7cd6878e-2d6c-49a8-8b92-550e5423240a"), null, false, "2QCPUJN3", "PC50" },
                    { new Guid("7ef3dd0e-ce71-4ae2-902e-c24439259fe6"), null, false, "2RXNMB6J", "PC39" },
                    { new Guid("80d976c3-905d-4344-87b1-62877891b44f"), null, true, "WeLoveITSSBIndonesia", "admin" },
                    { new Guid("852ce1f3-dd93-4077-a26c-501509a34ae8"), null, false, "M9XNRF6L", "PC38" },
                    { new Guid("8c800c5c-3c5b-4392-b725-866e3c697dd8"), null, false, "KJ32AVS5", "PC42" },
                    { new Guid("8d48cb19-3767-41df-afeb-5142ba16bf57"), null, false, "CBKZFE25", "PC18" },
                    { new Guid("96f8a7cd-ca9c-4337-ae34-7c9286f23747"), null, false, "27W09B1T", "PC31" },
                    { new Guid("97b34436-feed-4cc0-bb9e-4c8809a57e90"), null, false, "XG2LMPQH", "PC23" },
                    { new Guid("9845c938-d935-44e0-bac0-add5638b8a02"), null, false, "WDHTFU6Q", "PC01" },
                    { new Guid("99aa085b-b335-4a45-a973-3b3461dffe47"), null, false, "T7E9ZG53", "PC12" },
                    { new Guid("a3a89a75-2e57-4552-952a-f79389faa0e0"), null, false, "1MWHJ2XK", "PC46" },
                    { new Guid("aa7dc3f2-066d-4125-9957-8db0781a1b61"), null, false, "7WYP5F16", "PC08" },
                    { new Guid("b6142238-7543-401b-9bab-9ba31abfe820"), null, false, "JZAGP3BK", "PC10" },
                    { new Guid("b7ec5b15-57bf-46d3-a753-834d058d7ff8"), null, false, "9NB7SX4D", "PC22" },
                    { new Guid("ccfb77cf-28ed-4280-9ace-153de918f10d"), null, false, "U12MGQKZ", "PC30" },
                    { new Guid("d276f729-359b-43d1-bb68-257b42510a83"), null, false, "TJCEK0BY", "PC20" },
                    { new Guid("d7cd6743-2db2-4836-9ff0-c1adfde75d4e"), null, false, "M1EY4WX7", "PC44" },
                    { new Guid("d8482f6f-29ff-42e2-bc0a-d4e8bd949d71"), null, false, "CFBEVZN4", "PC06" },
                    { new Guid("d962bc97-fdd8-4741-aaa2-e67eb4709ae1"), null, false, "7K0AF5WE", "PC29" },
                    { new Guid("e1e31491-cda8-414c-9986-1e99609194e0"), null, false, "W0JLGBTZ", "PC45" },
                    { new Guid("e6138f0a-04cb-47d2-bb28-64a981100d44"), null, false, "6ULC3F0K", "PC05" },
                    { new Guid("e9c48db4-5712-4f4b-bb2f-3a191402cd67"), null, false, "Z574TCEB", "PC15" },
                    { new Guid("eaf4d9b7-7956-4833-9959-bb9571b2ddc3"), null, false, "51H4TN0L", "PC03" },
                    { new Guid("f4efab89-f698-4417-9296-3656f3b6936f"), null, false, "9IWNOYRJ", "PC32" },
                    { new Guid("f64e86c8-31af-4c16-b8bd-836891a41aee"), null, false, "VDFO6H1A", "PC49" },
                    { new Guid("fbfb9c18-c040-4082-b231-df26008fe1e7"), null, false, "5HCGNSB9", "PC48" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AdminUploadedFiles_CompetitionSessionID",
                table: "AdminUploadedFiles",
                column: "CompetitionSessionID");

            migrationBuilder.CreateIndex(
                name: "IX_AdminUploadedFiles_UserID",
                table: "AdminUploadedFiles",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CompetitorUploadedFiles_CompetitionSessionID",
                table: "CompetitorUploadedFiles",
                column: "CompetitionSessionID");

            migrationBuilder.CreateIndex(
                name: "IX_CompetitorUploadedFiles_UserID",
                table: "CompetitorUploadedFiles",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_Users_CompetitionSessionID",
                table: "Users",
                column: "CompetitionSessionID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdminUploadedFiles");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "CompetitorUploadedFiles");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "CompetitionSessions");
        }
    }
}
