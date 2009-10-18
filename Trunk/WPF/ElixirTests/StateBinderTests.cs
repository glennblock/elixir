using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
#if !SILVERLIGHT
using NUnit.Framework;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestFixture = Microsoft.VisualStudio.TestTools.UnitTesting.TestClassAttribute;
using Test = Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute;
#endif
using System.Linq.Expressions;
using Elixir;


namespace ElixirTests
{
    [TestFixture]
    [Ignore]
    public class StateBinderTests
    {
        [Test]
        public void State_Binder_Binds_To_State_Property()
        {
            var SaveButton = new Button();
            var Binding = new Binding();
            var vm = new CustomerViewModel {FirstName = "John Doe", State = EditState.Edit};
            var StateBinder = new StateGroupBinder<EditState>(SaveButton, Button.IsEnabledProperty, p=> vm.State, Binding);
            Assert.AreEqual("State", Binding.Path);
        }

        [Test]
        public void Control_Is_Updated_When_State_Changes()
        {
            var SaveButton = new Button();
            var Binding = new Binding();
            var vm = new CustomerViewModel { FirstName = "John Doe", State = EditState.Edit };
            var StateGroupBinder = new StateGroupBinder<EditState>(SaveButton, Button.IsEnabledProperty, p => vm.State, Binding);
            
            vm.State = EditState.Edit;
        }

        [Test]
        public void Delegate_Is_Invoked_When_State_Changes()
        {
            
        }

        public enum EditState {View, Edit}

        public class CustomerViewModel : Customer
        {
            private EditState _state = EditState.View;
            public EditState State
            {
                get {return _state;}
                set
                {
                    if (_state != value)
                    {
                        _state = value;
                        RaisePropertyChanged("State");    
                    }
                    
                }
            }
        }

        

    }

}
