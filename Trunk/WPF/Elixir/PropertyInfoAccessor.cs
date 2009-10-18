using System;
using System.Reflection;

namespace Elixir
{
    public class PropertyInfoAccessor<T> : IValueAccessor<T>
    {
        private string _property;
        private PropertyInfo _propertyInfo;
        private object _model;

        public PropertyInfoAccessor(string property, object model)
        {
            this._property = property;
            this._model = model;

            Type viewModelType = model.GetType();
            this._propertyInfo = viewModelType.GetProperty(property);
        }

        public T GetValue() 
        {
            return (T) _propertyInfo.GetValue(_model, null);
        }
    }
}