using Microsoft.VisualStudio.TestTools.UnitTesting;
using Electra;
using System;
using System.Collections.Generic;
using System.Text;

namespace Electra.Tests
{
    [TestClass()]
    public class ProgramTests
    {
        [TestMethod()]
        public void ElectraPupysTest()
        {
            Column price = new Column("Цена", 5, false);
            Column weight = new Column("Вес", 4, false);
            Column HDD = new Column("Ёмкость HDD", 4, true);
            Column clock_frequency = new Column("Тактовая частота", 5, true);
            Column RAM = new Column("ОП", 4, true);
            List<Exemplar> exList = new List<Exemplar>(){
                    new Exemplar("0 комп", new Dictionary<Column, double>()
                    { [price] = 10, [weight] = 5, [HDD] = 8, [clock_frequency] = 12, [RAM] = 12 }),
                    new Exemplar("1 комп", new Dictionary<Column, double>()
                    { [price] = 5, [weight] = 10, [HDD] = 8, [clock_frequency] = 8, [RAM] = 8 }),
                    new Exemplar("2 комп", new Dictionary<Column, double>()
                    { [price] = 15, [weight] = 5, [HDD] = 12, [clock_frequency] = 12, [RAM] = 8 }),
                    new Exemplar("3 комп", new Dictionary<Column, double>()
                    { [price] = 15, [weight] = 12, [HDD] = 12, [clock_frequency] = 8, [RAM] = 12 }),
                    new Exemplar("4 комп", new Dictionary<Column, double>()
                    { [price] = 10, [weight] = 10, [HDD] = 12, [clock_frequency] = 8, [RAM] = 8 }),
                    new Exemplar("5 комп", new Dictionary<Column, double>()
                    { [price] = 5, [weight] = 10, [HDD] = 12, [clock_frequency] = 4, [RAM] = 8 }),
                    new Exemplar("6 комп", new Dictionary<Column, double>()
                    { [price] = 15, [weight] = 15, [HDD] = 12, [clock_frequency] = 12, [RAM] = 12 }),
                    new Exemplar("7 комп", new Dictionary<Column, double>()
                    { [price] = 10, [weight] = 10, [HDD] = 12, [clock_frequency] = 12, [RAM] = 12 }),
                    new Exemplar("8 комп", new Dictionary<Column, double>()
                    { [price] = 15, [weight] = 15, [HDD] = 8, [clock_frequency] = 8, [RAM] = 12 }),
                    new Exemplar("9 комп", new Dictionary<Column, double>()
                    { [price] = 15, [weight] = 15, [HDD] = 12, [clock_frequency] = 8, [RAM] = 12 })
                };
            TableOfExemplars exemplars = new TableOfExemplars(new List<Column>() {
                    price, weight, HDD, clock_frequency, RAM
                });
            exemplars.AddRange(exList);
            TableOfResults res = StaticTools.ElectraMethod(exemplars);
            res.DecisionThreshold = 1;
            Console.WriteLine("Порог = 1");
            Console.WriteLine(res);
            Console.WriteLine();
            res.DecisionThreshold = 1.7;
            Console.WriteLine("Порог = 1.7");
            Console.WriteLine(res.ToString());

            MyAssert(0, 1, 2.6);
            MyAssert(0, 2, 2.25);
            MyAssert(0, 3, 3.5);
            MyAssert(0, 4, 3.25);
            MyAssert(0, 6, 2.25);
            MyAssert(0, 8, double.PositiveInfinity);
            MyAssert(0, 9, 3.5);

            MyAssert(1, 8, 2.25);

            MyAssert(2, 1, 2.6);
            MyAssert(2, 3, 2.25);
            MyAssert(2, 4, 1.8);
            MyAssert(2, 5, 1.8);

            void MyAssert(int a, int b, double expect)
                => Assert.AreEqual(expect, res[exList[a], exList[b]].Value, 0.01);
        }

        [TestMethod()]
        public void ElectraSorokTest()
        {
            Console.WriteLine("Begin.");
            Column price = new Column("Цена квартиры", 5, false);
            Column s = new Column("Площадь квартиры", 5, true);
            Column l = new Column("Удалённость от метро", 4, true);
            Column rate = new Column("Рейтинг района", 4, true);
            Column q = new Column("Качество состояния квартиры", 4, true);
            Column wc = new Column("Санузел", 2, true);
            List<Exemplar> exList = new List<Exemplar>(){
                    new Exemplar("Ясенево", new Dictionary<Column, double>()
                    { [price] = 10, [s] = 5, [l] = 12, [rate] = 10, [q] = 8, [wc] = 4 }),
                    new Exemplar("Медведково", new Dictionary<Column, double>()
                    { [price] = 5, [s] = 15, [l] = 4, [rate] = 10, [q] = 8, [wc] = 4 }),
                    new Exemplar("Солнцево", new Dictionary<Column, double>()
                    { [price] = 15, [s] = 5, [l] = 4, [rate] = 8, [q] = 4, [wc] = 2 }),
                    new Exemplar("Северное Бутово", new Dictionary<Column, double>()
                    { [price] = 5, [s] = 5, [l] = 12, [rate] = 10, [q] = 4, [wc] = 4 }),
                    new Exemplar("Марьино", new Dictionary<Column, double>()
                    { [price] = 15, [s] = 10, [l] = 12, [rate] = 10, [q] = 12, [wc] = 4 }),
                    new Exemplar("Восточное Бирюлёво", new Dictionary<Column, double>()
                    { [price] = 10, [s] = 15, [l] = 4, [rate] = 8, [q] = 12, [wc] = 2 }),
                    new Exemplar("Гольяново", new Dictionary<Column, double>()
                    { [price] = 15, [s] = 5, [l] = 8, [rate] = 8, [q] = 4, [wc] = 2 }),
                    new Exemplar("Новокосино", new Dictionary<Column, double>()
                    { [price] = 10, [s] = 10, [l] = 12, [rate] = 4, [q] = 4, [wc] = 2 }),
                    new Exemplar("Митино", new Dictionary<Column, double>()
                    { [price] = 5, [s] = 15, [l] = 12, [rate] = 10, [q] = 8, [wc] = 4 })

                };
            TableOfExemplars exemplars = new TableOfExemplars(new List<Column>() {
                    price, s, l, rate, q, wc
                });
            exemplars.AddRange(exList);
            Console.WriteLine(exemplars.ToString());
            TableOfResults res = StaticTools.ElectraMethod(exemplars);
            res.DecisionThreshold = 1;
            Console.WriteLine("Порог = 1");
            Console.WriteLine(res);
            Console.WriteLine();
            res.DecisionThreshold = 1.7;
            Console.WriteLine("Порог = 1.7");
            Console.WriteLine(res.ToString());
            res.DecisionThreshold = 0;
            MyAssert(1, 2, 0.4);
            MyAssert(1, 6, 1.11);
            MyAssert(1, 8, 2); // ! У Сорокина здесь 1.6, он забыл санузел.
            MyAssert(2, 1, 2.5);
            MyAssert(2, 4, 2.25);
            MyAssert(2, 5, 1.25);
            MyAssert(2, 6, 2.75);
            MyAssert(2, 7, 5);
            // MyAssert(2, 8, 2.25); // ! М = 5 + 5 + 4 + 4 + 2 = 20, Н = 4, 20 / 4 = 5
            MyAssert(4, 1, 1.25);
            MyAssert(4, 6, 1.66);
            // MyAssert(4, 8, 1.8); // !
            MyAssert(8, 3, 3.5);
            // MyAssert(8, 7, 4); // !
            // MyAssert(9, 1, 5); // !
            // MyAssert(9, 2, 2); // !
            // MyAssert(9, 4, 4.5); // !
            // MyAssert(9, 5, 1.66); // !
            // MyAssert(9, 6, 3.25); // !
            // MyAssert(9, 8, 9); // !

            void MyAssert(int a, int b, double expect)
                => Assert.AreEqual(expect, res[exList[a - 1], exList[b - 1]].Value, 0.01);
        }
    }
}