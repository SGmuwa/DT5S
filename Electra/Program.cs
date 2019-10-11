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

        public class Data_computers
        {
            public int price;
            public double weight;
            public int hdd;
            public double clock_frequency;
            public int RAM;
            public Data_computers(int price, double weight, int hdd, double clock_frequency, int RAM)
            {
                this.price = price;
                this.weight = weight;
                this.hdd = hdd;
                this.clock_frequency = clock_frequency;
                this.RAM = RAM;
            }
        }

        public class TPR_laba2_Electra
        {
            public static void main(string[] args)
            {
                string[, ] matrix;
                matrix = new string[10, 10];
                List<Data_computers> list = new List<Data_computers>();
                list.Add(new Data_computers(10, 5, 8, 12, 12));
                list.Add(new Data_computers(5, 10, 8, 8, 8));
                list.Add(new Data_computers(15, 5, 12, 12, 8));
                list.Add(new Data_computers(15, 12, 12, 8, 12));
                list.Add(new Data_computers(10, 10, 12, 8, 8));
                list.Add(new Data_computers(5, 10, 12, 4, 8));
                list.Add(new Data_computers(15, 15, 12, 12, 12));
                list.Add(new Data_computers(10, 10, 12, 12, 12));
                list.Add(new Data_computers(15, 15, 8, 8, 12));
                list.Add(new Data_computers(15, 15, 12, 8, 12));
                int weight_of_k1 = 5; int weight_of_k2 = 5; int weight_of_k3 = 4;
                int weight_of_k4 = 4; int weight_of_k5 = 4;
                for (int i = 0; i < 10; i++)
                    for (int j = 0; j < 10; j++) if (i == j) matrix[i, j] = "x";
                for (int i = 0; i < 10; i++)
                    for (int j = i + 1; j < 10; j++)
                    {
                        Data_computers Ai = list[i];
                        Data_computers Aj = list[j];
                        int Nij = 0; int Pij = 0; bool flag = true;
                        if (Ai.price < Aj.price) Pij += weight_of_k1;
                        else if (Ai.price > Aj.price) Nij += weight_of_k1;

                        if (Ai.clock_frequency > Aj.clock_frequency) Pij += weight_of_k2;
                        else if (Ai.clock_frequency < Aj.clock_frequency) Nij += weight_of_k2;
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
                            Dji = (float)1 / Dij;
                            if (Dji > 1) matrix[j, i] = Dji + "";
                            else matrix[j, i] = "-";
                        }
                        else if (Nij != 0 & Pij == 0) matrix[j, i] = "inf";
                        else if (Nij == 0 & Pij != 0) matrix[j, i] = "-";
                    }
                Console.WriteLine("Порог d = 1");
                for (int i = 0; i < 10; i++)
                {
                    for (int j = 0; j < 10; j++)
                    {
                        Console.Write(matrix[i, j] + "\t" + "\t");
                    }
                    Console.WriteLine();
                }
                Console.WriteLine();
                for (int i = 0; i < 10; i++)
                    for (int j = 0; j < 10; j++)
                    {
                        if (matrix[i, j].Equals("inf")) matrix[i, j] = "inf";
                        else if (matrix[i, j].Equals("x")) matrix[i, j] = "x";
                        else if (matrix[i, j].Equals("-")) matrix[i, j] = "-";
                        else if (float.Parse(matrix[i, j]) < 1.7) matrix[i, j] = "-";
                    }
                Console.WriteLine();
                Console.WriteLine("Порог c = 1.7");
                for (int i = 0; i < 10; i++)
                {
                    for (int j = 0; j < 10; j++)
                    {
                        Console.Write(matrix[i, j] + "\t" + "\t");
                    }
                    Console.WriteLine();
                }
            }
        }
    }
}
