using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Elixir
{
    public interface IValueAccessor<T>
    {
        T GetValue();
    }
}
