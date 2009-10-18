using System;
using System.Linq.Expressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Elixir;
using Expression=System.Linq.Expressions.Expression;

namespace Elixir
{
    //Todo: Add value converters

    public class ValueBinder<TModel> : IValueBinder
    {
        private Binding _binding;
        public Binding Binding
        {
            get { return _binding;} 
            set { _binding = value;}
        }

        private BindingMode _bindingMode = System.Windows.Data.BindingMode.TwoWay;
        public BindingMode BindingMode
        {
            get { return _bindingMode;}
            set { _bindingMode = value;}
        }
        
        private TModel _viewModel;
        public FrameworkElement ViewElement { get; private set; }
        public DependencyProperty Property { get; private set; }
        public string TargetPath { get; private set; }


        public ValueBinder(FrameworkElement element, DependencyProperty sourceProperty, TModel viewModel, string targetPath)
        {
            Initialize(element, sourceProperty, targetPath, null,viewModel );
        }


        public ValueBinder(FrameworkElement element, DependencyProperty sourceProperty, TModel viewModel, Expression<Func<TModel, object>> targetProperty)
        {
            Initialize(element, sourceProperty, targetProperty, null, viewModel);
        }

        public ValueBinder(FrameworkElement element, TModel viewModel)
        {
            Initialize(element, ViewBinder.GetPropertyForElement(element), element.Name, null,viewModel);
        }

        public ValueBinder(FrameworkElement element, TModel viewModel, Expression<Func<TModel, object>> targetProperty)
        {
            Initialize(element, ViewBinder.GetPropertyForElement(element), targetProperty, null, viewModel);
        }

        public ValueBinder(FrameworkElement element, Expression<Func<TModel, object>> targetProperty, TModel viewModel)
        {
            Initialize(element, ViewBinder.GetPropertyForElement(element), targetProperty,null,viewModel);
        }

        public ValueBinder(Expression<Func<TModel,object>> targetProperty, FrameworkElement view)
            : this(targetProperty, view, default(TModel))
        {
        }

        public ValueBinder(Expression<Func<TModel,object>> targetProperty, FrameworkElement view, TModel viewModel)
        {
            string path = ExpressionUtils.GetExpressionPropertyPath(targetProperty);
            string elementName = ViewBinder.GetElementForModelProperty(path);
            var element = (FrameworkElement) view.FindName(elementName);
            if (viewModel == null)
                viewModel = (TModel) ViewBinder.GetModel(element);
            
            Initialize(
                element,
                null,
                path, 
                null,
                viewModel
            );
        }

        private void Initialize(FrameworkElement element, DependencyProperty sourceProperty, Expression<Func<TModel, object>> targetProperty, Binding binding, TModel viewModel)
        {
            string targetPath = null;
            if (targetProperty != null)
                targetPath = ExpressionUtils.GetExpressionPropertyPath(targetProperty);
            else
                targetPath = element.Name;

            Initialize(element, sourceProperty, targetPath, binding,viewModel);
        }

        private void Initialize(FrameworkElement element, DependencyProperty sourceProperty, string targetPath, Binding binding, TModel viewModel)
        {
            this.ViewElement = element;
            this.Property = sourceProperty ?? ViewBinder.GetPropertyForElement(element);
            this.TargetPath = targetPath;
            this._viewModel = viewModel;
            if (binding != null)
            {
                this.Binding = binding;
                this.BindingMode = binding.Mode;
            }
        }

        public void Bind()
        {
            if (Binding == null)
                Binding = new Binding() {Mode=BindingMode};
            this.Binding.Source = _viewModel;
            this.Binding.Path = new PropertyPath(TargetPath);
            ViewElement.SetBinding(Property, this.Binding);

        }


        public void SetValue(object value)
        {
            this.ViewElement.SetValue(this.Property, value);
        }

        public object GetValue()
        {
            return this.ViewElement.GetValue(this.Property);
        }

        public T GetValue<T>()
        {
            return (T)this.GetValue();
        }

        public override string ToString()
        {
            return "ValueBinder: " + this.ViewElement.Name + "." + this.Property.Name + " <==> " + this.Binding.Path.Path;
        }
    }
}