using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace Elixir
{
    public class ValueBinderDefinition<TModel> : IValueBinderDefinition
    {
        private BindingMode _bindingMode = System.Windows.Data.BindingMode.TwoWay;
        private bool _createCalled = false;

        public System.Windows.Data.BindingMode BindingMode
        {
            get { return _bindingMode; }
            set { _bindingMode = value; }
        }

        public string TargetPath { get; private set; }
        public DependencyProperty Property { get; private set; }
        public string ViewElementName { get; private set; }

        public ValueBinderDefinition(Expression<Func<TModel, object>> targetProperty)
        {
            Initialize(null, targetProperty);
        }

        public ValueBinderDefinition(string elementName,Expression<Func<TModel, object>> targetProperty)
        {
            Initialize(elementName, targetProperty);
        }

        public ValueBinderDefinition(string elementName, DependencyProperty sourceProperty, Expression<Func<TModel, object>> targetProperty)
        {
            Property = sourceProperty;
            Initialize(elementName,targetProperty);
        }

        private void Initialize(string elementName, Expression<Func<TModel, object>> targetProperty)
        {

            if (targetProperty != null)
            {
                this.TargetPath = ExpressionUtils.GetExpressionPropertyPath(targetProperty);
            }
            else
                this.TargetPath = elementName;
            this.ViewElementName = elementName ?? this.TargetPath;
        }

        public IBinder Create(FrameworkElement view, object viewModel)
        {
            var element = (FrameworkElement)view.FindName(ViewElementName);

            if (!_createCalled)
            {
                if (this.Property == null)
                    this.Property = ViewBinder.GetPropertyForElement(element);

                _createCalled = true;
            }
            
            var binder = new ValueBinder<TModel>(element, this.Property, (TModel) viewModel, this.TargetPath);
            return binder;
        }
    }
}
