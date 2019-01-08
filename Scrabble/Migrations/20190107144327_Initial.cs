using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Scrabble.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WordDictionary",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WordDictionary", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "BoardTile",
                columns: table => new
                {
                    ID = table.Column<string>(nullable: false),
                    BoardLocationX = table.Column<int>(nullable: false),
                    BoardLocationY = table.Column<int>(nullable: false),
                    BoardTileType = table.Column<int>(nullable: false),
                    BoardID = table.Column<int>(nullable: false),
                    CharTileID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BoardTile", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Game",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    GameLanguage = table.Column<int>(nullable: false),
                    RackSize = table.Column<int>(nullable: false),
                    WordDictionaryID = table.Column<int>(nullable: false),
                    BoardID = table.Column<int>(nullable: false),
                    PouchID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Game", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Game_WordDictionary_WordDictionaryID",
                        column: x => x.WordDictionaryID,
                        principalTable: "WordDictionary",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Board",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Rows = table.Column<int>(nullable: false),
                    Columns = table.Column<int>(nullable: false),
                    GameID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Board", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Board_Game_GameID",
                        column: x => x.GameID,
                        principalTable: "Game",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Pouch",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    GameID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pouch", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Pouch_Game_GameID",
                        column: x => x.GameID,
                        principalTable: "Game",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Player",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    IsHuman = table.Column<bool>(nullable: false),
                    Score = table.Column<int>(nullable: false),
                    RackID = table.Column<int>(nullable: false),
                    PouchID = table.Column<int>(nullable: false),
                    GameID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Player", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Player_Game_GameID",
                        column: x => x.GameID,
                        principalTable: "Game",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Player_Pouch_PouchID",
                        column: x => x.PouchID,
                        principalTable: "Pouch",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Rack",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RackSize = table.Column<int>(nullable: false),
                    PlayerID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rack", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Rack_Player_PlayerID",
                        column: x => x.PlayerID,
                        principalTable: "Player",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "CharTile",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Letter = table.Column<string>(nullable: false),
                    Score = table.Column<int>(nullable: false),
                    WordDictionaryID = table.Column<int>(nullable: false),
                    PouchID = table.Column<int>(nullable: true),
                    RackID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharTile", x => x.ID);
                    table.ForeignKey(
                        name: "FK_CharTile_Pouch_PouchID",
                        column: x => x.PouchID,
                        principalTable: "Pouch",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CharTile_Rack_RackID",
                        column: x => x.RackID,
                        principalTable: "Rack",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CharTile_WordDictionary_WordDictionaryID",
                        column: x => x.WordDictionaryID,
                        principalTable: "WordDictionary",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Board_GameID",
                table: "Board",
                column: "GameID");

            migrationBuilder.CreateIndex(
                name: "IX_BoardTile_BoardID",
                table: "BoardTile",
                column: "BoardID");

            migrationBuilder.CreateIndex(
                name: "IX_BoardTile_CharTileID",
                table: "BoardTile",
                column: "CharTileID");

            migrationBuilder.CreateIndex(
                name: "IX_CharTile_PouchID",
                table: "CharTile",
                column: "PouchID");

            migrationBuilder.CreateIndex(
                name: "IX_CharTile_RackID",
                table: "CharTile",
                column: "RackID");

            migrationBuilder.CreateIndex(
                name: "IX_CharTile_WordDictionaryID",
                table: "CharTile",
                column: "WordDictionaryID");

            migrationBuilder.CreateIndex(
                name: "IX_Game_BoardID",
                table: "Game",
                column: "BoardID");

            migrationBuilder.CreateIndex(
                name: "IX_Game_PouchID",
                table: "Game",
                column: "PouchID");

            migrationBuilder.CreateIndex(
                name: "IX_Game_WordDictionaryID",
                table: "Game",
                column: "WordDictionaryID");

            migrationBuilder.CreateIndex(
                name: "IX_Player_GameID",
                table: "Player",
                column: "GameID");

            migrationBuilder.CreateIndex(
                name: "IX_Player_PouchID",
                table: "Player",
                column: "PouchID");

            migrationBuilder.CreateIndex(
                name: "IX_Player_RackID",
                table: "Player",
                column: "RackID");

            migrationBuilder.CreateIndex(
                name: "IX_Pouch_GameID",
                table: "Pouch",
                column: "GameID");

            migrationBuilder.CreateIndex(
                name: "IX_Rack_PlayerID",
                table: "Rack",
                column: "PlayerID");

            migrationBuilder.AddForeignKey(
                name: "FK_BoardTile_Board_BoardID",
                table: "BoardTile",
                column: "BoardID",
                principalTable: "Board",
                principalColumn: "ID",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_BoardTile_CharTile_CharTileID",
                table: "BoardTile",
                column: "CharTileID",
                principalTable: "CharTile",
                principalColumn: "ID",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Game_Board_BoardID",
                table: "Game",
                column: "BoardID",
                principalTable: "Board",
                principalColumn: "ID",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Game_Pouch_PouchID",
                table: "Game",
                column: "PouchID",
                principalTable: "Pouch",
                principalColumn: "ID",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Player_Rack_RackID",
                table: "Player",
                column: "RackID",
                principalTable: "Rack",
                principalColumn: "ID",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Board_Game_GameID",
                table: "Board");

            migrationBuilder.DropForeignKey(
                name: "FK_Player_Game_GameID",
                table: "Player");

            migrationBuilder.DropForeignKey(
                name: "FK_Pouch_Game_GameID",
                table: "Pouch");

            migrationBuilder.DropForeignKey(
                name: "FK_Player_Pouch_PouchID",
                table: "Player");

            migrationBuilder.DropForeignKey(
                name: "FK_Player_Rack_RackID",
                table: "Player");

            migrationBuilder.DropTable(
                name: "BoardTile");

            migrationBuilder.DropTable(
                name: "CharTile");

            migrationBuilder.DropTable(
                name: "Game");

            migrationBuilder.DropTable(
                name: "Board");

            migrationBuilder.DropTable(
                name: "WordDictionary");

            migrationBuilder.DropTable(
                name: "Pouch");

            migrationBuilder.DropTable(
                name: "Rack");

            migrationBuilder.DropTable(
                name: "Player");
        }
    }
}
