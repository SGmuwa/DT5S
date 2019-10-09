using System;

namespace Electra
{
    /// <summary>
    /// Критерий.
    /// </summary>
    public struct Column
    {
        /// <summary>
        /// Имя критерия.
        /// </summary>
        public string Name;
        /// <summary>
        /// Вес критерия.
        /// </summary>
        public double Weight;
        /// <summary>
        /// True, если чем больше показатель критерия, тем лучше. False, если лучше меньше.
        /// Например, True - личная зарплата. False - количество личных неудач.
        /// </summary>
        public bool isBetterMany;

        public bool Equals(Column other)
            => Name.Equals(other.Name)
            && Weight.Equals(other.Weight)
            && isBetterMany.Equals(other.isBetterMany);

        public override bool Equals(object obj)
        {
            if (obj is Column)
                return Equals((Column)obj);
            else
                throw new InvalidCastException();
        }

        public override int GetHashCode()
            => Name.GetHashCode()
            ^ Weight.GetHashCode()
            ^ isBetterMany.GetHashCode();

        public override string ToString()
            => $"{Name} ({Weight, 0}, {(isBetterMany ? "+" : "-")})";
    }
}
