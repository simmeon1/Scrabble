using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Scrabble.Models
{
    public class ScrabbleContext : DbContext
    {
        public ScrabbleContext (DbContextOptions<ScrabbleContext> options)
            : base(options)
        {          
        }

        public DbSet<Scrabble.Models.Game> Games { get; set; }
        public DbSet<Scrabble.Models.Board> Boards { get; set; }
        public DbSet<Scrabble.Models.BoardTile> BoardTiles { get; set; }
        public DbSet<Scrabble.Models.CharTile> CharTiles { get; set; }
        public DbSet<Scrabble.Models.Pouch> Pouchs { get; set; }
        public DbSet<Scrabble.Models.Rack> Racks { get; set; }
        //public DbSet<Scrabble.Models.User> User { get; set; }
        public DbSet<Scrabble.Models.Player> Players { get; set; }
        public DbSet<Scrabble.Models.Rack_CharTile> Rack_CharTiles { get; set; }
        public DbSet<Scrabble.Models.Pouch_CharTile> Pouch_CharTiles { get; set; }
        public DbSet<Scrabble.Models.GameLanguage> GameLanguages { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GameLanguage>().HasData(new GameLanguage { ID = 1, Language = "English" });

            modelBuilder.Entity<WordDictionary>().HasData(new WordDictionary { ID = 1, GameLanguageID = 1 });

            modelBuilder.Entity<Game>().HasData(new Game { ID = 1, GameLanguageID = 1, BoardID = 1, PouchID = 1, WordDictionaryID = 1, Log = "Enjoy the game!" });

            modelBuilder.Entity<Board>().HasData(new Board { ID = 1, Rows = 15, Columns = 15, GameID = 1 });

            modelBuilder.Entity<BoardTileType>().HasData(new BoardTileType { ID = 1, Type = "Normal" });
            modelBuilder.Entity<BoardTileType>().HasData(new BoardTileType { ID = 2, Type = "DoubleLetter" });
            modelBuilder.Entity<BoardTileType>().HasData(new BoardTileType { ID = 3, Type = "TripleLetter" });
            modelBuilder.Entity<BoardTileType>().HasData(new BoardTileType { ID = 4, Type = "DoubleWord" });
            modelBuilder.Entity<BoardTileType>().HasData(new BoardTileType { ID = 5, Type = "TripleWord" });
            modelBuilder.Entity<BoardTileType>().HasData(new BoardTileType { ID = 6, Type = "Start" });

            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 1, BoardLocationX = 0, BoardLocationY = 0, BoardID = 1, BoardTileTypeID = 5 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 2, BoardLocationX = 1, BoardLocationY = 0, BoardID = 1, BoardTileTypeID = 1 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 3, BoardLocationX = 2, BoardLocationY = 0, BoardID = 1, BoardTileTypeID = 1 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 4, BoardLocationX = 3, BoardLocationY = 0, BoardID = 1, BoardTileTypeID = 2 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 5, BoardLocationX = 4, BoardLocationY = 0, BoardID = 1, BoardTileTypeID = 1 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 6, BoardLocationX = 5, BoardLocationY = 0, BoardID = 1, BoardTileTypeID = 1 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 7, BoardLocationX = 6, BoardLocationY = 0, BoardID = 1, BoardTileTypeID = 1 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 8, BoardLocationX = 7, BoardLocationY = 0, BoardID = 1, BoardTileTypeID = 5 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 9, BoardLocationX = 8, BoardLocationY = 0, BoardID = 1, BoardTileTypeID = 1 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 10, BoardLocationX = 9, BoardLocationY = 0, BoardID = 1, BoardTileTypeID = 1 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 11, BoardLocationX = 10, BoardLocationY = 0, BoardID = 1, BoardTileTypeID = 1 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 12, BoardLocationX = 11, BoardLocationY = 0, BoardID = 1, BoardTileTypeID = 2 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 13, BoardLocationX = 12, BoardLocationY = 0, BoardID = 1, BoardTileTypeID = 1 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 14, BoardLocationX = 13, BoardLocationY = 0, BoardID = 1, BoardTileTypeID = 1 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 15, BoardLocationX = 14, BoardLocationY = 0, BoardID = 1, BoardTileTypeID = 5 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 16, BoardLocationX = 0, BoardLocationY = 1, BoardID = 1, BoardTileTypeID = 1 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 17, BoardLocationX = 1, BoardLocationY = 1, BoardID = 1, BoardTileTypeID = 4 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 18, BoardLocationX = 2, BoardLocationY = 1, BoardID = 1, BoardTileTypeID = 1 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 19, BoardLocationX = 3, BoardLocationY = 1, BoardID = 1, BoardTileTypeID = 1 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 20, BoardLocationX = 4, BoardLocationY = 1, BoardID = 1, BoardTileTypeID = 1 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 21, BoardLocationX = 5, BoardLocationY = 1, BoardID = 1, BoardTileTypeID = 3 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 22, BoardLocationX = 6, BoardLocationY = 1, BoardID = 1, BoardTileTypeID = 1 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 23, BoardLocationX = 7, BoardLocationY = 1, BoardID = 1, BoardTileTypeID = 1 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 24, BoardLocationX = 8, BoardLocationY = 1, BoardID = 1, BoardTileTypeID = 1 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 25, BoardLocationX = 9, BoardLocationY = 1, BoardID = 1, BoardTileTypeID = 3 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 26, BoardLocationX = 10, BoardLocationY = 1, BoardID = 1, BoardTileTypeID = 1 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 27, BoardLocationX = 11, BoardLocationY = 1, BoardID = 1, BoardTileTypeID = 1 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 28, BoardLocationX = 12, BoardLocationY = 1, BoardID = 1, BoardTileTypeID = 1 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 29, BoardLocationX = 13, BoardLocationY = 1, BoardID = 1, BoardTileTypeID = 4 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 30, BoardLocationX = 14, BoardLocationY = 1, BoardID = 1, BoardTileTypeID = 1 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 31, BoardLocationX = 0, BoardLocationY = 2, BoardID = 1, BoardTileTypeID = 1 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 32, BoardLocationX = 1, BoardLocationY = 2, BoardID = 1, BoardTileTypeID = 1 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 33, BoardLocationX = 2, BoardLocationY = 2, BoardID = 1, BoardTileTypeID = 4 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 34, BoardLocationX = 3, BoardLocationY = 2, BoardID = 1, BoardTileTypeID = 1 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 35, BoardLocationX = 4, BoardLocationY = 2, BoardID = 1, BoardTileTypeID = 1 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 36, BoardLocationX = 5, BoardLocationY = 2, BoardID = 1, BoardTileTypeID = 1 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 37, BoardLocationX = 6, BoardLocationY = 2, BoardID = 1, BoardTileTypeID = 2 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 38, BoardLocationX = 7, BoardLocationY = 2, BoardID = 1, BoardTileTypeID = 1 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 39, BoardLocationX = 8, BoardLocationY = 2, BoardID = 1, BoardTileTypeID = 2 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 40, BoardLocationX = 9, BoardLocationY = 2, BoardID = 1, BoardTileTypeID = 1 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 41, BoardLocationX = 10, BoardLocationY = 2, BoardID = 1, BoardTileTypeID = 1 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 42, BoardLocationX = 11, BoardLocationY = 2, BoardID = 1, BoardTileTypeID = 1 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 43, BoardLocationX = 12, BoardLocationY = 2, BoardID = 1, BoardTileTypeID = 4 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 44, BoardLocationX = 13, BoardLocationY = 2, BoardID = 1, BoardTileTypeID = 1 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 45, BoardLocationX = 14, BoardLocationY = 2, BoardID = 1, BoardTileTypeID = 1 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 46, BoardLocationX = 0, BoardLocationY = 3, BoardID = 1, BoardTileTypeID = 2 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 47, BoardLocationX = 1, BoardLocationY = 3, BoardID = 1, BoardTileTypeID = 1 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 48, BoardLocationX = 2, BoardLocationY = 3, BoardID = 1, BoardTileTypeID = 1 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 49, BoardLocationX = 3, BoardLocationY = 3, BoardID = 1, BoardTileTypeID = 4 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 50, BoardLocationX = 4, BoardLocationY = 3, BoardID = 1, BoardTileTypeID = 1 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 51, BoardLocationX = 5, BoardLocationY = 3, BoardID = 1, BoardTileTypeID = 1 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 52, BoardLocationX = 6, BoardLocationY = 3, BoardID = 1, BoardTileTypeID = 1 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 53, BoardLocationX = 7, BoardLocationY = 3, BoardID = 1, BoardTileTypeID = 2 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 54, BoardLocationX = 8, BoardLocationY = 3, BoardID = 1, BoardTileTypeID = 1 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 55, BoardLocationX = 9, BoardLocationY = 3, BoardID = 1, BoardTileTypeID = 1 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 56, BoardLocationX = 10, BoardLocationY = 3, BoardID = 1, BoardTileTypeID = 1 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 57, BoardLocationX = 11, BoardLocationY = 3, BoardID = 1, BoardTileTypeID = 4 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 58, BoardLocationX = 12, BoardLocationY = 3, BoardID = 1, BoardTileTypeID = 1 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 59, BoardLocationX = 13, BoardLocationY = 3, BoardID = 1, BoardTileTypeID = 1 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 60, BoardLocationX = 14, BoardLocationY = 3, BoardID = 1, BoardTileTypeID = 2 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 61, BoardLocationX = 0, BoardLocationY = 4, BoardID = 1, BoardTileTypeID = 1 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 62, BoardLocationX = 1, BoardLocationY = 4, BoardID = 1, BoardTileTypeID = 1 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 63, BoardLocationX = 2, BoardLocationY = 4, BoardID = 1, BoardTileTypeID = 1 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 64, BoardLocationX = 3, BoardLocationY = 4, BoardID = 1, BoardTileTypeID = 1 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 65, BoardLocationX = 4, BoardLocationY = 4, BoardID = 1, BoardTileTypeID = 4 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 66, BoardLocationX = 5, BoardLocationY = 4, BoardID = 1, BoardTileTypeID = 1 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 67, BoardLocationX = 6, BoardLocationY = 4, BoardID = 1, BoardTileTypeID = 1 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 68, BoardLocationX = 7, BoardLocationY = 4, BoardID = 1, BoardTileTypeID = 1 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 69, BoardLocationX = 8, BoardLocationY = 4, BoardID = 1, BoardTileTypeID = 1 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 70, BoardLocationX = 9, BoardLocationY = 4, BoardID = 1, BoardTileTypeID = 1 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 71, BoardLocationX = 10, BoardLocationY = 4, BoardID = 1, BoardTileTypeID = 4 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 72, BoardLocationX = 11, BoardLocationY = 4, BoardID = 1, BoardTileTypeID = 1 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 73, BoardLocationX = 12, BoardLocationY = 4, BoardID = 1, BoardTileTypeID = 1 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 74, BoardLocationX = 13, BoardLocationY = 4, BoardID = 1, BoardTileTypeID = 1 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 75, BoardLocationX = 14, BoardLocationY = 4, BoardID = 1, BoardTileTypeID = 1 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 76, BoardLocationX = 0, BoardLocationY = 5, BoardID = 1, BoardTileTypeID = 1 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 77, BoardLocationX = 1, BoardLocationY = 5, BoardID = 1, BoardTileTypeID = 3 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 78, BoardLocationX = 2, BoardLocationY = 5, BoardID = 1, BoardTileTypeID = 1 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 79, BoardLocationX = 3, BoardLocationY = 5, BoardID = 1, BoardTileTypeID = 1 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 80, BoardLocationX = 4, BoardLocationY = 5, BoardID = 1, BoardTileTypeID = 1 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 81, BoardLocationX = 5, BoardLocationY = 5, BoardID = 1, BoardTileTypeID = 3 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 82, BoardLocationX = 6, BoardLocationY = 5, BoardID = 1, BoardTileTypeID = 1 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 83, BoardLocationX = 7, BoardLocationY = 5, BoardID = 1, BoardTileTypeID = 1 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 84, BoardLocationX = 8, BoardLocationY = 5, BoardID = 1, BoardTileTypeID = 1 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 85, BoardLocationX = 9, BoardLocationY = 5, BoardID = 1, BoardTileTypeID = 3 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 86, BoardLocationX = 10, BoardLocationY = 5, BoardID = 1, BoardTileTypeID = 1 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 87, BoardLocationX = 11, BoardLocationY = 5, BoardID = 1, BoardTileTypeID = 1 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 88, BoardLocationX = 12, BoardLocationY = 5, BoardID = 1, BoardTileTypeID = 1 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 89, BoardLocationX = 13, BoardLocationY = 5, BoardID = 1, BoardTileTypeID = 3 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 90, BoardLocationX = 14, BoardLocationY = 5, BoardID = 1, BoardTileTypeID = 1 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 91, BoardLocationX = 0, BoardLocationY = 6, BoardID = 1, BoardTileTypeID = 1 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 92, BoardLocationX = 1, BoardLocationY = 6, BoardID = 1, BoardTileTypeID = 1 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 93, BoardLocationX = 2, BoardLocationY = 6, BoardID = 1, BoardTileTypeID = 2 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 94, BoardLocationX = 3, BoardLocationY = 6, BoardID = 1, BoardTileTypeID = 1 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 95, BoardLocationX = 4, BoardLocationY = 6, BoardID = 1, BoardTileTypeID = 1 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 96, BoardLocationX = 5, BoardLocationY = 6, BoardID = 1, BoardTileTypeID = 1 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 97, BoardLocationX = 6, BoardLocationY = 6, BoardID = 1, BoardTileTypeID = 2 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 98, BoardLocationX = 7, BoardLocationY = 6, BoardID = 1, BoardTileTypeID = 1 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 99, BoardLocationX = 8, BoardLocationY = 6, BoardID = 1, BoardTileTypeID = 2 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 100, BoardLocationX = 9, BoardLocationY = 6, BoardID = 1, BoardTileTypeID = 1 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 101, BoardLocationX = 10, BoardLocationY = 6, BoardID = 1, BoardTileTypeID = 1 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 102, BoardLocationX = 11, BoardLocationY = 6, BoardID = 1, BoardTileTypeID = 1 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 103, BoardLocationX = 12, BoardLocationY = 6, BoardID = 1, BoardTileTypeID = 2 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 104, BoardLocationX = 13, BoardLocationY = 6, BoardID = 1, BoardTileTypeID = 1 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 105, BoardLocationX = 14, BoardLocationY = 6, BoardID = 1, BoardTileTypeID = 1 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 106, BoardLocationX = 0, BoardLocationY = 7, BoardID = 1, BoardTileTypeID = 5 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 107, BoardLocationX = 1, BoardLocationY = 7, BoardID = 1, BoardTileTypeID = 1 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 108, BoardLocationX = 2, BoardLocationY = 7, BoardID = 1, BoardTileTypeID = 1 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 109, BoardLocationX = 3, BoardLocationY = 7, BoardID = 1, BoardTileTypeID = 2 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 110, BoardLocationX = 4, BoardLocationY = 7, BoardID = 1, BoardTileTypeID = 1 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 111, BoardLocationX = 5, BoardLocationY = 7, BoardID = 1, BoardTileTypeID = 1 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 112, BoardLocationX = 6, BoardLocationY = 7, BoardID = 1, BoardTileTypeID = 1 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 113, BoardLocationX = 7, BoardLocationY = 7, BoardID = 1, BoardTileTypeID = 6 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 114, BoardLocationX = 8, BoardLocationY = 7, BoardID = 1, BoardTileTypeID = 1 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 115, BoardLocationX = 9, BoardLocationY = 7, BoardID = 1, BoardTileTypeID = 1 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 116, BoardLocationX = 10, BoardLocationY = 7, BoardID = 1, BoardTileTypeID = 1 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 117, BoardLocationX = 11, BoardLocationY = 7, BoardID = 1, BoardTileTypeID = 2 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 118, BoardLocationX = 12, BoardLocationY = 7, BoardID = 1, BoardTileTypeID = 1 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 119, BoardLocationX = 13, BoardLocationY = 7, BoardID = 1, BoardTileTypeID = 1 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 120, BoardLocationX = 14, BoardLocationY = 7, BoardID = 1, BoardTileTypeID = 5 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 121, BoardLocationX = 0, BoardLocationY = 8, BoardID = 1, BoardTileTypeID = 1 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 122, BoardLocationX = 1, BoardLocationY = 8, BoardID = 1, BoardTileTypeID = 1 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 123, BoardLocationX = 2, BoardLocationY = 8, BoardID = 1, BoardTileTypeID = 2 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 124, BoardLocationX = 3, BoardLocationY = 8, BoardID = 1, BoardTileTypeID = 1 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 125, BoardLocationX = 4, BoardLocationY = 8, BoardID = 1, BoardTileTypeID = 1 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 126, BoardLocationX = 5, BoardLocationY = 8, BoardID = 1, BoardTileTypeID = 1 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 127, BoardLocationX = 6, BoardLocationY = 8, BoardID = 1, BoardTileTypeID = 2 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 128, BoardLocationX = 7, BoardLocationY = 8, BoardID = 1, BoardTileTypeID = 1 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 129, BoardLocationX = 8, BoardLocationY = 8, BoardID = 1, BoardTileTypeID = 2 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 130, BoardLocationX = 9, BoardLocationY = 8, BoardID = 1, BoardTileTypeID = 1 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 131, BoardLocationX = 10, BoardLocationY = 8, BoardID = 1, BoardTileTypeID = 1 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 132, BoardLocationX = 11, BoardLocationY = 8, BoardID = 1, BoardTileTypeID = 1 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 133, BoardLocationX = 12, BoardLocationY = 8, BoardID = 1, BoardTileTypeID = 2 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 134, BoardLocationX = 13, BoardLocationY = 8, BoardID = 1, BoardTileTypeID = 1 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 135, BoardLocationX = 14, BoardLocationY = 8, BoardID = 1, BoardTileTypeID = 1 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 136, BoardLocationX = 0, BoardLocationY = 9, BoardID = 1, BoardTileTypeID = 1 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 137, BoardLocationX = 1, BoardLocationY = 9, BoardID = 1, BoardTileTypeID = 3 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 138, BoardLocationX = 2, BoardLocationY = 9, BoardID = 1, BoardTileTypeID = 1 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 139, BoardLocationX = 3, BoardLocationY = 9, BoardID = 1, BoardTileTypeID = 1 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 140, BoardLocationX = 4, BoardLocationY = 9, BoardID = 1, BoardTileTypeID = 1 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 141, BoardLocationX = 5, BoardLocationY = 9, BoardID = 1, BoardTileTypeID = 3 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 142, BoardLocationX = 6, BoardLocationY = 9, BoardID = 1, BoardTileTypeID = 1 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 143, BoardLocationX = 7, BoardLocationY = 9, BoardID = 1, BoardTileTypeID = 1 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 144, BoardLocationX = 8, BoardLocationY = 9, BoardID = 1, BoardTileTypeID = 1 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 145, BoardLocationX = 9, BoardLocationY = 9, BoardID = 1, BoardTileTypeID = 3 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 146, BoardLocationX = 10, BoardLocationY = 9, BoardID = 1, BoardTileTypeID = 1 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 147, BoardLocationX = 11, BoardLocationY = 9, BoardID = 1, BoardTileTypeID = 1 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 148, BoardLocationX = 12, BoardLocationY = 9, BoardID = 1, BoardTileTypeID = 1 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 149, BoardLocationX = 13, BoardLocationY = 9, BoardID = 1, BoardTileTypeID = 3 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 150, BoardLocationX = 14, BoardLocationY = 9, BoardID = 1, BoardTileTypeID = 1 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 151, BoardLocationX = 0, BoardLocationY = 10, BoardID = 1, BoardTileTypeID = 1 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 152, BoardLocationX = 1, BoardLocationY = 10, BoardID = 1, BoardTileTypeID = 1 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 153, BoardLocationX = 2, BoardLocationY = 10, BoardID = 1, BoardTileTypeID = 1 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 154, BoardLocationX = 3, BoardLocationY = 10, BoardID = 1, BoardTileTypeID = 1 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 155, BoardLocationX = 4, BoardLocationY = 10, BoardID = 1, BoardTileTypeID = 4 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 156, BoardLocationX = 5, BoardLocationY = 10, BoardID = 1, BoardTileTypeID = 1 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 157, BoardLocationX = 6, BoardLocationY = 10, BoardID = 1, BoardTileTypeID = 1 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 158, BoardLocationX = 7, BoardLocationY = 10, BoardID = 1, BoardTileTypeID = 1 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 159, BoardLocationX = 8, BoardLocationY = 10, BoardID = 1, BoardTileTypeID = 1 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 160, BoardLocationX = 9, BoardLocationY = 10, BoardID = 1, BoardTileTypeID = 1 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 161, BoardLocationX = 10, BoardLocationY = 10, BoardID = 1, BoardTileTypeID = 4 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 162, BoardLocationX = 11, BoardLocationY = 10, BoardID = 1, BoardTileTypeID = 1 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 163, BoardLocationX = 12, BoardLocationY = 10, BoardID = 1, BoardTileTypeID = 1 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 164, BoardLocationX = 13, BoardLocationY = 10, BoardID = 1, BoardTileTypeID = 1 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 165, BoardLocationX = 14, BoardLocationY = 10, BoardID = 1, BoardTileTypeID = 1 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 166, BoardLocationX = 0, BoardLocationY = 11, BoardID = 1, BoardTileTypeID = 2 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 167, BoardLocationX = 1, BoardLocationY = 11, BoardID = 1, BoardTileTypeID = 1 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 168, BoardLocationX = 2, BoardLocationY = 11, BoardID = 1, BoardTileTypeID = 1 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 169, BoardLocationX = 3, BoardLocationY = 11, BoardID = 1, BoardTileTypeID = 4 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 170, BoardLocationX = 4, BoardLocationY = 11, BoardID = 1, BoardTileTypeID = 1 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 171, BoardLocationX = 5, BoardLocationY = 11, BoardID = 1, BoardTileTypeID = 1 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 172, BoardLocationX = 6, BoardLocationY = 11, BoardID = 1, BoardTileTypeID = 1 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 173, BoardLocationX = 7, BoardLocationY = 11, BoardID = 1, BoardTileTypeID = 2 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 174, BoardLocationX = 8, BoardLocationY = 11, BoardID = 1, BoardTileTypeID = 1 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 175, BoardLocationX = 9, BoardLocationY = 11, BoardID = 1, BoardTileTypeID = 1 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 176, BoardLocationX = 10, BoardLocationY = 11, BoardID = 1, BoardTileTypeID = 1 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 177, BoardLocationX = 11, BoardLocationY = 11, BoardID = 1, BoardTileTypeID = 4 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 178, BoardLocationX = 12, BoardLocationY = 11, BoardID = 1, BoardTileTypeID = 1 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 179, BoardLocationX = 13, BoardLocationY = 11, BoardID = 1, BoardTileTypeID = 1 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 180, BoardLocationX = 14, BoardLocationY = 11, BoardID = 1, BoardTileTypeID = 2 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 181, BoardLocationX = 0, BoardLocationY = 12, BoardID = 1, BoardTileTypeID = 1 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 182, BoardLocationX = 1, BoardLocationY = 12, BoardID = 1, BoardTileTypeID = 1 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 183, BoardLocationX = 2, BoardLocationY = 12, BoardID = 1, BoardTileTypeID = 4 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 184, BoardLocationX = 3, BoardLocationY = 12, BoardID = 1, BoardTileTypeID = 1 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 185, BoardLocationX = 4, BoardLocationY = 12, BoardID = 1, BoardTileTypeID = 1 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 186, BoardLocationX = 5, BoardLocationY = 12, BoardID = 1, BoardTileTypeID = 1 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 187, BoardLocationX = 6, BoardLocationY = 12, BoardID = 1, BoardTileTypeID = 2 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 188, BoardLocationX = 7, BoardLocationY = 12, BoardID = 1, BoardTileTypeID = 1 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 189, BoardLocationX = 8, BoardLocationY = 12, BoardID = 1, BoardTileTypeID = 2 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 190, BoardLocationX = 9, BoardLocationY = 12, BoardID = 1, BoardTileTypeID = 1 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 191, BoardLocationX = 10, BoardLocationY = 12, BoardID = 1, BoardTileTypeID = 1 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 192, BoardLocationX = 11, BoardLocationY = 12, BoardID = 1, BoardTileTypeID = 1 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 193, BoardLocationX = 12, BoardLocationY = 12, BoardID = 1, BoardTileTypeID = 4 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 194, BoardLocationX = 13, BoardLocationY = 12, BoardID = 1, BoardTileTypeID = 1 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 195, BoardLocationX = 14, BoardLocationY = 12, BoardID = 1, BoardTileTypeID = 1 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 196, BoardLocationX = 0, BoardLocationY = 13, BoardID = 1, BoardTileTypeID = 1 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 197, BoardLocationX = 1, BoardLocationY = 13, BoardID = 1, BoardTileTypeID = 4 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 198, BoardLocationX = 2, BoardLocationY = 13, BoardID = 1, BoardTileTypeID = 1 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 199, BoardLocationX = 3, BoardLocationY = 13, BoardID = 1, BoardTileTypeID = 1 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 200, BoardLocationX = 4, BoardLocationY = 13, BoardID = 1, BoardTileTypeID = 1 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 201, BoardLocationX = 5, BoardLocationY = 13, BoardID = 1, BoardTileTypeID = 3 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 202, BoardLocationX = 6, BoardLocationY = 13, BoardID = 1, BoardTileTypeID = 1 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 203, BoardLocationX = 7, BoardLocationY = 13, BoardID = 1, BoardTileTypeID = 1 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 204, BoardLocationX = 8, BoardLocationY = 13, BoardID = 1, BoardTileTypeID = 1 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 205, BoardLocationX = 9, BoardLocationY = 13, BoardID = 1, BoardTileTypeID = 3 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 206, BoardLocationX = 10, BoardLocationY = 13, BoardID = 1, BoardTileTypeID = 1 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 207, BoardLocationX = 11, BoardLocationY = 13, BoardID = 1, BoardTileTypeID = 1 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 208, BoardLocationX = 12, BoardLocationY = 13, BoardID = 1, BoardTileTypeID = 1 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 209, BoardLocationX = 13, BoardLocationY = 13, BoardID = 1, BoardTileTypeID = 4 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 210, BoardLocationX = 14, BoardLocationY = 13, BoardID = 1, BoardTileTypeID = 1 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 211, BoardLocationX = 0, BoardLocationY = 14, BoardID = 1, BoardTileTypeID = 5 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 212, BoardLocationX = 1, BoardLocationY = 14, BoardID = 1, BoardTileTypeID = 1 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 213, BoardLocationX = 2, BoardLocationY = 14, BoardID = 1, BoardTileTypeID = 1 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 214, BoardLocationX = 3, BoardLocationY = 14, BoardID = 1, BoardTileTypeID = 2 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 215, BoardLocationX = 4, BoardLocationY = 14, BoardID = 1, BoardTileTypeID = 1 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 216, BoardLocationX = 5, BoardLocationY = 14, BoardID = 1, BoardTileTypeID = 1 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 217, BoardLocationX = 6, BoardLocationY = 14, BoardID = 1, BoardTileTypeID = 1 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 218, BoardLocationX = 7, BoardLocationY = 14, BoardID = 1, BoardTileTypeID = 5 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 219, BoardLocationX = 8, BoardLocationY = 14, BoardID = 1, BoardTileTypeID = 1 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 220, BoardLocationX = 9, BoardLocationY = 14, BoardID = 1, BoardTileTypeID = 1 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 221, BoardLocationX = 10, BoardLocationY = 14, BoardID = 1, BoardTileTypeID = 1 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 222, BoardLocationX = 11, BoardLocationY = 14, BoardID = 1, BoardTileTypeID = 2 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 223, BoardLocationX = 12, BoardLocationY = 14, BoardID = 1, BoardTileTypeID = 1 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 224, BoardLocationX = 13, BoardLocationY = 14, BoardID = 1, BoardTileTypeID = 1 });
            modelBuilder.Entity<BoardTile>().HasData(new BoardTile { ID = 225, BoardLocationX = 14, BoardLocationY = 14, BoardID = 1, BoardTileTypeID = 5 });


            modelBuilder.Entity<CharTile>().HasData(new CharTile { ID = 1, Letter = '*', Score = 0, GameLanguageID = 1, WordDictionaryID = 1 });
            modelBuilder.Entity<CharTile>().HasData(new CharTile { ID = 2, Letter = 'A', Score = 1, GameLanguageID = 1, WordDictionaryID = 1 });
            modelBuilder.Entity<CharTile>().HasData(new CharTile { ID = 3, Letter = 'B', Score = 3, GameLanguageID = 1, WordDictionaryID = 1 });
            modelBuilder.Entity<CharTile>().HasData(new CharTile { ID = 4, Letter = 'C', Score = 3, GameLanguageID = 1, WordDictionaryID = 1 });
            modelBuilder.Entity<CharTile>().HasData(new CharTile { ID = 5, Letter = 'D', Score = 2, GameLanguageID = 1, WordDictionaryID = 1 });
            modelBuilder.Entity<CharTile>().HasData(new CharTile { ID = 6, Letter = 'E', Score = 1, GameLanguageID = 1, WordDictionaryID = 1 });
            modelBuilder.Entity<CharTile>().HasData(new CharTile { ID = 7, Letter = 'F', Score = 4, GameLanguageID = 1, WordDictionaryID = 1 });
            modelBuilder.Entity<CharTile>().HasData(new CharTile { ID = 8, Letter = 'G', Score = 2, GameLanguageID = 1, WordDictionaryID = 1 });
            modelBuilder.Entity<CharTile>().HasData(new CharTile { ID = 9, Letter = 'H', Score = 4, GameLanguageID = 1, WordDictionaryID = 1 });
            modelBuilder.Entity<CharTile>().HasData(new CharTile { ID = 10, Letter = 'I', Score = 1, GameLanguageID = 1, WordDictionaryID = 1 });
            modelBuilder.Entity<CharTile>().HasData(new CharTile { ID = 11, Letter = 'J', Score = 8, GameLanguageID = 1, WordDictionaryID = 1 });
            modelBuilder.Entity<CharTile>().HasData(new CharTile { ID = 12, Letter = 'K', Score = 5, GameLanguageID = 1, WordDictionaryID = 1 });
            modelBuilder.Entity<CharTile>().HasData(new CharTile { ID = 13, Letter = 'L', Score = 1, GameLanguageID = 1, WordDictionaryID = 1 });
            modelBuilder.Entity<CharTile>().HasData(new CharTile { ID = 14, Letter = 'M', Score = 3, GameLanguageID = 1, WordDictionaryID = 1 });
            modelBuilder.Entity<CharTile>().HasData(new CharTile { ID = 15, Letter = 'N', Score = 1, GameLanguageID = 1, WordDictionaryID = 1 });
            modelBuilder.Entity<CharTile>().HasData(new CharTile { ID = 16, Letter = 'O', Score = 1, GameLanguageID = 1, WordDictionaryID = 1 });
            modelBuilder.Entity<CharTile>().HasData(new CharTile { ID = 17, Letter = 'P', Score = 3, GameLanguageID = 1, WordDictionaryID = 1 });
            modelBuilder.Entity<CharTile>().HasData(new CharTile { ID = 18, Letter = 'Q', Score = 10, GameLanguageID = 1, WordDictionaryID = 1 });
            modelBuilder.Entity<CharTile>().HasData(new CharTile { ID = 19, Letter = 'R', Score = 1, GameLanguageID = 1, WordDictionaryID = 1 });
            modelBuilder.Entity<CharTile>().HasData(new CharTile { ID = 20, Letter = 'S', Score = 1, GameLanguageID = 1, WordDictionaryID = 1 });
            modelBuilder.Entity<CharTile>().HasData(new CharTile { ID = 21, Letter = 'T', Score = 1, GameLanguageID = 1, WordDictionaryID = 1 });
            modelBuilder.Entity<CharTile>().HasData(new CharTile { ID = 22, Letter = 'U', Score = 1, GameLanguageID = 1, WordDictionaryID = 1 });
            modelBuilder.Entity<CharTile>().HasData(new CharTile { ID = 23, Letter = 'V', Score = 4, GameLanguageID = 1, WordDictionaryID = 1 });
            modelBuilder.Entity<CharTile>().HasData(new CharTile { ID = 24, Letter = 'W', Score = 4, GameLanguageID = 1, WordDictionaryID = 1 });
            modelBuilder.Entity<CharTile>().HasData(new CharTile { ID = 25, Letter = 'X', Score = 8, GameLanguageID = 1, WordDictionaryID = 1 });
            modelBuilder.Entity<CharTile>().HasData(new CharTile { ID = 26, Letter = 'Y', Score = 4, GameLanguageID = 1, WordDictionaryID = 1 });
            modelBuilder.Entity<CharTile>().HasData(new CharTile { ID = 27, Letter = 'Z', Score = 10, GameLanguageID = 1, WordDictionaryID = 1 });

            modelBuilder.Entity<Pouch>().HasData(new Pouch { ID = 1, GameID = 1 });

            modelBuilder.Entity<Player>().HasData(new Player { ID = 1, IsHuman = true, Score = 0, GameID = 1, RackID = 1, PouchID = 1, AtHand = true });
            modelBuilder.Entity<Player>().HasData(new Player { ID = 2, IsHuman = false, Score = 0, GameID = 1, RackID = 2, PouchID = 1, AtHand = false });

            modelBuilder.Entity<Rack>().HasData(new Rack { ID = 1, PouchID = 1, RackSize = 7, GameID = 1, PlayerID = 1 });
            modelBuilder.Entity<Rack>().HasData(new Rack { ID = 2, PouchID = 1, RackSize = 7, GameID = 1, PlayerID = 2 });                          
           
            modelBuilder.Entity<Pouch_CharTile>().HasData(new Pouch_CharTile { ID = 1, PouchID = 1, CharTileID = 1, Count = 0 });
            modelBuilder.Entity<Pouch_CharTile>().HasData(new Pouch_CharTile { ID = 2, PouchID = 1, CharTileID = 2, Count = 9 });
            modelBuilder.Entity<Pouch_CharTile>().HasData(new Pouch_CharTile { ID = 3, PouchID = 1, CharTileID = 3, Count = 1 });
            modelBuilder.Entity<Pouch_CharTile>().HasData(new Pouch_CharTile { ID = 4, PouchID = 1, CharTileID = 4, Count = 1 });
            modelBuilder.Entity<Pouch_CharTile>().HasData(new Pouch_CharTile { ID = 5, PouchID = 1, CharTileID = 5, Count = 3 });
            modelBuilder.Entity<Pouch_CharTile>().HasData(new Pouch_CharTile { ID = 6, PouchID = 1, CharTileID = 6, Count = 12 });
            modelBuilder.Entity<Pouch_CharTile>().HasData(new Pouch_CharTile { ID = 7, PouchID = 1, CharTileID = 7, Count = 1 });
            modelBuilder.Entity<Pouch_CharTile>().HasData(new Pouch_CharTile { ID = 8, PouchID = 1, CharTileID = 8, Count = 2 });
            modelBuilder.Entity<Pouch_CharTile>().HasData(new Pouch_CharTile { ID = 9, PouchID = 1, CharTileID = 9, Count = 1 });
            modelBuilder.Entity<Pouch_CharTile>().HasData(new Pouch_CharTile { ID = 10, PouchID = 1, CharTileID = 10, Count = 7 });
            modelBuilder.Entity<Pouch_CharTile>().HasData(new Pouch_CharTile { ID = 11, PouchID = 1, CharTileID = 11, Count = 0 });
            modelBuilder.Entity<Pouch_CharTile>().HasData(new Pouch_CharTile { ID = 12, PouchID = 1, CharTileID = 12, Count = 1 });
            modelBuilder.Entity<Pouch_CharTile>().HasData(new Pouch_CharTile { ID = 13, PouchID = 1, CharTileID = 13, Count = 4 });
            modelBuilder.Entity<Pouch_CharTile>().HasData(new Pouch_CharTile { ID = 14, PouchID = 1, CharTileID = 14, Count = 1 });
            modelBuilder.Entity<Pouch_CharTile>().HasData(new Pouch_CharTile { ID = 15, PouchID = 1, CharTileID = 15, Count = 5 });
            modelBuilder.Entity<Pouch_CharTile>().HasData(new Pouch_CharTile { ID = 16, PouchID = 1, CharTileID = 16, Count = 7 });
            modelBuilder.Entity<Pouch_CharTile>().HasData(new Pouch_CharTile { ID = 17, PouchID = 1, CharTileID = 17, Count = 1 });
            modelBuilder.Entity<Pouch_CharTile>().HasData(new Pouch_CharTile { ID = 18, PouchID = 1, CharTileID = 18, Count = 1 });
            modelBuilder.Entity<Pouch_CharTile>().HasData(new Pouch_CharTile { ID = 19, PouchID = 1, CharTileID = 19, Count = 6 });
            modelBuilder.Entity<Pouch_CharTile>().HasData(new Pouch_CharTile { ID = 20, PouchID = 1, CharTileID = 20, Count = 4 });
            modelBuilder.Entity<Pouch_CharTile>().HasData(new Pouch_CharTile { ID = 21, PouchID = 1, CharTileID = 21, Count = 6 });
            modelBuilder.Entity<Pouch_CharTile>().HasData(new Pouch_CharTile { ID = 22, PouchID = 1, CharTileID = 22, Count = 3 });
            modelBuilder.Entity<Pouch_CharTile>().HasData(new Pouch_CharTile { ID = 23, PouchID = 1, CharTileID = 23, Count = 2 });
            modelBuilder.Entity<Pouch_CharTile>().HasData(new Pouch_CharTile { ID = 24, PouchID = 1, CharTileID = 24, Count = 2 });
            modelBuilder.Entity<Pouch_CharTile>().HasData(new Pouch_CharTile { ID = 25, PouchID = 1, CharTileID = 25, Count = 1 });
            modelBuilder.Entity<Pouch_CharTile>().HasData(new Pouch_CharTile { ID = 26, PouchID = 1, CharTileID = 26, Count = 2 });
            modelBuilder.Entity<Pouch_CharTile>().HasData(new Pouch_CharTile { ID = 27, PouchID = 1, CharTileID = 26, Count = 1 });

            modelBuilder.Entity<Rack_CharTile>().HasData(new Rack_CharTile { ID = 1, RackID = 1, CharTileID = 3, Count = 1 });
            modelBuilder.Entity<Rack_CharTile>().HasData(new Rack_CharTile { ID = 2, RackID = 1, CharTileID = 16, Count = 1 });
            modelBuilder.Entity<Rack_CharTile>().HasData(new Rack_CharTile { ID = 3, RackID = 1, CharTileID = 10, Count = 1 });
            modelBuilder.Entity<Rack_CharTile>().HasData(new Rack_CharTile { ID = 4, RackID = 1, CharTileID = 15, Count = 1 });
            modelBuilder.Entity<Rack_CharTile>().HasData(new Rack_CharTile { ID = 5, RackID = 1, CharTileID = 8, Count = 1 });
            modelBuilder.Entity<Rack_CharTile>().HasData(new Rack_CharTile { ID = 6, RackID = 1, CharTileID = 15, Count = 1 });
            modelBuilder.Entity<Rack_CharTile>().HasData(new Rack_CharTile { ID = 7, RackID = 1, CharTileID = 4, Count = 1 });
            modelBuilder.Entity<Rack_CharTile>().HasData(new Rack_CharTile { ID = 8, RackID = 2, CharTileID = 7, Count = 1 });
            modelBuilder.Entity<Rack_CharTile>().HasData(new Rack_CharTile { ID = 9, RackID = 2, CharTileID = 17, Count = 1 });
            modelBuilder.Entity<Rack_CharTile>().HasData(new Rack_CharTile { ID = 10, RackID = 2, CharTileID = 10, Count = 1 });
            modelBuilder.Entity<Rack_CharTile>().HasData(new Rack_CharTile { ID = 11, RackID = 2, CharTileID = 14, Count = 1 });
            modelBuilder.Entity<Rack_CharTile>().HasData(new Rack_CharTile { ID = 12, RackID = 2, CharTileID = 5, Count = 1 });
            modelBuilder.Entity<Rack_CharTile>().HasData(new Rack_CharTile { ID = 13, RackID = 2, CharTileID = 9, Count = 1 });
            modelBuilder.Entity<Rack_CharTile>().HasData(new Rack_CharTile { ID = 14, RackID = 2, CharTileID = 11, Count = 1 });
        }
    }
    
}
