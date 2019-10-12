using System;
using System.Collections.Generic;

namespace Analitic_Hierarchy_Process
{
    class Program
    {
        public static void Main(string[] args)
        {
            int n = 5;
            Console.WriteLine("Матрица парных суждений");
            double[,] matrix1 = {
                {1,   1,   2,   4,   5 },
                {NaN, 1,   3,   5,   5 },
                {NaN, NaN, 1,   3,   5 },
                {NaN, NaN, NaN, 1,   5 },
                {NaN, NaN, NaN, NaN, 1 },
            };
            for (int j = 0; j < 5; j++)
                for (int i = j + 1; i < 5; i++)
                {
                    matrix1[i, j] = 1 / matrix1[j, i];
                }
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                    Console.Write(matrix1[i, j] + "\t");
                Console.WriteLine();
            }
            double V1 = Math.Pow(matrix1[0, 0] * matrix1[0, 1] * matrix1[0, 2] * matrix1[0, 3] * matrix1[0, 4], 1.0 / 5);
            double V2 = Math.Pow(1 / matrix1[0, 1] * matrix1[1, 1] * matrix1[1, 2] * matrix1[1, 3] * matrix1[1, 4], 1.0 / 5);
            double V3 = Math.Pow(1 / matrix1[0, 2] * (1 / matrix1[1, 2]) * matrix1[2, 2] * matrix1[2, 3] * matrix1[2, 4], 1.0 / 5);
            double V4 = Math.Pow(1 / matrix1[0, 3] * (1 / matrix1[1, 3]) * (1 / matrix1[2, 3]) * matrix1[3, 3] * matrix1[3, 4], 1.0 / 5);
            double V5 = Math.Pow(1 / matrix1[0, 4] * (1 / matrix1[1, 4]) * (1 / matrix1[2, 4]) * (1 / matrix1[3, 4]) * matrix1[4, 4], 1.0 / 5);
            double sumVi = V1 + V2 + V3 + V4 + V5;
            double Wc1 = V1 / sumVi;
            double Wc2 = V2 / sumVi;
            double Wc3 = V3 / sumVi;
            double Wc4 = V4 / sumVi;
            double Wc5 = V5 / sumVi;
            Console.WriteLine($"Wci вектор приоритетов: {Wc1}, {Wc2}, {Wc3}, {Wc4}, {Wc5}.");
            double S1 = matrix1[0, 0] + (1 / matrix1[0, 1]) + (1 / matrix1[0, 2]) + (1 / matrix1[0, 3]) + (1 / matrix1[0, 4]);
            double S2 = matrix1[0, 1] + matrix1[1, 1] + (1 / matrix1[1, 2]) + (1 / matrix1[1, 3]) + (1 / matrix1[1, 4]);
            double S3 = matrix1[0, 2] + matrix1[1, 2] + matrix1[2, 2] + (1 / matrix1[2, 3]) + (1 / matrix1[2, 4]);
            double S4 = matrix1[0, 3] + matrix1[1, 3] + matrix1[2, 3] + matrix1[3, 3] + (1 / matrix1[3, 4]);
            double S5 = matrix1[0, 4] + matrix1[1, 4] + matrix1[2, 4] + matrix1[3, 4] + matrix1[4, 4];
            double P1 = S1 * Wc1;
            double P2 = S2 * Wc2;
            double P3 = S3 * Wc3;
            double P4 = S4 * Wc4;
            double P5 = S5 * Wc5;
            double Ymax = P1 + P2 + P3 + P4 + P5;
            double IS = (Ymax - n) / (n - 1);
            double OS = IS / SI;
            Console.WriteLine($"Максимальное среднее значение: {Ymax}\n" +
                $"Отношение согласованности: {OS}\n\n");

            ///Критерий K1
            Console.WriteLine("Критерий К1\n\n");
            double[,] matrixK1 = {
                {1, NaN, NaN, NaN, NaN},
                {3, 1,   NaN, NaN, NaN},
                {2, 3,   1,   NaN, NaN},
                {3, 5,   3,   1,   NaN,},
                {3, 2,   2,   3,   1},
            };

            for (int i = 0; i < 5; i++)
                for (int j = i + 1; j < 5; j++)
                {
                    matrixK1[i, j] = 1 / matrixK1[j, i];
                }
            List<double> listK1 = Function(matrixK1, 5);

            //Критерий К2
            Console.WriteLine("\nКритерий К2");
            double[,] matrixK2 = {
                {1, NaN, NaN, NaN, NaN },
                {1, 1,   NaN, NaN, NaN },
                {3, 3,   1,   NaN, NaN },
                {2, 2,   3,   1,   NaN },
                {2, 2,   3,   2,   1   },
            };
            for (int i = 0; i < 5; i++)
                for (int j = i + 1; j < 5; j++)
                {
                    matrixK2[i, j] = 1 / matrixK2[j, i];
                }
            List<double> listK2 = Function(matrixK2, 5);

            ///Критерий K3
            Console.WriteLine("\nКритерий К3");
            double[,] matrixK3 = {
                {1, NaN, NaN, NaN, NaN },
                {3, 1,   NaN, NaN, NaN },
                {1, 3,   1,   NaN, NaN },
                {3, 1,   3,   1,   NaN },
                {3, 1,   3,   1,   1   },
            };

            for (int i = 0; i < 5; i++)
                for (int j = i + 1; j < 5; j++)
                    matrixK3[i, j] = 1 / matrixK3[j, i];

            List<double> listK3 = Function(matrixK3, 5);
            ///Критерий K4
            Console.WriteLine("\nКритерий K4");
            double[,] matrixK4 = {
                {1, NaN, NaN, NaN, NaN },
                {1, 1,   NaN, NaN, NaN },
                {1, 1,   1,   NaN, NaN },
                {3, 2,   3,   1,   NaN },
                {1, 1,   1,   2,   1   },
            };
            for (int i = 0; i < 5; i++)
                for (int j = i + 1; j < 5; j++)
                    matrixK4[i, j] = 1 / matrixK4[j, i];
            List<double> listK4 = Function(matrixK4, 5);
            ///Критерий K5
            Console.WriteLine("\nКритерий K5");
            double[,] matrixK5 = {
                {1, NaN, NaN, NaN, NaN },
                {1, 1, NaN, NaN, NaN },
                {1, 1, 1, NaN, NaN },
                {1, 3, 3, 1, NaN },
                {1, 1, 1, 3, 1},
            };
            for (int i = 0; i < 5; i++)
                for (int j = i + 1; j < 5; j++)
                    matrixK5[i, j] = 1 / matrixK5[j, i];
            List<double> listK5 = Function(matrixK5, 5);
            double W1 = Wc1 * listK1[1] + Wc2 * listK2[1] + Wc3 * listK3[1] + Wc4 * listK4[1] + Wc5 * listK5[1];
            double W2 = Wc1 * listK1[2] + Wc2 * listK2[2] + Wc3 * listK3[2] + Wc4 * listK4[2] + Wc5 * listK5[2];
            double W3 = Wc1 * listK1[3] + Wc2 * listK2[3] + Wc3 * listK3[3] + Wc4 * listK4[3] + Wc5 * listK5[3];
            double W4 = Wc1 * listK1[4] + Wc2 * listK2[4] + Wc3 * listK3[4] + Wc4 * listK4[4] + Wc5 * listK5[4];
            double W5 = Wc1 * listK1[5] + Wc2 * listK2[5] + Wc3 * listK3[5] + Wc4 * listK4[5] + Wc5 * listK5[5];
            Console.WriteLine($"\n\nW (вектор приоритетов): {W1}, {W2}, {W3}, {W4}, {W5}");
        }

        public static List<double> Function(double[,] array, int k)
        {
            double V1 = 1;
            double V2 = 1;
            double V3 = 1;
            double V4 = 1;
            double V5 = 1;
            // Вычисление вектора приоритетов по данной матрице по каждой строке.
            for (int i = 0; i < k; i++)
                for (int j = 0; j < k; j++)
                {
                    if (i == 0) V1 *= array[i, j];
                    if (i == 1) V2 *= array[i, j];
                    if (i == 2) V3 *= array[i, j];
                    if (i == 3) V4 *= array[i, j];
                    if (i == 4) V5 *= array[i, j];
                }
            V1 = Math.Pow(V1, 1.0 / 5);
            V2 = Math.Pow(V2, 1.0 / 5);
            V3 = Math.Pow(V3, 1.0 / 5);
            V4 = Math.Pow(V4, 1.0 / 5);
            V5 = Math.Pow(V5, 1.0 / 5);
            // Нормирующий коэффициент.
            double sumVi = V1 + V2 + V3 + V4 + V5;
            double Wa1 = V1 / sumVi;
            double Wa2 = V2 / sumVi;
            double Wa3 = V3 / sumVi;
            double Wa4 = V4 / sumVi;
            double Wa5 = V5 / sumVi;
            Console.WriteLine($"Вектор приоритетов Wki:\nWki (вектор приоритетов): {Wa1}, {Wa2}, {Wa3}, {Wa4}, {Wa5}");
            double S1 = 0, S2 = 0, S3 = 0, S4 = 0, S5 = 0;
            for (int i = 0; i < k; i++)
                for (int j = 0; j < k; j++)
                {
                    if (j == 0) S1 += array[i, j];
                    if (j == 1) S2 += array[i, j];
                    if (j == 2) S3 += array[i, j];
                    if (j == 3) S4 += array[i, j];
                    if (j == 4) S5 += array[i, j];
                }
            double Р1 = S1 * Wa1;
            double Р2 = S2 * Wa2;
            double Р3 = S3 * Wa3;
            double P4 = S4 * Wa4;
            double Р5 = S5 * Wa5;
            double Ymax = Р1 + Р2 + Р3 + P4 + Р5;

            double IS = (Ymax - k) / (k - 1);
            double OS = IS / SI; // Отклонение от согласованности (Индекс Согласованности (ИС))
            Console.WriteLine($"Ymax (максимальное среднее значение) = {Ymax}\n" +
                $"ОС = {OS}"); // Отношение согласованности (OC)
            return new List<double> { OS, Wa1, Wa2, Wa3, Wa4, Wa5 };
        }

        private static readonly double NaN = double.NaN;
        private static readonly double SI = 1.12;
    }
}
