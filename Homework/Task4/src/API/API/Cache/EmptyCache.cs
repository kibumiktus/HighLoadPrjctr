namespace API.Cache
{
    public class EmptyCache : ICache
    {
        public object this[string key] => null;

        public void Add(string key, object Value)
        {
        }

        public void Remove(string key)
        {
        }

        public bool TryGetValue(string key, out object value)
        {
            value = null;
            return false;
        }
    }
}