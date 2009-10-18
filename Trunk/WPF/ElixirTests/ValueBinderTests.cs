using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
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
    public class ValueBinderTests 
    {
        [SetUp]
        public void Setup()
        {
            ViewBinder.Mappings.Add(new DefaultPropertyMapping(typeof(MockTarget<string>), MockTarget<string>.ValueProperty));
        }

        [Test]
        public void When_constructed_sets_binding_source()
        {
            var binding = new Binding();
            var sample = SampleBinderInfo<string>.GetDummyBinderInfo("TestValue");
            var binder = new ValueBinder<object>(sample.Element, sample.Property, sample.ViewModel, sample.SourcePath) { Binding = binding };
            binder.Bind();
            Assert.AreEqual(sample.ViewModel, binding.Source);
        }

        [Test]
        public void When_constructed_with_a_TextBox_then_sets_property_to_text()
        {
            var textBox = new TextBox();
            textBox.Name = "FirstName";
            var binder = new ValueBinder<object>(textBox,null);
            Assert.AreEqual(TextBox.TextProperty, binder.Property);
        }

        [Test]
        public void When_constructed_with_a_property_expression_then_binds_property_to_control()
        {
            var view = new OrderView();
            var vm = new OrderViewModel();
            var binder = new ValueBinder<OrderViewModel>(p => p.FirstName,view);
            Assert.AreEqual(binder.ViewElement, view.FirstName);
        }

        [Test]
        public void When_constructed_with_a_property_expression_then_binds_property_to_text_property()
        {
            var view = new OrderView();
            var vm = new OrderViewModel();
            var binder = new ValueBinder<OrderViewModel>(p => p.FirstName, view);
            Assert.AreEqual(binder.ViewElement, view.FirstName);
        }


        [Test]
        public void When_constructed_with_a_TextBox_then_can_set_Text()
        {
            var element = new MockTarget<string>();
            element.Name = "Value";
            var vm = new MockSource<string>("");
            var binder = new ValueBinder<object>(element,vm);
            binder.Bind();
            binder.SetValue("John Doe");
            Assert.AreEqual("John Doe", vm.Value);
        }

        [Test]
        public void When_constructed_with_a_TextBox_then_can_read_Text()
        {
            var element = new MockTarget<string>();
            element.Name = "Value";
            var vm = new MockSource<string>("");
            var binder = new ValueBinder<object>(element,vm);
            binder.Bind();
            vm.Value = "John Doe";
            Assert.AreEqual("John Doe", binder.GetValue());
        }

        [Test]
        public void When_constructed_with_a_TextBox_then_sets_target_path_to_TextBox_Name()
        {
            var element = new MockTarget<string>();
            element.Name = "Value";
            var binder = new ValueBinder<object>(element,null);
            Assert.AreEqual("Value", binder.TargetPath);
        }

        [Test]
        public void When_constructed_with_a_TextBox_then_sets_view_element()
        {
            var element = new MockTarget<string>();
            element.Name = "Value";
            var binder = new ValueBinder<object>(element,null);
            Assert.AreEqual(element, binder.ViewElement);
        }

        [Test]
        public void When_constructed_with_a_source_path_then_it_is_set()
        {
            var sample = SampleBinderInfo<string>.GetDummyBinderInfo("TestValue");
            var binder = new ValueBinder<object>(sample.Element, sample.Property, sample.ViewModel, sample.SourcePath);

            Assert.AreEqual(sample.SourcePath, binder.TargetPath);
        }

        [Test]
        public void When_constructed_with_an_expression_then_sets_path()
        {
            var binding = new Binding();
            var sample = SampleBinderInfo<string>.GetDummyBinderInfo("TestValue");
            var binder = new ValueBinder<object>(sample.Element, sample.Property, p=>sample.ViewModel.Value) {Binding=binding};

            Assert.AreEqual("ViewModel.Value", binder.TargetPath);
        }


        [Test]
        public void When_constructed_then_sets_binding_on_element()
        {
            var binding = new Binding();
            var sample = SampleBinderInfo<string>.GetDummyBinderInfo("TestValue");
            var binder = new ValueBinder<object>(sample.Element, sample.Property, sample.ViewModel, sample.SourcePath) {Binding=binding};
            binder.Bind();
            Assert.AreSame(binding, sample.Element.GetBindingExpression(sample.Property).ParentBinding);
        }

        [Test]
        public void When_constructed_with_element_and_expression_then_property_is_inferred()
        {
            var element = new TextBox();
            var vm = new MockSource<string>("my value");
            var viewBinder = new ViewBinder(element, vm);
            viewBinder.Add(new ValueBinder<object>(element, _ => vm.Value, vm));
            viewBinder.Bind();
            Assert.AreEqual("my value", element.Text);
        }

        [Test]
        public void When_constructed_with_element_and_expression_then_binding_mode_is_inferred()
        {
            ViewBinder.Mappings.Add(new DefaultPropertyMapping(typeof(MockTarget<string>), MockTarget<string>.ValueProperty));
            var element = new MockTarget<string>();
            var vm = new MockSource<string>("my value");
            var viewBinder = new ViewBinder(element, vm);
            viewBinder.Add(new ValueBinder<object>(element, _ => vm.Value,vm));
            viewBinder.Bind();
            element.SetValue(MockTarget<string>.ValueProperty, "new value");
            Assert.AreEqual("new value", vm.Value, "The binding should be inferred as two-way");
        }

        [Test]
        public void When_value_is_retrieved_then_it_is_returned()
        {
            var sample = SampleBinderInfo<string>.GetDummyBinderInfo("TestValue");
            var binder = new ValueBinder<object>(sample.Element, sample.Property, sample.ViewModel, sample.SourcePath);
            binder.Bind();
            Assert.AreEqual("TestValue", binder.GetValue());
        }

        [Test]
        public void When_value_is_set_then_viewmodel_property_is_updated()
        {
            var sample = SampleBinderInfo<string>.GetDummyBinderInfo("TestValue");
            var binder = new ValueBinder<object>(sample.Element, sample.Property, sample.ViewModel, sample.SourcePath);
            binder.Bind();
            binder.SetValue("New Value");
            Assert.AreEqual("New Value", binder.GetValue());
        }

        [Test]
        public void When_viewmodel_is_updated_using_twoway_binding_then_property_is_updated()
        {
            var sample = SampleBinderInfo<string>.GetDummyBinderInfo("TestValue");
            var binder = new ValueBinder<object>(sample.Element, sample.Property, sample.ViewModel, sample.SourcePath);
            binder.Bind();
            sample.ViewModel.Value = "New Value";

            Assert.AreEqual("New Value", binder.GetValue());
        }

        [Test]
        public void When_viewmodel_is_updated_using_onetime_binding_then_property_is_not_updated()
        {
            var sample = SampleBinderInfo<string>.GetDummyBinderInfo("TestValue");
            var binder = new ValueBinder<object>(sample.Element, sample.Property, sample.ViewModel, sample.SourcePath) {BindingMode = BindingMode.OneTime};
            binder.Bind();
            sample.ViewModel.Value = "New Value";

            Assert.AreEqual("TestValue", binder.GetValue());
        }

        [Test]
        public void When_value_is_set_using_onetime_binding_then_viewmodel_is_not_updated()
        {
            var sample = SampleBinderInfo<string>.GetDummyBinderInfo("TestValue");
            var binder = new ValueBinder<object>(sample.Element, sample.Property, sample.ViewModel, sample.SourcePath) { BindingMode = BindingMode.OneTime};
            binder.SetValue("New Value");
            Assert.AreEqual("TestValue", sample.ViewModel.Value);
        }

        [Test]
        public void When_value_is_set_using_twoway_binding_then_viewmodel_is_updated()
        {
            var sample = SampleBinderInfo<string>.GetDummyBinderInfo("TestValue");
            var binder = new ValueBinder<object>(sample.Element, sample.Property, sample.ViewModel, sample.SourcePath);
            binder.Bind();
            binder.SetValue("New Value");
            Assert.AreEqual("New Value", sample.ViewModel.Value);
        }

        public class SampleBinderInfo<T>
        {
            public MockSource<T> ViewModel { get; set; }
            public string SourcePath { get; set; }
            public FrameworkElement Element { get; set; }
            public DependencyProperty Property { get; set; }

            public static SampleBinderInfo<T> GetDummyBinderInfo(T value)
            {
                var sample = new SampleBinderInfo<T>()
                {
                    ViewModel = new MockSource<T>(value),
                    SourcePath = MockSource.Value,
                    Element = new MockTarget<T>(),
                    Property = MockTarget<T>.ValueProperty
                };
                return sample;
            }
        }
    }


    
}
