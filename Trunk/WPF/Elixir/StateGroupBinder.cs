using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Linq.Expressions;

namespace Elixir
{
    public class StateGroupBinder<T> : IBinder
    {
        public System.Windows.FrameworkElement ViewElement { get; private set; }
        public void Bind()
        {
            throw new System.NotImplementedException();
        }

        public Binding Binding
        {
            get { throw new System.NotImplementedException(); }
        }

        public StateGroupBinder(Button button, DependencyProperty property, Expression<Func<T, T>> stateProperty)
        {
            throw new NotImplementedException();
        }

        public StateGroupBinder(Button button, DependencyProperty property, Expression<Func<T, T>> stateProperty, Binding binding)
        {
            throw new NotImplementedException();
        }


    }
}
