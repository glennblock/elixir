using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using NUnit.Framework;
using Elixir;

namespace ElixirTests
{
    [TestFixture]
    public class ValueBinderDefinitionTests
    {
        [Test]
        public void When_constructed_with_property_expression_then_target_path_is_set()
        {
            var definition = new ValueBinderDefinition<OrderViewModel>(p => p.FirstName);
            Assert.AreEqual("FirstName", definition.TargetPath);
        }

        [Test]
        public void When_constructed_with_property_expression_then_view_element_name_is_set_to_target_property()
        {
            var definition = new ValueBinderDefinition<OrderViewModel>(p => p.FirstName);
            Assert.AreEqual("FirstName", definition.ViewElementName);
        }

        [Test]
        public void When_constructed_with_property_expression_then_property_is_set_to_default_property()
        {
            var vm = new OrderViewModel();
            var view = new OrderView();
            var definition = new ValueBinderDefinition<OrderViewModel>(p => p.FirstName);
            definition.Create(view, vm);
            Assert.AreEqual(TextBox.TextProperty, definition.Property);
        }

        [Test]
        public void When_constructed_with_an_element_name_and_a_property_expression_then_property_is_set_to_default_property()
        {
            var vm = new OrderViewModel();
            var view = new OrderView();
            var definition = new ValueBinderDefinition<OrderViewModel>("FirstName", p => p.FirstName);
            definition.Create(view, vm);
            Assert.AreEqual(TextBox.TextProperty, definition.Property);
        }

        [Test]
        public void When_constructed_with_an_element_name_and_a_property_expression_then_view_element_name_is_set()
        {
            var definition = new ValueBinderDefinition<OrderViewModel>("FirstName", p => p.FirstName);
            Assert.AreEqual("FirstName", definition.ViewElementName);
        }

        [Test]
        public void When_constructed_with_an_element_name_and_a_property_expression_then_target_path_is_set()
        {
            var definition = new ValueBinderDefinition<OrderViewModel>("FirstName", p => p.FirstName);
            Assert.AreEqual("FirstName", definition.TargetPath);
        }

        [Test]
        public void When_constructed_with_an_element_name_a_source_property_and_target_property_then_property_is_set_to_default_property()
        {
            var definition = new ValueBinderDefinition<OrderViewModel>("FirstName", TextBox.TextProperty, p => p.FirstName);
            Assert.AreEqual(TextBox.TextProperty, definition.Property);
        }

        [Test]
        public void When_constructed_with_an_element_name_a_source_property_and_target_property_then_element_name_is_set()
        {
            var definition = new ValueBinderDefinition<OrderViewModel>("FirstName", TextBox.TextProperty, p => p.FirstName);
            Assert.AreEqual("FirstName", definition.ViewElementName);
        }

        [Test]
        public void When_constructed_with_an_element_name_a_source_property_and_target_property_then_target_path_is_set()
        {
            var definition = new ValueBinderDefinition<OrderViewModel>("FirstName", TextBox.TextProperty, p => p.FirstName);
            Assert.AreEqual("FirstName", definition.TargetPath);
        }

        [Test]
        public void When_create_is_invoked_then_binder_is_created()
        {
            var definition = new ValueBinderDefinition<OrderViewModel>(p => p.FirstName);
            var vm = new OrderViewModel();
            var view = new OrderView();
            var binder = definition.Create(view, vm);
            Assert.IsNotNull(binder);
        }


    }
}
