using DawgSharp;
using Microsoft.Extensions.Primitives;
using Scrabble.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Scrabble.Helpers
{
    public static class Helper
    {
        public static void MakeEnglishDictionary()
        {
            var dawgBuilder = new DawgBuilder<bool>(); // <bool> is the value type.
                                                       // Key type is always string.
            string[] lines = File.ReadLines(@"C:\Users\Simeon\Desktop\Scrabble\Scrabble\Helpers\englishWords.txt").ToArray();
            //foreach (string key in lines)
            //{
            //    dawgBuilder.Insert(key, true);
            //}

            foreach (string key in lines)
            {
                dawgBuilder.Insert(key, true);
            }

            //foreach (string key in new[] { "Aaron", "abacus", "abashed" })
            //{
            //    dawgBuilder.Insert(key, true);
            //}

            var dawg = dawgBuilder.BuildDawg(); // Computer is working.  Please wait ...

            //dawg.SaveTo(File.Create(@"C:\Users\Simeon\Desktop\Scrabble\Scrabble\Helpers\EnglishDAWG.bin"));
            dawg.SaveTo(File.Create(@"C:\Users\Simeon\Desktop\Scrabble\Scrabble\Helpers\test.bin"));
            //dawg.SaveTo(File.Create(@"C:\Users\Simeon\Desktop\Scrabble\Scrabble\Helpers\dawg.bin"));

            //var h = "hello";
        }
        public static bool CheckWordValidity(string word, bool alwaysExists = false)
        {
            Stream fs = File.Open(@"C:\Users\Simeon\Desktop\Scrabble\Scrabble\Helpers\englishDawg.bin", FileMode.Open, FileAccess.Read);
            var dawg = Dawg<bool>.Load(fs);

            if (!alwaysExists && !dawg[word.ToLower()])
            {
                return false;
            }
            return true;
        }
        public static int GetWordScore (List<BoardTile> word)
        {
            int score = 0;
            var doubleWordTilesUsed = 0;
            var tripleWordTilesUsed = 0;
            foreach (BoardTile letter in word)
            {
                switch (letter.BoardTileType.Type)
                {
                    case "DoubleLetter":
                        score += letter.CharTile.Score * 2;
                        break;
                    case "TripleLetter":
                        score += letter.CharTile.Score * 3;
                        break;
                    case "DoubleWord":
                        doubleWordTilesUsed += 1;
                        score += letter.CharTile.Score;
                        break;
                    case "TripleWord":
                        tripleWordTilesUsed += 1;
                        score += letter.CharTile.Score;
                        break;
                    default:
                        score += letter.CharTile.Score;
                        break;
                }
            }
            for (int i = 0; i < doubleWordTilesUsed; i++)
            {
                score *= 2;
            }
            for (int i = 0; i < tripleWordTilesUsed; i++)
            {
                score *= 3;
            }
            return score;
        }
        public static HttpStatusCodeResult GetWordScores(Game game, List<KeyValuePair<string, StringValues>> data) {
            List<string> playedWords = new List<string>();
            string logBuilder = "";
            logBuilder += "Player" + game.GetPlayerAtHand().ID + " played ";
            foreach (KeyValuePair<string, StringValues> wordData in data)
            {
                playedWords.Add(wordData.Value);
            }
            //var playedWords = data[0].Value.ToString().Split(",");              
            for (int i = 0; i < playedWords.Count; i++)
            {
                var playedWord = playedWords[i];
                var currentScoreOfMove = 0;
                var playedWordString = "";
                var usedBoardTiles = new List<BoardTile>();               
                var playedTiles = playedWord.Split(",");
                foreach (string playedTile in playedTiles)
                {
                    var tileDetails = playedTile.Split("_");
                    int tileX = Int32.Parse(tileDetails[0]);
                    int tileY = Int32.Parse(tileDetails[1]);
                    int tileCharTileId = Int32.Parse(tileDetails[2]);
                    playedWordString += game.WordDictionary.CharTiles.Where(c => c.ID == tileCharTileId).FirstOrDefault().Letter;
                    game.Board.PlayTile(tileX, tileY, tileCharTileId, usedBoardTiles);
                    //currentScoreOfMove += game.WordDictionary.CharTiles.Where(i => i.ID == tileCharTileId).FirstOrDefault().Score;
                }

                if (!CheckWordValidity(playedWordString, true))
                {
                    return new HttpStatusCodeResult(404, playedWordString + " is not a legal word.");
                }
                currentScoreOfMove = GetWordScore(usedBoardTiles);
                playedWordString = playedWordString.ToUpper();
                game.AddScoreToPlayer(game.GetPlayerAtHand(), currentScoreOfMove);
                logBuilder += playedWordString + " for " + currentScoreOfMove + " points";
                if (i == playedWords.Count - 1)
                {
                    logBuilder += ". ";
                } else if (i == playedWords.Count - 2)
                {
                    logBuilder += " and ";
                } else
                {
                    logBuilder += " , ";
                }
                //game.Log += "Player" + game.GetPlayerAtHand().ID + " played " + playedWordString + " for " + currentScoreOfMove + " points.";
                //game.Log += "-------------------------";
            }
            game.Log += logBuilder;
            game.Log += "Player" + game.GetPlayerAtHand().ID + " now has " + game.GetPlayerAtHand().Score + " points.";
            game.Log += "-------------------------";
            return new HttpStatusCodeResult(200, "Good move :)");
        }
    }
}
