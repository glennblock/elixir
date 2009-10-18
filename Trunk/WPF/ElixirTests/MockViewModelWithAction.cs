using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Input;

namespace ElixirTests
{
    public class MockViewModelWithAction : INotifyPropertyChanged
    {
        private bool _isActionEnabled;
        private CommandAction _command = new CommandAction(5);

        public bool IsActionEnabled
        {
            get { return this._isActionEnabled; }
            set
            {
                if (this._isActionEnabled != value)
                {
                    this._isActionEnabled = value;
                    this.PropertyChanged(this, new PropertyChangedEventArgs("IsActionEnabled"));
                }
            }
        }

        public bool ActionCalled { get; set; }

        public CommandAction Command { get { return this._command; } }

        public void Action()
        {
            this.ActionCalled = true;
        }

        public void Action(string arg)
        {
            this.ActionCalled = true;
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        #endregion

        public class CommandAction : ICommand
        {
            public int CallCount { get; set; }
            public int MaxCalls { get; set; }

            public CommandAction(int maxCalls)
            {
                this.MaxCalls = maxCalls;
            }

            #region ICommand Members

            public bool CanExecute(object parameter)
            {
                return this.CallCount < this.MaxCalls;
            }

            public event EventHandler CanExecuteChanged = delegate { };

            public void Execute(object parameter)
            {
                bool couldExecute = this.CanExecute(parameter);

                if (couldExecute)
                {
                    this.CallCount++;

                    if (!this.CanExecute(parameter))
                    {
                        this.CanExecuteChanged(this, EventArgs.Empty);
                    }
                }
            }

            #endregion
        }

    }
}
