﻿using Microsoft.EntityFrameworkCore.Metadata;
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
                    Language = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameLanguages", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "CharTiles",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Letter = table.Column<string>(nullable: false),
                    Score = table.Column<int>(nullable: false),
                    GameLanguageID = table.Column<int>(nullable: false)
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
                });

            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    GameLanguageID = table.Column<int>(nullable: false),
                    BoardID = table.Column<int>(nullable: false),
                    PouchID = table.Column<int>(nullable: false)
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
                name: "Pouchs",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    GameID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pouchs", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Pouchs_Games_GameID",
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
                        name: "FK_Players_Pouchs_PouchID",
                        column: x => x.PouchID,
                        principalTable: "Pouchs",
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
                        name: "FK_Pouch_CharTiles_Pouchs_PouchID",
                        column: x => x.PouchID,
                        principalTable: "Pouchs",
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
                        name: "FK_Racks_Pouchs_PouchID",
                        column: x => x.PouchID,
                        principalTable: "Pouchs",
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
                values: new object[] { 1, "Normal" });

            migrationBuilder.InsertData(
                table: "GameLanguages",
                columns: new[] { "ID", "Language" },
                values: new object[] { 1, "English" });

            migrationBuilder.InsertData(
                table: "CharTiles",
                columns: new[] { "ID", "GameLanguageID", "Letter", "Score" },
                values: new object[,]
                {
                    { 1, 1, "*", 0 },
                    { 26, 1, "Y", 4 },
                    { 25, 1, "X", 8 },
                    { 24, 1, "W", 4 },
                    { 23, 1, "V", 4 },
                    { 22, 1, "U", 1 },
                    { 21, 1, "T", 1 },
                    { 20, 1, "S", 1 },
                    { 19, 1, "R", 1 },
                    { 18, 1, "Q", 10 },
                    { 17, 1, "P", 3 },
                    { 16, 1, "O", 1 },
                    { 15, 1, "N", 1 },
                    { 14, 1, "M", 3 },
                    { 13, 1, "L", 1 },
                    { 12, 1, "K", 5 },
                    { 11, 1, "J", 8 },
                    { 10, 1, "I", 1 },
                    { 9, 1, "H", 4 },
                    { 8, 1, "G", 2 },
                    { 7, 1, "F", 4 },
                    { 6, 1, "E", 1 },
                    { 5, 1, "D", 2 },
                    { 4, 1, "C", 3 },
                    { 3, 1, "B", 3 },
                    { 2, 1, "A", 1 },
                    { 27, 1, "Z", 10 }
                });

            migrationBuilder.InsertData(
                table: "Games",
                columns: new[] { "ID", "BoardID", "GameLanguageID", "PouchID" },
                values: new object[] { 1, 1, 1, 1 });

            migrationBuilder.InsertData(
                table: "Boards",
                columns: new[] { "ID", "Columns", "GameID", "Rows" },
                values: new object[] { 1, 4, 1, 4 });

            migrationBuilder.InsertData(
                table: "Pouchs",
                columns: new[] { "ID", "GameID" },
                values: new object[] { 1, 1 });

            migrationBuilder.InsertData(
                table: "BoardTiles",
                columns: new[] { "ID", "BoardID", "BoardLocationX", "BoardLocationY", "BoardTileTypeID", "CharTileID" },
                values: new object[,]
                {
                    { 1, 1, 0, 0, 1, null },
                    { 145, 1, 9, 9, 1, null },
                    { 146, 1, 10, 9, 1, null },
                    { 147, 1, 11, 9, 1, null },
                    { 148, 1, 12, 9, 1, null },
                    { 149, 1, 13, 9, 1, null },
                    { 150, 1, 14, 9, 1, null },
                    { 151, 1, 0, 10, 1, null },
                    { 152, 1, 1, 10, 1, null },
                    { 153, 1, 2, 10, 1, null },
                    { 154, 1, 3, 10, 1, null },
                    { 155, 1, 4, 10, 1, null },
                    { 144, 1, 8, 9, 1, null },
                    { 156, 1, 5, 10, 1, null },
                    { 158, 1, 7, 10, 1, null },
                    { 159, 1, 8, 10, 1, null },
                    { 160, 1, 9, 10, 1, null },
                    { 161, 1, 10, 10, 1, null },
                    { 162, 1, 11, 10, 1, null },
                    { 163, 1, 12, 10, 1, null },
                    { 164, 1, 13, 10, 1, null },
                    { 165, 1, 14, 10, 1, null },
                    { 166, 1, 0, 11, 1, null },
                    { 167, 1, 1, 11, 1, null },
                    { 168, 1, 2, 11, 1, null },
                    { 157, 1, 6, 10, 1, null },
                    { 169, 1, 3, 11, 1, null },
                    { 143, 1, 7, 9, 1, null },
                    { 141, 1, 5, 9, 1, null },
                    { 117, 1, 11, 7, 1, null },
                    { 118, 1, 12, 7, 1, null },
                    { 119, 1, 13, 7, 1, null },
                    { 120, 1, 14, 7, 1, null },
                    { 121, 1, 0, 8, 1, null },
                    { 122, 1, 1, 8, 1, null },
                    { 123, 1, 2, 8, 1, null },
                    { 124, 1, 3, 8, 1, null },
                    { 125, 1, 4, 8, 1, null },
                    { 126, 1, 5, 8, 1, null },
                    { 127, 1, 6, 8, 1, null },
                    { 142, 1, 6, 9, 1, null },
                    { 128, 1, 7, 8, 1, null },
                    { 130, 1, 9, 8, 1, null },
                    { 131, 1, 10, 8, 1, null },
                    { 132, 1, 11, 8, 1, null },
                    { 133, 1, 12, 8, 1, null },
                    { 134, 1, 13, 8, 1, null },
                    { 135, 1, 14, 8, 1, null },
                    { 136, 1, 0, 9, 1, null },
                    { 137, 1, 1, 9, 1, null },
                    { 138, 1, 2, 9, 1, null },
                    { 139, 1, 3, 9, 1, null },
                    { 140, 1, 4, 9, 1, null },
                    { 129, 1, 8, 8, 1, null },
                    { 170, 1, 4, 11, 1, null },
                    { 171, 1, 5, 11, 1, null },
                    { 172, 1, 6, 11, 1, null },
                    { 202, 1, 6, 13, 1, null },
                    { 203, 1, 7, 13, 1, null },
                    { 204, 1, 8, 13, 1, null },
                    { 205, 1, 9, 13, 1, null },
                    { 206, 1, 10, 13, 1, null },
                    { 207, 1, 11, 13, 1, null },
                    { 208, 1, 12, 13, 1, null },
                    { 209, 1, 13, 13, 1, null },
                    { 210, 1, 14, 13, 1, null },
                    { 211, 1, 0, 14, 1, null },
                    { 212, 1, 1, 14, 1, null },
                    { 201, 1, 5, 13, 1, null },
                    { 213, 1, 2, 14, 1, null },
                    { 215, 1, 4, 14, 1, null },
                    { 216, 1, 5, 14, 1, null },
                    { 217, 1, 6, 14, 1, null },
                    { 218, 1, 7, 14, 1, null },
                    { 219, 1, 8, 14, 1, null },
                    { 220, 1, 9, 14, 1, null },
                    { 221, 1, 10, 14, 1, null },
                    { 222, 1, 11, 14, 1, null },
                    { 223, 1, 12, 14, 1, null },
                    { 224, 1, 13, 14, 1, null },
                    { 225, 1, 14, 14, 1, null },
                    { 214, 1, 3, 14, 1, null },
                    { 200, 1, 4, 13, 1, null },
                    { 199, 1, 3, 13, 1, null },
                    { 198, 1, 2, 13, 1, null },
                    { 173, 1, 7, 11, 1, null },
                    { 174, 1, 8, 11, 1, null },
                    { 175, 1, 9, 11, 1, null },
                    { 176, 1, 10, 11, 1, null },
                    { 177, 1, 11, 11, 1, null },
                    { 178, 1, 12, 11, 1, null },
                    { 179, 1, 13, 11, 1, null },
                    { 180, 1, 14, 11, 1, null },
                    { 181, 1, 0, 12, 1, null },
                    { 182, 1, 1, 12, 1, null },
                    { 183, 1, 2, 12, 1, null },
                    { 184, 1, 3, 12, 1, null },
                    { 185, 1, 4, 12, 1, null },
                    { 186, 1, 5, 12, 1, null },
                    { 187, 1, 6, 12, 1, null },
                    { 188, 1, 7, 12, 1, null },
                    { 189, 1, 8, 12, 1, null },
                    { 190, 1, 9, 12, 1, null },
                    { 191, 1, 10, 12, 1, null },
                    { 192, 1, 11, 12, 1, null },
                    { 193, 1, 12, 12, 1, null },
                    { 194, 1, 13, 12, 1, null },
                    { 195, 1, 14, 12, 1, null },
                    { 196, 1, 0, 13, 1, null },
                    { 197, 1, 1, 13, 1, null },
                    { 116, 1, 10, 7, 1, null },
                    { 114, 1, 8, 7, 1, null },
                    { 115, 1, 9, 7, 1, null },
                    { 112, 1, 6, 7, 1, null },
                    { 31, 1, 0, 2, 1, null },
                    { 32, 1, 1, 2, 1, null },
                    { 33, 1, 2, 2, 1, null },
                    { 34, 1, 3, 2, 1, null },
                    { 35, 1, 4, 2, 1, null },
                    { 36, 1, 5, 2, 1, null },
                    { 37, 1, 6, 2, 1, null },
                    { 38, 1, 7, 2, 1, null },
                    { 39, 1, 8, 2, 1, null },
                    { 40, 1, 9, 2, 1, null },
                    { 41, 1, 10, 2, 1, null },
                    { 30, 1, 14, 1, 1, null },
                    { 42, 1, 11, 2, 1, null },
                    { 44, 1, 13, 2, 1, null },
                    { 45, 1, 14, 2, 1, null },
                    { 46, 1, 0, 3, 1, null },
                    { 47, 1, 1, 3, 1, null },
                    { 48, 1, 2, 3, 1, null },
                    { 49, 1, 3, 3, 1, null },
                    { 50, 1, 4, 3, 1, null },
                    { 51, 1, 5, 3, 1, null },
                    { 52, 1, 6, 3, 1, null },
                    { 53, 1, 7, 3, 1, null },
                    { 54, 1, 8, 3, 1, null },
                    { 43, 1, 12, 2, 1, null },
                    { 29, 1, 13, 1, 1, null },
                    { 28, 1, 12, 1, 1, null },
                    { 27, 1, 11, 1, 1, null },
                    { 2, 1, 1, 0, 1, null },
                    { 3, 1, 2, 0, 1, null },
                    { 4, 1, 3, 0, 1, null },
                    { 5, 1, 4, 0, 1, null },
                    { 6, 1, 5, 0, 1, null },
                    { 7, 1, 6, 0, 1, null },
                    { 8, 1, 7, 0, 1, null },
                    { 9, 1, 8, 0, 1, null },
                    { 10, 1, 9, 0, 1, null },
                    { 11, 1, 10, 0, 1, null },
                    { 12, 1, 11, 0, 1, null },
                    { 13, 1, 12, 0, 1, null },
                    { 14, 1, 13, 0, 1, null },
                    { 15, 1, 14, 0, 1, null },
                    { 16, 1, 0, 1, 1, null },
                    { 17, 1, 1, 1, 1, null },
                    { 18, 1, 2, 1, 1, null },
                    { 19, 1, 3, 1, 1, null },
                    { 20, 1, 4, 1, 1, null },
                    { 21, 1, 5, 1, 1, null },
                    { 22, 1, 6, 1, 1, null },
                    { 23, 1, 7, 1, 1, null },
                    { 24, 1, 8, 1, 1, null },
                    { 25, 1, 9, 1, 1, null },
                    { 26, 1, 10, 1, 1, null },
                    { 55, 1, 9, 3, 1, null },
                    { 113, 1, 7, 7, 1, null },
                    { 56, 1, 10, 3, 1, null },
                    { 58, 1, 12, 3, 1, null },
                    { 88, 1, 12, 5, 1, null },
                    { 89, 1, 13, 5, 1, null },
                    { 90, 1, 14, 5, 1, null },
                    { 91, 1, 0, 6, 1, null },
                    { 92, 1, 1, 6, 1, null },
                    { 93, 1, 2, 6, 1, null },
                    { 94, 1, 3, 6, 1, null },
                    { 95, 1, 4, 6, 1, null },
                    { 96, 1, 5, 6, 1, null },
                    { 97, 1, 6, 6, 1, null },
                    { 98, 1, 7, 6, 1, null },
                    { 57, 1, 11, 3, 1, null },
                    { 99, 1, 8, 6, 1, null },
                    { 101, 1, 10, 6, 1, null },
                    { 102, 1, 11, 6, 1, null },
                    { 103, 1, 12, 6, 1, null },
                    { 104, 1, 13, 6, 1, null },
                    { 105, 1, 14, 6, 1, null },
                    { 106, 1, 0, 7, 1, null },
                    { 107, 1, 1, 7, 1, null },
                    { 108, 1, 2, 7, 1, null },
                    { 109, 1, 3, 7, 1, null },
                    { 110, 1, 4, 7, 1, null },
                    { 111, 1, 5, 7, 1, null },
                    { 100, 1, 9, 6, 1, null },
                    { 86, 1, 10, 5, 1, null },
                    { 87, 1, 11, 5, 1, null },
                    { 84, 1, 8, 5, 1, null },
                    { 59, 1, 13, 3, 1, null },
                    { 60, 1, 14, 3, 1, null },
                    { 61, 1, 0, 4, 1, null },
                    { 62, 1, 1, 4, 1, null },
                    { 63, 1, 2, 4, 1, null },
                    { 64, 1, 3, 4, 1, null },
                    { 65, 1, 4, 4, 1, null },
                    { 66, 1, 5, 4, 1, null },
                    { 67, 1, 6, 4, 1, null },
                    { 68, 1, 7, 4, 1, null },
                    { 85, 1, 9, 5, 1, null },
                    { 70, 1, 9, 4, 1, null },
                    { 69, 1, 8, 4, 1, null },
                    { 72, 1, 11, 4, 1, null },
                    { 83, 1, 7, 5, 1, null },
                    { 82, 1, 6, 5, 1, null },
                    { 81, 1, 5, 5, 1, null },
                    { 71, 1, 10, 4, 1, null },
                    { 79, 1, 3, 5, 1, null },
                    { 80, 1, 4, 5, 1, null },
                    { 77, 1, 1, 5, 1, null },
                    { 76, 1, 0, 5, 1, null },
                    { 75, 1, 14, 4, 1, null },
                    { 74, 1, 13, 4, 1, null },
                    { 73, 1, 12, 4, 1, null },
                    { 78, 1, 2, 5, 1, null }
                });

            migrationBuilder.InsertData(
                table: "Players",
                columns: new[] { "ID", "AtHand", "GameID", "IsHuman", "Name", "PouchID", "RackID", "Score" },
                values: new object[,]
                {
                    { 1, true, 1, true, null, 1, 1, 0 },
                    { 2, false, 1, false, null, 1, 2, 0 }
                });

            migrationBuilder.InsertData(
                table: "Pouch_CharTiles",
                columns: new[] { "ID", "CharTileID", "Count", "PouchID" },
                values: new object[,]
                {
                    { 1, 1, 5, 1 },
                    { 2, 2, 3, 1 }
                });

            migrationBuilder.InsertData(
                table: "Racks",
                columns: new[] { "ID", "GameID", "PlayerID", "PouchID", "RackSize" },
                values: new object[] { 1, 1, 1, 1, 7 });

            migrationBuilder.InsertData(
                table: "Racks",
                columns: new[] { "ID", "GameID", "PlayerID", "PouchID", "RackSize" },
                values: new object[] { 2, 1, 2, 1, 7 });

            migrationBuilder.InsertData(
                table: "Rack_CharTiles",
                columns: new[] { "ID", "CharTileID", "Count", "RackID" },
                values: new object[] { 1, 3, 5, 1 });

            migrationBuilder.InsertData(
                table: "Rack_CharTiles",
                columns: new[] { "ID", "CharTileID", "Count", "RackID" },
                values: new object[] { 2, 4, 4, 2 });

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
                name: "IX_Games_GameLanguageID",
                table: "Games",
                column: "GameLanguageID");

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
                name: "IX_Pouchs_GameID",
                table: "Pouchs",
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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BoardTiles");

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
                name: "Pouchs");

            migrationBuilder.DropTable(
                name: "Games");

            migrationBuilder.DropTable(
                name: "GameLanguages");
        }
    }
}
