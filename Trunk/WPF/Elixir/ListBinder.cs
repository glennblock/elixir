using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Controls.Primitives;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.ComponentModel;
using System.Collections.Specialized;
using System.Collections;
using System.Collections.Generic;
using Elixir;
using Expression = System.Linq.Expressions.Expression;
using System.Reflection;

namespace Elixir
{
    public class ListBinder
    {
        public static IBinder ForControl<TModel>(FrameworkElement control, PropertyInfo property)
        {
            Type collectionType = property.PropertyType;

            if (typeof(IEnumerable).IsAssignableFrom(collectionType.GetGenericTypeDefinition()))
            {
                var genericTypes = collectionType.GetGenericArguments();
                int length = genericTypes.Length;
                var copy = new Type[length + 1];
                Array.Copy(genericTypes, copy, length);
                copy[length] = typeof (TModel);
                
                Type listBinderType = typeof(ListBinder<,>).MakeGenericType(copy);

                return (IBinder)Activator.CreateInstance(listBinderType, control);
            }

            return null;
        }
    }

    public class ListBinder<TItem,TModel> : IDependencyPropertyBinder
    {
        public IValueBinder ItemsBinder { get; private set; }
        public IValueBinder SelectedItemBinder { get; private set; }
        public string SelectedItemsPath { get; private set; }

        private TModel _viewModel;
        private ListBox _listbox;
        private IValueAccessor<IEnumerable<TItem>> _selectedItems;
        private bool _isLatched;

        public System.Windows.FrameworkElement ViewElement { get; private set; }

        public ListBinder(ListBox listBox)
        {
            var vm = (TModel) ViewBinder.GetModel(listBox);
            Initialize(listBox, vm);
        }

        public ListBinder(Expression<Func<TModel, IEnumerable<TItem>>> listItemsSource, FrameworkElement view, TModel viewModel)
        {
            string listElementName = ExpressionUtils.GetExpressionPropertyPath(listItemsSource);
            var listElement = view.FindName(listElementName);
            //var vm = ExpressionUtils.GetModelFromExpression(listItemsSource);
            if (listElement is ListBox)
                Initialize((ListBox) listElement, viewModel);
        }

        public ListBinder(ListBox listBox, Expression<Func<TModel, IEnumerable<TItem>>> listItemsSource, Expression<Func<TModel, TItem>> selectedItem, TModel viewModel)
        {
            //this._viewModel = ExpressionUtils.GetModelFromExpression(listItemsSource);
            this._viewModel = viewModel;
            Initialize(listBox, ExpressionUtils.GetExpressionPropertyPath(selectedItem), null, ExpressionUtils.GetExpressionPropertyPath(listItemsSource));
        }

        public ListBinder(ListBox listBox, Expression<Func<TModel, IEnumerable<TItem>>> listItemsSource, Expression<Func<TModel, IEnumerable<TItem>>> selectedItems, TModel viewModel)
        {
            //this._viewModel = ExpressionUtils.GetModelFromExpression(listItemsSource);
            this._viewModel = viewModel;
            if (selectedItems != null)
                this._selectedItems = new ExpressionAccessor<IEnumerable<TItem>,TModel>(selectedItems, _viewModel);

            Initialize(listBox, null, ExpressionUtils.GetExpressionPropertyPath(selectedItems), ExpressionUtils.GetExpressionPropertyPath(listItemsSource));
        }

        private void Initialize(ListBox listBox, TModel viewModel)
        {
            this._viewModel = viewModel;
            Initialize(listBox, null, null, listBox.Name);
        }

        private void Initialize(ListBox listBox, string selectedItemProperty, string selectedItemsProperty, string itemsProperty)
        {
            this.ViewElement = listBox;
            this._listbox = listBox;
            this.ItemsBinder = new ValueBinder<TModel>(listBox, ListBox.ItemsSourceProperty, _viewModel, itemsProperty) { BindingMode = BindingMode.OneWay };

            var viewModelType = _viewModel.GetType();;

            selectedItemProperty = selectedItemProperty ?? ViewBinder.GetSelectedItemProperty(listBox.Name, viewModelType);

            if (selectedItemProperty != null)
                this.SelectedItemBinder = new ValueBinder<TModel>(listBox, ListBox.SelectedItemProperty, _viewModel, selectedItemProperty);

            selectedItemsProperty = selectedItemsProperty ?? ViewBinder.GetSelectedItemsProperty(listBox.Name, viewModelType);

            if (selectedItemsProperty != null)
                this.SelectedItemsPath = selectedItemsProperty;
        }

        public void Bind()
        {
            _listbox.SelectionChanged += new SelectionChangedEventHandler(ViewSelectedItemsChanged);
            if (SelectedItemsPath != null)
            {
                this._selectedItems = new PropertyInfoAccessor<IEnumerable<TItem>>(this.SelectedItemsPath, _viewModel);
                INotifyCollectionChanged collection = (INotifyCollectionChanged) _selectedItems.GetValue();
                collection.CollectionChanged += new NotifyCollectionChangedEventHandler(ViewModelSelectedItemsChanged);
            }

            if (SelectedItemBinder != null)
                SelectedItemBinder.Bind();

            this.ItemsBinder.Bind();

            if (SelectedItemsPath != null)
            {
                this.SyncFromViewModelToView();
            }
        }

        public Binding Binding
        {
            get { return ItemsBinder.Binding; }
        }

        /*
        public ListBinder(ListBox listBox, object viewModel)
        {
            this.ItemsBinder = new ValueBinder(listBox, ListBox.ItemsSourceProperty,viewModel, listBox.Name );
            this.SelectedItemBinder = new ValueBinder(listBox, ListBox.SelectedItemProperty, viewModel,ListBinder.GetSelectedItemProperty(listBox.Name));
            this.SelectedItemsPath = ListBinder.GetSelectedItemsProperty(listBox.Name);
            this._selectedItems = ExpressionUtils.GetPropertyFuncFromProperty<ObservableCollection<TItem>>(viewModel, this.SelectedItemsPath);
            listBox.SelectionChanged += new SelectionChangedEventHandler(ViewSelectedItemsChanged);
        }
         */

        /*
        public ListBinder(Selector selector, Expression<Func<object, ObservableCollection<TItem>>> listItemsSource, Expression<Func<object, TItem>> selectedItem)
        {
            Initialize(selector, listItemsSource, selectedItem);
        }
        
        private void Initialize(Selector selector, Expression<Func<object, ObservableCollection<TItem>>> listItemsSource, Expression<Func<object, TItem>> selectedItem)
        {
            this.ViewElement = selector;
            this._viewModel = ExpressionUtils.GetModelFromExpression(listItemsSource);
            this.ItemsBinder = new ValueBinder(selector, Selector.ItemsSourceProperty, this._viewModel, ExpressionUtils.GetExpressionPropertyPath(listItemsSource),BindingMode.OneWay);
            this.SelectedItemBinder = new ValueBinder(selector, Selector.SelectedItemProperty, this._viewModel, ExpressionUtils.GetExpressionPropertyPath(selectedItem), System.Windows.Data.BindingMode.TwoWay);
        }
        

        public ListBinder(Selector selector, Expression<Func<object, ObservableCollection<TItem>>> listItemsSource, Expression<Func<object, TItem>> selectedItem, Expression<Func<object, ObservableCollection<TItem>>> selectedItems)
            :this(selector, listItemsSource, selectedItem)
        {
            this._selectedItems = new ExpressionAccessor<ObservableCollection<TItem>>(selectedItems, _viewModel);
            this.SelectedItemsPath = ExpressionUtils.GetExpressionPropertyPath(selectedItems);
            selector.SelectionChanged += new SelectionChangedEventHandler(ViewSelectedItemsChanged);
        }

        public ListBinder(Selector selector, Expression<Func<object, ObservableCollection<TItem>>> listItemsSource, Expression<Func<object, object>> selectedItem)
        {
            this.ViewElement = selector;
            this._viewModel = ExpressionUtils.GetModelFromExpression(listItemsSource);
            this.ItemsBinder = new ValueBinder(selector, Selector.ItemsSourceProperty, this._viewModel, ExpressionUtils.GetExpressionPropertyPath(listItemsSource),BindingMode.OneWay);
            this.SelectedItemBinder = new ValueBinder(selector, Selector.SelectedItemProperty, this._viewModel, ExpressionUtils.GetExpressionPropertyPath(selectedItem), System.Windows.Data.BindingMode.TwoWay);
        }
         * 
         * */


        private void ViewSelectedItemsChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this._isLatched)
            {
                return;
            }

            this._isLatched = true;

            try
            {
                ICollection<TItem> list;

                if (this._selectedItems != null)
                {
                    list = (ICollection<TItem>) this._selectedItems.GetValue();
                }
                else
                {
                    return;
                }

                foreach (object removed in e.RemovedItems)
                {
                    list.Remove((TItem)removed);
                }

                foreach (object added in e.AddedItems)
                {
                    list.Add((TItem)added);
                }
            }
            finally
            {
                this._isLatched = false;
            }
        }

        private void ViewModelSelectedItemsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            SyncFromViewModelToView();
        }

        private void SyncFromViewModelToView()
        {
            if (this._listbox == null || this._isLatched)
            {
                return;
            }

            this._isLatched = true;

            try
            {
                this._listbox.SelectedItems.Clear();

                ICollection<TItem> collection = (ICollection<TItem>) this._selectedItems.GetValue();

                foreach (object selection in collection)
                {
                    this._listbox.SelectedItems.Add(selection);
                }
            }
            finally
            {
                this._isLatched = false;
            }
        }

        public DependencyProperty Property
        {
            get { return Selector.ItemsSourceProperty; }
        }

        /*
        public ViewBinder BindToTemplate(DependencyProperty templateProperty)
        {
            DataTemplate template = (DataTemplate) ViewElement.GetValue(templateProperty);
            
        }*/

        public override string ToString()
        {
            return "ListBinder: " + this.ViewElement.Name + "." + this.Property + " <==> " + this.Binding.Path.Path + ", " + this.ViewElement.Name + ".SelectedItems <==> " + this.SelectedItemsPath;
        }
    }
}
