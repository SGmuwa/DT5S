using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Text;

namespace Electra
{
    class Table : IEnumerable<Exemplar>, IEnumerable
    {
        public readonly ReadOnlyCollection<Column> Columns;

        private readonly List<Exemplar> Exemplares = new List<Exemplar>();

        public Table(IEnumerable<Column> columns)
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
                sb.Append(StrWithLength(n, max));
                sb.Append(' ');
            }
            sb.Length--;
            foreach(var n in this)
            {
                foreach(var c in Columns)
                {
                    sb.Append(StrWithLength(n[c], max));
                    sb.Append(' ');
                }
                sb.Length--;
                sb.Append('\n');
            }
            sb.Length--;
            return sb.ToString();


            static string StrWithLength(object toInsert, int len)
                    => string.Format(string.Format("{{0, {0}}}", len), toInsert.ToString());
        }

        public IEnumerator<Exemplar> GetEnumerator() => Exemplares.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => Exemplares.GetEnumerator();
    }
}
