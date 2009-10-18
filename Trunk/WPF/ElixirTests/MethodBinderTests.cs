using System;
using System.Net;

using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
#if !SILVERLIGHT
using NUnit.Framework;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestFixture = Microsoft.VisualStudio.TestTools.UnitTesting.TestClassAttribute;
using Test = Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute;
using SetUp = Microsoft.VisualStudio.TestTools.UnitTesting.TestInitializeAttribute;
#endif
using Elixir;

namespace ElixirTests
{
    [TestFixture]
    public class MethodBinderTests 
    {
        private bool _actionWasCalled;

        private void SetAction()
        {
            this._actionWasCalled = true;
        }

        [SetUp]
        public void SetUp()
        {
            this._actionWasCalled = false;
        }


        [Test]
        public void When_constructed_with_a_control_then_sets_event_name()
        {
            var button = new Button();
            button.Name = "Action";
            var vm = new MockViewModelWithAction();
            button.SetValue(ViewBinder.ModelProperty, vm);
            var binder = new MethodBinder(button,vm);
            Assert.AreEqual("Click",binder.EventName);
        }
        
        [Test]
        public void When_constructed_with_an_expression_then_binds_to_control()
        {
            var vm = new OrderViewModel();
            var view = new OrderView();
            var binder = new MethodBinder(p => vm.Save(),view, vm);
            Assert.AreEqual(view.Save, binder.ViewElement);
        }

        [Test]
        public void When_constructed_with_an_expression_then_binds_to_vm_action()
        {
            var vm = new OrderViewModel();
            var view = new OrderView();
            var binder = new MethodBinder(p => vm.Save(), view,vm);
            Assert.AreEqual("Save", binder.ActionName);
        }

        [Test]
        public void When_constructed_with_an_expression_then_binds_to_vm_is_enabled_action()
        {
            var vm = new OrderViewModel();
            var view = new OrderView();
            var binder = new MethodBinder(p => vm.Save(), view,vm);
            Assert.AreEqual("IsSaveEnabled", binder.IsEnabledBinder.TargetPath);
        }

        [Test]
        public void When_constructed_then_subscribes_to_event()
        {
            var source = new MockEventSource();
            var binder = new MethodBinder<MockEventArgs>(source, "MockEvent", e => this.SetAction());
            binder.Bind();
            Assert.IsNotNull(source.EventHandler);
        }

        [Test]
        public void When_event_is_raised_then_action_is_called()
        {
            var source = new MockEventSource();
            var binder = new MethodBinder<MockEventArgs>(source, "MockEvent", e => this.SetAction());
            binder.Bind();
            source.RaiseMockEvent("foo");
            Assert.IsTrue(this._actionWasCalled);
        }

        [Test]
        public void When_command_is_disabled_and_event_is_raised_then_action_is_not_called()
        {
            var source = new MockEventSource();
            var target = new MockViewModelWithAction() {IsActionEnabled = false};
            var binder = new MethodBinder<MockEventArgs>(source, "MockEvent", e => this.SetAction(), p => target.IsActionEnabled,target);
            binder.Bind();
            source.RaiseMockEvent("foo");
            Assert.IsFalse(this._actionWasCalled);
        }

        [Test]
        public void When_constructed_with_action_disabled_then_source_gets_disabled()
        {
            var source = new MockEventSource();
            var target = new MockViewModelWithAction() {IsActionEnabled = false};
            var binder = new MethodBinder<MockEventArgs>(source, "MockEvent", e => this.SetAction(), p => target.IsActionEnabled,target);
            binder.Bind();
            Assert.IsFalse(source.IsEnabled);
        }

        [Test]
        public void Source_Is_Enabled_When_Target_Command_Is_Enabled_During_Initialization()
        {
            var source = new MockEventSource();
            var target = new MockViewModelWithAction() {IsActionEnabled = true};
            var binder = new MethodBinder<MockEventArgs>(source, "MockEvent", e => this.SetAction(), p => target.IsActionEnabled,target);
            Assert.IsTrue(source.IsEnabled);
        }

        [Test]
        public void Source_Is_Disabled_When_Target_Command_Gets_Disabled_After_Initialization()
        {
            var source = new MockEventSource();
            var target = new MockViewModelWithAction() {IsActionEnabled = true};
            var binder = new MethodBinder<MockEventArgs>(source, "MockEvent", e => target.Action(), p => target.IsActionEnabled,target);
            binder.Bind();
            target.IsActionEnabled = false;
            Assert.IsFalse(source.IsEnabled);
        }

        [Test]
        public void Source_Is_Enabled_When_Target_Command_Gets_Enabled_After_Initialization()
        {
            var source = new MockEventSource();
            var target = new MockViewModelWithAction() {IsActionEnabled = false};
            var binder = new MethodBinder<MockEventArgs>(source, "MockEvent", e => target.Action(), p => target.IsActionEnabled,target);
            target.IsActionEnabled = true;
            Assert.IsTrue(source.IsEnabled);
        }

        [Test]
        public void Source_Gets_Disabled_When_Target_Command_Is_Disabled_During_Initialization_With_Expression()
        {
            var source = new MockEventSource();
            var target = new MockViewModelWithAction() { IsActionEnabled = false };
            var binder = new MethodBinder<MockEventArgs>(source, "MockEvent", e => this.SetAction(), p => target.IsActionEnabled,target);
            binder.Bind();
            Assert.IsFalse(source.IsEnabled);
        }

        [Test]
        public void Source_Is_Enabled_When_Target_Command_Is_Enabled_During_Initialization_With_Expression()
        {
            var source = new MockEventSource();
            var target = new MockViewModelWithAction() { IsActionEnabled = true };
            var binder = new MethodBinder<MockEventArgs>(source, "MockEvent", e => this.SetAction(), p => target.IsActionEnabled,target);
            Assert.IsTrue(source.IsEnabled);
        }

        [Test]
        public void Source_Is_Disabled_When_Target_Command_Gets_Disabled_After_Initialization_With_Expression()
        {
            var source = new MockEventSource();
            var target = new MockViewModelWithAction() { IsActionEnabled = true };
            var binder = new MethodBinder<MockEventArgs>(source, "MockEvent", e => target.Action(), p => target.IsActionEnabled,target);
            binder.Bind();
            target.IsActionEnabled = false;
            Assert.IsFalse(source.IsEnabled);
        }

        [Test]
        public void Source_Is_Enabled_When_Target_Command_Gets_Enabled_After_Initialization_With_Expression()
        {
            var source = new MockEventSource();
            var target = new MockViewModelWithAction() { IsActionEnabled = false };
            var binder = new MethodBinder<MockEventArgs>(source, "MockEvent", e => target.Action(), p => target.IsActionEnabled,target);
            target.IsActionEnabled = true;
            Assert.IsTrue(source.IsEnabled);
        }

   
        [Test]
        public void MethodBinder_Doesnt_Support_Parameters_On_Actions()
        {
            var target = new MockViewModelWithAction();

            AssertExt.ExpectException<NotSupportedException>(() => 
            {
                var binder = new MethodBinder<MockEventArgs>(new UserControl(), "MockEvent", e => target.Action("will fail"));
            });
        }

        //WireEvent<Button>(control=>control.Click).From(MyControl).To<MyViewModel, MyEventArgs>((vm, e) => vm.MyMethod(e.MyArg));

    }


    public class MockEventSource : Control
    {
        public EventHandler<MockEventArgs> EventHandler;

        public event EventHandler<MockEventArgs> MockEvent
        {
            add
            {
                this.EventHandler += value;
            }
            remove
            {
                this.EventHandler -= value;
            }
        }

        public void RaiseMockEvent(string arg)
        {
            this.EventHandler(this, new MockEventArgs { Arg = arg });
        }
    }

    public class MockEventArgs : RoutedEventArgs
    {
        public string Arg { get; set; }
    }



}
