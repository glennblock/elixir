using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using ElixirSampleClient;
using NUnit.Framework;
using Elixir;

namespace ElixirSampleClientTests
{
    [TestFixture]
    public class ClientTests
    {   
        private IViewBinderFluent<ContactViewModel> _viewBinder;
        private Window1 _window;

        [SetUp]
        public void Setup()
        {
            _window = new Window1();
            _viewBinder = _window.Binder;
        }

        [Test]
        public void When_binder_is_constructed_then_FirstName_is_bound_to_FirstName_on_model()
        {
            IValueBinder binder = (IValueBinder) _viewBinder.GetPropertyBinder("FirstName", TextBox.TextProperty);
            Assert.AreEqual("FirstName", binder.TargetPath);
        }

        [Test]
        public void When_binder_is_constructed_then_LastName_is_bound_to_LastName_on_model()
        {
            IValueBinder binder = (IValueBinder) _viewBinder.GetPropertyBinder("LastName", TextBox.TextProperty);
            Assert.AreEqual("LastName", binder.TargetPath);

        }

        [Test]
        public void When_binder_is_constructed_then_Address_is_bound_to_Address_on_model()
        {
            IValueBinder binder = (IValueBinder) _viewBinder.GetPropertyBinder("Address", TextBox.TextProperty);
            Assert.AreEqual("Address", binder.TargetPath);
        }

        [Test]
        public void When_binder_is_constructed_then_Cities_is_bound_to_Cities_on_model()
        {
            ListBinder<City,object> binder = (ListBinder<City,object>) _viewBinder.GetPropertyBinder("Cities", ListBox.ItemsSourceProperty);
            Assert.AreEqual("Cities", binder.ItemsBinder.TargetPath);
        }

        [Test]
        public void When_binder_is_constructed_then_Cities_is_bound_to_SelectedCity_on_model()
        {
            ListBinder<City,object> binder = (ListBinder<City,object>)_viewBinder.GetPropertyBinder("Cities", ListBox.ItemsSourceProperty);
            Assert.AreEqual("SelectedCity", binder.SelectedItemBinder.TargetPath);
        }

        [Test]
        public void When_binder_is_constructed_then_Cities_is_bound_to_SelectedCities_on_model()
        {
            ListBinder<City,object> binder = (ListBinder<City,object>)_viewBinder.GetPropertyBinder("Cities", ListBox.ItemsSourceProperty);
            Assert.AreEqual("SelectedCities", binder.SelectedItemsPath);
        }

        [Test]
        public void When_binder_is_constructed_then_Save_is_bound_to_Save_on_model()
        {
            var binder = _viewBinder.GetMethodBinder("Save", "Click");
            Assert.AreEqual("Save", binder.ActionName);
        }

        [Test]
        public void When_binder_is_constructed_then_Save_is_bound_to_IsSavedEnabled_on_model()
        {
            var binder = _viewBinder.GetMethodBinder("Save", "Click");
            Assert.AreEqual("IsSaveEnabled", binder.ActionEnabledPath);
        }

                
    }
}
