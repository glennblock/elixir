using System.Collections.ObjectModel;

namespace ElixirTests
{
    public class CustomerListViewModel : System.ComponentModel.INotifyPropertyChanged
    {
        private Customer _selectedCustomer;

        public ObservableCollection<Customer> Customers { get; private set; }

        public Customer SelectedCustomer
        {
            get { return this._selectedCustomer; }
            set
            {
                if (this._selectedCustomer != value)
                {
                    this._selectedCustomer = value;
                    this.PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs("SelectedCustomer"));
                }
            }
        }

        public ObservableCollection<Customer> SelectedCustomers { get; private set; }

        public CustomerListViewModel(params Customer[] customers)
        {
            this.Customers = new ObservableCollection<Customer>();
            this.SelectedCustomers = new ObservableCollection<Customer>();
            foreach(Customer customer in customers)
                this.Customers.Add(customer);
        }

        public CustomerListViewModel(params string[] customerNames)
        {
            this.Customers = new ObservableCollection<Customer>();
            this.SelectedCustomers = new ObservableCollection<Customer>();

            foreach (string name in customerNames)
            {
                this.Customers.Add(new Customer { FirstName = name });
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged = delegate { };
    }
}