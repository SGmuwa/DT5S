﻿using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Electra
{
    class TableOfExemplars : IEnumerable<Exemplar>, IEnumerable
    {
        public readonly ReadOnlyCollection<Column> Columns;

        private readonly HashSet<Exemplar> Exemplares = new HashSet<Exemplar>();

        public TableOfExemplars(IEnumerable<Column> columns)
        {
            Columns = new ReadOnlyCollection<Column>(new List<Column>(columns));
        }

        public void Add(Exemplar exemplar)
        {
            foreach (var a in Columns)
                if (exemplar.ContainsKey(a) == false)
                    throw new ArgumentException();
            Exemplares.Add(exemplar);
        }

        public IEnumerable<KeyValuePair<Exemplar, Exemplar>> Join() =>
            Exemplares.Join(Exemplares, _ => _, _ => _, (a, b) => new KeyValuePair<Exemplar, Exemplar>(a, b));

        public override string ToString()
        {
            int max = 0;
            foreach (var n in Columns)
                if (n.ToString().Length > max)
                    max = n.ToString().Length;
            foreach (var n in Exemplares)
                foreach (var col in n)
                    if (col.Value.ToString().Length > max)
                        max = col.Value.ToString().Length;
            StringBuilder sb = new StringBuilder();
            foreach (var n in Columns)
            {
                sb.Append(n.ToString(max));
                sb.Append(' ');
            }
            sb.Length--;
            foreach(var n in this)
            {
                foreach(var c in Columns)
                {
                    sb.Append(n[c].ToString(max));
                    sb.Append(' ');
                }
                sb.Length--;
                sb.Append('\n');
            }
            sb.Length--;
            return sb.ToString();
        }

        public IEnumerator<Exemplar> GetEnumerator() => Exemplares.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => Exemplares.GetEnumerator();
    }
}