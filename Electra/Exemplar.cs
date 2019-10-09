using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Electra
{
    class Exemplar : IReadOnlyDictionary<Column, double>
    {
        private readonly ReadOnlyDictionary<Column, double> Values;

        public Exemplar(Dictionary<Column, double> values)
        {
            if (values == null) throw new ArgumentNullException();
            Values = new ReadOnlyDictionary<Column, double>(values);
        }

        public double this[Column key] => Values[key];

        public IEnumerable<Column> Keys => ((IReadOnlyDictionary<Column, double>)Values).Keys;

        public int Count => Values.Count;

        IEnumerable<double> IReadOnlyDictionary<Column, double>.Values => ((IReadOnlyDictionary<Column, double>)Values).Values;

        public bool ContainsKey(Column key)
        {
            return Values.ContainsKey(key);
        }

        public IEnumerator<KeyValuePair<Column, double>> GetEnumerator()
        {
            return Values.GetEnumerator();
        }

        public bool TryGetValue(Column key, [MaybeNullWhen(false)] out double value)
        {
            return Values.TryGetValue(key, out value);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Values.GetEnumerator();
        }
    }
}
