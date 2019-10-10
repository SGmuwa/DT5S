using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;

namespace Electra
{
    public static class Program
    {
        public static void Main()
        {
            Column price = new Column("Цена", 5, false);
            Column weight = new Column("Вес", 5, false);
            Column HDD = new Column("Ёмкость HDD", 4, true);
            Column clock_frequency = new Column("Тактовая частота", 4, true);
            Column RAM = new Column("ОП", 4, true);
            TableOfExemplars exemplars = new TableOfExemplars(new List<Column>() {
                    price, weight, HDD, clock_frequency, RAM
                })
                {
                    new Exemplar("1 комп", new Dictionary<Column, double>()
                    { [price] = 10, [weight] = 5, [HDD] = 8, [clock_frequency] = 12, [RAM] = 12 }),
                    new Exemplar("2 комп", new Dictionary<Column, double>()
                    { [price] = 5, [weight] = 10, [HDD] = 8, [clock_frequency] = 8, [RAM] = 8 }),
                    new Exemplar("3 комп", new Dictionary<Column, double>()
                    { [price] = 15, [weight] = 5, [HDD] = 12, [clock_frequency] = 12, [RAM] = 8 }),
                    new Exemplar("4 комп", new Dictionary<Column, double>()
                    { [price] = 15, [weight] = 12, [HDD] = 12, [clock_frequency] = 8, [RAM] = 12 }),
                    new Exemplar("5 комп", new Dictionary<Column, double>()
                    { [price] = 10, [weight] = 10, [HDD] = 12, [clock_frequency] = 8, [RAM] = 8 }),
                    new Exemplar("6 комп", new Dictionary<Column, double>()
                    { [price] = 5, [weight] = 10, [HDD] = 12, [clock_frequency] = 4, [RAM] = 8 }),
                    new Exemplar("7 комп", new Dictionary<Column, double>()
                    { [price] = 15, [weight] = 15, [HDD] = 12, [clock_frequency] = 12, [RAM] = 12 }),
                    new Exemplar("8 комп", new Dictionary<Column, double>()
                    { [price] = 10, [weight] = 10, [HDD] = 12, [clock_frequency] = 12, [RAM] = 12 }),
                    new Exemplar("9 комп", new Dictionary<Column, double>()
                    { [price] = 15, [weight] = 15, [HDD] = 8, [clock_frequency] = 8, [RAM] = 12 }),
                    new Exemplar("10 комп", new Dictionary<Column, double>()
                    { [price] = 15, [weight] = 15, [HDD] = 12, [clock_frequency] = 8, [RAM] = 12 })
                };
            var res = StaticTools.ElectraMethod(exemplars);
            res.DecisionThreshold = 1;
            Console.WriteLine("Порог = 1");
            Console.WriteLine(res);
            Console.WriteLine();
            res.DecisionThreshold = 1.7;
            Console.WriteLine("Порог = 1.7");
            Console.WriteLine(res.ToString());

            Console.ReadKey();
        }
    }
}
