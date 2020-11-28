using System;
using System.Collections.Generic;
using System.Linq;

namespace Control.Lens
{
    public class Lens<O, P>
    {
        public Func<O, P> Get { get; }

        public Func<P, Func<O, O>> Set { get; }

        public Lens(Func<O, P> get, Func<P, Func<O, O>> set)
        {
            Get = get;
            Set = set;
        }
    }

    public static class Create
    {
        public static Lens<O, Q> Compose<O, P, Q>(
            this Lens<O, P> op,
            Lens<P, Q> pq)
            => new Lens<O, Q>(
                o => pq.Get(op.Get(o)),
                q => o => op.Set(pq.Set(q)(op.Get(o)))(o));

        public static Lens<IEnumerable<O>, O> Pos<O>(int i)
            => new Lens<IEnumerable<O>, O>(
                l => l.ElementAt(i),
                o => l => l.Take(i).Append(o).Concat(l.Skip(i + 1)));
    }
}
