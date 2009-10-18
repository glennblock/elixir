using System;
using System.Linq.Expressions;

namespace Elixir
{
    public class ExpressionInvoker<TArgs> : IActionInvoker<TArgs>
    {
        private readonly Action<TArgs> _action;

        public ExpressionInvoker(Expression<Action<TArgs>> action)
        {
            _action = (Action<TArgs>) action.Compile();     
        }

        public void Invoke(TArgs args)
        {
            _action(args);
        }
    }
}