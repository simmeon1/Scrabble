using Scrabble.Classes;

namespace Scrabble.Models
{
    /// <summary>
    /// Represents Game languages 
    /// </summary>
    public class GameLanguage
    {
        public LanguageEnum ID { get; set; }

        public string Language { get; set; }

        public int CountOfLetters { get; set; }
    }
}
