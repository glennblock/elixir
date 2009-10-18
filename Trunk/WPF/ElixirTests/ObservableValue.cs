using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace ElixirTests
{
    public class ObservableValue<T>
        : INotifyPropertyChanged
    {
        public ObservableValue(T initialValue)
        {
            this.Value = initialValue;    
        }

        private T _value;
        public T Value { 
            get { return _value;}
            set { 
                if (!Equals(_value, value))
                {
                    _value = value;
                    var notifyPropertyChanged = _value as INotifyPropertyChanged;
                    if (notifyPropertyChanged != null)
                        notifyPropertyChanged.PropertyChanged+=(s,e) => RaisePropertyChanged(e.PropertyName);
                    
                    RaisePropertyChanged("Value");
                }

            }
        }

        private void RaisePropertyChanged(string property)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(property));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }

}
