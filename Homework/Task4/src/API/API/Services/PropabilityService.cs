using System;
using System.Buffers;

using System.Collections.Generic;
using System.Linq;

namespace API.Services
{
    public class PropabilityService
    {
        private static Dictionary<decimal, PropabilityService> _services = new Dictionary<decimal, PropabilityService>();
        public static PropabilityService GetInstance(decimal propability)
        {
            if (!_services.ContainsKey(propability))
                _services.Add(propability, new PropabilityService(propability));
            return _services[propability];
        }
        public decimal Propability { get; private set; }
        public int Length { get; set; } = 100;

        private Queue<bool> _queue;

        public PropabilityService(decimal propability)
        {
            Propability = propability;
            _queue = new Queue<bool>();
        }

        public void AppendPropabilityVariants()
        {
            var pool = ArrayPool<bool>.Shared;
            var various = pool.Rent(Length);
            for (int i = 0; i < Propability * Length; i++)
            {
                various[i] = true;
            }
            for (int i = (int)Math.Round(Propability * Length, MidpointRounding.ToZero); i < Length; i++)
            {
                various[i] = false;
            }
            foreach (var item in various.Take(Length).OrderBy(v => Guid.NewGuid()))
            {
                _queue.Enqueue(item);
            }
            pool.Return(various);
        }

        public bool RentValue()
        {
            if (_queue.Count * 2 < Length)
                AppendPropabilityVariants();
            
            return _queue.Peek();
        }
    }
}
