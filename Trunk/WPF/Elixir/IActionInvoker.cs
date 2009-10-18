namespace Elixir
{
    public interface IActionInvoker<TArgs>
    {
        void Invoke(TArgs args);
    }
}