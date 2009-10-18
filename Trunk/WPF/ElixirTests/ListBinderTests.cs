using System;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Net;
using System.Reflection;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Controls.Primitives;
using System.Collections.Generic;
#if !SILVERLIGHT
using NUnit.Framework;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestFixture = Microsoft.VisualStudio.TestTools.UnitTesting.TestClassAttribute;
using Test = Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute;
#endif
using Elixir;


namespace ElixirTests
{
    [TestFixture]
    public class ListBinderTests
    {
        [Test]
        public void When_passing_in_the_list_property_that_ends_in_s_then_selected_item_property_is_returned_without_s()
        {
            string selectedItemProperty = ViewBinder.GetSelectedItemProperty("Customers", typeof(CustomerListViewModel));
            Assert.AreEqual("SelectedCustomer", selectedItemProperty);
        }

        [Test]
        public void When_passing_in_a_list_property_that_ends_in_ies_then_selected_item_property_is_returned_with_y()
        {
            string selectedItemProperty = ViewBinder.GetSelectedItemProperty("Cities", typeof (ViewModelWithCitiesList));
            Assert.AreEqual("SelectedCity", selectedItemProperty);
        }

        public class ViewModelWithCitiesList
        {
            public string Cities { get; set; }
            public string SelectedCity { get; set;}
        }

        [Test]
        public void When_passing_in_the_list_property_then_selected_items_property_is_returned()
        {
            string selectedItemProperty = ViewBinder.GetSelectedItemsProperty("Customers", typeof(CustomerListViewModel));
            Assert.AreEqual("SelectedCustomers", selectedItemProperty);
        }
        
        [Test]
        public void When_constructed_with_an_expression_then_items_binder_is_bound_to_the_control()
        {
            var view = new OrderView();
            var vm = new OrderViewModel();
            var binder = new ListBinder<string,OrderViewModel>(p => p.Customers, view,vm);
            Assert.AreEqual(view.Customers, binder.ViewElement);
        }

        [Test]
        public void When_constructed_with_an_expression_then_items_binder_is_bound_to_vm_items()
        {
            var view = new OrderView();
            var vm = new OrderViewModel();
            var binder = new ListBinder<string,OrderViewModel>(p => p.Customers, view,vm);
            Assert.AreEqual("Customers", binder.ItemsBinder.TargetPath);
        }

        [Test]
        public void When_constructed_with_an_expression_then_item_binder_is_bound_to_vm_selectedItem()
        {
            var view = new OrderView();
            var vm = new OrderViewModel();
            var binder = new ListBinder<string,OrderViewModel>(p => p.Customers, view,vm);
            Assert.AreEqual("SelectedCustomer", binder.SelectedItemBinder.TargetPath);
        }

        [Test]
        public void When_constructed_with_an_expression_then_item_binder_is_bound_to_vm_selectedItems()
        {
            var view = new OrderView();
            var vm = new OrderViewModel();
            var binder = new ListBinder<string,OrderViewModel>(p => p.Categories, view, vm);
            Assert.AreEqual("SelectedCategories", binder.SelectedItemsPath);
        }


        [Test]
        public void When_constructed_with_an_expression_then_items_binder_is_bound_to_items_source_property()
        {
            var view = new OrderView();
            var vm = new OrderViewModel();
            var binder = new ListBinder<string, OrderViewModel>(p => p.Customers, view, vm);
            Assert.AreEqual(ListBox.ItemsSourceProperty,binder.ItemsBinder.Property);
        }

        [Test]
        public void When_constructed_with_an_expression_then_selected_item_binder_is_bound_to_selected_item_property()
        {
            var view = new OrderView();
            var vm = new OrderViewModel();
            var binder = new ListBinder<string, OrderViewModel>(p => p.Customers, view, vm);
            Assert.AreEqual(binder.SelectedItemBinder.Property, ListBox.SelectedItemProperty);
        }

        [Test]
        public void When_constructed_with_a_ListBox_then_items_binder_is_set_to_the_list_name()
        {
            ListBox customerList = new ListBox();
            customerList.Name = "Customers";
            CustomerListViewModel vm = new CustomerListViewModel("Jeff", "Glenn");
            customerList.SetValue(ViewBinder.ModelProperty, vm);

            ListBinder<Customer, CustomerListViewModel> binder = new ListBinder<Customer, CustomerListViewModel>(customerList);
            Assert.AreEqual("Customers", binder.ItemsBinder.TargetPath);
        }


        [Test]
        public void When_constructed_with_a_ListBox_then_selected_item_binder_is_set_to_selected_convention()
        {
            ListBox customerList = new ListBox();
            customerList.Name = "Customers";
            CustomerListViewModel vm = new CustomerListViewModel("Jeff", "Glenn");
            customerList.SetValue(ViewBinder.ModelProperty, vm);

            ListBinder<Customer, CustomerListViewModel> binder = new ListBinder<Customer, CustomerListViewModel>(customerList);
            Assert.AreEqual("SelectedCustomer", binder.SelectedItemBinder.TargetPath);
        }

        [Test]
        public void When_constructed_with_a_ListBox_then_selected_items_binder_is_set_to_selected_items_convention()
        {
            ListBox customerList = new ListBox();
            customerList.Name = "Customers";
            CustomerListViewModel vm = new CustomerListViewModel("Jeff", "Glenn");
            customerList.SetValue(ViewBinder.ModelProperty, vm);

            ListBinder<Customer, CustomerListViewModel> binder = new ListBinder<Customer, CustomerListViewModel>(customerList);
            Assert.AreEqual("SelectedCustomers", binder.SelectedItemsPath);
        }

        
        [Test]
        [Description("Construct a list binder with a selector control, binding to an ObservableCollection and a selected item")]
        public void When_constructed_then_it_is_bound_to_the_list_on_the_viewmodel()
        {
            ListBox customerList = new ListBox();
            CustomerListViewModel vm = new CustomerListViewModel("Jeff", "Glenn");
            ListBinder<Customer, CustomerListViewModel> binder = new ListBinder<Customer, CustomerListViewModel>(customerList, p => p.Customers, p => p.SelectedCustomer, vm);
            binder.Bind();
            Assert.AreEqual(2, customerList.Items.Count);
        }

        [Test]
        [Description("Selecting an item from the selector control updates the Model's SelectedItem property")]
        public void When_list_item_is_selected_then_viewmodel_is_updated()
        {
            ListBox customerList = new ListBox();
            CustomerListViewModel vm = new CustomerListViewModel("Jeff", "Glenn");

            ListBinder<Customer, CustomerListViewModel> binder = new ListBinder<Customer, CustomerListViewModel>(customerList, p => p.Customers, p => p.SelectedCustomer, vm);
            binder.Bind();
            customerList.SelectedIndex = 1;

            Assert.AreEqual("Glenn", vm.SelectedCustomer.FirstName);
        }

        [Test]
        [Description("Updating the Model's SelectedItem property changes the selected item in the selector control")]
        public void When_selected_property_is_updated_in_the_vm_then_list_is_updated()
        {
            ListBox customerList = new ListBox();
            CustomerListViewModel vm = new CustomerListViewModel("Jeff", "Glenn");

            ListBinder<Customer, CustomerListViewModel> binder = new ListBinder<Customer, CustomerListViewModel>(customerList, p => p.Customers, p => p.SelectedCustomer, vm);
            binder.Bind();

            vm.SelectedCustomer = vm.Customers[1];

            Assert.AreEqual("Glenn", ((Customer)customerList.SelectedItem).FirstName);
        }

        [Test]
        [Description("The user selects multiple customers in the listbox, and the Model is informed of the multiple selections")]
        public void When_multiple_items_are_selected_then_vm_selected_items_are_updated()
        {
            ListBox customerList = new ListBox();
            customerList.SelectionMode = SelectionMode.Multiple;
            CustomerListViewModel vm = new CustomerListViewModel("Nick", "Jeff", "Glenn", "Hamilton");

            ListBinder<Customer, CustomerListViewModel> binder = new ListBinder<Customer, CustomerListViewModel>(customerList, p => p.Customers, p => p.SelectedCustomers, vm);
            binder.Bind();

            customerList.SelectedItems.Add(customerList.Items[1]);
            customerList.SelectedItems.Add(customerList.Items[2]);

            Assert.AreEqual(2, vm.SelectedCustomers.Count);
            Assert.AreEqual("Jeff", vm.SelectedCustomers[0].FirstName);
            Assert.AreEqual("Glenn", vm.SelectedCustomers[1].FirstName);
        }

        [Test]
        [Description("The Model sets multiple selections and the listbox reflects that")]
        public void When_multiple_items_are_selected_in_the_vm_then_the_list_items_are_selected()
        {
            ListBox customerList = new ListBox();
            customerList.SelectionMode = SelectionMode.Multiple;
            CustomerListViewModel vm = new CustomerListViewModel("Nick", "Jeff", "Glenn", "Hamilton");

            ListBinder<Customer, CustomerListViewModel> binder = new ListBinder<Customer, CustomerListViewModel>(customerList, p => p.Customers, p => p.SelectedCustomers, vm);
            binder.Bind();

            vm.SelectedCustomers.Add(vm.Customers[1]);
            vm.SelectedCustomers.Add(vm.Customers[2]);

            Assert.AreEqual(2, customerList.SelectedItems.Count);
            Assert.AreEqual("Jeff", ((Customer)customerList.SelectedItems[0]).FirstName);
            Assert.AreEqual("Glenn", ((Customer)customerList.SelectedItems[1]).FirstName);
        }

        [Test]
        [Description("SelectedItems is sync'd to the UI at load time")]
        public void When_selected_items_exist_at_load_then_the_view_reflects_the_items()
        {
            ListBox customerList = new ListBox();
            customerList.SelectionMode = SelectionMode.Multiple;
            CustomerListViewModel vm = new CustomerListViewModel("Nick", "Jeff", "Glenn", "Hamilton");

            vm.SelectedCustomers.Add(vm.Customers[1]);
            vm.SelectedCustomers.Add(vm.Customers[2]);

            ListBinder<Customer, CustomerListViewModel> binder = new ListBinder<Customer, CustomerListViewModel>(customerList, p => p.Customers, p => p.SelectedCustomers, vm);
            binder.Bind();

            Assert.AreEqual(2, customerList.SelectedItems.Count);
            Assert.AreEqual("Jeff", ((Customer)customerList.SelectedItems[0]).FirstName);
            Assert.AreEqual("Glenn", ((Customer)customerList.SelectedItems[1]).FirstName);
        }
    }
}
