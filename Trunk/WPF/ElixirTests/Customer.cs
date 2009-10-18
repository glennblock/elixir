using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.ComponentModel;

namespace ElixirTests
{
public class Customer : INotifyPropertyChanged
{
    private string _firstName;

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

    public ObservableValue<string> Name { get; set; }
    
    private string _lastName;

    public string LastName
    {
        get { return this._lastName; }
        set
        {
            if (this._lastName != value)
            {
                this._lastName = value;
                this.RaisePropertyChanged("LastName");
            }
        }
    }


    protected void RaisePropertyChanged(string property)
    {
        this.PropertyChanged(this, new PropertyChangedEventArgs(property));
    }

    public event PropertyChangedEventHandler PropertyChanged = delegate { };
}

}
