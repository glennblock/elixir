using System;
using System.Linq.Expressions;
namespace Elixir
{
    public interface IMethodBinder : IBinder
    {
        string ActionName { get; }
        string ActionEnabledPath { get; }
        string EventName { get; }
        IValueBinder IsEnabledBinder { get; }
    }
}
