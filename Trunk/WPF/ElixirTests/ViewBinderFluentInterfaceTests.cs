using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using NUnit.Framework;
using Elixir;

namespace ElixirTests
{
    [TestFixture]
    public class ViewBinderFluentInterfaceTests
    {
        [Test]
        public void When_adding_a_value_then_value_binder_is_created()
        {
            var vm = new OrderViewModel {FirstName = "Jeff"};
            var view = new OrderView();
            var viewBinder = ViewBinder.For(view, vm).
                Value(p => p.FirstName);

            var binder = viewBinder.GetBinders<IValueBinder>("FirstName").First();
            Assert.IsNotNull(binder);
        }

        [Test]
        public void When_adding_a_value_and_passing_the_element_then_value_binder_is_created()
        {
            var vm = new OrderViewModel { FirstName = "Jeff" };
            var view = new OrderView();
            var viewBinder = ViewBinder.For(view, vm).
                Value(view.FirstName,p => vm.FirstName);
            
            var binder = viewBinder.GetBinders<IValueBinder>("FirstName").First();
            Assert.IsNotNull(binder);
        }

        [Test]
        public void When_adding_a_value_and_passing_the_element_and_the_source_property_then_value_binder_is_created()
        {
            var vm = new OrderViewModel { FirstName = "Jeff" };
            var view = new OrderView();
            var viewBinder = ViewBinder.For(view, vm).
                Value(view.FirstName, TextBox.TextProperty, p => vm.FirstName);

            var binder = viewBinder.GetBinders<IValueBinder>("FirstName").First();
            Assert.IsNotNull(binder);
        }

        [Test]
        public void When_adding_a_list_then_list_binder_is_created()
        {
            var vm = new OrderViewModel();
            vm.Customers.Add("Jeff Handley");
            var view = new OrderView();
            var viewBinder = ViewBinder.For(view, vm).
                List(p => vm.Customers);

            var binder = viewBinder.GetBinders<ListBinder<string, OrderViewModel>>("Customers").First();
            Assert.IsNotNull(binder);
        }

        [Test]
        public void When_adding_a_list_and_passing_the_listBox_and_selected_item_then_list_binder_is_created()
        {
            var vm = new OrderViewModel();
            vm.Customers.Add("Jeff Handley");
            var view = new OrderView();
            var viewBinder = ViewBinder.For(view, vm).
                List(view.Customers, p => vm.Customers, p=>vm.SelectedCustomer);

            var binder = viewBinder.GetBinders<ListBinder<string, OrderViewModel>>("Customers").First();
            Assert.IsNotNull(binder);
            
        }

        [Test]
        public void When_adding_a_list_and_passing_the_listBox_and_selected_items_then_list_binder_is_created()
        {
            var vm = new OrderViewModel();
            vm.Categories.Add("Category1");
            var view = new OrderView();
            var viewBinder = ViewBinder.For(view, vm).
                List(view.Categories, p => vm.Categories, p => vm.SelectedCategories);

            var binder = viewBinder.GetBinders<ListBinder<string, OrderViewModel>>("Categories").First();
            Assert.IsNotNull(binder);

        }

        [Test]
        public void When_adding_an_action_then_method_binder_is_created()
        {
            var vm = new OrderViewModel();
            var view = new OrderView();
            var viewBinder = ViewBinder.For(view, vm)
                .Action(p => vm.Save());
            
            var binder = viewBinder.GetBinders<IMethodBinder>("Save").First();
            Assert.IsNotNull(binder);
        }

        [Test]
        public void When_adding_an_action_and_passing_the_control_and_the_is_enabled_then_method_binder_is_created()
        {
            var vm = new OrderViewModel();
            var view = new OrderView();
            var viewBinder = ViewBinder.For(view, vm).
                Action(view.Save, "Click", p => vm.Save(), p=>vm.IsSaveEnabled);

            var binder = viewBinder.GetBinders<IMethodBinder>("Save").First();
            Assert.IsNotNull(binder);
        }

        [Test]
        public void When_adding_a_value_with_property_expression_then_binder_definition_is_constructed()
        {
            var templateBinder = new TemplateBinder<OrderViewModel>("Order");
            var definition = templateBinder.Value(p => p.FirstName);
            Assert.IsNotNull(definition);
        }

        [Test]
        public void When_adding_a_value_with_an_element_name_and_a_property_expression_then_binder_is_constructed()
        {
            var templateBinder = new TemplateBinder<OrderViewModel>("Order");
            var definition = templateBinder.Value("FirstName", p => p.FirstName);
            Assert.IsNotNull(definition);

        }

        [Test]
        public void When_adding_a_value_with_an_element_name_a_source_property_and_target_property_then_binder_is_constructed()
        {
            var templateBinder = new TemplateBinder<OrderViewModel>("Order");
            var definition = templateBinder.Value("FirstName", TextBox.TextProperty, p => p.FirstName);
            Assert.IsNotNull(definition);
        }
        

    }
}
