using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace ElixirSampleClient
{
    public class ContactViewModel : INotifyPropertyChanged
    {
        private string _firstName;
        private string _lastName;
        private string _address;
        private bool _onMailingList = true;
        private bool _isSaveEnabled;

        public string FirstName
        {
            get { return this._firstName; }
            set
            {
                if (this._firstName != value)
                {
                    this._firstName = value;
                    this.RaisePropertyChanged("FirstName");
                }
            }
        }

        public string LastName
        {
            get { return this._lastName; }
            set
            {
                if (this._lastName != value)
                {
                    this._lastName = value;
                    this.RaisePropertyChanged("FirstName");
                }
            }
        }

        public string Address
        {
            get { return this._address; }
            set
            {
                if (this._address != value)
                {
                    this._address = value;
                    this.RaisePropertyChanged("Address");
                }
            }
        }

        public bool OnMailingList
        {
            get { return this._onMailingList; }
            set
            {
                if (this._onMailingList != value)
                {
                    this._onMailingList = value;
                    this.RaisePropertyChanged("OnMailingList");
                }
            }
        }

        public void Save()
        {
            IsSaveEnabled = false;
        }

        public bool IsSaveEnabled
        {
            get { return _isSaveEnabled; }
            set
            {
                if (_isSaveEnabled != value)
                {
                    _isSaveEnabled = value;
                    RaisePropertyChanged("IsSaveEnabled");
                }
            }
        }

        public ObservableCollection<City> Cities { get; private set; }
        public ObservableCollection<City> SelectedCities { get; private set; }
        public City SelectedCity { get; set; }

        public ContactViewModel()
        {
            this.Cities = new ObservableCollection<City>() { new City { CityName = "Cincinnati" }, new City { CityName = "San Francisco" }, new City { CityName = "Redmond" }, new City { CityName = "Seattle" } };
            this.SelectedCities = new ObservableCollection<City>() { this.Cities[1], this.Cities[2] };
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            if (propertyName != "IsSaveEnabled")
                IsSaveEnabled = true;
        }

        #endregion
    }

    public class City
    {
        public string CityName { get; set; }
    }
}
