using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Linq.Expressions;

namespace Elixir
{
    public class ViewBinder<TModel> : ViewBinder, IViewBinderFluent<TModel>
    {
        public ViewBinder()
        {
            this.TemplateBinders = new List<ITemplateBinder>();
            ViewBinder.TemplateInitialized += new EventHandler<TemplateInitializedEventArgs>(ViewBinder_TemplateInitialized);
        }

        void ViewBinder_TemplateInitialized(object sender, TemplateInitializedEventArgs e)
        {
            var templateBinder = this.TemplateBinders.Where(t => t.Template == e.Template).FirstOrDefault();
            if (templateBinder != null)
            {
                var viewBinder = templateBinder.CreateViewBinder(e.TemplateContent, e.TemplateContent.DataContext);
                this.ViewBinders.Add(viewBinder);
                viewBinder.Bind();
            }

        }

        public ViewBinder(FrameworkElement view, TModel model)
            :this()
        {
            this.ViewBinders = new List<IViewBinder>();
            this.ViewElement = view;
            this.Model = model;
            view.SetValue(ViewBinder.ModelProperty, model);
        }

        public void AddBinders(params IBinder[] binders)
        {
            this.AddRange(binders);
        }



        public TModel Model
        {
            get { return (TModel) base.Model; }
            set { base.Model = value; }
        }


        IViewBinderFluent<TModel> IViewBinderFluent<TModel>.Value(Expression<Func<TModel, object>> targetProperty)
        {
            var binder = new ValueBinder<TModel>(targetProperty, this.ViewElement);
            Add(binder);
            return this;
        }

        IViewBinderFluent<TModel> IViewBinderFluent<TModel>.Value(FrameworkElement element, Expression<Func<TModel, object>> targetProperty)
        {
            var binder = new ValueBinder<TModel>(element, targetProperty, Model);
            Add(binder);
            return this;
        }

        IViewBinderFluent<TModel> IViewBinderFluent<TModel>.Value(FrameworkElement element, DependencyProperty sourceProperty, Expression<Func<TModel, object>> targetProperty)
        {
            var binder = new ValueBinder<TModel>(element, sourceProperty, Model, targetProperty);
            Add(binder);
            return this;
        }

        IViewBinderFluent<TModel> IViewBinderFluent<TModel>.List<TItem>(Expression<Func<TModel, IEnumerable<TItem>>> listItemsSource)
        {
            var binder = new ListBinder<TItem, TModel>(listItemsSource, ViewElement, Model);
            Add(binder);
            return this;
        }

        IViewBinderFluent<TModel> IViewBinderFluent<TModel>.List<TItem>(ListBox listbox, Expression<Func<TModel, IEnumerable<TItem>>> listItemsSource, Expression<Func<TModel, IEnumerable<TItem>>> selectedItems)
        {
            var binder = new ListBinder<TItem, TModel>(listbox, listItemsSource, selectedItems, Model);
            Add(binder);
            return this;
        }

        IViewBinderFluent<TModel> IViewBinderFluent<TModel>.List<TItem>(ListBox listbox, Expression<Func<TModel, IEnumerable<TItem>>> listItemsSource, Expression<Func<TModel, TItem>> selectedItem)
        {
            var binder = new ListBinder<TItem, TModel>(listbox, listItemsSource, selectedItem, Model);
            Add(binder);
            return this;
        }


        IViewBinderFluent<TModel> IViewBinderFluent<TModel>.Action(Expression<Action<object>> action)
        {
            var binder = new MethodBinder(action, ViewElement, Model);
            Add(binder);
            return this;
        }

        IViewBinderFluent<TModel> IViewBinderFluent<TModel>.Action(Control control, string eventName, Expression<Action<RoutedEventArgs>> action, Expression<Func<object, bool>> actionEnabled) 
        {
            MethodBinder<RoutedEventArgs> binder = new MethodBinder<RoutedEventArgs>(control, eventName, action, actionEnabled, (INotifyPropertyChanged) Model);
            Add(binder);
            return this;
        }

        IViewBinderFluent<TModel> IViewBinderFluent<TModel>.Template<TTemplateModel>(string template, Action<TemplateBinder<TTemplateModel>> templateRegistration)
        {
            TemplateBinder<TTemplateModel> binder = new TemplateBinder<TTemplateModel>(template);
            TemplateBinders.Add(binder);
            if (templateRegistration != null)            
                templateRegistration(binder);
            return this;
        }

        IViewBinderFluent<TModel> IViewBinderFluent<TModel>.AddBinder(IBinder newBinder)
        {
            Add(newBinder);
            return this;
        }

        public IDependencyPropertyBinder GetPropertyBinder(string viewElementName, DependencyProperty dependencyProperty)
        {
            var element = (FrameworkElement)ViewElement.FindName(viewElementName);
            if (element != null)
                return GetPropertyBinder(element, dependencyProperty);

            return null;
        }

        public IDependencyPropertyBinder GetPropertyBinder(FrameworkElement viewElement, DependencyProperty dependencyProperty)
        {
            return this
                .Where(b => b.ViewElement == viewElement)
                .OfType<IDependencyPropertyBinder>()
                .Where(pb => pb.Property == dependencyProperty)
                .SingleOrDefault();
        }

        public IMethodBinder GetMethodBinder(string controlName, string eventName)
        {
            var control = (Control)ViewElement.FindName(controlName);
            if (control != null)
                return GetMethodBinder(control, eventName);

            return null;
        }


        public IMethodBinder GetMethodBinder(FrameworkElement control, string eventName)
        {
            return this
                .Where(b => b.ViewElement == control)
                .OfType<IMethodBinder>()
                .Where(mb => mb.EventName == eventName)
                .SingleOrDefault();

        }

        public ITemplateBinder GetTemplateBinder(string template)
        {
            return TemplateBinders
                .Where(t => t.Template == template)
                .SingleOrDefault();
        }

        public IEnumerable<T> GetBinders<T>(FrameworkElement viewElement)
        {
            return this.Where(b => b.ViewElement == viewElement).OfType<T>();
        }

        public IEnumerable<T> GetBinders<T>(string viewElementName)
        {
            FrameworkElement element = (FrameworkElement)ViewElement.FindName(viewElementName);
            if (element != null)
                return GetBinders<T>(element);

            return null;
        }

    }

    public class ViewBinder : List<IBinder>, IViewBinder, IFrameworkElementBinder
    {
        public static IViewBinderFluent<TModel> For<TModel>(FrameworkElement element, TModel model)
        {
            return new ViewBinder<TModel>(element, model);
        }

        public ViewBinder()
        {
            
        }

        public IList<IViewBinder> ViewBinders { get; set; }
        public IList<ITemplateBinder> TemplateBinders { get; set; }

        public void Bind()
        {
            foreach (IBinder binder in this.Where(b => !(b is IFrameworkElementBinder)))
            {
                binder.Bind();
            }
        }


        Binding IBinder.Binding
        {
            get { return null; }
        }

        private FrameworkElement _viewElement;
        public FrameworkElement ViewElement
        {
            get { return this._viewElement; }
            set
            {
                this._viewElement = value;
                ViewBinder.SetInstance(this._viewElement, this);
            }
        }

        public object Model { get; set; }

        void IFrameworkElementBinder.Bind(FrameworkElement view)
        {
            this.Bind();

            this.ViewElement = view;

            foreach (IFrameworkElementBinder binder in this.OfType<IFrameworkElementBinder>())
            {
                binder.Bind(this.ViewElement);
            }
        }


        
        public ViewBinder(FrameworkElement view, object model)
        {
            this._viewElement = view;
            this.Model = model;
        }

        static ViewBinder()
        {
            _mappings = new List<IMapping>();
            _mappings.Add(new DefaultPropertyMapping(typeof(ToggleButton), ToggleButton.IsCheckedProperty));
            _mappings.Add(new DefaultPropertyMapping(typeof(ContentControl), ContentControl.ContentProperty));
            _mappings.Add(new DefaultPropertyMapping(typeof(ItemsControl), ItemsControl.ItemsSourceProperty));
            _mappings.Add(new DefaultPropertyMapping(typeof(CheckBox), ToggleButton.IsCheckedProperty));
            _mappings.Add(new DefaultPropertyMapping(typeof(TextBox), TextBox.TextProperty));

            _mappings.Add(new DefaultEventMapping(typeof(Button), "Click"));
        }

        private static IList<IMapping> _mappings;
        public static IList<IMapping> Mappings
        {
            get { return _mappings; }
        }

        public static DependencyProperty GetPropertyForElementType<TElementType>()
        {
            DefaultPropertyMapping mapping = Mappings.
                OfType<DefaultPropertyMapping>().
                LastOrDefault(p => p.Key.IsAssignableFrom(typeof(TElementType)));

            return mapping == null ? null : mapping.Value;
            
        }

        public static DependencyProperty GetPropertyForElement(UIElement element)
        {
            DefaultPropertyMapping mapping = Mappings.
                OfType<DefaultPropertyMapping>().
                LastOrDefault(p => p.Key.IsAssignableFrom(element.GetType()));

            return mapping == null ? null : mapping.Value;
        }

        public static string GetEventForElement(FrameworkElement element)
        {
            return Mappings.
                OfType<DefaultEventMapping>().
                LastOrDefault(p => p.Key.IsAssignableFrom(element.GetType())).Value;
        }

        public static string GetSelectedItemProperty(string listProperty, Type viewModelType)
        {
            string selectedItemProperty = "Selected";

            if (listProperty.EndsWith("ies"))
                selectedItemProperty += listProperty.Substring(0, listProperty.Length - 3) + "y";
            else if (listProperty.EndsWith("s"))
                selectedItemProperty += listProperty.Substring(0, listProperty.Length - 1);
            else
                selectedItemProperty += listProperty;

            if (viewModelType.GetProperty(selectedItemProperty) == null)
                return null;

            return selectedItemProperty;
        }

        public static string GetSelectedItemsProperty(string listProperty, Type viewModelType)
        {
            string selectedItemsProperty = "Selected" + listProperty;
            if (viewModelType.GetProperty(selectedItemsProperty) == null)
                return null;
            
            return selectedItemsProperty;
        }

        public static string GetActionEnabled(string action, Type viewModelType)
        {
            string enabledProperty = string.Format("Is{0}Enabled", action);
            if (viewModelType.GetProperty(enabledProperty) == null)
                return null;

            return enabledProperty;
        }

        public static string GetElementForModelProperty(string path)
        {
            return path;
        }



        #region TemplateKey

        public static event EventHandler<TemplateInitializedEventArgs> TemplateInitialized = delegate { };


        public static readonly DependencyProperty TemplateKey =
            DependencyProperty.RegisterAttached("TemplateKey", typeof(string),
                                                typeof(FrameworkElement), new PropertyMetadata(null, TemplateKeyChanged));

        public static void SetTemplateKey(FrameworkElement element, string templateKey)
        {
            element.SetValue(TemplateKey, templateKey);
        }

        public static string GetTemplateKey(FrameworkElement element)
        {
            return (string)element.GetValue(TemplateKey);
        }


        public static void TemplateKeyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            string key = (string)e.NewValue;
            FrameworkElement element = (FrameworkElement)d;
            element.Loaded += (s, ea) => TemplateInitialized(
                null,
                new TemplateInitializedEventArgs(key, element)
            );
    }

        #endregion

        #region Model

        public static readonly DependencyProperty ModelProperty =
            DependencyProperty.RegisterAttached("Model", typeof(object), typeof(UIElement)
#if !SILVERLIGHT
                                                , new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits)
#else
, null
#endif
);

        public static void SetModel(UIElement element, object viewModel)
        {
            element.SetValue(ModelProperty, viewModel);
        }

        public static object GetModel(UIElement element)
        {
            object viewModel;

            while ((viewModel = element.GetValue(ModelProperty)) == null
                && (element = System.Windows.Media.VisualTreeHelper.GetParent(element) as UIElement) != null) { }

            return element == null ? null : (object)element.GetValue(ModelProperty);
        }

        #endregion  

        #region Binders

        public static readonly DependencyProperty BindersProperty =
            DependencyProperty.RegisterAttached("Binders", typeof(IFrameworkElementBinder), typeof(UIElement), null);

        public static void SetBinders(UIElement element, object bindings)
        {
            IFrameworkElementBinder binder = bindings as IFrameworkElementBinder ?? new ViewBinder();

            element.SetValue(BindersProperty, binder);
            FrameworkElement frameworkElement = element as FrameworkElement;

            frameworkElement.Loaded += (s, e) =>
            {
                binder.Bind(frameworkElement);
            };
        }

        #endregion

        #region ViewBinder Instance

        public static readonly DependencyProperty InstanceProperty = DependencyProperty.RegisterAttached("Instance", typeof(IViewBinder), typeof(ViewBinder)
#if !SILVERLIGHT
, new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits)
#else
, null
#endif
);

        public static void SetInstance(DependencyObject element, IViewBinder binder)
        {
            element.SetValue(InstanceProperty, binder);
        }

        public static IViewBinder GetInstance(DependencyObject element)
        {
            IViewBinder binder;

            while ((binder = (IViewBinder)element.GetValue(InstanceProperty)) == null
                && (element = System.Windows.Media.VisualTreeHelper.GetParent(element) as UIElement) != null) { }

            return element == null ? null : (IViewBinder)element.GetValue(InstanceProperty);
        }

        #endregion
    }
}
