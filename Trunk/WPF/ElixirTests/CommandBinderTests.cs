using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    public class CommandBinderTests
    {
        [Test]
        public void Source_Is_Enabled_When_Target_ICommand_CanExecute_Is_True_During_Initialization()
        {
            var source = new MockEventSource();
            var target = new MockViewModelWithAction();
            var binder = new CommandBinder<MockEventArgs>(source, "MockEvent", target.Command);
            binder.Bind();
            Assert.IsTrue(source.IsEnabled);
        }

        [Test]
        public void Source_Is_Enabled_When_ICommand_CanExecute_Is_True_After_Initialization()
        {
            var source = new MockEventSource();
            var target = new MockViewModelWithAction();
            var binder = new CommandBinder<MockEventArgs>(source, "MockEvent", target.Command);
            binder.Bind();
            Assert.IsTrue(source.IsEnabled);
        }

        [Test]
        public void Target_Command_CallCount_Is_Zero_After_Initialization_Of_CommandBinder()
        {
            var source = new MockEventSource();
            var target = new MockViewModelWithAction();
            var binder = new CommandBinder<MockEventArgs>(source, "MockEvent", target.Command);
            binder.Bind();
            Assert.AreEqual(0, target.Command.CallCount);
        }

        [Test]
        public void Target_Command_Is_Executed_When_Source_Event_Is_Raised_And_Command_Is_Enabled()
        {
            var source = new MockEventSource();
            var target = new MockViewModelWithAction();
            var binder = new CommandBinder<MockEventArgs>(source, "MockEvent", target.Command);
            binder.Bind();
            source.RaiseMockEvent("foo");
            Assert.AreEqual(1, target.Command.CallCount);
        }

        [Test]
        public void Source_Is_Disabled_When_Target_Command_Becomes_Disabled()
        {
            var source = new MockEventSource();
            var target = new MockViewModelWithAction();
            var binder = new CommandBinder<MockEventArgs>(source, "MockEvent", target.Command);
            target.Command.MaxCalls = 1;
            binder.Bind();
            source.RaiseMockEvent("foo");
            Assert.IsFalse(source.IsEnabled);
        }

    }
}
