using System;
using System.Reflection;

namespace Elixir
{
    public class MethodInfoInvoker<TArgs> : IActionInvoker<TArgs>
    {
        private string _method;
        private MethodInfo _methodInfo;
        private object _model;

        public MethodInfoInvoker(string method, object model)
        {
            this._method = method;
            this._model = model;
            
            Type viewModelType = model.GetType();
            this._methodInfo = viewModelType.GetMethod(method,new Type[]{});
        }

        public void Invoke(TArgs args)
        {
            _methodInfo.Invoke(_model, null);
        }
    }
}