using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lab4
{
    public class pongSettings
    {
       public enum Difficulty
        {
            Easy,
            Medium,
            Hard
        }
       public bool powerUps { get; set; }
       public Difficulty difficulty { get; set; }
       public bool barriers { get; set; }
       public bool music { get; set; }

        public pongSettings()
        {
            powerUps = true;
            difficulty = Difficulty.Easy;
            barriers = true;
            music = true;
        } 

    }
}
