using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Battleship.Model
{
    internal class Ship
    {
        private List<int> shipHP;

        public List<int> ShipHP
        {
            get { return shipHP; }
            set { shipHP = value; OnPropertyChanged("ShipHP"); }
        }
        private List<int> shipCells;

        public List<int> ShipCells
        {
            get { return shipCells; }
            set { shipCells = value; OnPropertyChanged("ShipCells"); }
        }
        public Ship(List<int> shipHP)
        {
            ShipHP = shipHP;
            ShipCells = new List<int>();
            foreach  (int id in shipHP)
            {
                ShipCells.Add(id);
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string args = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(args));
            }
        }
    }
}
