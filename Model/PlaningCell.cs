using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Battleship.Model
{
    internal class PlaningCell : INotifyPropertyChanged
    {
        private int id;

        public int ID
        {
            get { return id; }
            set { id = value; OnPropertyChanged("ID"); }
        }

        private List<int> hostedShips;

        public List<int> HostedShips
        {
            get { return hostedShips; }
            set { hostedShips = value; OnPropertyChanged("HostedShips"); }
        }

        private int maxSizeOfHostedShip;

        public int MaxSizeOfHostedShip
        {
            get { return maxSizeOfHostedShip; }
            set { maxSizeOfHostedShip = value; OnPropertyChanged("MaxSizeOfHostedShip"); }
        }
        private string direction;

        public string Direction
        {
            get { return direction; }
            set { direction = value; OnPropertyChanged("Direction"); }
        }
        private bool isTkn;

        public bool IsTkn
        {
            get { return isTkn; }
            set { isTkn = value; OnPropertyChanged("IsTkn"); }
        }
        public PlaningCell(int id)
        {
            ID = id;
            Direction = "None";
            HostedShips = new List<int>();
            MaxSizeOfHostedShip = -1;
            IsTkn = false;
        }

        public RelayCommand ClickCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    IsTkn = !IsTkn;
                });
            }
            set
            {
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
