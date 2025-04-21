using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace UploadProject.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                table: "Users",
                columns: new[] { "ID", "CompetitionSessionID", "IsAdmin", "Password", "Username" },
                values: new object[,]
                {
                    { new Guid("0b1eb367-f5fd-4b3f-a890-2d4b6a88035e"), null, false, "5X4T08WY", "PC11" },
                    { new Guid("0b42e173-dab6-4c59-af4d-3a2136f8c805"), null, false, "B8Q1S2VY", "PC38" },
                    { new Guid("12cf9dce-243f-41e9-bff1-7bf07b7a8408"), null, false, "C0HUZ8NP", "PC25" },
                    { new Guid("14095725-9dd0-443c-831f-5d9068ceb244"), null, false, "VBY3AGX0", "PC45" },
                    { new Guid("3dc59819-96a2-4882-9e50-117351e81d12"), null, false, "WSBX85UP", "PC28" },
                    { new Guid("416dae84-9294-4a8a-9d8d-327d74a8d58c"), null, false, "G6BE73YX", "PC23" },
                    { new Guid("4835bc59-ea3a-4ed5-9940-f26a0cd2fe83"), null, false, "MAPZ589U", "PC02" },
                    { new Guid("535514c1-c065-40c5-9ff1-e8c8c6e3d947"), null, false, "OET5HN6R", "PC06" },
                    { new Guid("5ad361ae-25ee-4cb8-a254-3123c7a7e771"), null, false, "VBU8HQT1", "PC35" },
                    { new Guid("5c681d9e-462b-463e-9bca-9f392899f0d2"), null, false, "9L0NT7J6", "PC04" },
                    { new Guid("5e46d995-2fee-49d6-9dff-72392b9df935"), null, false, "IRO5MEU4", "PC08" },
                    { new Guid("6068faa6-6acd-44ef-9e00-b398addf2fa8"), null, false, "3ZGOUNMJ", "PC43" },
                    { new Guid("60f61f1a-37f9-4a80-815f-6ed58c14cfdc"), null, true, "WeLoveITSSBIndonesia", "admin2" },
                    { new Guid("6c5e698d-3fb5-4d80-9021-83e8de71009f"), null, false, "5K0CPONL", "PC34" },
                    { new Guid("74d8840d-37e8-4f9f-af13-781a404a2e60"), null, false, "6WXAIYDM", "PC13" },
                    { new Guid("7622e89e-c91c-4e19-b501-93e15db40a49"), null, false, "TOKJ97QY", "PC17" },
                    { new Guid("7a2ebe98-f6fe-455e-9755-b48fc2da28dc"), null, false, "YXINWEJA", "PC40" },
                    { new Guid("804bfed2-b00e-4de6-970a-d85df702d096"), null, false, "DK8JBW2F", "PC15" },
                    { new Guid("8356b83a-b5bc-4ec4-ba47-d0c8017519d4"), null, false, "03WO26X7", "PC10" },
                    { new Guid("886bf770-912e-408b-8e0a-209d7434f98d"), null, false, "UT69WXCE", "PC31" },
                    { new Guid("8912e9d7-5e60-4887-9036-af512b4cd952"), null, false, "KHONGFR1", "PC21" },
                    { new Guid("89d8e81f-2e2b-41fb-8171-0c228373e5e5"), null, false, "6AQWOM0J", "PC33" },
                    { new Guid("96c63313-34ff-46eb-bbcb-6df260ac3353"), null, false, "D0T7BR45", "PC16" },
                    { new Guid("9c94e7de-2d7e-4731-a290-e76a18b04dfb"), null, false, "6TBONRF5", "PC29" },
                    { new Guid("a0e85403-18d9-459a-b189-6debe8f2dea3"), null, false, "4V1QZNSM", "PC03" },
                    { new Guid("a1f84658-5b47-449e-b258-9ca09fcde852"), null, false, "DXB52JZF", "PC41" },
                    { new Guid("abf1b37e-2b48-4dfb-9c3a-411c5768b356"), null, false, "Q4CLJXNA", "PC24" },
                    { new Guid("b0d4304b-681f-4da4-a839-d4bddc91b838"), null, false, "4EKD5JU0", "PC50" },
                    { new Guid("b543ccce-c16a-4797-8936-d64d3df9353e"), null, false, "DXNQ85HY", "PC09" },
                    { new Guid("b8472993-df58-4715-aeda-137a39d54801"), null, false, "05AKG8S1", "PC47" },
                    { new Guid("b98e5dda-3f06-4350-a5d1-8a3faf3a29cb"), null, false, "YRZ26PHQ", "PC12" },
                    { new Guid("bdc5aad5-4b53-4267-83de-ef0ee6a8e7db"), null, false, "D0B8QOAC", "PC14" },
                    { new Guid("c2847c71-1b7b-4928-ab40-ee01a894e0c8"), null, false, "RFXISE9Y", "PC26" },
                    { new Guid("c2aa716f-ebd9-4a0f-87e2-c1d8ee60ad52"), null, false, "P2748RFB", "PC46" },
                    { new Guid("c431d81f-883c-41d5-aedf-d418bf71fc14"), null, false, "0GKB7E59", "PC42" },
                    { new Guid("ce0c33ea-e7de-47ed-847a-bb9c041a2f57"), null, false, "S5ETJOVY", "PC27" },
                    { new Guid("ceb01e02-988b-46f7-b63e-d1595429996c"), null, false, "U1FGBK6J", "PC32" },
                    { new Guid("cfc6c7ed-6213-4f00-823c-c76b420b59bb"), null, false, "Q2E0FUX3", "PC49" },
                    { new Guid("d7e5db0c-f5d9-44c4-8c8a-36e4adee3ae9"), null, false, "AHEOTSKL", "PC48" },
                    { new Guid("d8faee54-b490-46ac-a5d8-8e585d7095b7"), null, false, "EOAWVF2S", "PC18" },
                    { new Guid("dbbf0200-82f7-4238-86c5-dc04ed174ff1"), null, false, "RYONL1GA", "PC39" },
                    { new Guid("e0361d0e-802a-4d76-9340-472ba2aa7085"), null, true, "WeLoveITSSBIndonesia", "admin" },
                    { new Guid("e0f799aa-ab5c-40af-8977-18037acb9f1f"), null, false, "PWDY61MX", "PC37" },
                    { new Guid("e7ecd0bf-8a21-4e26-8d4a-2b8af03920a4"), null, false, "CMDEKHWQ", "PC01" },
                    { new Guid("ea585224-eada-491e-9b80-190b7a52117e"), null, false, "LXUT0S7W", "PC05" },
                    { new Guid("ebbfa0ac-bf17-48bc-843b-c97f710ee570"), null, false, "Q7F0XK1R", "PC30" },
                    { new Guid("ec99a87f-af1e-476d-ae30-bdd503d7f1d8"), null, false, "PAMHRQZ9", "PC19" },
                    { new Guid("f0fc9903-126a-41f3-bd5b-404185a166fa"), null, false, "W1UZB0PA", "PC36" },
                    { new Guid("f174ffb0-7ebd-4e47-a93c-693d1297afe6"), null, false, "RCHNOMES", "PC20" },
                    { new Guid("f6c952c5-7431-4f52-a087-59f38f8b1d0f"), null, false, "N6VZPO1S", "PC44" },
                    { new Guid("faa4561d-e29a-4b6d-bf12-fec8b267595f"), null, false, "1U0G2TVS", "PC22" },
                    { new Guid("fe8634b3-1fd4-46aa-9bae-c63766eff35e"), null, false, "O7F3MYLH", "PC07" }
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
                name: "CompetitorUploadedFiles");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "CompetitionSessions");
        }
    }
}
