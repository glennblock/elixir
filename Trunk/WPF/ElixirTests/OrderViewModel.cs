using System.Collections.ObjectModel;
using System.ComponentModel;

namespace ElixirTests
{
    public class OrderViewModel : INotifyPropertyChanged
    {

        public OrderViewModel()
        {
            this.Categories = new ObservableCollection<string>();
            this.SelectedCategories = new ObservableCollection<string>();
            this.Customers = new ObservableCollection<string>();
        }

        public bool SavedWasCalled;
 
        public void Save()
        {
            SavedWasCalled = true;
        }

        public bool IsSaveEnabled { get; set; }
        
        public string FirstName { get; set; }
        public ObservableCollection<string> Categories { get; private set; }
        public ObservableCollection<string> SelectedCategories { get; set; }

        public ObservableCollection<string> Customers { get; private set; }
        public string SelectedCustomer { get; set; }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        private void RaisePropertyChanged(string property)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(property));
        }
    }
}