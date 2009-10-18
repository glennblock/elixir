using System;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
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
    public class ExpressionUtilsTests
    {
        // Cannot do this because it blows up with a field access exception
        //[Test]
        //public void Can_Retrieve_ViewModel_From_First_Class_Property_Expression()
        //{
        //    var dummy = new Dummy();
        //    Expression<Func<bool>> expr = () => dummy.SimpleBool;
        //    var root = ExpressionUtils.GetExpressionRootObject(expr);

        //    Assert.AreSame(dummy, root);
        //}

        [Test]
        public void When_property_is_passed_then_path_is_retrieved()
        {
            Dummy dummy;
            dummy = new Dummy();
            Expression<Func<bool>> expr = () => dummy.SimpleBool;
            string path = ExpressionUtils.GetExpressionPropertyPath(expr);

            Assert.AreEqual("SimpleBool", path);
        }

        [Test]
        public void When_nested_property_is_passed_then_path_is_retrieved()
        {
            var dummy = new Dummy();
            Expression<Func<bool>> expr = () => dummy.Nested.SimpleBool;
            string path = ExpressionUtils.GetExpressionPropertyPath(expr);

            Assert.AreEqual("Nested.SimpleBool", path);
        }

        [Test]
        public void When_deeply_nested_property_is_passed_then_path_is_retrieved()
        {
            var dummy = new Dummy();
            Expression<Func<bool>> expr = () => dummy.Nested.Nested.Nested.Nested.SimpleBool;
            string path = ExpressionUtils.GetExpressionPropertyPath(expr);

            Assert.AreEqual("Nested.Nested.Nested.Nested.SimpleBool", path);
        }

        [Test]
        public void When_two_identical_method_actions_are_compared_then_they_match()
        {
            var vm = new MockViewModelWithAction();
            Expression<Action<RoutedEventArgs>> action1 = p => vm.Action();
            Expression<Action<RoutedEventArgs>> action2 = p => vm.Action();
            Assert.IsTrue(action1.ExpressionEquals(action2));
        }

        [Test]
        public void When_two_different_method_actions_are_compared_then_they_do_not_match()
        {
            var vm = new MockViewModelWithAction();
            Expression<Action<RoutedEventArgs>> action1 = p => vm.Action();
            Expression<Action<RoutedEventArgs>> action2 = p => DummyAction();
            Assert.IsFalse(action1.ExpressionEquals(action2));
        }

        [Test]
        public void When_expression_is_viewmodel_property_then_viewmodel_can_be_extracted()
        {
            var vm = new MockViewModelWithAction();
            Expression<Func<object, object>> expr = p => vm.Command;
            Assert.AreSame(vm, ExpressionUtils.GetModelFromExpression(expr));
        }

        [Test]
        public void When_expression_is_viewmodel_then_viewmodel_can_be_extracted()
        {
            var vm = new MockViewModelWithAction();
            Expression<Func<object, object>> expr = p => vm;
            Assert.AreSame(vm, ExpressionUtils.GetModelFromExpression(expr));
        }

        public void DummyAction()
        {
        }

        /*
        [Test]
        public void When_property_is_passed_then_property_expression_is_created()
        {
            CustomerListViewModel vm = new CustomerListViewModel();
            var result = ExpressionUtils.GetPropertyFuncFromProperty<ObservableCollection<Customer>>(vm, "Customers");
            var customers = result.DynamicInvoke(new object[] { vm });
            Assert.AreSame(vm.Customers, customers);
        }
         */
    }


    public class Dummy
    {
        public bool SimpleBool { get; set; }
        private Dummy _nested;

        public Dummy Nested
        {
            get
            {
                _nested = _nested ?? new Dummy();
                return _nested;
            }
        }
    }

}
