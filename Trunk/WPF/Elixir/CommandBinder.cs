using System;
using System.Reflection;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace Elixir
{
    public class CommandBinder<TEventArgs> : IBinder
    {
        public ICommand Command { get; private set; }
        private Type _sourceType;
        private Control _control;
        public string EventName { get; private set; }

        public System.Windows.FrameworkElement ViewElement { get; private set; }

        public CommandBinder(Control control, string eventName, ICommand command)
        {
            this.ViewElement = control;
            this.Command = command;
            this.EventName = eventName;
            this._sourceType = control.GetType();
            this._control = control;
        }

        public void Bind()
        {
            EventInfo eventInfo = _sourceType.GetEvent(EventName);
            Delegate eventDelegate = Delegate.CreateDelegate(eventInfo.EventHandlerType, this, "EventHandler");
            _sourceType.GetEvent(EventName).AddEventHandler(_control, eventDelegate);
            this.BindSourceIsEnabled(_control, Command);
        }

        Binding IBinder.Binding
        {
            get { return null; }
        }

            private void BindSourceIsEnabled(Control control, ICommand command)
        {
            command.CanExecuteChanged += (s, e) =>
                {
                    control.IsEnabled = command.CanExecute(null);
                };
        }

        private void EventHandler(object sender, TEventArgs e)
        {
            if (this.Command.CanExecute(null))
            {
                this.Command.Execute(null);
            }
        }
    }
}