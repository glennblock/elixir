using System;
using System.Net;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace Elixir
{
    public class Binders : Collection<Bind>, ISupportInitialize
    {
        #region ISupportInitialize Members

        void ISupportInitialize.BeginInit()
        {
            
        }

        void ISupportInitialize.EndInit()
        {
            //foreach (Bind bind in this)
            //{
            //    (bind as IBinder).Bind();
            //}
        }

        #endregion
    }


}
