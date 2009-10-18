using System;
using System.Collections.Generic;
using System.Linq;
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
using System.Collections.ObjectModel;

namespace Elixir
{
    /// <summary>
    /// Interaction logic for ViewBinderDisplay.xaml
    /// </summary>
    public partial class ViewBinderDisplay : UserControl
    {
        public ViewBinderDisplay()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(ViewBinderDisplay_Loaded);
        }

        void ViewBinderDisplay_Loaded(object sender, RoutedEventArgs e)
        {
            IViewBinder instance = ViewBinder.GetInstance(this.Parent);

            ViewBinder<ViewBinderViewModel> binder = new ViewBinder<ViewBinderViewModel>(this, new ViewBinderViewModel(instance));
            //binder.List(i => i.Binders);
            binder.Bind();
        }

        private class ViewBinderViewModel
        {
            private IViewBinder _binder;
            private ObservableCollection<IBinder> _binders;

            public ViewBinderViewModel(IViewBinder binder)
            {
                this._binder = binder;
                this._binders = new ObservableCollection<IBinder>(binder);
            }

            public ObservableCollection<IBinder> Binders
            {
                get { return this._binders; }
            }
        }
    }
}
