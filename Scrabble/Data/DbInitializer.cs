//using ContosoUniversity.Models;
//using Scrabble.Models;
//using System;
//using System.Linq;

//namespace ContosoUniversity.Data
//{
//    public static class DbInitializer
//    {
//        public static void Initialize(ScrabbleContext context)
//        {
//            context.Database.EnsureCreated();

//            /*// Look for any students.
//            if (context.Games.Any())
//            {
//                return;   // DB has been seeded
//            }*/

//            var games = new Game[]
//            {
//            new Game{ID=1, GameLanguageID=1, GameLanguage=1, CurrentPlayerID=1, BoardID=1, PouchID=1}         
//            };
//            foreach (Game g in games)
//            {
//                context.Games.Add(g);
//            }
//            context.SaveChanges();
//        }
//    }
//}