namespace Elixir
{
    public abstract class Mapping<TKey, TValue> : IMapping
    {
        public Mapping(TKey key, TValue value)
        {
            this.Key = key;
            this.Value = value;
        }

        public TKey Key { get; private set; }
        public TValue Value { get; private set; }

        #region IMapping Members

        object IMapping.Key
        {
            get { return this.Key; }
        }

        object IMapping.Value
        {
            get { return this.Value; }
        }

        #endregion
    }
}