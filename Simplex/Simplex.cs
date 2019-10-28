using System;
using System.Collections.Generic;

namespace Simplex
{
    /// <summary>
    /// Реализация симплекса.
    /// </summary>
    public static class Simplex
    {
        /// <summary>
        /// Запускает завимодействие пользователя с симплекс-методом.
        /// </summary>
        public static void UI()
        {
            int perem = 2; // Переменных 2
            int N = 4; // Ограничений 4
            Console.WriteLine("Программа реализована для 2 переменных и 4 уравнений в системе");
            Console.WriteLine("Если в симплекс таблице элемент - буква, то необходимо ввести: -0");
            double[,] matrix;
            double[,] matrix2;
            matrix = new double[N + 3, perem + 3]; // Строк, столбцов
            matrix2 = new double[N, perem]; // Строк, столбцов
            List<double> list = new List<double>();
            IList<double> list2 = new double[N];
            Console.WriteLine("Введите элементы симплекс таблицы по строкам! Для решения исходной задачи: ");
            for (int y = 0; y < N + 3; y++) {
                for (int x = 0; x < perem + 3; x++) { // Столбцы
                    if (!double.TryParse(Console.ReadLine(), out matrix[y, x]))
                    {
                        Console.WriteLine("Вы ввели не корректное число");
                        y--;
                    }
                }
            }
            for (int x = 0; x < perem; x++) //Коэффициенты целевой функции
                list.Add(matrix[0, x + 2]);
            for (int y = 0, i = 0; y < N; y++) //свободные
                list2[i++] = matrix[y + 2, perem + 2];
            for (int y = 0; y < N; y++)
                for (int x = 0; x < perem; x++)
                    matrix2[y, x] = matrix[y + 2, x + 2];
            Console.WriteLine("\n");
            int tmpPerem = perem;
            int index = 1;
            int jndex = 0;
            while (tmpPerem != 0) {
                double delta = GetDelta(matrix, index++, N, perem);
                matrix[N + 3 - 1, jndex + 2] = delta;
                tmpPerem--;
                jndex++;
                double Q = GetDelta(matrix, 0, N, perem); // A0
                matrix[N + 3 - 1, perem + 3 - 1] = Q;
            }
            Console.WriteLine("Решение исходной задачи симплекс-методом:");
            for (int k = 0; k < perem; k++)
                if (matrix[N + 3 - 1, k + 2] < 0)
                {
                    int razreshColumn = PermitColumn(matrix, N, perem);
                    int razreshString = PermitString(matrix, razreshColumn, N, perem);
                    matrix = NewSimplexMatrix(matrix, N + 3, perem + 3, razreshColumn, razreshString);
                    for (int d = 0; d < N + 3; d++)
                    {
                        for (int t = 0; t < perem + 3; t++)
                            Console.Write(matrix[d, t] + "\t");
                        Console.WriteLine();
                    }
                    Console.WriteLine("\n");
                    k = -1;
                    continue;
                }

            Console.WriteLine($"Коэффициенты целевой функции для двойственной задачи:\n"
                + $"{string.Join("  ", list2)}\n\n" // Свободные
                + $"Транспонированная матрица для двойственной задачи:");
            double[,] transMatrix = new double[perem, N];
            transMatrix = matrix2.Transpose();
            for (int t = 0; t < perem; t++) {
                for (int m = 0; m < N; m++)
                    Console.Write($"{transMatrix[t, m], 6 : N4}");
                Console.WriteLine();
            }
            Console.WriteLine($"Свободные коэфициенты для двойственной задачи:\n{string.Join("  ", list)}");
            int forlist3 = 0;
            Console.WriteLine("\n\nБазисные переменные из последней симплекс таблицы:");
            List<int> list3 = new List<int>();
            for (int k = 0; k < N; k++) // Сохранение базисных переменных
            {
                forlist3 = (int) matrix[k + 2, 1];
                list3.Add(forlist3);
                Console.Write(list3[k] + " ");
            }
            Console.WriteLine();
            double max_profit = matrix[N + 2, perem + 2]; // ответ прибыль 53
            int numBasisPerem = N; double[,] matrixBasis;
            matrixBasis = new double[numBasisPerem, numBasisPerem]; // строк, столбцов
            for (int t = 0; t < numBasisPerem; t++)
                for (int k = 0; k < numBasisPerem; k++)
                    matrixBasis[t, k] = k == t
                        ? 1
                        : 0;
            int tempIndex = -1; double[,] matrixD;
            matrixD = new double[N, N];//строк, столбцов
            for (int t = 0; t < N; t++)
                for (int k = 0; k < N; k++)
                    matrixD[t, k] = 0;
            int @in = 0;
            for (int indexBasis = 0; indexBasis < list3.Count; indexBasis++)
            {
                if (list3[indexBasis] <= perem) {
                    tempIndex = list3[indexBasis];
                    for (int c = 0; c < N; c++) {
                        matrixD[c, @in] = matrix2[c, tempIndex - 1];
                    }
                    @in++;
                }
                else if (list3[indexBasis] > perem)
                {
                    tempIndex = list3[indexBasis];
                    for (int c = 0; c < N; c++)
                        matrixD[c, @in] = matrixBasis[c, tempIndex - 3];
                    @in++;
                }
            }
            Console.WriteLine("\nСоставленная матрица D");
            for (int d = 0; d < N; d++) {
                for (int t = 0; t < N; t++)
                    Console.Write(matrixD[d, t] + "\t");
                Console.WriteLine();
            }
            Console.WriteLine("\nОбратная Матрица D");
            matrixD = matrixD.Inversion();
            for (int y = 0; y < matrixD.GetLength(0); y++) {
                for (int x = 0; x < matrixD.GetLength(1); x++)
                    Console.Write(matrixD[y, x] + "\t");
                Console.WriteLine();
            }
            list3.Clear(); // базисный вектор 5 3 0 0
            Console.WriteLine("Базисиный вектор");
            for (int d = 0; d < N; d++) {
                int tmp = (int) matrix[d + 2, 0];
                list3.Add(tmp);
                Console.Write(list3[d] + "  ");
            }
            Console.WriteLine();
            double result = 0;
            List<double> list4 = new List<double>();
            for (int t = 0; t < N; t++) {
                for (int k = 0; k < N; k++)
                    result += matrixD[k, t] * list3[k];
                list4.Add(result);
                result = 0;
            }
            result = 0; Console.WriteLine("Базисный вектор (5,3,0,0) умножаем на обратную матрицу D");
            for (int t = 0; t < list4.Count; t++)
                Console.WriteLine("" + list4[t]);
            for (int t = 0; t < N; t++)
                result += list2[t] * list4[t];
            Console.WriteLine("Получившийся вектор умножаем на свободные коэфициенты прямой задачи. Результат для Gmin: " + result);
            Console.WriteLine("Результат для Fmax: " + max_profit);
        }

        /// <summary>
        /// Расчёт относительных оценок и оптимального плана.
        /// </summary>
        /// <param name="matrix"></param>
        /// <param name="a"></param>
        /// <param name="N"></param>
        /// <param name="perem"></param>
        /// <returns></returns>
        public static double GetDelta(double[,] matrix, int a, int N, int perem)
        {
            double delta = 0;
            if (a != 0) {
                for (int i = 2; i < N + 2 - 1; i++)
                    delta += matrix[i, 0] * matrix[i, a + 1];
                delta = delta - matrix[0, a + 1];
            } else if (a == 0) {
                for (int i = 2; i < N + 2 - 1; i++)
                    delta += matrix[i, 0] * matrix[i, perem + 3 - 1];
            }
            return delta;
        }

        /// <summary>
        /// Нахождение разрешающего столбца.
        /// </summary>
        /// <param name="matrix"></param>
        /// <param name="N"></param>
        /// <param name="perem"></param>
        /// <returns></returns>
        private static int PermitColumn(double[,] matrix, int N, int perem)
        {
            double[] mas;
            mas = new double[perem];
            for (int i = 0; i < perem; i++) {
                mas[i] = matrix[N + 3 - 1, i + 2];
            }
            double min_elem = mas[0];
            int index_min_elem = 0;
            for (int j = 1; j < perem; j++)
                if ((mas[j] < min_elem) && mas[j] < 0) {
                    min_elem = mas[j];
                    index_min_elem = j;
                }
            return index_min_elem + 2;
        }

        /// <summary>
        /// Нахождение разрешающей строки.
        /// </summary>
        /// <param name="matrix"></param>
        /// <param name="razreshColumn">Индекс разрешающего столбца.</param>
        /// <param name="N"></param>
        /// <param name="perem"></param>
        /// <returns></returns>
        private static int PermitString(double[,] matrix, int razreshColumn, int N, int perem) {
            double[] mas; mas = new double[N + 2]; mas[0] = -0; mas[1] = -0;
            double min_elem = 0; int index_min_elem;
            for (int i = 2; i < N + 2; i++)
                mas[i] = matrix[i, perem + 2] / matrix[i, razreshColumn];
            min_elem = mas[2]; index_min_elem = 2;
            for (int j = 3; j < N + 2; j++)
                if (mas[j] > 0 && mas[j] < min_elem) {
                    min_elem = mas[j];
                    index_min_elem = j;
                }
            return index_min_elem;
        }

        
        private static double[,] NewSimplexMatrix(double[,] matrix, int strSipleks, int colum, int permitColumn, int permitString)
        {
            double[,] matrix2;
            matrix2 = new double[strSipleks, colum];
            for (int i = 0; i < strSipleks; i++)
                for (int j = 0; j < colum; j++)
                    matrix2[i, j] = matrix[i, j];
            matrix2[permitString, 0] = matrix[0, permitColumn];
            matrix2[0, permitColumn] = matrix[permitString, 0];
            matrix2[permitString, 1] = matrix[1, permitColumn];
            matrix2[1, permitColumn] = matrix[permitString, 1];
            double razreshElemetn = matrix[permitString, permitColumn];
            for (int k = 2; k <= colum - 1; k++) //разрешающая строка (просмотр столбцов) было к = 4
            {
                if (k != permitColumn) {
                    matrix2[permitString, k] = matrix[permitString, k] / razreshElemetn;
                }
            }
            for (int k = 2; k <= strSipleks - 1; k++) //разрешающая столбец (просмотр строк) было к = 6
            {
                if (k != permitString) {
                    matrix2[k, permitColumn] = (-1) * (matrix[k, permitColumn] / razreshElemetn);
                }
            }
            matrix2[permitString, permitColumn] = 1 / razreshElemetn;
            for (int str = 2; str < strSipleks; str++)
                for (int col = 2; col < colum; col++) //столбцы  было col < 5
                    if (str != permitString && col != permitColumn)
                        matrix2[str, col] = (matrix[str, col] * razreshElemetn - matrix[permitString, col] * matrix[str, permitColumn]) / razreshElemetn;
            return matrix2;
        }


        public static double[,] Inversion(this double[,] input)
        {
            double[,] output = (double[,])input.Clone();
            double temp;
            double[,] E = new double[output.GetLength(0), output.GetLength(1)];
            for (int i = 0; i < E.GetLength(0); i++)
                for (int j = 0; j < E.GetLength(1); j++)
                    E[i, j] = i == j
                        ? 1
                        : 0;
            for (int k = 0; k < E.GetLength(0); k++) {
                temp = output[k, k];
                for (int j = 0; j < E.GetLength(1); j++) {
                    output[k, j] /= temp;
                    E[k, j] /= temp;
                }
                for (int i = k + 1; i < E.GetLength(0); i++) {
                    temp = output[i, k];
                    for (int j = 0; j < E.GetLength(1); j++) {
                        output[i, j] -= output[k, j] * temp;
                        E[i, j] -= E[k, j] * temp;
                    }
                }
            }
            for (int k = E.GetLength(0) - 1; k > 0; k--) {
                for (int i = k - 1; i >= 0; i--) {
                    temp = output[i, k];
                    for (int j = 0; j < E.GetLength(1); j++) {
                        output[i, j] -= output[k, j] * temp;
                        E[i, j] -= E[k, j] * temp;
                    }
                }
            }
            for (int i = 0; i < E.GetLength(0); i++)
                for (int j = 0; j < E.GetLength(1); j++)
                    output[i, j] = E[i, j];
            return output;
        }

        /// <summary>
        /// Транспонирование исходной матрицы.
        /// </summary>
        /// <param name="m">Матрица, которую надо транспортировать.</param>
        /// <returns>Транспонированная матрица.</returns>
        public static double[,] Transpose(this double[,] m)
        {
            double[,] temp = new double[m.GetLength(1), m.GetLength(0)];
            for (int i = 0; i < m.GetLength(0); i++)
                for (int j = 0; j < m.GetLength(1); j++)
                    temp[j, i] = m[i, j];
            return temp;
        }
    }
}