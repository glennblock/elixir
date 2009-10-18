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

namespace ElixirTests
{
    public class MockTarget<T> : FrameworkElement, INotifyPropertyChanged
    {
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(T), typeof(MockTarget<T>), new PropertyMetadata(ValueChanged));

        public static T GetValue(MockTarget<T> element)
        {
            return (T) element.GetValue(ValueProperty);
        }

        public static void SetValue(MockTarget<T> element, T value)
        {
            element.SetValue(ValueProperty, value);
        }

        public static void ValueChanged(DependencyObject element, DependencyPropertyChangedEventArgs e)
        {
            ((MockTarget<T>)element).PropertyChanged(element, new PropertyChangedEventArgs(MockSource.Value));
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        #endregion
    }

    public class MockSource
    {
        public const string Value = "Value";
    }

    public class MockSource<T> : INotifyPropertyChanged
    {
        private T _value;

        public MockSource(T value)
        {
            this._value = value;
        }

        public T Value
        {
            get { return this._value; }
            set
            {
                this._value = value;
                this.PropertyChanged(this, new PropertyChangedEventArgs(MockSource.Value));
            }
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        #endregion
    }



}
