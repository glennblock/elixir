using System.Collections.Generic;
using System.Windows;

namespace Elixir
{
    public interface ITemplateBinder
    {
        string Template { get; set; }
        IList<IBinderDefinition> BinderDefinitions { get;}
        IViewBinder CreateViewBinder(FrameworkElement view, object viewModel);
    }
}