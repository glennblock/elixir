using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Reflection;
using System.Collections.ObjectModel;

namespace Elixir
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:ViewModelBinding"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:ViewModelBinding;assembly=ViewModelBinding"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Browse to and select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:Bind/>
    ///
    /// </summary>
    public class Bind : Control, IBinder, IFrameworkElementBinder
    {
        static Bind()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Bind), new FrameworkPropertyMetadata(typeof(Bind)));
        }

        private string _control;
        private FrameworkElement _element;
        private IBinder _binder;
        private bool _initialized;

        public string Control
        {
            get
            {
                return this._control ?? this.Value ?? this.Action ?? this.List;
            }
            set
            {
                this._control = value;
            }
        }

        public string ControlProperty { get; set; }
        public BindingMode BindingMode { get; set; }
        public IValueConverter Converter { get; set; }
        public string ConverterParameter { get; set; }

        public string Value { get; set; }
        public string Action { get; set; }
        public string List { get; set; }

        public Bind()
        {
            this.Loaded += new RoutedEventHandler(Bind_Loaded);
        }

        private void Bind_Loaded(object sender, RoutedEventArgs e)
        {
            (this as IBinder).Bind();
        }

        private IBinder GetValueBinder(FrameworkElement element, object viewModel)
        {
            return new ValueBinder<object>(element, viewModel);
        }

        private IBinder GetActionBinder(Control element, object viewModel)
        {
            return new MethodBinder(element, viewModel);
        }

        private IBinder GetListBinder(FrameworkElement element, object viewModel)
        {
            PropertyInfo property = viewModel.GetType().GetProperty(this.List);
            return ListBinder.ForControl<object>(element, property);
        }

        private FrameworkElement ValidateConfiguration()
        {
            int vmProperties =
                (string.IsNullOrEmpty(this.Value) ? 0 : 1)
                + (string.IsNullOrEmpty(this.Action) ? 0 : 1)
                + (string.IsNullOrEmpty(this.List) ? 0 : 1);

            if (vmProperties != 1)
            {
                throw new InvalidOperationException("You must set only one of the following properties: Value, Action, List");
            }

            object element = this._element.FindName(this.Control);

            if (element == null)
            {
                throw new ArgumentException(string.Format("The control named '{0}' could not be found.", this.Control));
            }

            FrameworkElement frameworkElement = element as FrameworkElement;

            if (frameworkElement == null)
            {
                throw new InvalidCastException(string.Format("Could not convert the element named '{0}' to a FrameworkElement.", this.Control));
            }

            return frameworkElement;
        }

        private void Initialize()
        {
            this._element = ValidateConfiguration();

            if (!string.IsNullOrEmpty(this.Value))
            {
                this._binder = GetValueBinder(this._element, ViewBinder.GetModel(this._element));
            }
            else if (!string.IsNullOrEmpty(this.Action))
            {
                this._binder = GetActionBinder(this._element as Control, ViewBinder.GetModel(this._element));
            }
            else if (!string.IsNullOrEmpty(this.List))
            {
                this._binder = GetListBinder(this._element, ViewBinder.GetModel(this._element));
            }

            this._initialized = true;
        }

        #region IBinder Members

        public FrameworkElement ViewElement
        {
            get { return this._element; }
        }

        void IBinder.Bind()
        {
            if (!this._initialized)
            {
                Initialize();
            }

            if (this._binder != null)
            {
                this._binder.Bind();
            }
        }

        public Binding Binding
        {
            get { return this._binder.Binding; }
        }

        #endregion

        #region IFrameworkElementBinder Members

        void IFrameworkElementBinder.Bind(FrameworkElement element)
        {
            this._element = element;
            (this as IBinder).Bind();
        }

        #endregion
    }
}
