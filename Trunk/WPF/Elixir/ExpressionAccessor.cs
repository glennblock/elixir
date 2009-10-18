using System;
using System.Linq.Expressions;

namespace Elixir
{
    public class ExpressionAccessor<TValue,TModel> : IValueAccessor<TValue>
    {
        private Func<TModel, TValue> _getter;
        private TModel _model;

        public ExpressionAccessor(Expression<Func<TModel, TValue>> expression, TModel model)
        {
            this._getter = expression.Compile();
            this._model = model;
        }

        public TValue GetValue()
        {
            return _getter(_model);
        }
    }
}