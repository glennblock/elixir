using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Elixir;
using ElixirSampleClient;

namespace ElixirSampleClient
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            //should look into IDataErrorInfo

            InitializeComponent();
            Bind();
     
        }

        #region FI

        /*
                <UserControl>
                  <UserControl.ViewBinder>
                    <ViewBinder>
                      <BindControl Control="FirstName" />
                      <BindControl Control="Undo" />
                      <Value From="{StaticResource MyResource}" To="{Binding Path=Text, Control.Name=FirstName}
                      <BindControl Control="LastName" ModelProperty="CustomerLastNaem" Converter="..." />
                </UserControl>
             
             
                ViewBinder.RegisterActionConvention("{0}", "{0}", "Can{0}");
                ViewBinder.RegisterActionConvention("{0}", "Execute{0}", "Is{0}Enabled");
                ViewBinder.RegisterBindingConvention("{0}", "{0}Enabled");

                var binder = 
	                ViewBinder.
                    bindView(this).toViewModel(vm)
	                bindControl(FirstName).
	                bindControl(Undo).	
	                Value(FirstName.Text).toResource("MyResource").
	                bindControl(FirstName).toProperty(vm=>vm.FirstName).
		                setEnabledTo(vm=>vm.FirstNameEnabled)
	                bindControl(LastName).toProperty(vm=>vm.LastName).withConverter(LastNameConverter)
	                bindControl(Save).toAction(vm=>vm.Save()).
		                setEnabledTo(vm=>vm.IsSaveEnabled).
	                bindControl(Undo).for(Click()).toAction(vm=>vm.Undo()).
		                setEnabledTo(vm=>vm.UndoEnabled).

	                StateGroup(vm=>vm.State).
		                when(EditStates.Clean).
			                .disable(Save, Undo).
		                when(EditStates.Dirty).
			                .enable(Save, Undo).
		                when(EditStates.View).
			                .disable(FirstName, LastName).
			                .hide(Save, Undo)
                            .action(p=>DoSomething(););

            */

        #endregion

        private void Bind()
        {
            ContactViewModel vm = new ContactViewModel() { FirstName = "Jeff", LastName = "Handley", Address = "123 Any Street" };
            
            //ExplicitBind(vm);
            ExplicitBindWithConventions(vm);
            //ConventionallyBind(vm);

        }

        #region Explicit Binding

        public void ExplicitBind(ContactViewModel vm)
        {
            Binder = new ViewBinder<ContactViewModel>(this, vm);
            Binder.Value(FirstName, TextBox.TextProperty, p =>p.FirstName);
            Binder.Value(LastName, TextBox.TextProperty, p => p.LastName);
            Binder.Value(Address, TextBox.TextProperty, p => p.Address);
            Binder.Action(Save, EventNames.Click, p => vm.Save(), p => vm.IsSaveEnabled);
            Binder.List(Cities, p => vm.Cities, p => vm.SelectedCities);
            Binder.Bind();
        }

        #endregion

        #region Bind with Conventions

        public void ExplicitBindWithConventions(ContactViewModel vm)
        {
            Binder = ViewBinder.For(this, vm).
                Value(p => p.FirstName).
                Value(p=>p.LastName).
                Value(p => p.Address).
                AddBinder(new ValueBinder<ContactViewModel>(this.OnMailingList, vm)).
                List(p => p.Cities).
                Action(p => vm.Save()).
                Template<City>("City", t => t.Value(p => p.CityName));
            
            Binder.Bind();
        }

        #endregion

        #region Autobind

        public void ConventionallyBind(ContactViewModel vm)
        {
            Binder = ConventionalViewBinder.For(this, vm);
            Binder.Bind();
        }

        #endregion

        public class EventNames
        {
            public static string Click = "Click";
        }

        internal static IViewBinderFluent<ContactViewModel> _binder;
        public IViewBinderFluent<ContactViewModel> Binder
        {
            get { return _binder; }
            private set { _binder = value; }
        }



    }
}