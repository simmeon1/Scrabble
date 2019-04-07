using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Scrabble.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BoardTileType",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Type = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BoardTileType", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "GameLanguages",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Language = table.Column<string>(nullable: true),
                    CountOfLetters = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameLanguages", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "WordDictionaries",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    GameLanguageID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WordDictionaries", x => x.ID);
                    table.ForeignKey(
                        name: "FK_WordDictionaries_GameLanguages_GameLanguageID",
                        column: x => x.GameLanguageID,
                        principalTable: "GameLanguages",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "CharTiles",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Letter = table.Column<string>(nullable: false),
                    Score = table.Column<int>(nullable: false),
                    GameLanguageID = table.Column<int>(nullable: false),
                    WordDictionaryID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharTiles", x => x.ID);
                    table.ForeignKey(
                        name: "FK_CharTiles_GameLanguages_GameLanguageID",
                        column: x => x.GameLanguageID,
                        principalTable: "GameLanguages",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_CharTiles_WordDictionaries_WordDictionaryID",
                        column: x => x.WordDictionaryID,
                        principalTable: "WordDictionaries",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    GameLanguageID = table.Column<int>(nullable: false),
                    BoardID = table.Column<int>(nullable: false),
                    PouchID = table.Column<int>(nullable: false),
                    WordDictionaryID = table.Column<int>(nullable: false),
                    Log = table.Column<string>(nullable: true),
                    IsFinished = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Games_GameLanguages_GameLanguageID",
                        column: x => x.GameLanguageID,
                        principalTable: "GameLanguages",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Games_WordDictionaries_WordDictionaryID",
                        column: x => x.WordDictionaryID,
                        principalTable: "WordDictionaries",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Boards",
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
                    table.PrimaryKey("PK_Boards", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Boards_Games_GameID",
                        column: x => x.GameID,
                        principalTable: "Games",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Pouches",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    GameID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pouches", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Pouches_Games_GameID",
                        column: x => x.GameID,
                        principalTable: "Games",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "BoardTiles",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BoardLocationX = table.Column<int>(nullable: false),
                    BoardLocationY = table.Column<int>(nullable: false),
                    BoardTileTypeID = table.Column<int>(nullable: false),
                    BoardID = table.Column<int>(nullable: false),
                    CharTileID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BoardTiles", x => x.ID);
                    table.ForeignKey(
                        name: "FK_BoardTiles_Boards_BoardID",
                        column: x => x.BoardID,
                        principalTable: "Boards",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_BoardTiles_BoardTileType_BoardTileTypeID",
                        column: x => x.BoardTileTypeID,
                        principalTable: "BoardTileType",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_BoardTiles_CharTiles_CharTileID",
                        column: x => x.CharTileID,
                        principalTable: "CharTiles",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Players",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    IsHuman = table.Column<bool>(nullable: false),
                    AtHand = table.Column<bool>(nullable: false),
                    Score = table.Column<int>(nullable: false),
                    SkipsOrRedrawsUsed = table.Column<int>(nullable: false),
                    RackID = table.Column<int>(nullable: false),
                    PouchID = table.Column<int>(nullable: false),
                    GameID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Players_Games_GameID",
                        column: x => x.GameID,
                        principalTable: "Games",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Players_Pouches_PouchID",
                        column: x => x.PouchID,
                        principalTable: "Pouches",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Pouch_CharTiles",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PouchID = table.Column<int>(nullable: false),
                    CharTileID = table.Column<int>(nullable: false),
                    Count = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pouch_CharTiles", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Pouch_CharTiles_CharTiles_CharTileID",
                        column: x => x.CharTileID,
                        principalTable: "CharTiles",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Pouch_CharTiles_Pouches_PouchID",
                        column: x => x.PouchID,
                        principalTable: "Pouches",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Moves",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    IsHorizontal = table.Column<bool>(nullable: false),
                    Start = table.Column<int>(nullable: false),
                    End = table.Column<int>(nullable: false),
                    Index = table.Column<int>(nullable: false),
                    Word = table.Column<string>(nullable: true),
                    Score = table.Column<int>(nullable: false),
                    IsNew = table.Column<bool>(nullable: false),
                    MoveNumber = table.Column<int>(nullable: false),
                    PlayerID = table.Column<int>(nullable: false),
                    GameID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Moves", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Moves_Games_GameID",
                        column: x => x.GameID,
                        principalTable: "Games",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Moves_Players_PlayerID",
                        column: x => x.PlayerID,
                        principalTable: "Players",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Racks",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RackSize = table.Column<int>(nullable: false),
                    PlayerID = table.Column<int>(nullable: false),
                    PouchID = table.Column<int>(nullable: false),
                    GameID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Racks", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Racks_Games_GameID",
                        column: x => x.GameID,
                        principalTable: "Games",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Racks_Players_PlayerID",
                        column: x => x.PlayerID,
                        principalTable: "Players",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Racks_Pouches_PouchID",
                        column: x => x.PouchID,
                        principalTable: "Pouches",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Rack_CharTiles",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RackID = table.Column<int>(nullable: false),
                    CharTileID = table.Column<int>(nullable: false),
                    Count = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rack_CharTiles", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Rack_CharTiles_CharTiles_CharTileID",
                        column: x => x.CharTileID,
                        principalTable: "CharTiles",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Rack_CharTiles_Racks_RackID",
                        column: x => x.RackID,
                        principalTable: "Racks",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.InsertData(
                table: "BoardTileType",
                columns: new[] { "ID", "Type" },
                values: new object[,]
                {
                    { 1, "Normal" },
                    { 2, "DoubleLetter" },
                    { 3, "TripleLetter" },
                    { 4, "DoubleWord" },
                    { 5, "TripleWord" },
                    { 6, "Start" }
                });

            migrationBuilder.InsertData(
                table: "GameLanguages",
                columns: new[] { "ID", "CountOfLetters", "Language" },
                values: new object[,]
                {
                    { 1, 26, "English" },
                    { 2, 30, "Bulgarian" }
                });

            migrationBuilder.InsertData(
                table: "WordDictionaries",
                columns: new[] { "ID", "GameLanguageID" },
                values: new object[] { 1, 1 });

            migrationBuilder.InsertData(
                table: "WordDictionaries",
                columns: new[] { "ID", "GameLanguageID" },
                values: new object[] { 2, 2 });

            migrationBuilder.InsertData(
                table: "CharTiles",
                columns: new[] { "ID", "GameLanguageID", "Letter", "Score", "WordDictionaryID" },
                values: new object[,]
                {
                    { 1, 1, "*", 0, 1 },
                    { 84, 2, "Я", 5, 2 },
                    { 83, 2, "Ю", 8, 2 },
                    { 82, 2, "Ь", 10, 2 },
                    { 81, 2, "Ъ", 3, 2 },
                    { 80, 2, "Щ", 10, 2 },
                    { 79, 2, "Ш", 8, 2 },
                    { 78, 2, "Ч", 5, 2 },
                    { 77, 2, "Ц", 8, 2 },
                    { 76, 2, "Х", 5, 2 },
                    { 75, 2, "Ф", 10, 2 },
                    { 74, 2, "У", 5, 2 },
                    { 73, 2, "Т", 1, 2 },
                    { 72, 2, "С", 1, 2 },
                    { 71, 2, "Р", 1, 2 },
                    { 70, 2, "П", 1, 2 },
                    { 69, 2, "О", 1, 2 },
                    { 68, 2, "Н", 1, 2 },
                    { 67, 2, "М", 2, 2 },
                    { 66, 2, "Л", 2, 2 },
                    { 65, 2, "К", 2, 2 },
                    { 64, 2, "Й", 5, 2 },
                    { 63, 2, "И", 1, 2 },
                    { 62, 2, "З", 4, 2 },
                    { 61, 2, "Ж", 4, 2 },
                    { 60, 2, "Е", 1, 2 },
                    { 85, 2, "А", 0, 2 },
                    { 59, 2, "Д", 2, 2 },
                    { 86, 2, "Б", 0, 2 },
                    { 88, 2, "Г", 0, 2 },
                    { 113, 2, "Ю", 0, 2 },
                    { 112, 2, "Ь", 0, 2 },
                    { 111, 2, "Ъ", 0, 2 },
                    { 110, 2, "Щ", 0, 2 },
                    { 109, 2, "Ш", 0, 2 },
                    { 108, 2, "Ч", 0, 2 },
                    { 107, 2, "Ц", 0, 2 },
                    { 106, 2, "Х", 0, 2 },
                    { 105, 2, "Ф", 0, 2 },
                    { 104, 2, "У", 0, 2 },
                    { 103, 2, "Т", 0, 2 },
                    { 102, 2, "С", 0, 2 },
                    { 101, 2, "Р", 0, 2 },
                    { 100, 2, "П", 0, 2 },
                    { 99, 2, "О", 0, 2 },
                    { 98, 2, "Н", 0, 2 },
                    { 97, 2, "М", 0, 2 },
                    { 96, 2, "Л", 0, 2 },
                    { 95, 2, "К", 0, 2 },
                    { 94, 2, "Й", 0, 2 },
                    { 93, 2, "И", 0, 2 },
                    { 92, 2, "З", 0, 2 },
                    { 91, 2, "Ж", 0, 2 },
                    { 90, 2, "Е", 0, 2 },
                    { 89, 2, "Д", 0, 2 },
                    { 87, 2, "В", 0, 2 },
                    { 58, 2, "Г", 3, 2 },
                    { 57, 2, "В", 2, 2 },
                    { 56, 2, "Б", 2, 2 },
                    { 26, 1, "Y", 4, 1 },
                    { 25, 1, "X", 8, 1 },
                    { 24, 1, "W", 4, 1 },
                    { 23, 1, "V", 4, 1 },
                    { 22, 1, "U", 1, 1 },
                    { 21, 1, "T", 1, 1 },
                    { 20, 1, "S", 1, 1 },
                    { 19, 1, "R", 1, 1 },
                    { 18, 1, "Q", 10, 1 },
                    { 17, 1, "P", 3, 1 },
                    { 16, 1, "O", 1, 1 },
                    { 15, 1, "N", 1, 1 },
                    { 14, 1, "M", 3, 1 },
                    { 13, 1, "L", 1, 1 },
                    { 12, 1, "K", 5, 1 },
                    { 11, 1, "J", 8, 1 },
                    { 10, 1, "I", 1, 1 },
                    { 9, 1, "H", 4, 1 },
                    { 8, 1, "G", 2, 1 },
                    { 7, 1, "F", 4, 1 },
                    { 6, 1, "E", 1, 1 },
                    { 5, 1, "D", 2, 1 },
                    { 4, 1, "C", 3, 1 },
                    { 3, 1, "B", 3, 1 },
                    { 2, 1, "A", 1, 1 },
                    { 27, 1, "Z", 10, 1 },
                    { 28, 1, "A", 0, 1 },
                    { 29, 1, "B", 0, 1 },
                    { 30, 1, "C", 0, 1 },
                    { 55, 2, "А", 1, 2 },
                    { 54, 2, "*", 0, 2 },
                    { 53, 1, "Z", 0, 1 },
                    { 52, 1, "Y", 0, 1 },
                    { 51, 1, "X", 0, 1 },
                    { 50, 1, "W", 0, 1 },
                    { 49, 1, "V", 0, 1 },
                    { 48, 1, "U", 0, 1 },
                    { 47, 1, "T", 0, 1 },
                    { 46, 1, "S", 0, 1 },
                    { 45, 1, "R", 0, 1 },
                    { 114, 2, "Я", 0, 2 },
                    { 44, 1, "Q", 0, 1 },
                    { 42, 1, "O", 0, 1 },
                    { 41, 1, "N", 0, 1 },
                    { 40, 1, "M", 0, 1 },
                    { 39, 1, "L", 0, 1 },
                    { 38, 1, "K", 0, 1 },
                    { 37, 1, "J", 0, 1 },
                    { 36, 1, "I", 0, 1 },
                    { 35, 1, "H", 0, 1 },
                    { 34, 1, "G", 0, 1 },
                    { 33, 1, "F", 0, 1 },
                    { 32, 1, "E", 0, 1 },
                    { 31, 1, "D", 0, 1 },
                    { 43, 1, "P", 0, 1 }
                });

            migrationBuilder.InsertData(
                table: "Games",
                columns: new[] { "ID", "BoardID", "GameLanguageID", "IsFinished", "Log", "PouchID", "WordDictionaryID" },
                values: new object[,]
                {
                    { 1, 1, 1, false, "Enjoy the game!", 1, 1 },
                    { 2, 2, 2, false, "Enjoy the game!", 2, 2 }
                });

            migrationBuilder.InsertData(
                table: "Boards",
                columns: new[] { "ID", "Columns", "GameID", "Rows" },
                values: new object[,]
                {
                    { 1, 15, 1, 15 },
                    { 2, 15, 2, 15 }
                });

            migrationBuilder.InsertData(
                table: "Pouches",
                columns: new[] { "ID", "GameID" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 2, 2 }
                });

            migrationBuilder.InsertData(
                table: "BoardTiles",
                columns: new[] { "ID", "BoardID", "BoardLocationX", "BoardLocationY", "BoardTileTypeID", "CharTileID" },
                values: new object[,]
                {
                    { 1, 1, 0, 0, 5, null },
                    { 309, 2, 8, 5, 1, null },
                    { 308, 2, 7, 5, 1, null },
                    { 307, 2, 6, 5, 1, null },
                    { 306, 2, 5, 5, 3, null },
                    { 305, 2, 4, 5, 1, null },
                    { 304, 2, 3, 5, 1, null },
                    { 303, 2, 2, 5, 1, null },
                    { 302, 2, 1, 5, 3, null },
                    { 301, 2, 0, 5, 1, null },
                    { 300, 2, 14, 4, 1, null },
                    { 299, 2, 13, 4, 1, null },
                    { 310, 2, 9, 5, 3, null },
                    { 298, 2, 12, 4, 1, null },
                    { 296, 2, 10, 4, 4, null },
                    { 295, 2, 9, 4, 1, null },
                    { 294, 2, 8, 4, 1, null },
                    { 293, 2, 7, 4, 1, null },
                    { 292, 2, 6, 4, 1, null },
                    { 291, 2, 5, 4, 1, null },
                    { 290, 2, 4, 4, 4, null },
                    { 289, 2, 3, 4, 1, null },
                    { 288, 2, 2, 4, 1, null },
                    { 287, 2, 1, 4, 1, null },
                    { 286, 2, 0, 4, 1, null },
                    { 297, 2, 11, 4, 1, null },
                    { 285, 2, 14, 3, 2, null },
                    { 311, 2, 10, 5, 1, null },
                    { 313, 2, 12, 5, 1, null },
                    { 337, 2, 6, 7, 1, null },
                    { 336, 2, 5, 7, 1, null },
                    { 335, 2, 4, 7, 1, null },
                    { 334, 2, 3, 7, 2, null },
                    { 333, 2, 2, 7, 1, null },
                    { 332, 2, 1, 7, 1, null },
                    { 331, 2, 0, 7, 5, null },
                    { 330, 2, 14, 6, 1, null },
                    { 329, 2, 13, 6, 1, null },
                    { 328, 2, 12, 6, 2, null },
                    { 327, 2, 11, 6, 1, null },
                    { 312, 2, 11, 5, 1, null },
                    { 326, 2, 10, 6, 1, null },
                    { 324, 2, 8, 6, 2, null },
                    { 323, 2, 7, 6, 1, null },
                    { 322, 2, 6, 6, 2, null },
                    { 321, 2, 5, 6, 1, null },
                    { 320, 2, 4, 6, 1, null },
                    { 319, 2, 3, 6, 1, null },
                    { 318, 2, 2, 6, 2, null },
                    { 317, 2, 1, 6, 1, null },
                    { 316, 2, 0, 6, 1, null },
                    { 315, 2, 14, 5, 1, null },
                    { 314, 2, 13, 5, 3, null },
                    { 325, 2, 9, 6, 1, null },
                    { 284, 2, 13, 3, 1, null },
                    { 283, 2, 12, 3, 1, null },
                    { 282, 2, 11, 3, 4, null },
                    { 252, 2, 11, 1, 1, null },
                    { 251, 2, 10, 1, 1, null },
                    { 250, 2, 9, 1, 3, null },
                    { 249, 2, 8, 1, 1, null },
                    { 248, 2, 7, 1, 1, null },
                    { 247, 2, 6, 1, 1, null },
                    { 246, 2, 5, 1, 3, null },
                    { 245, 2, 4, 1, 1, null },
                    { 244, 2, 3, 1, 1, null },
                    { 243, 2, 2, 1, 1, null },
                    { 242, 2, 1, 1, 4, null },
                    { 253, 2, 12, 1, 1, null },
                    { 241, 2, 0, 1, 1, null },
                    { 239, 2, 13, 0, 1, null },
                    { 238, 2, 12, 0, 1, null },
                    { 237, 2, 11, 0, 2, null },
                    { 236, 2, 10, 0, 1, null },
                    { 235, 2, 9, 0, 1, null },
                    { 234, 2, 8, 0, 1, null },
                    { 233, 2, 7, 0, 5, null },
                    { 232, 2, 6, 0, 1, null },
                    { 231, 2, 5, 0, 1, null },
                    { 230, 2, 4, 0, 1, null },
                    { 229, 2, 3, 0, 2, null },
                    { 240, 2, 14, 0, 5, null },
                    { 254, 2, 13, 1, 4, null },
                    { 255, 2, 14, 1, 1, null },
                    { 256, 2, 0, 2, 1, null },
                    { 281, 2, 10, 3, 1, null },
                    { 280, 2, 9, 3, 1, null },
                    { 279, 2, 8, 3, 1, null },
                    { 278, 2, 7, 3, 2, null },
                    { 277, 2, 6, 3, 1, null },
                    { 276, 2, 5, 3, 1, null },
                    { 275, 2, 4, 3, 1, null },
                    { 274, 2, 3, 3, 4, null },
                    { 273, 2, 2, 3, 1, null },
                    { 272, 2, 1, 3, 1, null },
                    { 271, 2, 0, 3, 2, null },
                    { 270, 2, 14, 2, 1, null },
                    { 269, 2, 13, 2, 1, null },
                    { 268, 2, 12, 2, 4, null },
                    { 267, 2, 11, 2, 1, null },
                    { 266, 2, 10, 2, 1, null },
                    { 265, 2, 9, 2, 1, null },
                    { 264, 2, 8, 2, 2, null },
                    { 263, 2, 7, 2, 1, null },
                    { 262, 2, 6, 2, 2, null },
                    { 261, 2, 5, 2, 1, null },
                    { 260, 2, 4, 2, 1, null },
                    { 259, 2, 3, 2, 1, null },
                    { 258, 2, 2, 2, 4, null },
                    { 257, 2, 1, 2, 1, null },
                    { 338, 2, 7, 7, 6, null },
                    { 228, 2, 2, 0, 1, null },
                    { 339, 2, 8, 7, 1, null },
                    { 341, 2, 10, 7, 1, null },
                    { 422, 2, 1, 13, 4, null },
                    { 421, 2, 0, 13, 1, null },
                    { 420, 2, 14, 12, 1, null },
                    { 419, 2, 13, 12, 1, null },
                    { 418, 2, 12, 12, 4, null },
                    { 417, 2, 11, 12, 1, null },
                    { 416, 2, 10, 12, 1, null },
                    { 415, 2, 9, 12, 1, null },
                    { 414, 2, 8, 12, 2, null },
                    { 413, 2, 7, 12, 1, null },
                    { 412, 2, 6, 12, 2, null },
                    { 423, 2, 2, 13, 1, null },
                    { 411, 2, 5, 12, 1, null },
                    { 409, 2, 3, 12, 1, null },
                    { 408, 2, 2, 12, 4, null },
                    { 407, 2, 1, 12, 1, null },
                    { 406, 2, 0, 12, 1, null },
                    { 405, 2, 14, 11, 2, null },
                    { 404, 2, 13, 11, 1, null },
                    { 403, 2, 12, 11, 1, null },
                    { 402, 2, 11, 11, 4, null },
                    { 401, 2, 10, 11, 1, null },
                    { 400, 2, 9, 11, 1, null },
                    { 399, 2, 8, 11, 1, null },
                    { 410, 2, 4, 12, 1, null },
                    { 398, 2, 7, 11, 2, null },
                    { 424, 2, 3, 13, 1, null },
                    { 426, 2, 5, 13, 3, null },
                    { 450, 2, 14, 14, 5, null },
                    { 449, 2, 13, 14, 1, null },
                    { 448, 2, 12, 14, 1, null },
                    { 447, 2, 11, 14, 2, null },
                    { 446, 2, 10, 14, 1, null },
                    { 445, 2, 9, 14, 1, null },
                    { 444, 2, 8, 14, 1, null },
                    { 443, 2, 7, 14, 5, null },
                    { 442, 2, 6, 14, 1, null },
                    { 441, 2, 5, 14, 1, null },
                    { 440, 2, 4, 14, 1, null },
                    { 425, 2, 4, 13, 1, null },
                    { 439, 2, 3, 14, 2, null },
                    { 437, 2, 1, 14, 1, null },
                    { 436, 2, 0, 14, 5, null },
                    { 435, 2, 14, 13, 1, null },
                    { 434, 2, 13, 13, 4, null },
                    { 433, 2, 12, 13, 1, null },
                    { 432, 2, 11, 13, 1, null },
                    { 431, 2, 10, 13, 1, null },
                    { 430, 2, 9, 13, 3, null },
                    { 429, 2, 8, 13, 1, null },
                    { 428, 2, 7, 13, 1, null },
                    { 427, 2, 6, 13, 1, null },
                    { 438, 2, 2, 14, 1, null },
                    { 397, 2, 6, 11, 1, null },
                    { 396, 2, 5, 11, 1, null },
                    { 395, 2, 4, 11, 1, null },
                    { 365, 2, 4, 9, 1, null },
                    { 364, 2, 3, 9, 1, null },
                    { 363, 2, 2, 9, 1, null },
                    { 362, 2, 1, 9, 3, null },
                    { 361, 2, 0, 9, 1, null },
                    { 360, 2, 14, 8, 1, null },
                    { 359, 2, 13, 8, 1, null },
                    { 358, 2, 12, 8, 2, null },
                    { 357, 2, 11, 8, 1, null },
                    { 356, 2, 10, 8, 1, null },
                    { 355, 2, 9, 8, 1, null },
                    { 366, 2, 5, 9, 3, null },
                    { 354, 2, 8, 8, 2, null },
                    { 352, 2, 6, 8, 2, null },
                    { 351, 2, 5, 8, 1, null },
                    { 350, 2, 4, 8, 1, null },
                    { 349, 2, 3, 8, 1, null },
                    { 348, 2, 2, 8, 2, null },
                    { 347, 2, 1, 8, 1, null },
                    { 346, 2, 0, 8, 1, null },
                    { 345, 2, 14, 7, 5, null },
                    { 344, 2, 13, 7, 1, null },
                    { 343, 2, 12, 7, 1, null },
                    { 342, 2, 11, 7, 2, null },
                    { 353, 2, 7, 8, 1, null },
                    { 367, 2, 6, 9, 1, null },
                    { 368, 2, 7, 9, 1, null },
                    { 369, 2, 8, 9, 1, null },
                    { 394, 2, 3, 11, 4, null },
                    { 393, 2, 2, 11, 1, null },
                    { 392, 2, 1, 11, 1, null },
                    { 391, 2, 0, 11, 2, null },
                    { 390, 2, 14, 10, 1, null },
                    { 389, 2, 13, 10, 1, null },
                    { 388, 2, 12, 10, 1, null },
                    { 387, 2, 11, 10, 1, null },
                    { 386, 2, 10, 10, 4, null },
                    { 385, 2, 9, 10, 1, null },
                    { 384, 2, 8, 10, 1, null },
                    { 383, 2, 7, 10, 1, null },
                    { 382, 2, 6, 10, 1, null },
                    { 381, 2, 5, 10, 1, null },
                    { 380, 2, 4, 10, 4, null },
                    { 379, 2, 3, 10, 1, null },
                    { 378, 2, 2, 10, 1, null },
                    { 377, 2, 1, 10, 1, null },
                    { 376, 2, 0, 10, 1, null },
                    { 375, 2, 14, 9, 1, null },
                    { 374, 2, 13, 9, 3, null },
                    { 373, 2, 12, 9, 1, null },
                    { 372, 2, 11, 9, 1, null },
                    { 371, 2, 10, 9, 1, null },
                    { 370, 2, 9, 9, 3, null },
                    { 340, 2, 9, 7, 1, null },
                    { 227, 2, 1, 0, 1, null },
                    { 226, 2, 0, 0, 5, null },
                    { 113, 1, 7, 7, 6, null },
                    { 30, 1, 14, 1, 1, null },
                    { 112, 1, 6, 7, 1, null },
                    { 111, 1, 5, 7, 1, null },
                    { 110, 1, 4, 7, 1, null },
                    { 109, 1, 3, 7, 2, null },
                    { 108, 1, 2, 7, 1, null },
                    { 107, 1, 1, 7, 1, null },
                    { 106, 1, 0, 7, 5, null },
                    { 105, 1, 14, 6, 1, null },
                    { 104, 1, 13, 6, 1, null },
                    { 103, 1, 12, 6, 2, null },
                    { 114, 1, 8, 7, 1, null },
                    { 102, 1, 11, 6, 1, null },
                    { 100, 1, 9, 6, 1, null },
                    { 99, 1, 8, 6, 2, null },
                    { 98, 1, 7, 6, 1, null },
                    { 97, 1, 6, 6, 2, null },
                    { 96, 1, 5, 6, 1, null },
                    { 95, 1, 4, 6, 1, null },
                    { 94, 1, 3, 6, 1, null },
                    { 93, 1, 2, 6, 2, null },
                    { 92, 1, 1, 6, 1, null },
                    { 91, 1, 0, 6, 1, null },
                    { 90, 1, 14, 5, 1, null },
                    { 101, 1, 10, 6, 1, null },
                    { 89, 1, 13, 5, 3, null },
                    { 115, 1, 9, 7, 1, null },
                    { 117, 1, 11, 7, 2, null },
                    { 141, 1, 5, 9, 3, null },
                    { 140, 1, 4, 9, 1, null },
                    { 139, 1, 3, 9, 1, null },
                    { 138, 1, 2, 9, 1, null },
                    { 137, 1, 1, 9, 3, null },
                    { 136, 1, 0, 9, 1, null },
                    { 135, 1, 14, 8, 1, null },
                    { 134, 1, 13, 8, 1, null },
                    { 133, 1, 12, 8, 2, null },
                    { 132, 1, 11, 8, 1, null },
                    { 131, 1, 10, 8, 1, null },
                    { 116, 1, 10, 7, 1, null },
                    { 130, 1, 9, 8, 1, null },
                    { 128, 1, 7, 8, 1, null },
                    { 127, 1, 6, 8, 2, null },
                    { 126, 1, 5, 8, 1, null },
                    { 125, 1, 4, 8, 1, null },
                    { 124, 1, 3, 8, 1, null },
                    { 123, 1, 2, 8, 2, null },
                    { 122, 1, 1, 8, 1, null },
                    { 121, 1, 0, 8, 1, null },
                    { 120, 1, 14, 7, 5, null },
                    { 119, 1, 13, 7, 1, null },
                    { 118, 1, 12, 7, 1, null },
                    { 129, 1, 8, 8, 2, null },
                    { 88, 1, 12, 5, 1, null },
                    { 87, 1, 11, 5, 1, null },
                    { 86, 1, 10, 5, 1, null },
                    { 56, 1, 10, 3, 1, null },
                    { 55, 1, 9, 3, 1, null },
                    { 54, 1, 8, 3, 1, null },
                    { 53, 1, 7, 3, 2, null },
                    { 52, 1, 6, 3, 1, null },
                    { 51, 1, 5, 3, 1, null },
                    { 50, 1, 4, 3, 1, null },
                    { 49, 1, 3, 3, 4, null },
                    { 48, 1, 2, 3, 1, null },
                    { 47, 1, 1, 3, 1, null },
                    { 46, 1, 0, 3, 2, null },
                    { 57, 1, 11, 3, 4, null },
                    { 45, 1, 14, 2, 1, null },
                    { 43, 1, 12, 2, 4, null },
                    { 42, 1, 11, 2, 1, null },
                    { 41, 1, 10, 2, 1, null },
                    { 40, 1, 9, 2, 1, null },
                    { 39, 1, 8, 2, 2, null },
                    { 38, 1, 7, 2, 1, null },
                    { 37, 1, 6, 2, 2, null },
                    { 36, 1, 5, 2, 1, null },
                    { 35, 1, 4, 2, 1, null },
                    { 34, 1, 3, 2, 1, null },
                    { 33, 1, 2, 2, 4, null },
                    { 44, 1, 13, 2, 1, null },
                    { 58, 1, 12, 3, 1, null },
                    { 59, 1, 13, 3, 1, null },
                    { 60, 1, 14, 3, 2, null },
                    { 85, 1, 9, 5, 3, null },
                    { 84, 1, 8, 5, 1, null },
                    { 83, 1, 7, 5, 1, null },
                    { 82, 1, 6, 5, 1, null },
                    { 81, 1, 5, 5, 3, null },
                    { 80, 1, 4, 5, 1, null },
                    { 79, 1, 3, 5, 1, null },
                    { 78, 1, 2, 5, 1, null },
                    { 77, 1, 1, 5, 3, null },
                    { 76, 1, 0, 5, 1, null },
                    { 75, 1, 14, 4, 1, null },
                    { 74, 1, 13, 4, 1, null },
                    { 73, 1, 12, 4, 1, null },
                    { 72, 1, 11, 4, 1, null },
                    { 71, 1, 10, 4, 4, null },
                    { 70, 1, 9, 4, 1, null },
                    { 69, 1, 8, 4, 1, null },
                    { 68, 1, 7, 4, 1, null },
                    { 67, 1, 6, 4, 1, null },
                    { 66, 1, 5, 4, 1, null },
                    { 65, 1, 4, 4, 4, null },
                    { 64, 1, 3, 4, 1, null },
                    { 63, 1, 2, 4, 1, null },
                    { 62, 1, 1, 4, 1, null },
                    { 61, 1, 0, 4, 1, null },
                    { 142, 1, 6, 9, 1, null },
                    { 143, 1, 7, 9, 1, null },
                    { 144, 1, 8, 9, 1, null },
                    { 145, 1, 9, 9, 3, null },
                    { 225, 1, 14, 14, 5, null },
                    { 224, 1, 13, 14, 1, null },
                    { 223, 1, 12, 14, 1, null },
                    { 222, 1, 11, 14, 2, null },
                    { 221, 1, 10, 14, 1, null },
                    { 220, 1, 9, 14, 1, null },
                    { 219, 1, 8, 14, 1, null },
                    { 218, 1, 7, 14, 5, null },
                    { 217, 1, 6, 14, 1, null }
                });

            migrationBuilder.InsertData(
                table: "BoardTiles",
                columns: new[] { "ID", "BoardID", "BoardLocationX", "BoardLocationY", "BoardTileTypeID", "CharTileID" },
                values: new object[,]
                {
                    { 216, 1, 5, 14, 1, null },
                    { 215, 1, 4, 14, 1, null },
                    { 29, 1, 13, 1, 4, null },
                    { 214, 1, 3, 14, 2, null },
                    { 212, 1, 1, 14, 1, null },
                    { 211, 1, 0, 14, 5, null },
                    { 210, 1, 14, 13, 1, null },
                    { 209, 1, 13, 13, 4, null },
                    { 208, 1, 12, 13, 1, null },
                    { 207, 1, 11, 13, 1, null },
                    { 206, 1, 10, 13, 1, null },
                    { 205, 1, 9, 13, 3, null },
                    { 204, 1, 8, 13, 1, null },
                    { 203, 1, 7, 13, 1, null },
                    { 202, 1, 6, 13, 1, null },
                    { 213, 1, 2, 14, 1, null },
                    { 32, 1, 1, 2, 1, null },
                    { 28, 1, 12, 1, 1, null },
                    { 26, 1, 10, 1, 1, null },
                    { 2, 1, 1, 0, 1, null },
                    { 3, 1, 2, 0, 1, null },
                    { 4, 1, 3, 0, 2, null },
                    { 5, 1, 4, 0, 1, null },
                    { 6, 1, 5, 0, 1, null },
                    { 7, 1, 6, 0, 1, null },
                    { 8, 1, 7, 0, 5, null },
                    { 9, 1, 8, 0, 1, null },
                    { 10, 1, 9, 0, 1, null },
                    { 11, 1, 10, 0, 1, null },
                    { 12, 1, 11, 0, 2, null },
                    { 27, 1, 11, 1, 1, null },
                    { 13, 1, 12, 0, 1, null },
                    { 15, 1, 14, 0, 5, null },
                    { 16, 1, 0, 1, 1, null },
                    { 17, 1, 1, 1, 4, null },
                    { 18, 1, 2, 1, 1, null },
                    { 19, 1, 3, 1, 1, null },
                    { 20, 1, 4, 1, 1, null },
                    { 21, 1, 5, 1, 3, null },
                    { 22, 1, 6, 1, 1, null },
                    { 23, 1, 7, 1, 1, null },
                    { 24, 1, 8, 1, 1, null },
                    { 25, 1, 9, 1, 3, null },
                    { 14, 1, 13, 0, 1, null },
                    { 200, 1, 4, 13, 1, null },
                    { 201, 1, 5, 13, 3, null },
                    { 198, 1, 2, 13, 1, null },
                    { 169, 1, 3, 11, 4, null },
                    { 168, 1, 2, 11, 1, null },
                    { 167, 1, 1, 11, 1, null },
                    { 166, 1, 0, 11, 2, null },
                    { 165, 1, 14, 10, 1, null },
                    { 164, 1, 13, 10, 1, null },
                    { 163, 1, 12, 10, 1, null },
                    { 162, 1, 11, 10, 1, null },
                    { 161, 1, 10, 10, 4, null },
                    { 160, 1, 9, 10, 1, null },
                    { 159, 1, 8, 10, 1, null },
                    { 199, 1, 3, 13, 1, null },
                    { 158, 1, 7, 10, 1, null },
                    { 156, 1, 5, 10, 1, null },
                    { 155, 1, 4, 10, 4, null },
                    { 154, 1, 3, 10, 1, null },
                    { 153, 1, 2, 10, 1, null },
                    { 152, 1, 1, 10, 1, null },
                    { 151, 1, 0, 10, 1, null },
                    { 150, 1, 14, 9, 1, null },
                    { 149, 1, 13, 9, 3, null },
                    { 148, 1, 12, 9, 1, null },
                    { 147, 1, 11, 9, 1, null },
                    { 146, 1, 10, 9, 1, null },
                    { 157, 1, 6, 10, 1, null },
                    { 171, 1, 5, 11, 1, null },
                    { 170, 1, 4, 11, 1, null },
                    { 173, 1, 7, 11, 2, null },
                    { 197, 1, 1, 13, 4, null },
                    { 196, 1, 0, 13, 1, null },
                    { 195, 1, 14, 12, 1, null },
                    { 194, 1, 13, 12, 1, null },
                    { 193, 1, 12, 12, 4, null },
                    { 192, 1, 11, 12, 1, null },
                    { 191, 1, 10, 12, 1, null },
                    { 190, 1, 9, 12, 1, null },
                    { 189, 1, 8, 12, 2, null },
                    { 188, 1, 7, 12, 1, null },
                    { 172, 1, 6, 11, 1, null },
                    { 186, 1, 5, 12, 1, null },
                    { 187, 1, 6, 12, 2, null },
                    { 184, 1, 3, 12, 1, null },
                    { 183, 1, 2, 12, 4, null },
                    { 182, 1, 1, 12, 1, null },
                    { 181, 1, 0, 12, 1, null },
                    { 180, 1, 14, 11, 2, null },
                    { 179, 1, 13, 11, 1, null },
                    { 178, 1, 12, 11, 1, null },
                    { 177, 1, 11, 11, 4, null },
                    { 176, 1, 10, 11, 1, null },
                    { 175, 1, 9, 11, 1, null },
                    { 174, 1, 8, 11, 1, null },
                    { 185, 1, 4, 12, 1, null },
                    { 31, 1, 0, 2, 1, null }
                });

            migrationBuilder.InsertData(
                table: "Players",
                columns: new[] { "ID", "AtHand", "GameID", "IsHuman", "Name", "PouchID", "RackID", "Score", "SkipsOrRedrawsUsed" },
                values: new object[,]
                {
                    { 5, true, 2, true, null, 2, 5, 0, 0 },
                    { 6, false, 2, false, null, 2, 6, 0, 0 },
                    { 2, false, 1, false, "High Scorer Bot", 1, 2, 0, 0 },
                    { 1, true, 1, true, "Simeon", 1, 1, 0, 0 },
                    { 4, false, 1, false, "Rack Balancer Bot", 1, 4, 0, 0 },
                    { 3, false, 1, true, "Dobromir", 1, 3, 0, 0 }
                });

            migrationBuilder.InsertData(
                table: "Pouch_CharTiles",
                columns: new[] { "ID", "CharTileID", "Count", "PouchID" },
                values: new object[,]
                {
                    { 56, 56, 3, 2 },
                    { 57, 57, 4, 2 },
                    { 58, 58, 3, 2 },
                    { 59, 59, 2, 2 },
                    { 60, 60, 8, 2 },
                    { 61, 61, 2, 2 },
                    { 62, 62, 2, 2 },
                    { 63, 63, 8, 2 },
                    { 64, 64, 1, 2 },
                    { 65, 65, 3, 2 },
                    { 66, 66, 3, 2 },
                    { 67, 67, 4, 2 },
                    { 68, 68, 4, 2 },
                    { 70, 70, 4, 2 },
                    { 71, 71, 4, 2 },
                    { 72, 72, 4, 2 },
                    { 73, 73, 5, 2 },
                    { 74, 74, 3, 2 },
                    { 75, 75, 1, 2 },
                    { 76, 76, 1, 2 },
                    { 77, 77, 1, 2 },
                    { 78, 78, 2, 2 },
                    { 79, 79, 1, 2 },
                    { 80, 80, 1, 2 },
                    { 81, 81, 2, 2 },
                    { 82, 82, 1, 2 },
                    { 69, 69, 9, 2 },
                    { 83, 83, 1, 2 },
                    { 54, 54, 0, 2 },
                    { 1, 1, 0, 1 },
                    { 27, 27, 1, 1 },
                    { 26, 26, 2, 1 },
                    { 25, 25, 1, 1 },
                    { 24, 24, 2, 1 },
                    { 23, 23, 2, 1 },
                    { 22, 22, 4, 1 },
                    { 21, 21, 6, 1 },
                    { 20, 20, 4, 1 },
                    { 19, 19, 6, 1 },
                    { 18, 18, 1, 1 },
                    { 17, 17, 2, 1 },
                    { 16, 16, 8, 1 },
                    { 55, 55, 12, 2 },
                    { 15, 15, 6, 1 },
                    { 13, 13, 4, 1 },
                    { 12, 12, 1, 1 },
                    { 11, 11, 1, 1 },
                    { 10, 10, 9, 1 },
                    { 9, 9, 2, 1 },
                    { 8, 8, 3, 1 },
                    { 7, 7, 2, 1 },
                    { 6, 6, 12, 1 },
                    { 5, 5, 4, 1 },
                    { 4, 4, 2, 1 },
                    { 3, 3, 2, 1 },
                    { 2, 2, 9, 1 },
                    { 14, 14, 2, 1 },
                    { 84, 84, 2, 2 }
                });

            migrationBuilder.InsertData(
                table: "Racks",
                columns: new[] { "ID", "GameID", "PlayerID", "PouchID", "RackSize" },
                values: new object[,]
                {
                    { 1, 1, 1, 1, 7 },
                    { 2, 1, 2, 1, 7 },
                    { 3, 1, 3, 1, 7 },
                    { 4, 1, 4, 1, 7 },
                    { 5, 2, 5, 2, 7 },
                    { 6, 2, 6, 2, 7 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Boards_GameID",
                table: "Boards",
                column: "GameID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BoardTiles_BoardID",
                table: "BoardTiles",
                column: "BoardID");

            migrationBuilder.CreateIndex(
                name: "IX_BoardTiles_BoardTileTypeID",
                table: "BoardTiles",
                column: "BoardTileTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_BoardTiles_CharTileID",
                table: "BoardTiles",
                column: "CharTileID");

            migrationBuilder.CreateIndex(
                name: "IX_CharTiles_GameLanguageID",
                table: "CharTiles",
                column: "GameLanguageID");

            migrationBuilder.CreateIndex(
                name: "IX_CharTiles_WordDictionaryID",
                table: "CharTiles",
                column: "WordDictionaryID");

            migrationBuilder.CreateIndex(
                name: "IX_Games_GameLanguageID",
                table: "Games",
                column: "GameLanguageID");

            migrationBuilder.CreateIndex(
                name: "IX_Games_WordDictionaryID",
                table: "Games",
                column: "WordDictionaryID");

            migrationBuilder.CreateIndex(
                name: "IX_Moves_GameID",
                table: "Moves",
                column: "GameID");

            migrationBuilder.CreateIndex(
                name: "IX_Moves_PlayerID",
                table: "Moves",
                column: "PlayerID");

            migrationBuilder.CreateIndex(
                name: "IX_Players_GameID",
                table: "Players",
                column: "GameID");

            migrationBuilder.CreateIndex(
                name: "IX_Players_PouchID",
                table: "Players",
                column: "PouchID");

            migrationBuilder.CreateIndex(
                name: "IX_Pouch_CharTiles_CharTileID",
                table: "Pouch_CharTiles",
                column: "CharTileID");

            migrationBuilder.CreateIndex(
                name: "IX_Pouch_CharTiles_PouchID",
                table: "Pouch_CharTiles",
                column: "PouchID");

            migrationBuilder.CreateIndex(
                name: "IX_Pouches_GameID",
                table: "Pouches",
                column: "GameID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Rack_CharTiles_CharTileID",
                table: "Rack_CharTiles",
                column: "CharTileID");

            migrationBuilder.CreateIndex(
                name: "IX_Rack_CharTiles_RackID",
                table: "Rack_CharTiles",
                column: "RackID");

            migrationBuilder.CreateIndex(
                name: "IX_Racks_GameID",
                table: "Racks",
                column: "GameID");

            migrationBuilder.CreateIndex(
                name: "IX_Racks_PlayerID",
                table: "Racks",
                column: "PlayerID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Racks_PouchID",
                table: "Racks",
                column: "PouchID");

            migrationBuilder.CreateIndex(
                name: "IX_WordDictionaries_GameLanguageID",
                table: "WordDictionaries",
                column: "GameLanguageID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BoardTiles");

            migrationBuilder.DropTable(
                name: "Moves");

            migrationBuilder.DropTable(
                name: "Pouch_CharTiles");

            migrationBuilder.DropTable(
                name: "Rack_CharTiles");

            migrationBuilder.DropTable(
                name: "Boards");

            migrationBuilder.DropTable(
                name: "BoardTileType");

            migrationBuilder.DropTable(
                name: "CharTiles");

            migrationBuilder.DropTable(
                name: "Racks");

            migrationBuilder.DropTable(
                name: "Players");

            migrationBuilder.DropTable(
                name: "Pouches");

            migrationBuilder.DropTable(
                name: "Games");

            migrationBuilder.DropTable(
                name: "WordDictionaries");

            migrationBuilder.DropTable(
                name: "GameLanguages");
        }
    }
}
