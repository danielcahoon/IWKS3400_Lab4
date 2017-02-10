using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lab4
{
    class pongSettings
    {
        
       public bool powerUps { get; set; }
       public int difficulty { get; set; }
       public bool barriers { get; set; }
       public bool music { get; set; }

        public pongSettings()
        {
            powerUps = true;
            difficulty = 1;
            barriers = true;
            music = true;
        } 

    }
}
