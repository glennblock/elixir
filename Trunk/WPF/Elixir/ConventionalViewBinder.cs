using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Markup;

namespace Elixir
{
    public class ConventionalViewBinder<TModel> : ViewBinder<TModel> where TModel : class
    {
        protected ConventionalViewBinder()
        {
        }

        public ConventionalViewBinder(Control view, TModel viewModel)
        {
            ViewBinder.SetModel(view, viewModel);
            Initialize(view);
            BindActions();
            BindProperties();
        }

        protected void Initialize(FrameworkElement view)
        {
            TModel model = ViewBinder.GetModel(view) as TModel;
            this.ViewElement = view;
            this.Model = model;
            this._viewModelType = model.GetType();
        }

        private Type _viewModelType;

        private void BindActions()
        {
            var methods = _viewModelType.GetMethods(BindingFlags.Instance | BindingFlags.Public).Where(m => m.GetParameters().Count() == 0);
            foreach (MethodInfo method in methods)
            {
                Control control = (Control)ViewElement.FindName(method.Name);
                if (control != null)
                {
                    var methodBinder = new MethodBinder(control, Model);
                    Add(methodBinder);
                }
            }
        }

        private void BindProperties()
        {
            var properties = _viewModelType.GetProperties(BindingFlags.Instance | BindingFlags.Public).Where(p => p.CanRead && p.CanWrite);
            foreach (PropertyInfo property in properties)
            {
                Control control = (Control)ViewElement.FindName(property.Name);
                if (control != null)
                {

                    if (control is ListBox)
                    {
                        Add(ListBinder.ForControl<TModel>(control, property));
                    }
                    else
                    {
                        Add(new ValueBinder<TModel>(control, Model));
                    }
                }
            }
        }
    }

    [ContentProperty("Overrides")]
    public class ConventionalViewBinder 
    {
        public static ConventionalViewBinder<TModel> For<TModel>(Control view, TModel model) where TModel:class
        {
            return new ConventionalViewBinder<TModel>(view, model);

        }


        public IList<IBinder> Overrides { get; set; }

        public ConventionalViewBinder()
        {
            this.Overrides = new List<IBinder>();
        }

        #region IFrameworkElementBinder Members

        public void Bind(FrameworkElement element)
        {
            /*
            this.Initialize(element);

            foreach (IBinder binder in this.Overrides)
            {
                Add(binder);
            }

            (this as IFrameworkElementBinder).Bind(element);
             * */
        }

        #endregion
    }
}
