using System;
using System.Collections.Generic;
using System.Text;

namespace project1
{
    class Player
    {
        public string position { get; set; }
        public int pick { get; set; } //1st is the best, 5th is the worst (of the best)
        public int price { get; set; }
        public string state { get; set; }
        public string name { get; set; }
 

        public Player(string _name, string _state, string _position, int price, int _pick)
        {
            this.name = _name;
            this.state = _state;
            this.position = _position;
            this.price = price;
            this.pick = _pick;
        }
        public Player()
        {
            
        }

        public List<string> getDisplayableList()
        {
            List<string> dispList = new List<string>();
            dispList.Add(this.name);
            dispList.Add(this.state);
            dispList.Add(this.price.ToString());
            return dispList;
        }
        public override string ToString()
        {
            return "name:" + this.name + "  position: " + this.position + "  state: " + this.state + "   price: " + this.price + "   pick: " + this.pick; ;
        }
    }
}
