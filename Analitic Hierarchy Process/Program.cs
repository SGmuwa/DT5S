using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Analitic_Hierarchy_Process
{
    class Program
    {
        public static void Main(string[] args)
        {
            int k = 5;
            Console.WriteLine("Матрица парных суждений");
            double[,] matrixA = {
                {1,   1,   2,   4,   5 },
                {NaN, 1,   3,   5,   5 },
                {NaN, NaN, 1,   3,   5 },
                {NaN, NaN, NaN, 1,   5 },
                {NaN, NaN, NaN, NaN, 1 },
            };
            for (int j = 0; j < k; j++)
                for (int i = j + 1; i < k; i++)
                {
                    matrixA[i, j] = 1 / matrixA[j, i];
                }
            for (int i = 0; i < k; i++)
            {
                for (int j = 0; j < k; j++)
                    Console.Write(matrixA[i, j] + "\t");
                Console.WriteLine();
            }
            double[] V = new double[5];
            double V1 = Math.Pow(matrixA[0, 0] * matrixA[0, 1] * matrixA[0, 2] * matrixA[0, 3] * matrixA[0, 4], 1.0 / 5);
            double V2 = Math.Pow(1 / matrixA[0, 1] * matrixA[1, 1] * matrixA[1, 2] * matrixA[1, 3] * matrixA[1, 4], 1.0 / 5);
            double V3 = Math.Pow(1 / matrixA[0, 2] * (1 / matrixA[1, 2]) * matrixA[2, 2] * matrixA[2, 3] * matrixA[2, 4], 1.0 / 5);
            double V4 = Math.Pow(1 / matrixA[0, 3] * (1 / matrixA[1, 3]) * (1 / matrixA[2, 3]) * matrixA[3, 3] * matrixA[3, 4], 1.0 / 5);
            double V5 = Math.Pow(1 / matrixA[0, 4] * (1 / matrixA[1, 4]) * (1 / matrixA[2, 4]) * (1 / matrixA[3, 4]) * matrixA[4, 4], 1.0 / 5);
            double sumVi = V1 + V2 + V3 + V4 + V5;
            double Wc1 = V1 / sumVi;
            double Wc2 = V2 / sumVi;
            double Wc3 = V3 / sumVi;
            double Wc4 = V4 / sumVi;
            double Wc5 = V5 / sumVi;
            Console.WriteLine($"Wci вектор приоритетов: {Wc1}, {Wc2}, {Wc3}, {Wc4}, {Wc5}.");
            double S1 = matrixA[0, 0] + (1 / matrixA[0, 1]) + (1 / matrixA[0, 2]) + (1 / matrixA[0, 3]) + (1 / matrixA[0, 4]);
            double S2 = matrixA[0, 1] + matrixA[1, 1] + (1 / matrixA[1, 2]) + (1 / matrixA[1, 3]) + (1 / matrixA[1, 4]);
            double S3 = matrixA[0, 2] + matrixA[1, 2] + matrixA[2, 2] + (1 / matrixA[2, 3]) + (1 / matrixA[2, 4]);
            double S4 = matrixA[0, 3] + matrixA[1, 3] + matrixA[2, 3] + matrixA[3, 3] + (1 / matrixA[3, 4]);
            double S5 = matrixA[0, 4] + matrixA[1, 4] + matrixA[2, 4] + matrixA[3, 4] + matrixA[4, 4];
            double P1 = S1 * Wc1;
            double P2 = S2 * Wc2;
            double P3 = S3 * Wc3;
            double P4 = S4 * Wc4;
            double P5 = S5 * Wc5;
            double Ymax = P1 + P2 + P3 + P4 + P5;
            double IS = (Ymax - k) / (k - 1);
            double OS = IS / SI[k];
            Console.WriteLine($"Максимальное среднее значение: {Ymax}\n" +
                $"Отношение согласованности: {OS}\n\n");

            double[][,] matrixK = {
                new double[,] // K0
                {
                    {1, NaN, NaN, NaN, NaN },
                    {3, 1,   NaN, NaN, NaN },
                    {2, 3,   1,   NaN, NaN },
                    {3, 5,   3,   1,   NaN },
                    {3, 2,   2,   3,   1   },
                },
                new double[,] // К1
                {
                    {1, NaN, NaN, NaN, NaN },
                    {1, 1,   NaN, NaN, NaN },
                    {3, 3,   1,   NaN, NaN },
                    {2, 2,   3,   1,   NaN },
                    {2, 2,   3,   2,   1   },
                },
                new double[,] // К2
                {
                    {1, NaN, NaN, NaN, NaN },
                    {3, 1,   NaN, NaN, NaN },
                    {1, 3,   1,   NaN, NaN },
                    {3, 1,   3,   1,   NaN },
                    {3, 1,   3,   1,   1   },
                },
                new double[,] // К3
                {
                    {1, NaN, NaN, NaN, NaN },
                    {1, 1,   NaN, NaN, NaN },
                    {1, 1,   1,   NaN, NaN },
                    {3, 2,   3,   1,   NaN },
                    {1, 1,   1,   2,   1   },
                },
                new double[,] // К4
                {
                    {1, NaN, NaN, NaN, NaN },
                    {1, 1,   NaN, NaN, NaN },
                    {1, 1,   1,   NaN, NaN },
                    {1, 3,   3,   1,   NaN },
                    {1, 1,   1,   3,   1   },
                }
            };
            double[][] listK = new double[k][];
            for (int K = 0; K < k; K++)
            {
                Console.WriteLine($"Критерий К{K}\n\n");
                for (int i = 0; i < k; i++)
                    for (int j = i + 1; j < k; j++)
                    {
                        matrixK[K][i, j] = 1 / matrixK[K][j, i];
                    }
                listK[K] = SearchOSWa(matrixK[K]);
            }
            double[] W = new double[k];
            double[] Wc = { Wc1, Wc2, Wc3, Wc4, Wc5 };
            for(int i = 0; i < k; i++)
                for(int j = 0; j < k; j++)
                    W[i] += Wc[j] * listK[j][i];
            Console.WriteLine($"\n\nW (вектор приоритетов): {string.Join(", ", W)}");
        }

        /// <summary>
        /// Поиск векторы приоритетов.
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static double[] SearchOSWa(double[,] array)
        {
            if (array.GetLength(0) != array.GetLength(1) || array.Rank != 2)
                throw new ArgumentException();
            int k = array.GetLength(0);
            double[] V = new double[k];
            // Вычисление вектора приоритетов по данной матрице по каждой строке.
            for (int i = 0; i < k; i++)
            {
                V[i] = 1;
                for (int j = 0; j < k; j++)
                    V[i] *= array[i, j];
                V[i] = Math.Pow(V[i], 1.0 / k);
            }
            // Нормирующий коэффициент.

            double sumVi = V.Sum();
            double[] Wa = new double[k];
            for (int i = 0; i < k; i++)
                Wa[i] = V[i] / sumVi;
            Console.WriteLine($"Вектор приоритетов Wki:\nWki (вектор приоритетов): {string.Join(", ", Wa)}");
            double[] S = new double[k];
            for (int i = 0; i < k; i++)
                for (int j = 0; j < k; j++)
                    S[j] += array[i, j];
            double[] P = new double[k];
            for (int i = 0; i < k; i++)
                P[i] = S[i] * Wa[i];
            double Ymax = P.Sum();

            double IS = (Ymax - k) / (k - 1);
            double OS = IS / SI[k]; // Отклонение от согласованности (Индекс Согласованности (ИС))
            Console.WriteLine($"Ymax (максимальное среднее значение) = {Ymax}\n" +
                $"ОС = {OS}"); // Отношение согласованности (OC)
            if (OS > OSok)
                Console.WriteLine($"OS = {OS}. Expected {OSok} or less.");
            return Wa;
        }

        private static readonly double NaN = double.NaN;
        /// <summary>
        /// Среднее значение индекса случайной согласованности.
        /// Для определения того, насколько точно ИС отражает согласованность
        /// суждений его необходимо сравнить со случайным индексом (СИ)
        /// согласованности, который соответствует матрице со случайными суждениями,
        /// выбранными из данной шкалы.
        /// </summary>
        private static readonly ReadOnlyDictionary<int, double> SI =
            new ReadOnlyDictionary<int, double>(
                new Dictionary<int, double>()
                {
                    [1] = 0.00, [2] = 0.00, [3] = 0.58, [4] = 0.90, [5] = 1.12, [6] = 1.24, [7] = 1.32, [8] = 1.41, [9] = 1.45, [10] = 1.49, [11] = 1.51, [12] = 1.48, [13] = 1.56, [14] = 1.57, [15] = 1.59
                });

        private const double OSok = 0.1;
    }
}
