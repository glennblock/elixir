//-----------------------------------------------------------------------
// <copyright company="Microsoft">
//      (c) Copyright Microsoft Corporation.
//      This source is subject to [###LICENSE_NAME###].
//      Please see [###LICENSE_LINK###] for details.
//      All other rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.ComponentModel;
using System.Windows.Data;
using Expression = System.Linq.Expressions.Expression;

namespace Elixir
{
    public class MethodBinder : MethodBinder<RoutedEventArgs>
    {
        public MethodBinder(Control control, object viewModel)
        {
            Initialize(control,viewModel);
        }

        public MethodBinder(Expression<Action<object>> action, FrameworkElement view, object viewModel)
        {
            string actionName = ExpressionUtils.GetMethodFromExpression(action);
            //object vm = ExpressionUtils.GetModelFromExpression(action);
            Control control = (Control) view.FindName(actionName);
            Initialize(control,viewModel);

        }

        private void Initialize(Control control, object viewModel)
        {
            this.EventName = ViewBinder.GetEventForElement(control);
            this.ActionName = control.Name;
            this.ViewElement = control;
            this.TargetAction = new MethodInfoInvoker<RoutedEventArgs>(this.ActionName, viewModel);

            string actionEnabledProperty = ViewBinder.GetActionEnabled(this.ActionName, viewModel.GetType());
            if (actionEnabledProperty != null)
            {
                this.ActionEnabledPath = actionEnabledProperty;
                BindSourceIsEnabled(control, viewModel, this.ActionEnabledPath);
            }
        }
    }


    public class MethodBinder<TEventArgs> : IBinder, IMethodBinder
        where TEventArgs:RoutedEventArgs
    {
        #region Static fields and constants
        #endregion

        #region Member fields

        protected IActionInvoker<TEventArgs> TargetAction { get; set; }
        
        #endregion

        #region Properties

        public System.Windows.FrameworkElement ViewElement { get; protected set; }
        public string EventName { get; protected set; }
        public string ActionEnabledPath { get; protected set; }
        public string ActionName { get; protected set; }
        public IValueBinder IsEnabledBinder { get; private set; }
        
        #endregion

        #region All Constructors

        protected MethodBinder()
        {
            
        }


        public MethodBinder(Control control, string eventName, Expression<Action<TEventArgs>> viewModelAction)
        {
            this.EnsureParameterlessMethodCall(viewModelAction);

            this.ViewElement = control;
            this.TargetAction = new ExpressionInvoker<TEventArgs>(viewModelAction);
            this.ActionName = ExpressionUtils.GetMethodFromExpression(viewModelAction);
            this.EventName = eventName;
        }


        public MethodBinder(Control control, string eventName, Expression<Action<TEventArgs>> viewModelAction, Expression<Func<object, bool>> actionEnabled, INotifyPropertyChanged viewModel)
            :this(control, eventName, viewModelAction)
        {
            //INotifyPropertyChanged viewModel = ExpressionUtils.GetModelFromExpression(actionEnabled);
            this.ActionEnabledPath = ExpressionUtils.GetExpressionPropertyPath(actionEnabled);
            
            if (viewModel != null)
                BindSourceIsEnabled(control, viewModel, this.ActionEnabledPath);
        }

        protected void HookEvent(Control control, string eventName)
        {
            Type sourceType = control.GetType();
            EventInfo eventInfo = sourceType.GetEvent(eventName);
            Delegate eventDelegate = Delegate.CreateDelegate(eventInfo.EventHandlerType, this, "EventHandler");
            sourceType.GetEvent(eventName).AddEventHandler(control, eventDelegate);
        }


        #endregion

        #region Methods
        
        public void Bind()
        {
            HookEvent((Control) ViewElement, this.EventName);
            if (this.IsEnabledBinder != null)
                this.IsEnabledBinder.Bind();
        }

        public Binding Binding
        {
            get { return null; }
        }


        private void EnsureParameterlessMethodCall(Expression<Action<TEventArgs>> viewModelAction)
        {
            MethodCallExpression method = viewModelAction.Body as MethodCallExpression;

            if (method == null || method.Arguments.Count > 0)
            {
                throw new NotSupportedException("MethodBinder can only be used to invoke parameterless methods.  The Expression<Action<TEventArgs>> supplied must map to a MethodCallExpression that uses 0 arguments.");
            }
        }

        public void EventHandler(object sender, TEventArgs e)
        {
            if (this.IsEnabledBinder == null || this.IsEnabledBinder.GetValue<bool>())
            {
                this.TargetAction.Invoke(e);
            }
        }

        protected void BindSourceIsEnabled(Control control, object viewModel, string path)
        {
            this.IsEnabledBinder = new ValueBinder<object>(control, Control.IsEnabledProperty, viewModel, path);
        }

        public override string ToString()
        {
            return "MethodBinder: " + this.ViewElement.Name + "." + this.EventName + " => " + this.ActionName + "(), " + this.ViewElement.Name + "." + this.IsEnabledBinder.Property.Name + " => " + this.IsEnabledBinder.Binding.Path.Path;
        }

        #endregion
    }
}
