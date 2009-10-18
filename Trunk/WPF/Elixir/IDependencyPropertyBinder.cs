using System;
namespace Elixir
{
    public interface IDependencyPropertyBinder : IBinder
    {
        System.Windows.DependencyProperty Property { get; }
    }
}
