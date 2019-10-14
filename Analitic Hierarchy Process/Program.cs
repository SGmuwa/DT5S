using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Analitic_Hierarchy_Process
{
    class Program
    {
        public static void Main()
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
                    matrixA[i, j] = 1 / matrixA[j, i];
            Console.WriteLine(matrixA.TableToString());
            double[] Wc = SearchOSWa(matrixA);

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
                Console.WriteLine($"Критерий К{K}");
                for (int i = 0; i < k; i++)
                    for (int j = i + 1; j < k; j++)
                    {
                        matrixK[K][i, j] = 1 / matrixK[K][j, i];
                    }
                Console.WriteLine(matrixK[K].TableToString());
                listK[K] = SearchOSWa(matrixK[K]);
            }
            double[] W = new double[k];
            for(int i = 0; i < k; i++)
                for(int j = 0; j < k; j++)
                    W[i] += Wc[j] * listK[j][i];
            Console.WriteLine($"\n\nW (вектор приоритетов): {string.Join(", ", W)}");
        }

        /// <summary>
        /// Поиск векторов приоритетов.
        /// </summary>
        /// <returns>Приоритеты.</returns>
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
            if (OS > OSok)
                Console.WriteLine($"Отношение согласованности = {OS}. Ожидалось {OSok} или меньше.");
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
