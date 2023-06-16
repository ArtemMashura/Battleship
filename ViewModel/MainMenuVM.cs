using Battleship.Model;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Battleship.ViewModel
{
    internal class MainMenuVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string args = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(args));
            }
        }
        private string isEnabled;
        public string IsEnabled
        {
            get { return isEnabled; }
            set { isEnabled = value; OnPropertyChanged("IsEnabled"); }
        }
        private string winner;
        public string Winner
        {
            get { return winner; }
            set { winner = value; OnPropertyChanged("Winner"); }
        }
        private string errorMessage;
        public string ErrorMessage
        {
            get { return errorMessage; }
            set { errorMessage = value; OnPropertyChanged("ErrorMessage"); }
        }
        private string errorMessageContent;
        public string ErrorMessageContent
        {
            get { return errorMessageContent; }
            set { errorMessageContent = value; OnPropertyChanged("ErrorMessageContent"); }
        }
        private int shipSelector;
        public int ShipSelector
        {
            get { return shipSelector; }
            set { shipSelector = value; OnPropertyChanged("ShipSelector"); }
        }
        private int fourHPShips;
        public int FourHPShips
        {
            get { return fourHPShips; }
            set { fourHPShips = value;}
        }
        private int threeHPShips;
        public int ThreeHPShips
        {
            get { return threeHPShips; }
            set { threeHPShips = value; }
        }
        private int twoHPShips;
        public int TwoHPShips
        {
            get { return twoHPShips; }
            set { twoHPShips = value; }
        }
        private int oneHPShips;
        public int OneHPShips
        {
            get { return oneHPShips; }
            set { oneHPShips = value; }
        }
        private bool turnCounter;
        public bool TurnCounter
        {
            get { return turnCounter; }
            set { turnCounter = value; OnPropertyChanged("TurnCounter"); }
        }
        private Visibility win;
        public Visibility Win
        {
            get { return win; }
            set { win = value; OnPropertyChanged("Win"); }
        }
        private Visibility insertWin;
        public Visibility InsertWin
        {
            get { return insertWin; }
            set { insertWin = value; OnPropertyChanged("InsertWin"); }
        }
        
        private ObservableCollection<Model.Cell> cellList;
        public ObservableCollection<Model.Cell> CellList
        {
            get { return cellList; }
            set { cellList = value; OnPropertyChanged("CellList"); }
        }
        private ObservableCollection<Model.Cell> cellList1;
        public ObservableCollection<Model.Cell> CellList1
        {
            get { return cellList1; }
            set { cellList1 = value; OnPropertyChanged("CellList1"); }
        }
        private PlaningCell foundCell;
        public PlaningCell FoundCell
        {
            get { return foundCell; }
            set { foundCell = value; OnPropertyChanged("FoundCell"); }
        }
        private ObservableCollection<Model.PlaningCell> planingCellList;
        public ObservableCollection<Model.PlaningCell> PlaningCellList
        {
            get { return planingCellList; }
            set { planingCellList = value; OnPropertyChanged("PlaningCellList"); }
        }
        private ObservableCollection<Model.PlaningCell> planingCellList1;
        public ObservableCollection<Model.PlaningCell> PlaningCellList1
        {
            get { return planingCellList1; }
            set { planingCellList1 = value; OnPropertyChanged("PlaningCellList1"); }
        }

        private ObservableCollection<Model.Ship> shipList;
        public ObservableCollection<Model.Ship> ShipList
        {
            get { return shipList; }
            set { shipList = value; OnPropertyChanged("ShipList"); }
        }
        private ObservableCollection<Model.Ship> shipList1;
        public ObservableCollection<Model.Ship> ShipList1
        {
            get { return shipList1; }
            set { shipList1 = value; OnPropertyChanged("ShipList1"); }
        }
        private List<List<int>> authedShips;
        public List<List<int>> AuthedShips
        {
            get { return authedShips; }
            set { authedShips = value; OnPropertyChanged("AuthedShips"); }
        }
        void AddCheckIfGotHit(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
                foreach (Cell item in e.NewItems)
                    item.PropertyChanged += CheckIfGotHit;

            if (e.OldItems != null)
                foreach (Cell item in e.OldItems)
                    item.PropertyChanged -= CheckIfGotHit;
        }
        void CheckIfGotHit(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Triggered")
            {
                Cell cell = (sender as Cell);
                if (cell.IsTaken)
                {
                    if (cell.ID < ammount_of_cells)
                    {
                        if (CheckIfDead(cell, ShipList, CellList))
                        {
                            Winner = "Игрок 1 победил";
                            InsertWin = Visibility.Visible;
                        }
                    }
                    else
                    {
                        if (CheckIfDead(cell, ShipList1, CellList1))
                        {
                            Winner = "Игрок 2 победил";
                            InsertWin = Visibility.Visible;

                        }
                    }
                }
                else
                {
                    TurnCounter = !TurnCounter;
                }
            }

        }
        void AddPlaceShipUpdate(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
                foreach (PlaningCell item in e.NewItems)
                    item.PropertyChanged += PlacedCellIsCorrect;

            if (e.OldItems != null)
                foreach (PlaningCell item in e.OldItems)
                    item.PropertyChanged -= PlacedCellIsCorrect;
        }

        void PlacedCellIsCorrect(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsTkn")
            {
                PlaningCell cell = (sender as PlaningCell);
                if (cell.IsTkn == true)
                {
                    if (cell.ID < ammount_of_cells)
                    {
                        PlacementCheck(cell, PlaningCellList);
                    }
                    else
                    {
                        PlacementCheck(cell, PlaningCellList1);

                    }
                }
                else
                {
                    if (cell.ID < ammount_of_cells)
                    {
                        for (int i = 0; i < PlaningCellList.Count; i++)
                        {
                            PlaningCellList[i].HostedShips.Remove(cell.ID);
                        }
                        if (cell.HostedShips.Count == 1)
                        {
                            int remainingCell = cell.HostedShips[0];
                            PlaningCellList[remainingCell].HostedShips.Clear();
                            PlaningCellList[remainingCell].Direction = "None";
                        }
                        cell.HostedShips.Clear();
                    }
                    else 
                    {
                        for (int i = 0; i < PlaningCellList1.Count; i++)
                        {
                            PlaningCellList1[i].HostedShips.Remove(cell.ID);
                        }
                        if (cell.HostedShips.Count == 1)
                        {
                            int remainingCell = cell.HostedShips[0] - ammount_of_cells;
                            PlaningCellList1[remainingCell].HostedShips.Clear();
                            PlaningCellList1[remainingCell].Direction = "None";
                        }
                        cell.HostedShips.Clear();
                    }
                    cell.MaxSizeOfHostedShip = -1;
                    cell.Direction = "None";
                }
            }


        }

        public void PlacementCheck(PlaningCell cell, ObservableCollection<Model.PlaningCell> cellList)
        {
            int minID = cellList[0].ID;
            int maxID = cellList[ammount_of_cells - 1].ID;
            int cellID = cell.ID;
            if (cellID >= ammount_of_cells)
            {
                cellID -= ammount_of_cells;
            }
            if (cell.ID - FieldWidth - 1 >= minID && !forbiddenLeftChecks.Contains(cell.ID - FieldWidth - 1) && cellList[cellID - FieldWidth - 1].HostedShips.Count != 0)
            {
                cell.IsTkn = false;
                ErrorMessage = "Visible";
                ErrorMessageContent = "Diagonal";
            }
            else if (cell.ID - FieldWidth + 1 >= minID && !forbiddenRightChecks.Contains(cell.ID - FieldWidth + 1) && cellList[cellID - FieldWidth + 1].HostedShips.Count != 0)
            {
                cell.IsTkn = false;
                ErrorMessage = "Visible";
                ErrorMessageContent = "Diagonal";

            }
            else if (cell.ID + FieldWidth - 1 <= maxID && !forbiddenLeftChecks.Contains(cell.ID + FieldWidth - 1) && cellList[cellID + FieldWidth - 1].HostedShips.Count != 0)
            {
                cell.IsTkn = false;
                ErrorMessage = "Visible";
                ErrorMessageContent = "Diagonal";

            }
            else if (cell.ID + FieldWidth + 1 <= maxID && !forbiddenRightChecks.Contains(cell.ID + FieldWidth + 1) && cellList[cellID + FieldWidth + 1].HostedShips.Count != 0)
            {
                cell.IsTkn = false;
                ErrorMessage = "Visible";
                ErrorMessageContent = "Diagonal";
            }
            else
            {
                int newID = 0;
                int insertNewID = 0;
                string Dir = "";
                string response = CheckCardinalSides(cell, minID, maxID, cellList);
                if (response == "Conflict")
                {
                    cell.IsTkn = false;
                    ErrorMessage = "Visible";
                    ErrorMessageContent = "Conflict";
                }
                else if (response == "")
                {
                    cell.HostedShips.Add(cell.ID);
                    cell.MaxSizeOfHostedShip = ShipSelector;
                    cell.Direction = "None";
                    ErrorMessage = "Hidden";
                }
                else
                {
                    if (response == "Down")
                    {
                        newID = cellID + FieldWidth;
                        insertNewID = cell.ID + FieldWidth;
                        Dir = "Vertical";
                    }
                    else if (response == "Up")
                    {
                        newID = cellID - FieldWidth;
                        insertNewID = cell.ID - FieldWidth;
                        Dir = "Vertical";
                    }

                    else if (response == "Right")
                    {
                        newID = cellID + 1;
                        insertNewID = cell.ID + 1;
                        Dir = "Horizontal";
                    }
                    else if (response == "Left")
                    {
                        newID = cellID - 1;
                        insertNewID = cell.ID - 1;
                        Dir = "Horizontal";
                    }

                    if (cellList[newID].MaxSizeOfHostedShip == ShipSelector)
                    {
                        if (cellList[newID].MaxSizeOfHostedShip > cellList[newID].HostedShips.Count)
                        {
                            if (cellList[newID].Direction == "None")
                            {
                                cellList[newID].Direction = Dir;
                                cell.Direction = Dir;

                                cellList[newID].HostedShips.Add(cell.ID);

                                cell.HostedShips.Add(insertNewID);
                                cell.HostedShips.Add(cell.ID);
                                cell.MaxSizeOfHostedShip = ShipSelector;
                                ErrorMessage = "Hidden";

                            }
                            else if (cellList[newID].Direction == Dir)
                            {
                                cellList[newID].HostedShips.Add(cell.ID);
                                List<int> newData = new List<int>();
                                int rewrite_ammount = cellList[newID].HostedShips.Count;
                                for (int i = 0; i < rewrite_ammount; i++)
                                {
                                    newData.Add(cellList[newID].HostedShips[i]);
                                }
                                for (int k = 0; k < rewrite_ammount; k++)
                                {
                                    int rewriteID = newData[k];
                                    if (rewriteID >= ammount_of_cells)
                                    {
                                        rewriteID -= ammount_of_cells;
                                    }
                                    cellList[rewriteID].HostedShips.Clear();
                                    for (int i = 0; i < rewrite_ammount; i++)
                                    {
                                        cellList[rewriteID].HostedShips.Add(newData[i]);
                                    }
                                }

                                cell.Direction = Dir;
                                cell.MaxSizeOfHostedShip = ShipSelector;
                                ErrorMessage = "Hidden";
                            }
                            else
                            {
                                cell.IsTkn = false;
                                ErrorMessage = "Visible";
                                ErrorMessageContent = "WrongDirection";
                            }
                        }
                        else
                        {
                            ErrorMessage = "Visible";
                            ErrorMessageContent = "TooBig";
                        }
                    }
                }

                


            }


        }

        public string CheckCardinalSides(PlaningCell cell, int minID, int maxID, ObservableCollection<Model.PlaningCell> cellList)
        {
            string check = "";
            int cellID = cell.ID;
            if (cellID >= ammount_of_cells)
            {
                cellID -= ammount_of_cells;
            }
            if (cell.ID - FieldWidth >= minID)
            {
                int newID = cellID - FieldWidth;
                if (cellList[newID].MaxSizeOfHostedShip != -1)
                {
                    if (check != "")
                    {
                        check = "Conflict";
                        return check;
                    }
                    else check = "Up";
                }
            }
            if (cell.ID + FieldWidth <= maxID)
            {
                int newID = cellID + FieldWidth;
                if (cellList[newID].MaxSizeOfHostedShip != -1)
                {
                    if (check != "")
                    {
                        check = "Conflict";
                        return check;
                    }
                    else check = "Down";
                }
            }
            if (cell.ID - 1 >= minID && !forbiddenLeftChecks.Contains(cell.ID - 1))
            {
                int newID = cellID - 1;
                if (cellList[newID].MaxSizeOfHostedShip != -1)
                {
                    if (check != "")
                    {
                        check = "Conflict";
                        return check;
                    }
                    else check = "Left";
                }
            }
            if (cell.ID + 1 <= maxID && !forbiddenRightChecks.Contains(cell.ID + 1))
            {
                int newID = cellID + 1;
                if (cellList[newID].MaxSizeOfHostedShip != -1)
                {
                    if (check != "")
                    {
                        check = "Conflict";
                        return check;
                    }
                    else check = "Right";
                }
            }
            return check;
        }

        public bool CheckIfDead(Cell cell, ObservableCollection<Model.Ship> ships, ObservableCollection<Model.Cell> cellList)
        {
            for (int i = 0; i < ships.Count; i++)
            {
                for (int m = 0; m < ships[i].ShipHP.Count; m++)
                {
                    if (cell.ID == ships[i].ShipHP[m])
                    {
                        ships[i].ShipHP.RemoveAt(m);
                        if (ships[i].ShipHP.Count == 0)
                        {
                            int remove = i;
                            int minID = cellList[0].ID;
                            int maxID = cellList[ammount_of_cells - 1].ID;
                            for (int j = 0; j < ships[i].ShipCells.Count; j++)
                            {
                                for (int k = 0; k < cellList.Count; k++)
                                {
                                    if (cellList[k].ID == ships[i].ShipCells[j])
                                    {
                                        cellList[k].IsDead = true;

                                        if (cellList[k].ID - FieldWidth - 1 >= minID && !forbiddenLeftChecks.Contains(cellList[k].ID - FieldWidth - 1))
                                        {
                                            int newID = cellList[k].ID - FieldWidth - 1;
                                            ScanCell(newID, cellList);
                                        }
                                        if (cellList[k].ID - FieldWidth >= minID)
                                        {
                                            int newID = cellList[k].ID - FieldWidth;
                                            ScanCell(newID, cellList);
                                        }
                                        if (cellList[k].ID - FieldWidth + 1 >= minID && !forbiddenRightChecks.Contains(cellList[k].ID - FieldWidth + 1))
                                        {
                                            int newID = cellList[k].ID - FieldWidth + 1;
                                            ScanCell(newID, cellList);
                                        }

                                        if (cellList[k].ID + FieldWidth - 1 <= maxID && !forbiddenLeftChecks.Contains(cellList[k].ID + FieldWidth - 1))
                                        {
                                            int newID = cellList[k].ID + FieldWidth - 1;
                                            ScanCell(newID, cellList);
                                        }
                                        if (cellList[k].ID + FieldWidth <= maxID)
                                        {
                                            int newID = cellList[k].ID + FieldWidth;
                                            ScanCell(newID, cellList);
                                        }
                                        if (cellList[k].ID + FieldWidth + 1 <= maxID && !forbiddenRightChecks.Contains(cellList[k].ID + FieldWidth + 1))
                                        {
                                            int newID = cellList[k].ID + FieldWidth + 1;
                                            ScanCell(newID, cellList);
                                        }

                                        if (cellList[k].ID - 1 >= minID && !forbiddenLeftChecks.Contains(cellList[k].ID - 1))
                                        {
                                            int newID = cellList[k].ID - 1;
                                            ScanCell(newID, cellList);
                                        }
                                        if (cellList[k].ID + 1 <= maxID && !forbiddenRightChecks.Contains(cellList[k].ID + 1))
                                        {
                                            int newID = cellList[k].ID + 1;
                                            ScanCell(newID, cellList);
                                        }
                                    }
                                }

                            }
                            ships.RemoveAt(remove);
                            if (ships.Count == 0)
                            {
                                
                                Win = Visibility.Visible;
                                return true;
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
            return false;
        }

        

        public void ScanCell(int newID, ObservableCollection<Model.Cell> cellList)
        {
            for (int k = 0; k < ammount_of_cells; k++)
            {
                if (cellList[k].ID == newID)
                {
                    cellList[k].IsDiscovered = true;
                    cellList[k].IsEnabled = false;
                }
            }
        }
        
        
        public int ammount_of_cells;

        private Int32 fieldWidth;
        public Int32 FieldWidth
        {
            get { return fieldWidth; }
            set { fieldWidth = value; OnPropertyChanged("FieldWidth"); }
        }
        private Int32 fieldHeight;
        public Int32 FieldHeight
        {
            get { return fieldHeight; }
            set { fieldHeight = value; OnPropertyChanged("FieldHeight"); }
        }

        private List<int> forbiddenLeftChecks;
        public List<int> ForbiddenLeftChecks
        {
            get { return forbiddenLeftChecks; }
            set { forbiddenLeftChecks = value; }
        }
        private List<int> forbiddenRightChecks;
        public List<int> ForbiddenRightChecks
        {
            get { return forbiddenRightChecks; }
            set { forbiddenRightChecks = value; }
        }
        private List<int> shipHP;
        public List<int> ShipHP
        {
            get { return shipHP; }
            set { shipHP = value; }
        }
        public MainMenuVM() 
        {
            InsertWin = Visibility.Hidden;
            FieldWidth = 10;
            FieldHeight = 10;
            Win = Visibility.Hidden;
            Stage1 = "Visible";
            Stage2 = "Hidden";
            Stage3 = "Hidden";
            IsEnabled = "Hidden";

            ErrorMessage = "Hidden";
            ErrorMessageContent = "";

            ShipSelector = 4;
            ShipList = new ObservableCollection<Model.Ship>();
            ShipList1 = new ObservableCollection<Model.Ship>();

            CellList = new ObservableCollection<Model.Cell>();
            CellList.CollectionChanged += AddCheckIfGotHit;
            CellList1 = new ObservableCollection<Model.Cell>();
            CellList1.CollectionChanged += AddCheckIfGotHit;

            PlaningCellList = new ObservableCollection<PlaningCell>();
            PlaningCellList.CollectionChanged += AddPlaceShipUpdate;
            PlaningCellList1 = new ObservableCollection<PlaningCell>();
            PlaningCellList1.CollectionChanged += AddPlaceShipUpdate;

            AuthedShips = new List<List<int>>();
            ShipHP = new List<int>();


        }


        private string stage1;

        public string Stage1
        {
            get { return stage1; }
            set { stage1 = value; OnPropertyChanged("Stage1"); }
        }
        private string stage2;

        public string Stage2
        {
            get { return stage2; }
            set { stage2 = value; OnPropertyChanged("Stage2"); }
        }
        private string stage3;
        public string Stage3
        {
            get { return stage3; }
            set { stage3 = value; OnPropertyChanged("Stage3"); }
        }
        

        public RelayCommand EnterStage2
        {
            get
            {
                return new RelayCommand(() =>
                {

                    if (FieldHeight * FieldWidth > 60)
                    {
                        ShipList.Clear();
                        ShipList1.Clear();

                        CellList.Clear();
                        CellList1.Clear();

                        PlaningCellList.Clear();
                        PlaningCellList1.Clear();

                        ammount_of_cells = FieldHeight * FieldWidth;
                        ForbiddenLeftChecks = new List<int>();
                        for (int i = 0; i < FieldHeight * 2; i++)
                        {
                            ForbiddenLeftChecks.Add(i * FieldWidth - 1);
                        }

                        ForbiddenRightChecks = new List<int>();
                        for (int i = 0; i < FieldHeight * 2; i++)
                        {
                            ForbiddenRightChecks.Add(i * FieldWidth);
                        }
                        for (int i = 0; i < ammount_of_cells; i++)
                        {
                            PlaningCellList.Add(new Model.PlaningCell(i));
                            PlaningCellList1.Add(new Model.PlaningCell(i + ammount_of_cells));
                        }
                        Stage1 = "Hidden";
                        Stage2 = "Visible";
                    }
                    
                });
            }
            set
            {
            }
        }

        public RelayCommand EnterStage3
        {
            get
            {
                return new RelayCommand(() =>
                {
                    ShipList.Clear();
                    CellList.Clear();
                    AuthedShips.Clear();
                    int counter = 0;
                    bool Overflow = false;
                    List<int> ship = new List<int>();
                    List<int> scanedIDs = new List<int>();
                    
                    FourHPShips = 1;
                    ThreeHPShips = 2;
                    TwoHPShips = 3;
                    OneHPShips = 4;
                    for (int i = 0; i < ammount_of_cells; i++)
                    {
                        if (PlaningCellList[i].MaxSizeOfHostedShip != -1)
                        {
                            bool detectCopy = false;
                            for (int j = 0; j < AuthedShips.Count; j++)
                            {
                                ship.Clear();
                                for (int m = 0; m < AuthedShips[j].Count; m++)
                                {
                                    ship.Add(AuthedShips[j][m]);
                                }
                                if (ship.Count == PlaningCellList[i].HostedShips.Count)
                                {
                                    for (int m = 0; m < PlaningCellList[i].HostedShips.Count; m++)
                                    {
                                        if (ship[m] == PlaningCellList[i].HostedShips[m])
                                        {
                                            detectCopy = true;
                                            break;
                                        }
                                    }
                                }
                            }
                            if (!detectCopy)
                            {
                                
                                foreach (int id in PlaningCellList[i].HostedShips)
                                {
                                    scanedIDs.Add(id);
                                }
                                switch (PlaningCellList[i].HostedShips.Count)
                                {
                                    case 1:
                                        if (OneHPShips == 0)
                                        {
                                            Overflow = true;
                                        }
                                        else
                                        {
                                            OneHPShips--;
                                        }
                                        break;
                                    case 2:
                                        if (TwoHPShips == 0)
                                        {
                                            Overflow = true;
                                        }
                                        else
                                        {
                                            TwoHPShips--;
                                        }
                                        break;
                                    case 3:
                                        if (ThreeHPShips == 0)
                                        {
                                            Overflow = true;
                                        }
                                        else
                                        {
                                            ThreeHPShips--;
                                        }
                                        break;
                                    case 4:
                                        if (FourHPShips == 0)
                                        {
                                            Overflow = true;
                                        }
                                        else
                                        {
                                            FourHPShips--;
                                        }
                                        break;
                                }
                                if (Overflow)
                                {
                                    break;
                                }
                                switch (counter)
                                {
                                    case 0:
                                        List<int> list1 = new List<int>();
                                        for (int j = 0; j < PlaningCellList[i].HostedShips.Count; j++)
                                        {
                                            list1.Add(PlaningCellList[i].HostedShips[j]);
                                        }
                                        Ship ship1 = new Ship(list1);
                                        ShipList.Add(ship1);
                                        counter++;
                                        break;
                                    case 1:
                                        List<int> list2 = new List<int>();
                                        for (int j = 0; j < PlaningCellList[i].HostedShips.Count; j++)
                                        {
                                            list2.Add(PlaningCellList[i].HostedShips[j]);
                                        }
                                        Ship ship2 = new Ship(list2);
                                        ShipList.Add(ship2);
                                        counter++;
                                        break;
                                    case 2:
                                        List<int> list3 = new List<int>();
                                        for (int j = 0; j < PlaningCellList[i].HostedShips.Count; j++)
                                        {
                                            list3.Add(PlaningCellList[i].HostedShips[j]);
                                        }
                                        Ship ship3 = new Ship(list3);
                                        ShipList.Add(ship3);
                                        counter++;
                                        break;
                                    case 3:
                                        List<int> list4 = new List<int>();
                                        for (int j = 0; j < PlaningCellList[i].HostedShips.Count; j++)
                                        {
                                            list4.Add(PlaningCellList[i].HostedShips[j]);
                                        }
                                        Ship ship4 = new Ship(list4);
                                        ShipList.Add(ship4);
                                        counter++;
                                        break;
                                    case 4:
                                        List<int> list5 = new List<int>();
                                        for (int j = 0; j < PlaningCellList[i].HostedShips.Count; j++)
                                        {
                                            list5.Add(PlaningCellList[i].HostedShips[j]);
                                        }
                                        Ship ship5 = new Ship(list5);
                                        ShipList.Add(ship5);
                                        counter++;
                                        break;
                                    case 5:
                                        List<int> list6 = new List<int>();
                                        for (int j = 0; j < PlaningCellList[i].HostedShips.Count; j++)
                                        {
                                            list6.Add(PlaningCellList[i].HostedShips[j]);
                                        }
                                        Ship ship6 = new Ship(list6);
                                        ShipList.Add(ship6);
                                        counter++;
                                        break;
                                    case 6:
                                        List<int> list7 = new List<int>();
                                        for (int j = 0; j < PlaningCellList[i].HostedShips.Count; j++)
                                        {
                                            list7.Add(PlaningCellList[i].HostedShips[j]);
                                        }
                                        Ship ship7 = new Ship(list7);
                                        ShipList.Add(ship7);
                                        counter++;
                                        break;
                                    case 7:
                                        List<int> list8 = new List<int>();
                                        for (int j = 0; j < PlaningCellList[i].HostedShips.Count; j++)
                                        {
                                            list8.Add(PlaningCellList[i].HostedShips[j]);
                                        }
                                        Ship ship8 = new Ship(list8);
                                        ShipList.Add(ship8);
                                        counter++;
                                        break;
                                    case 8:
                                        List<int> list9 = new List<int>();
                                        for (int j = 0; j < PlaningCellList[i].HostedShips.Count; j++)
                                        {
                                            list9.Add(PlaningCellList[i].HostedShips[j]);
                                        }
                                        Ship ship9 = new Ship(list9);
                                        ShipList.Add(ship9);
                                        counter++;
                                        break;
                                    case 9:
                                        List<int> list10 = new List<int>();
                                        for (int j = 0; j < PlaningCellList[i].HostedShips.Count; j++)
                                        {
                                            list10.Add(PlaningCellList[i].HostedShips[j]);
                                        }
                                        Ship ship10 = new Ship(list10);
                                        ShipList.Add(ship10);
                                        counter++;
                                        break;

                                }

                                AuthedShips.Add(PlaningCellList[i].HostedShips);
                            }
                            
                        }
                        if (scanedIDs.Contains(i))
                        {
                            CellList.Add(new Model.Cell(i, true));
                        }
                        else
                        {
                            CellList.Add(new Model.Cell(i));
                        }
                    }
                    AuthedShips.Clear();
                    if (Overflow)
                    {
                        ErrorMessage = "Visible";
                        ErrorMessageContent = "Overflow";
                    }
                    else if (OneHPShips == 0 && TwoHPShips == 0 && ThreeHPShips == 0 && FourHPShips == 0)
                    {
                        Stage2 = "Hidden";
                        Stage3 = "Visible";
                        ErrorMessage = "Hidden";
                    }
                    else
                    {
                        ErrorMessage = "Visible";
                        ErrorMessageContent = "Not Enough";
                    }



                });
            }
            set
            {
            }
        }

        public RelayCommand Finish
        {
            get
            {
                return new RelayCommand(() =>
                {
                    ShipList1.Clear();
                    CellList1.Clear();
                    AuthedShips.Clear();
                    int counter = 0;
                    bool Overflow = false;
                    List<int> ship = new List<int>();
                    List<int> scanedIDs = new List<int>();
                    FourHPShips = 1;
                    ThreeHPShips = 2;
                    TwoHPShips = 3;
                    OneHPShips = 4;
                    for (int i = 0; i < ammount_of_cells; i++)
                    {
                        if (PlaningCellList1[i].MaxSizeOfHostedShip != -1)
                        {
                            bool detectCopy = false;
                            for (int j = 0; j < AuthedShips.Count; j++)
                            {
                                ship.Clear();
                                for (int m = 0; m < AuthedShips[j].Count; m++)
                                {
                                    ship.Add(AuthedShips[j][m]);
                                }
                                if (ship.Count == PlaningCellList1[i].HostedShips.Count)
                                {
                                    for (int m = 0; m < PlaningCellList1[i].HostedShips.Count; m++)
                                    {
                                        if (ship[m] == PlaningCellList1[i].HostedShips[m])
                                        {
                                            detectCopy = true;
                                            break;
                                        }
                                    }
                                }
                            }
                            if (!detectCopy)
                            {
                                ShipHP.Clear();
                                foreach (int id in PlaningCellList1[i].HostedShips)
                                {
                                    scanedIDs.Add(id);
                                    ShipHP.Add(id);
                                }
                                switch (PlaningCellList1[i].HostedShips.Count)
                                {
                                    case 1:
                                        if (OneHPShips == 0)
                                        {
                                            Overflow = true;
                                        }
                                        else
                                        {
                                            OneHPShips--;
                                        }
                                        break;
                                    case 2:
                                        if (TwoHPShips == 0)
                                        {
                                            Overflow = true;
                                        }
                                        else
                                        {
                                            TwoHPShips--;
                                        }
                                        break;
                                    case 3:
                                        if (ThreeHPShips == 0)
                                        {
                                            Overflow = true;
                                        }
                                        else
                                        {
                                            ThreeHPShips--;
                                        }
                                        break;
                                    case 4:
                                        if (FourHPShips == 0)
                                        {
                                            Overflow = true;
                                        }
                                        else
                                        {
                                            FourHPShips--;
                                        }
                                        break;
                                }
                                if (Overflow)
                                {
                                    break;
                                }
                                switch (counter)
                                {
                                    case 0:
                                        List<int> list1 = new List<int>();
                                        for (int j = 0; j < PlaningCellList1[i].HostedShips.Count; j++)
                                        {
                                            list1.Add(PlaningCellList1[i].HostedShips[j]);
                                        }
                                        Ship ship1 = new Ship(list1);
                                        ShipList1.Add(ship1);
                                        counter++;
                                        break;
                                    case 1:
                                        List<int> list2 = new List<int>();
                                        for (int j = 0; j < PlaningCellList1[i].HostedShips.Count; j++)
                                        {
                                            list2.Add(PlaningCellList1[i].HostedShips[j]);
                                        }
                                        Ship ship2 = new Ship(list2);
                                        ShipList1.Add(ship2);
                                        counter++;
                                        break;
                                    case 2:
                                        List<int> list3 = new List<int>();
                                        for (int j = 0; j < PlaningCellList1[i].HostedShips.Count; j++)
                                        {
                                            list3.Add(PlaningCellList1[i].HostedShips[j]);
                                        }
                                        Ship ship3 = new Ship(list3);
                                        ShipList1.Add(ship3);
                                        counter++;
                                        break;
                                    case 3:
                                        List<int> list4 = new List<int>();
                                        for (int j = 0; j < PlaningCellList1[i].HostedShips.Count; j++)
                                        {
                                            list4.Add(PlaningCellList1[i].HostedShips[j]);
                                        }
                                        Ship ship4 = new Ship(list4);
                                        ShipList1.Add(ship4);
                                        counter++;
                                        break;
                                    case 4:
                                        List<int> list5 = new List<int>();
                                        for (int j = 0; j < PlaningCellList1[i].HostedShips.Count; j++)
                                        {
                                            list5.Add(PlaningCellList1[i].HostedShips[j]);
                                        }
                                        Ship ship5 = new Ship(list5);
                                        ShipList1.Add(ship5);
                                        counter++;
                                        break;
                                    case 5:
                                        List<int> list6 = new List<int>();
                                        for (int j = 0; j < PlaningCellList1[i].HostedShips.Count; j++)
                                        {
                                            list6.Add(PlaningCellList1[i].HostedShips[j]);
                                        }
                                        Ship ship6 = new Ship(list6);
                                        ShipList1.Add(ship6);
                                        counter++;
                                        break;
                                    case 6:
                                        List<int> list7 = new List<int>();
                                        for (int j = 0; j < PlaningCellList1[i].HostedShips.Count; j++)
                                        {
                                            list7.Add(PlaningCellList1[i].HostedShips[j]);
                                        }
                                        Ship ship7 = new Ship(list7);
                                        ShipList1.Add(ship7);
                                        counter++;
                                        break;
                                    case 7:
                                        List<int> list8 = new List<int>();
                                        for (int j = 0; j < PlaningCellList1[i].HostedShips.Count; j++)
                                        {
                                            list8.Add(PlaningCellList1[i].HostedShips[j]);
                                        }
                                        Ship ship8 = new Ship(list8);
                                        ShipList1.Add(ship8);
                                        counter++;
                                        break;
                                    case 8:
                                        List<int> list9 = new List<int>();
                                        for (int j = 0; j < PlaningCellList1[i].HostedShips.Count; j++)
                                        {
                                            list9.Add(PlaningCellList1[i].HostedShips[j]);
                                        }
                                        Ship ship9 = new Ship(list9);
                                        ShipList1.Add(ship9);
                                        counter++;
                                        break;
                                    case 9:
                                        List<int> list10 = new List<int>();
                                        for (int j = 0; j < PlaningCellList1[i].HostedShips.Count; j++)
                                        {
                                            list10.Add(PlaningCellList1[i].HostedShips[j]);
                                        }
                                        Ship ship10 = new Ship(list10);
                                        ShipList1.Add(ship10);
                                        counter++;
                                        break;


                                }
                                AuthedShips.Add(PlaningCellList1[i].HostedShips);
                            }

                        }
                        if (scanedIDs.Contains(i + ammount_of_cells))
                        {
                            CellList1.Add(new Model.Cell(i + ammount_of_cells, true));
                        }
                        else
                        {
                            CellList1.Add(new Model.Cell(i + ammount_of_cells));
                        }
                    }
                    AuthedShips.Clear();
                    if (Overflow)
                    {
                        ErrorMessage = "Visible";
                        ErrorMessageContent = "Overflow";
                    }
                    else if (OneHPShips == 0 && TwoHPShips == 0 && ThreeHPShips == 0 && FourHPShips == 0)
                    {
                        Stage3 = "Hidden";
                        IsEnabled = "Visible";
                        ErrorMessage = "Hidden";
                    }
                    else
                    {
                        ErrorMessage = "Visible";
                        ErrorMessageContent = "Not Enough";
                    }


                });
            }
            set
            {
            }
        }

        public RelayCommand SetSelectorTo4
        {
            get
            {
                return new RelayCommand(() =>
                {

                    ShipSelector = 4;
                });
            }
            set
            {
            }
        }

        public RelayCommand SetSelectorTo3
        {
            get
            {
                return new RelayCommand(() =>
                {

                    ShipSelector = 3;
                });
            }
            set
            {
            }
        }

        public RelayCommand SetSelectorTo2
        {
            get
            {
                return new RelayCommand(() =>
                {

                    ShipSelector = 2;
                });
            }
            set
            {
            }
        }

        public RelayCommand SetSelectorTo1
        {
            get
            {
                return new RelayCommand(() =>
                {

                    ShipSelector = 1;
                });
            }
            set
            {
            }
        }
        private string insertedWinner;
        public string InsertedWinner
        {
            get { return insertedWinner; }
            set { insertedWinner = value; OnPropertyChanged("InsertedWinner"); }
        }
        public RelayCommand InsertingWin
        {
            get
            {
                return new RelayCommand(() =>
                {
                    int wins = 0;
                    bool winnerFound = false;
                    InsertWin = Visibility.Hidden;
                    string strConn = "Server=localhost\\SQLEXPRESS;" +
                    "Database=mvvm_sql_db;" +
                    "Trusted_Connection=True;" +
                    "TrustServerCertificate=True;";

                    string sqlComm = "SELECT * FROM [leaderboard];";
                    using (SqlConnection conn = new SqlConnection(strConn))
                    {
                        try
                        {
                            conn.Open();
                            Console.WriteLine("CONNECTED");

                            SqlCommand command = new SqlCommand(sqlComm, conn);

                            SqlDataReader reader = command.ExecuteReader();

                            for (int i = 0; i < reader.FieldCount; i+=3)
                            {
                                if (InsertedWinner == (string)reader[i+1])
                                {
                                    winnerFound = true;
                                    wins = (int)reader[i+2];
                                }
                            }
                            Console.WriteLine();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                    if (winnerFound)
                    {
                        string sqlComm1 = $"UPDATE[leaderboard]SET [wins] = {wins} WHERE [user] = {InsertedWinner}";
                        using (SqlConnection conn = new SqlConnection(strConn))
                        {
                            try
                            {
                                conn.Open();
                                Console.WriteLine("CONNECTED");

                                SqlCommand command = new SqlCommand(sqlComm1, conn);

                                if (command.ExecuteNonQuery() > 0)
                                    Console.WriteLine("ADDED!");
                                else
                                    Console.WriteLine("ERROR!");

                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }
                    }
                    else
                    {
                        string sqlComm1 = $"INSERT INTO[leaderboard]([user], [wins]) VALUES('{InsertedWinner}', '{1}'); ";
                        using (SqlConnection conn = new SqlConnection(strConn))
                        {
                            try
                            {
                                conn.Open();
                                Console.WriteLine("CONNECTED");

                                SqlCommand command = new SqlCommand(sqlComm1, conn);

                                if (command.ExecuteNonQuery() > 0)
                                    Console.WriteLine("ADDED!");
                                else
                                    Console.WriteLine("ERROR!");

                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }
                    }
                    
                });
            }
            set
            {
            }
        }
    }
}
