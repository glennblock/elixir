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
    public class MethodInfoInvokerTests
    {
        [Test]
        public void When_invoked_then_method_is_called()
        {
            var vm = new MockViewModelWithAction();
            var invoker = new MethodInfoInvoker<string>("Action", vm);
            invoker.Invoke("Foo");
            Assert.IsTrue(vm.ActionCalled);
        } 
    }
}
