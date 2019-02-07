using System.Collections.Generic;

namespace project1
{
    internal class Player
    {
        public Player(string _name, string _state, string _position, int price, int _pick)
        {
            name = _name;
            state = _state;
            position = _position;
            this.price = price;
            pick = _pick;
        }

        public Player()
        {
            //the spaced buffer below is to clear out spaces. Yes, it's a poor way of doing it but ehh. It works and I don't have to update the whole console then 
            name = "                      ";
            state = "                      ";
            position = "                      ";
        }

        public string position { get; set; }
        public int pick { get; set; } //1st is the best, 5th is the worst (of the best)
        public int price { get; set; }
        public string state { get; set; }
        public string name { get; set; }

        public List<string> getDisplayableList()
        {
            List<string> dispList = new List<string>();

            dispList.Add(name);
            dispList.Add(state);
            dispList.Add(name == "                      " ? "                      " : price.ToString("C0"));
            return dispList;
        }

        public override string ToString()
        {
            return "name:" + name + "  position: " + position + "  state: " + state + "   price: " + price +
                   "   pick: " + pick;
            ;
        }
    }
}