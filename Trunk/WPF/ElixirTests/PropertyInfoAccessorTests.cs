using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public class PropertyInfoAccessorTests
    {
        [Test]
        public void When_value_is_retrieved_then_property_is_invoked()
        {
            CustomerListViewModel vm = new CustomerListViewModel(new Customer());
            var accessor = new PropertyInfoAccessor<ObservableCollection<Customer>>("Customers", vm);
            var customers = accessor.GetValue();
            Assert.AreSame(vm.Customers, customers);
        }

    }
}
