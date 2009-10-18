using System.Windows.Data;

namespace Elixir
{
    public interface IValueBinder : IBinder, IDependencyPropertyBinder
    {
        BindingMode BindingMode { get; set; }
        void SetValue(object value);
        object GetValue();
        T GetValue<T>();
        string TargetPath { get; }
    }
}