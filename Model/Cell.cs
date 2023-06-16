using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Battleship.Model
{
    internal class Cell : INotifyPropertyChanged
    {
        private int id;

        public int ID
        {
            get { return id; }
            set { id = value; OnPropertyChanged("ID"); }
        }
        private bool isEnabled;

        public bool IsEnabled
        {
            get { return isEnabled; }
            set { isEnabled = value; OnPropertyChanged("IsEnabled"); }
        }
        private bool isDiscovered;

        public bool IsDiscovered
        {
            get { return isDiscovered; }
            set { isDiscovered = value; OnPropertyChanged("IsDiscovered"); }
        }
        private bool isTaken;

        public bool IsTaken
        {
            get { return isTaken; }
            set { isTaken = value; OnPropertyChanged("IsTaken"); }
        }
        private bool isTakenPublic;

        public bool IsTakenPublic
        {
            get { return isTakenPublic; }
            set { isTakenPublic = value; OnPropertyChanged("IsTakenPublic"); }
        }
        private bool isDead;

        public bool IsDead
        {
            get { return isDead; }
            set { isDead = value; OnPropertyChanged("IsDead"); }
        }
        private bool isAuthorized;
        public bool IsAuthorized
        {
            get { return isAuthorized; }
            set { isAuthorized = value; OnPropertyChanged("IsAuthorized"); }
        }
        private bool triggered;
        public bool Triggered
        {
            get { return triggered; }
            set { triggered = value; OnPropertyChanged("Triggered"); }
        }
        public Cell(int id)
        {
            ID = id;
            IsEnabled = true;
            IsAuthorized = false;
            IsDiscovered = false;
            IsTaken = false;
            IsDead = false;
            Triggered = false;
        }

        public Cell(int id, bool isTaken)
        {
            ID = id;
            IsEnabled = true;
            IsAuthorized = false;
            IsDiscovered = false;
            IsTaken = isTaken;
            IsDead = false;
            Triggered = false;
        }

        public RelayCommand ClickCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    Triggered = true;
                    if (IsTaken)
                    {
                        IsTakenPublic = true;
                    }
                    IsEnabled = false;
                    IsDiscovered = true;
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
