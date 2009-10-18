using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Elixir;
using Assert=Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace ElixirTests
{
    [TestFixture]
    public class ConventionalViewBinderTests
    {
        [Test]
        public void When_constructed_then_view_model_is_set_on_view()
        {
            OrderView view = new OrderView();
            OrderViewModel vm = new OrderViewModel();
            var vmBinder = ConventionalViewBinder<OrderViewModel>.For(view, vm);
            Assert.AreEqual(vm, view.GetValue(ViewBinder.ModelProperty));
        }

        [Test]
        public void When_constructed_then_method_binder_is_created_for_Save_button()
        {
            OrderView view = new OrderView();
            OrderViewModel vm = new OrderViewModel();
            var vmBinder = ConventionalViewBinder.For(view, vm);
            var saveBinder = vmBinder.GetMethodBinder(view.Save, "Click");
            Assert.IsNotNull(saveBinder);
        }

        [Test]
        public void When_constructed_then_list_binder_is_created_for_Categories()
        {
            OrderView view = new OrderView();
            OrderViewModel vm = new OrderViewModel();
            var vmBinder = ConventionalViewBinder.For(view, vm);
            var categoriesBinder = vmBinder.GetPropertyBinder(view.Categories, ListBox.ItemsSourceProperty);
            Assert.IsNotNull(categoriesBinder);
        }

        [Test]
        public void When_constructed_then_list_binder_is_created_for_Customers()
        {
            OrderView view = new OrderView();
            OrderViewModel vm = new OrderViewModel();
            var vmBinder = ConventionalViewBinder.For(view, vm);
            var customersBinder = vmBinder.GetPropertyBinder(view.Customers, ListBox.ItemsSourceProperty);
            Assert.IsNotNull(customersBinder);
        }

        [Test]
        public void When_constructed_then_list_binder_for_customers_is_generic_collection_of_string()
        {
            OrderView view = new OrderView();
            OrderViewModel vm = new OrderViewModel();
            var vmBinder = ConventionalViewBinder.For(view, vm);
            var customersBinder = vmBinder.GetPropertyBinder(view.Customers, ListBox.ItemsSourceProperty);
            var binderType = customersBinder.GetType();
            Assert.IsTrue(binderType.IsAssignableFrom(typeof(ListBinder<string,OrderViewModel>)));
        }

        [Test]
        public void When_constructed_then_value_binder_is_created_for_FirstName()
        {
            OrderView view = new OrderView();
            OrderViewModel vm = new OrderViewModel();
            var vmBinder = ConventionalViewBinder.For(view, vm);
            var firstNameBinder = vmBinder.GetPropertyBinder(view.FirstName, TextBox.TextProperty);
            Assert.IsNotNull(firstNameBinder);
        }

        
    }
}


