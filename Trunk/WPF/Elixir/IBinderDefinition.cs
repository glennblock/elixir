using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Elixir
{
    public interface IBinderDefinition
    {
        IBinder Create(FrameworkElement view, object viewModel);
    }
}
