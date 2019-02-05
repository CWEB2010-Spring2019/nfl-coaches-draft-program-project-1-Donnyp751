using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace project1
{
    class ConsoleDisplayManager
    {
        private int rows{ get; set; }
        private int columns { get; set; }
        private int placeHolderWidth = 30;
        private int placeHolderHeight = 4;
        

        public ConsoleDisplayManager(List<Player>players)
        {
            rows = 6;
            columns = 9;

            foreach (Player player in players)
            {   
                //calculate where everything will be placed
                
            }
        }

        

        

    }
}
