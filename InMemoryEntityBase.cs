using System;
using System.Collections.Generic;
using System.Text;

namespace IgniteQueryEntityAffinityRepro
{
    [Serializable]
    public abstract class InMemoryEntityBase : InMemoryEntityBase<int>
    {
    }

    [Serializable]
    public abstract class InMemoryEntityBase<TKey>
    {
        public abstract TKey Id { get; set; }

        public abstract TKey PartnerId { get; set; }
    }
}
