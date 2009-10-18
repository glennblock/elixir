#if !SILVERLIGHT
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using Microsoft.Practices.Composite.Presentation.Commands;

namespace Elixir
{
    
    //Todo: Add Keybinder implementation
    public class ShortcutBinder : IBinder
    {
        public FrameworkElement ViewElement { get; private set; }
        public InputBinding InputBinding { get; private set; }
        private ICommand _command; 

        public ShortcutBinder(Control control, Expression<Action<object>> viewModelAction, Expression<Func<object, bool>> actionEnabled, KeyGesture keyGesture){
            this.ViewElement = control;
            var action = viewModelAction.Compile();
            var enabled = actionEnabled.Compile();
            _command = new DelegateCommand<object>(p => action(p), p => enabled(p));
            this.InputBinding = new InputBinding(_command, keyGesture);
        }

        public void Bind()
        {
            throw new System.NotImplementedException();
        }

        public Binding Binding
        {
            get { throw new System.NotImplementedException(); }
        }
    }
}
#endif