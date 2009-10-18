namespace Elixir
{
    public interface IValueBinderDefinition : IDependencyPropertyBinderDefinition
    {
        System.Windows.Data.BindingMode BindingMode { get; set; }
        string TargetPath { get; }
        string ViewElementName { get; }
    }
}