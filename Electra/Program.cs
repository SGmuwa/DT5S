using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;

namespace Electra
{
    class Program
    {
        static void Main(string[] args)
        {
            TPR_laba2_Electra.main(null);
            Console.ReadKey();
        }

        public struct CandidateForLove
        {
            /// <summary>
            /// Вес.
            /// </summary>
            public double Weight;
            /// <summary>
            /// Рост.
            /// </summary>
            public double Height;
            /// <summary>
            /// Количество хронических заболеваний.
            /// </summary>
            public ulong Diseases;
            /// <summary>
            /// Зарплата в месяц.
            /// </summary>
            public decimal Salary;
            /// <summary>
            /// Сколько человек знает человеческих языков.
            /// </summary>
            public ulong Languages;

            /// <summary>
            /// Создание кандидата для любви.
            /// </summary>
            /// <param name="weight">Вес.</param>
            /// <param name="height">Рост.</param>
            /// <param name="diseases">Количество хронических заболеваний</param>
            /// <param name="salary">Зарплата в месяц.</param>
            /// <param name="languages">Сколько знает человеческих языков.</param>
            public CandidateForLove(double weight, double height, ulong diseases, decimal salary, ulong languages)
            {
                Weight = weight;
                Height = height;
                Diseases = diseases;
                Salary = salary;
                Languages = languages;
            }
        }



        public class TPR_laba2_Electra
        {
            public static void main(string[] args)
            {
                ElementOfTable[,] matrix = new ElementOfTable[10, 10];
                TableOfExemplars exemplars = new TableOfExemplars(new List<Column>() {
                    /**/
                });
                List<CandidateForLove> list = new List<CandidateForLove>
                {
                    new CandidateForLove(10, 5, 8, 12, 12),
                    new CandidateForLove(5, 10, 8, 8, 8),
                    new CandidateForLove(15, 5, 12, 12, 8),
                    new CandidateForLove(15, 12, 12, 8, 12),
                    new CandidateForLove(10, 10, 12, 8, 8),
                    new CandidateForLove(5, 10, 12, 4, 8),
                    new CandidateForLove(15, 15, 12, 12, 12),
                    new CandidateForLove(10, 10, 12, 12, 12),
                    new CandidateForLove(15, 15, 8, 8, 12),
                    new CandidateForLove(15, 15, 12, 8, 12)
                };
                int weight_of_k1 = 5; int weight_of_k2 = 5; int weight_of_k3 = 4;
                int weight_of_k4 = 4; int weight_of_k5 = 4;
                for (int i = 0; i < 10; i++)
                    for (int j = 0; j < 10; j++) if (i == j) matrix[i, j] = "x";
                for (int i = 0; i < 10; i++)
                    for (int j = i + 1; j < 10; j++)
                    {
                        CandidateForLove Ai = list[i];
                        CandidateForLove Aj = list[j];
                        int Nij = 0; int Pij = 0; if (Ai.price < Aj.price) Pij += weight_of_k1;
                        else if (Ai.price > Aj.price) Nij += weight_of_k1;

                        if (Ai.clock_frequency > Aj.clock_frequency)
                            Pij += weight_of_k2;
                        else if (Ai.clock_frequency < Aj.clock_frequency)
                            Nij += weight_of_k2;
                        if (Ai.weight < Aj.weight) Pij += weight_of_k3;
                        else if (Ai.weight < Aj.weight) Nij += weight_of_k3;
                        if (Ai.hdd > Aj.hdd) Pij += weight_of_k4;
                        else if (Ai.hdd < Aj.hdd) Nij += weight_of_k4;
                        if (Ai.RAM > Aj.RAM) Pij += weight_of_k5;
                        else if (Ai.RAM < Aj.RAM) Nij += weight_of_k5;
                        float Dij = 0; float Dji = 0;
                        if (Pij != 0 & Nij != 0)
                        {
                            Dij = (float)(Pij) / (Nij);
                            if (Dij > 1)
                                matrix[i, j] = Dij + "";
                            else matrix[i, j] = "-";
                        }
                        else if (Pij == 0 & Nij != 0) matrix[i, j] = "-";
                        else if ((Pij != 0 | Pij == 0) & Nij == 0) matrix[i, j] = "inf";
                        if (Pij != 0 & Nij != 0)
                        {
                            Dji = 1 / Dij;
                            if (Dji > 1) matrix[j, i] = $"{Dji : 00.0}";
                            else matrix[j, i] = "-";
                        }
                        else if (Nij != 0 & Pij == 0) matrix[j, i] = "inf";
                        else if (Nij == 0 & Pij != 0) matrix[j, i] = "-";
                    }
                Console.WriteLine("Порог d = 1");
                Console.WriteLine(ToString(matrix));
                for (int i = 0; i < 10; i++)
                    for (int j = 0; j < 10; j++)
                    {
                        if (matrix[i, j].Equals("inf")) matrix[i, j] = "inf";
                        else if (matrix[i, j].Equals("x")) matrix[i, j] = "x";
                        else if (matrix[i, j].Equals("-")) matrix[i, j] = "-";
                        else if (matrix[i, j] < 1.7) matrix[i, j]
                          = "-";
                    }
                Console.WriteLine();
                Console.WriteLine("Порог c = 1.7");
                Console.WriteLine(ToString(matrix));
            }

            private static string ToString(ElementOfTable[,] array, string separatorX = " ", string separatorY = "\n")
            {
                StringBuilder sb = new StringBuilder();
                int max = 0;
                foreach (string n in array)
                    if (n.Length > max)
                        max = n.Length;
                Console.WriteLine("max = " + max);
                for (int i = 0; i < array.GetLength(0); i++)
                {
                    for (int j = 0; j < array.GetLength(1); j++)
                    {
                        sb.Append(StrWithLength(array[i, j], -max));
                        sb.Append(separatorX);
                    }
                    sb.Append(separatorY);
                }
                if (sb.Length > 0)
                    sb.Length -= separatorX.Length + separatorY.Length;
                return sb.ToString();

                static string StrWithLength(object toInsert, int len)
                    => string.Format(string.Format("{{0, {0}}}", len), toInsert.ToString());
            }
        }
    }
}
