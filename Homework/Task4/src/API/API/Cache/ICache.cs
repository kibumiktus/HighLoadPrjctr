namespace API.Cache
{
    public interface ICache
    {
        void Add(string key, object value);
        void Remove(string key);
        bool TryGetValue(string key, out object value);
        object this[string key] { get; }
    }
}