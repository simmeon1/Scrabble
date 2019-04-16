using Scrabble.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Scrabble.Models
{
    /// <summary>
    /// Represents BotType
    /// Can be either High_Scorer or Rack_Balancer
    /// </summary>
    public class BotType
    {
        public BotTypeEnum ID { get; set; }

        public string Type { get; set; }
    }
}
