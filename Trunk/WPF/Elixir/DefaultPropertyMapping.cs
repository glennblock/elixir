using System;
using System.Collections.Generic;
using System.Windows;

namespace Elixir
{
    public class DefaultPropertyMapping : Mapping<Type, DependencyProperty>
    {
        public DefaultPropertyMapping(Type key, DependencyProperty value)
            : base(key, value)
        {
        }
    }
}