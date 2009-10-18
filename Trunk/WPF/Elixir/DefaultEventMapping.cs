using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Elixir
{
    public class DefaultEventMapping : Mapping<Type, string>
    {
        public DefaultEventMapping(Type key, string value) : 
            base(key, value)
        {
        }
    }
}
