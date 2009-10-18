using System.Windows;
using System.Windows.Data;

namespace Elixir
{
    public interface IBinder
    {
        FrameworkElement ViewElement { get; }
        void Bind();
        Binding Binding { get; }
    }
}