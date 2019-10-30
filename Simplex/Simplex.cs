using System;
using System.Collections.Generic;
using System.Text;

namespace Simplex
{
    /// <summary>
    /// Реализация симплекса.
    /// </summary>
    public static class Simplex
    {
        /// <summary>
        /// Запускает взаимодействие пользователя с симплекс-методом.
        /// </summary>
        public static void UI()
        {
            int countVars = 2; // Переменных 2
            int countLimits = 4; // Ограничений 4
            Console.WriteLine("Программа реализована для 2 переменных и 4 уравнений в системе.\n"
             + "Если в симплекс таблице элемент - буква, то необходимо ввести: -0\n"
             + "Формат записи координат: [строка, столбец].\n"
             + "Введите элементы симплекс таблицы по строкам! Для решения исходной задачи: ");
            double[,] matrix;
            double[,] subMatrix;
            matrix = new double[countLimits + 3, countVars + 3]; // Строк, столбцов
            subMatrix = new double[countLimits, countVars]; // Строк, столбцов
            for (int y = 0; y < countLimits + 3; y++) {
                for (int x = 0; x < countVars + 3; x++) // Столбцы
                {
                    Console.Write($"[{y}, {x}]: ");
                    if (!double.TryParse(Console.ReadLine(), out matrix[y, x]))
                    {
                        Console.WriteLine("Вы ввели не корректное число");
                        x--;
                    }
                }
            }
            IList<double> targetParams = new double[countVars];
            IList<double> freeParams = new double[countLimits];
            for (int x = 0; x < countVars; x++) // Коэффициенты целевой функции
                targetParams[x] = matrix[0, x + 2];
            for (int y = 0, i = 0; y < countLimits; y++) // Свободные
                freeParams[i++] = matrix[y + 2, countVars + 2];
            for (int y = 0; y < subMatrix.GetLength(0); y++)
                for (int x = 0; x < subMatrix.GetLength(1); x++)
                    subMatrix[y, x] = matrix[y + 2, x + 2];
            Console.WriteLine("\n");
            for(int tmpVar = countVars, index_1 = 1, index_2 = 0; tmpVar != 0; index_1++, index_2++, tmpVar--)
            {
                double delta = GetDelta(matrix, index_1, countLimits, countVars);
                matrix[countLimits + 2, index_2 + 2] = delta;
                double Q = GetDelta(matrix, 0, countLimits, countVars); // A0
                matrix[countLimits + 2, countVars + 2] = Q;
            }
            Console.WriteLine("Решение исходной задачи симплекс-методом:");
            for (int y = 0; y < countVars; y++)
                if (matrix[countLimits + 2, y + 2] < 0)
                {
                    int targetColumn = PermitColumn(matrix, countLimits, countVars);
                    int targetString = PermitString(matrix, targetColumn, countLimits, countVars);
                    matrix = NewSimplexMatrix(matrix, countLimits + 3, countVars + 3, targetColumn, targetString);
                    for (int d = 0; d < countLimits + 3; d++)
                    {
                        for (int t = 0; t < countVars + 3; t++)
                            Console.Write(matrix[d, t] + "\t");
                        Console.WriteLine();
                    }
                    Console.WriteLine("\n");
                    y = -1;
                    continue;
                }

            Console.WriteLine($"Коэффициенты целевой функции для двойственной задачи:\n"
                + $"{string.Join("  ", freeParams)}\n\n" // Свободные
                + $"Транспонированная матрица для двойственной задачи:");
            double[,] transMatrix = subMatrix.Transpose();
            for (int t = 0; t < countVars; t++)
            {
                for (int m = 0; m < countLimits; m++)
                    Console.Write($"{transMatrix[t, m], 6}");
                Console.WriteLine();
            }
            Console.WriteLine($"Свободные коэффициенты для двойственной задачи:\n{string.Join("  ", targetParams)}");
            Console.WriteLine("\n\nБазисные переменные из последней симплекс таблицы:");
            IList<int> baseVars = new int[countLimits];
            for (int y = 0; y < countLimits; y++) // Сохранение базисных переменных
            {
                baseVars[y] = (int)matrix[y + 2, 1];
                Console.Write(baseVars[y] + " ");
            }
            Console.WriteLine();
            double max_profit = matrix[countLimits + 2, countVars + 2]; // Ответ прибыль 53
            int numBasisVar = countLimits; double[,] matrixBasis;
            matrixBasis = new double[numBasisVar, numBasisVar]; // строк, столбцов
            for (int t = 0; t < numBasisVar; t++)
                for (int k = 0; k < numBasisVar; k++)
                    matrixBasis[t, k] = k == t
                        ? 1
                        : 0;
            double[,] matrixD;
            matrixD = new double[countLimits, countLimits]; // строк, столбцов
            int @in = 0;
            for (int indexBasis = 0; indexBasis < baseVars.Count; indexBasis++)
            {
                int tempIndex = baseVars[indexBasis];
                if (baseVars[indexBasis] <= countVars)
                    for (int c = 0; c < countLimits; c++)
                        matrixD[c, @in] = subMatrix[c, tempIndex - 1];
                else // if (baseVars[indexBasis] > countVar)
                    for (int c = 0; c < countLimits; c++)
                        matrixD[c, @in] = matrixBasis[c, tempIndex - 3];
                @in++;
            }
            Console.WriteLine($"\nСоставленная матрица D:\n{matrixD.TableToString()}");
            matrixD = matrixD.Inversion();
            Console.WriteLine($"\nОбратная Матрица D:\n{matrixD.TableToString()}");
            // Базисный вектор 5 3 0 0.
            for (int i = 0; i < baseVars.Count; i++)
                baseVars[i] = (int) matrix[i + 2, 0];
            Console.WriteLine($"Базисный вектор:\n{string.Join(" ", baseVars)}\n");
            double result = 0;
            List<double> list4 = new List<double>();
            for (int t = 0; t < countLimits; t++) {
                for (int k = 0; k < countLimits; k++)
                    result += matrixD[k, t] * baseVars[k];
                list4.Add(result);
                result = 0;
            }
            result = 0; Console.WriteLine("Базисный вектор (5,3,0,0) умножаем на обратную матрицу D");
            for (int t = 0; t < list4.Count; t++)
                Console.WriteLine("" + list4[t]);
            for (int t = 0; t < countLimits; t++)
                result += freeParams[t] * list4[t];
            Console.WriteLine("Получившийся вектор умножаем на свободные коэффициенты прямой задачи. Результат для G -> min: " + result);
            Console.WriteLine("Результат для F -> max: " + max_profit);
        }

        /// <summary>
        /// Расчёт относительных оценок и оптимального плана.
        /// </summary>
        /// <param name="matrix"></param>
        /// <param name="a"></param>
        /// <param name="countLimits"></param>
        /// <param name="countVar"></param>
        /// <returns></returns>
        public static double GetDelta(double[,] matrix, int a, int countLimits, int countVar)
        {
            double delta = 0;
            if (a != 0) {
                for (int i = 2; i < countLimits + 2 - 1; i++)
                    delta += matrix[i, 0] * matrix[i, a + 1];
                delta = delta - matrix[0, a + 1];
            } else if (a == 0) {
                for (int i = 2; i < countLimits + 2 - 1; i++)
                    delta += matrix[i, 0] * matrix[i, countVar + 2];
            }
            return delta;
        }

        /// <summary>
        /// Нахождение разрешающего столбца.
        /// </summary>
        /// <param name="matrix"></param>
        /// <param name="countLimits"></param>
        /// <param name="countVars"></param>
        /// <returns></returns>
        private static int PermitColumn(double[,] matrix, int countLimits, int countVars)
        {
            double[] mas;
            mas = new double[countVars];
            for (int i = 0; i < countVars; i++) {
                mas[i] = matrix[countLimits + 2, i + 2];
            }
            double min_elem = mas[0];
            int index_min_elem = 0;
            for (int j = 1; j < countVars; j++)
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
        /// <param name="targetColumn">Индекс разрешающего столбца.</param>
        /// <param name="countLimits">Количество ограничений.</param>
        /// <param name="countVars">Количество переменных.</param>
        /// <returns></returns>
        private static int PermitString(double[,] matrix, int targetColumn, int countLimits, int countVars) {
            double[] mas; mas = new double[countLimits + 2]; mas[0] = -0; mas[1] = -0;
            double min_elem = 0; int index_min_elem;
            for (int i = 2; i < countLimits + 2; i++)
                mas[i] = matrix[i, countVars + 2] / matrix[i, targetColumn];
            min_elem = mas[2]; index_min_elem = 2;
            for (int j = 3; j < countLimits + 2; j++)
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
            double targetElement = matrix[permitString, permitColumn];
            for (int k = 2; k < colum; k++) // Разрешающая строка (просмотр столбцов) было к = 4
                if (k != permitColumn)
                    matrix2[permitString, k] = matrix[permitString, k] / targetElement;
            for (int k = 2; k < strSipleks; k++) // Разрешающая столбец (просмотр строк) было к = 6
                if (k != permitString)
                    matrix2[k, permitColumn] = (-1) * (matrix[k, permitColumn] / targetElement);
            matrix2[permitString, permitColumn] = 1 / targetElement;
            for (int str = 2; str < strSipleks; str++)
                for (int col = 2; col < colum; col++) // столбцы  было col < 5
                    if (str != permitString && col != permitColumn)
                        matrix2[str, col] = (matrix[str, col] * targetElement - matrix[permitString, col] * matrix[str, permitColumn]) / targetElement;
            return matrix2;
        }

        /// <summary>
        /// Создаёт обратную матрицу из входной.
        /// </summary>
        /// <param name="input">Матрица, которую надо инвертировать.</param>
        /// <returns>Новая матрица, которая обратна к входной.</returns>
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

        /// <summary>
        /// Преобразует объект в строку, вставляя дополнительные пробелы,
        /// чтобы подогнать под размер len.
        /// </summary>
        /// <param name="toInsert">Объект, который надо преобразовать в строку.</param>
        /// <param name="len">Минимальная длинна выходной строки.</param>
        /// <returns>Строка, представляющая объект toInstert.</returns>
        internal static string ToString(this object toInsert, int len)
                    => string.Format(string.Format("{{0, {0}}}", len), toInsert);

        /// <summary>
        /// Преобразует объект в строку заданного формата, вставляя дополнительные пробелы,
        /// чтобы подогнать под размер <code>len</code>.
        /// </summary>
        /// <param name="toInsert">Объект, который надо преобразовать в строку.</param>
        /// <param name="len">Минимальная длинна выходной строки.</param>
        /// <param name="format">Формат вывода.</param>
        /// <returns>Строка, представляющая объект <code>toInstert</code>.</returns>
        internal static string ToString(this object toInsert, int len, string format)
                    => string.Format(string.Format("{{0, {0} :{1}}}", len, format), toInsert);        

        /// <summary>
        /// Превращает двухмерную таблицу в строку.
        /// </summary>
        /// <param name="array">Таблица.</param>
        /// <param name="format">Формат вывода каждого элемента.</param>
        /// <returns>Строковое представление каждого элемента массива в виде таблицы.</returns>
        internal static string TableToString(this Array array, string format = null)
        {
            if (array.Rank != 2)
                throw new NotSupportedException();
            int max = -1;
            foreach (var a in array)
                if (a.ToString(0, format).Length > max)
                    max = a.ToString(0, format).Length;
            max++;
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < array.GetLength(0); i++)
            {
                for (int j = 0; j < array.GetLength(1); j++)
                {
                    sb.Append(array.GetValue(i, j).ToString(max, format));
                }
                sb.Append('\n');
            }
            return sb.ToString();
        }
    }
}