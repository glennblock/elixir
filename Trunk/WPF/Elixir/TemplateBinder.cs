using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Linq.Expressions;
using System.Windows.Documents;

namespace Elixir
{
    public class TemplateBinder<TModel> : ITemplateBinder
    {
        public IList<IBinderDefinition> BinderDefinitions { get; private set; }

        public TemplateBinder(string template)
        {
            Template = template;            
            this.BinderDefinitions = new List<IBinderDefinition>();
        }

        public string Template { get; set; }

        public IValueBinderDefinition Value(Expression<Func<TModel, object>> targetProperty)
        {
            var binderDefinition = new ValueBinderDefinition<TModel>(targetProperty);
            BinderDefinitions.Add(binderDefinition);
            return binderDefinition;
        }

        public IValueBinderDefinition Value(string elementName, Expression<Func<TModel, object>> targetProperty)
        {
            var binderDefinition = new ValueBinderDefinition<TModel>(elementName, targetProperty);
            BinderDefinitions.Add(binderDefinition);
            return binderDefinition;
        }

        public IValueBinderDefinition Value(string elementName, DependencyProperty sourceProperty, Expression<Func<TModel, object>> targetProperty)
        {
            var binderDefinition = new ValueBinderDefinition<TModel>(elementName, sourceProperty, targetProperty);
            BinderDefinitions.Add(binderDefinition);
            return binderDefinition;
        }

        public IViewBinder CreateViewBinder(FrameworkElement view, object viewModel)
        {
            ViewBinder<TModel> viewBinder = new ViewBinder<TModel>(view, (TModel) viewModel);
            foreach(var binderDefinition in this.BinderDefinitions) 
                viewBinder.AddBinders(binderDefinition.Create(view, viewModel));
            return viewBinder;
        }

    }
}
