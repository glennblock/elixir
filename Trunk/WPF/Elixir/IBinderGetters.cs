using System.Collections.Generic;
using System.Windows;

namespace Elixir
{
    public interface IBinderGetters
    {
        IDependencyPropertyBinder GetPropertyBinder(string viewElementName, DependencyProperty dependencyProperty);
        IDependencyPropertyBinder GetPropertyBinder(FrameworkElement viewElement, DependencyProperty dependencyProperty);
        IMethodBinder GetMethodBinder(string controlName, string eventName);
        IMethodBinder GetMethodBinder(FrameworkElement control, string eventName);
        ITemplateBinder GetTemplateBinder(string template);
        IEnumerable<T> GetBinders<T>(FrameworkElement viewElement);
        IEnumerable<T> GetBinders<T>(string viewElementName);
    }
}