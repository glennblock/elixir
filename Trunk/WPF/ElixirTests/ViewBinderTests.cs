using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using NUnit.Framework;
using Elixir;
using System.Windows.Controls;
using System.Windows;
using System.Linq.Expressions;

namespace ElixirTests
{
    [TestFixture]
    public class ViewBinderTests
    {
        [Test]
        public void When_binder_is_constructed_then_view_is_set()
        {
            UserControl view = new UserControl();
            object viewModel = new object();
            var binder = ViewBinder.For(view, viewModel);
            Assert.AreSame(view, binder.ViewElement);
        }

        [Test]
        public void When_binder_is_constructed_then_viewmodel_is_initialized()
        {
            UserControl view = new UserControl();
            object viewModel = new object();
            var binder = new ViewBinder(view, viewModel);

            Assert.AreSame(viewModel, binder.Model);
        }

        [Test]
        public void When_value_binder_is_added_then_it_can_be_retrieved_by_its_type()
        {
            var view = new UserControl();
            var mockBinder = new MockBinder(view);

            var viewModelBinder = new ViewBinder<object>(view, null);
            viewModelBinder.Add(mockBinder);

            var binders = viewModelBinder.GetBinders<MockBinder>(view);
            Assert.AreEqual(mockBinder, binders.Single(), "There should be a single binder found that equals our mockBinder");
        }

        [Test]
        public void When_list_binder_is_added_then_it_can_be_retrieved_by_its_type()
        {
            ListBox citiesList = new ListBox();
            CustomerListViewModel vm = new CustomerListViewModel("Jeff", "Glenn");

            ListBinder<Customer,CustomerListViewModel> binder = new ListBinder<Customer,CustomerListViewModel>(citiesList, p => vm.Customers, p => vm.SelectedCustomer,vm);

            var viewModelBinder = new ViewBinder<CustomerListViewModel>(citiesList, vm);
            viewModelBinder.Add(binder);

            var binders = viewModelBinder.GetBinders<ListBinder<Customer,CustomerListViewModel>>(citiesList);
            Assert.AreEqual(binder, binders.Single(), "There should be a single list binder found");
        }

        [Test]
        public void When_value_binder_is_added_then_it_can_be_retrieved_by_its_bound_property()
        {
            var view = new UserControl();
            var viewModel = new { Value = "my value" };
            var valueBinder = new ValueBinder<object>(view, UserControl.ContentProperty, p => viewModel.Value) {BindingMode = BindingMode.OneWay};

            var viewModelBinder = new ViewBinder<object>(view, viewModel);
            viewModelBinder.Add(valueBinder);

            var binder = viewModelBinder.GetPropertyBinder(view, UserControl.ContentProperty);
            Assert.AreEqual(valueBinder, binder);
        }

        public void Can_Verify_Property_Is_Bound()
        {
        }


        [Test]
        public void When_method_binder_is_added_then_it_can_be_retrieved_by_its_event()
        {
            var button = new Button();
            var view = new UserControl();
            var vm = new MockViewModelWithAction();
            var buttonBinder = new MethodBinder<RoutedEventArgs>(button, "Click", p => vm.Action());
            var viewModelBinder = new ViewBinder<object>(view, vm);
            viewModelBinder.Add(buttonBinder);

            var binder = viewModelBinder.GetMethodBinder(button, "Click");
            Assert.AreEqual(buttonBinder, binder);
        }

        [Test]
        public void When_retrieving_default_property_for_Label_then_ContentProperty_is_returned()
        {
            var label = new Label();
            var property = ViewBinder.GetPropertyForElement(label);
            Assert.AreEqual(ContentControl.ContentProperty, property);
        }

        [Test]
        public void When_retrieving_default_property_for_CheckBox_then_IsCheckedProperty_is_returned()
        {
            var checkBox = new CheckBox();
            var binder = new ViewBinder();
            var property = ViewBinder.GetPropertyForElement(checkBox);
            Assert.AreEqual(ToggleButton.IsCheckedProperty, property);
        }

        [Test]
        public void When_adding_a_template_binder_then_it_can_be_retrieved()
        {
            var customer = new Customer() { FirstName = "Jeff", LastName = "Handley" };
            var customerListVM = new CustomerListViewModel(customer);
            var templateView = new StackPanel();
            var view = new ListBox();
            var viewBinder = ViewBinder.For(view, customerListVM).
                Template<Customer>("Customer", null);
            var templateBinder = viewBinder.GetTemplateBinder("Customer");
            Assert.IsNotNull(templateBinder);
        }
        
        private class MockBinder : IDependencyPropertyBinder
        {
            public System.Windows.FrameworkElement ViewElement { get; private set; }
            public DependencyProperty Property { get; private set; }

            public MockBinder(FrameworkElement viewElement, DependencyProperty property)
            {
                this.ViewElement = viewElement;
                this.Property = property;
            }

            public MockBinder(FrameworkElement viewElement)
                : this(viewElement, null)
            {
            }

            public void Bind()
            {
            }

            public Binding Binding
            {
                get { throw new System.NotImplementedException(); }
            }
        }
    }
}
