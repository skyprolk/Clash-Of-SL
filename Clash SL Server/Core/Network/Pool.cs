namespace CSS.Core.Network
{ 
    using System;
    using System.Collections.Concurrent;

    internal class Pool<T>
    {
        public Pool(Func<T> factory, int count = 10)
        {
            _bag = new ConcurrentBag<T>();
            _factory = factory;
            for (int i = 0; i < count; i++)
            {
                var obj = factory();
                _bag.Add(obj);
            }
        }

        private readonly Func<T> _factory;
        private readonly ConcurrentBag<T> _bag;

        public int Count => _bag.Count;

        public void Push(T obj)
        {
            _bag.Add(obj);
        }

        public T Get()
        {
            var d = default(T);
            if (!_bag.TryTake(out d))
                return _factory();
            return d;
        }
    }
}
