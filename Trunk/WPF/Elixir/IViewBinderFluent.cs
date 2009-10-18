using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Elixir
{
    public interface IViewBinder : IBinder, IList<IBinder>
    {
        IList<ITemplateBinder> TemplateBinders { get; }
        void Bind();
    }

    public interface IViewBinderFluent<TModel> : IBinderGetters
    {
        IViewBinderFluent<TModel> Value(Expression<Func<TModel, object>> targetProperty);
        IViewBinderFluent<TModel> Value(FrameworkElement element, Expression<Func<TModel, object>> targetProperty);
        IViewBinderFluent<TModel> Value(FrameworkElement element, DependencyProperty sourceProperty, Expression<Func<TModel, object>> targetProperty);
        IViewBinderFluent<TModel> List<TItem>(Expression<Func<TModel, IEnumerable<TItem>>> listItemsSource);
        IViewBinderFluent<TModel> List<TItem>(ListBox listbox, Expression<Func<TModel, IEnumerable<TItem>>> listItemsSource, Expression<Func<TModel, IEnumerable<TItem>>> selectedItems);
        IViewBinderFluent<TModel> List<TItem>(ListBox listbox, Expression<Func<TModel, IEnumerable<TItem>>> listItemsSource, Expression<Func<TModel, TItem>> selectedItem);
        IViewBinderFluent<TModel> Action(Expression<Action<object>> action);
        IViewBinderFluent<TModel> Action(Control control, string eventName, Expression<Action<RoutedEventArgs>> action, Expression<Func<object, bool>> actionEnabled);
        IViewBinderFluent<TModel> Template<TTemplateModel>(string template, Action<TemplateBinder<TTemplateModel>> templateRegistration);
        IViewBinderFluent<TModel> AddBinder(IBinder newBinder);
        void Bind();
        FrameworkElement ViewElement {get;set;}
        TModel Model { get; set; }
    }

}
