using System;
using System.Collections.Generic;

namespace Simplex
{
    /// <summary>
    /// Реализация симплекса.
    /// </summary>
    public class Simplex
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
            List<double> list2 = new List<double>();
            Console.WriteLine("Введите элементы симплекс таблицы по строкам! Для решения исходной задачи: ");
            for (int i = 0; i < N + 3; i++) {
                for (int j = 0; j < perem + 3; j++) { // Столбцы
                    if (!double.TryParse(Console.ReadLine(), out matrix[i, j]))
                    {
                        Console.WriteLine("Вы ввели не корректное число");
                        i--;
                    }
                }
            }
            for (int i = 0; i < perem; i++) //Коэффициенты целевой функции
                list.Add(matrix[0, i + 2]);
            for (int i = 0; i < N; i++) //свободные
                list2.Add(matrix[i + 2, perem + 2]);
            for (int i = 0; i < N; i++)
                for (int j = 0; j < perem; j++)
                    matrix2[i, j] = matrix[i + 2, j + 2];
            Console.WriteLine("");Console.WriteLine("");
            int tmpPerem = perem;
            int index = 1;
            int jndex = 0;
            while (tmpPerem != 0) {
                double delta = delta_computing(matrix, index++, N, perem);
                matrix[N + 3 - 1, jndex + 2] = delta;
                tmpPerem--;
                jndex++;
                double Q = delta_computing(matrix, 0, N, perem);//A0
                matrix[N + 3 - 1, perem + 3 - 1] = Q;
            }
            Console.WriteLine("Решение исходной задачи симплекс-методом:");
            int flag;
            do
            {
                flag = 0;
                for (int k = 0; k < perem; k++)
                    if (matrix[N + 3 - 1, k + 2] < 0)
                        flag = 1;
                if (flag == 1)
                {
                    int razreshColumn = razresh_column(matrix, N, perem);
                    int razreshString = razresh_string(matrix, razreshColumn, N, perem);
                    matrix = new_simpleks_matrix(matrix, N + 3, perem + 3, razreshColumn, razreshString);
                }
                if (flag == 1)
                    for (int d = 0; d < N + 3; d++)
                    {
                        for (int t = 0; t < perem + 3; t++)
                        {
                            Console.Write(matrix[d, t] + "\t");
                        }
                        Console.WriteLine();
                    }
                Console.WriteLine();
                Console.WriteLine();
            } while (flag == 1);


            Console.WriteLine("Коэффициенты целевой функции для двойственной задачи:");
            for (int t = 0; t < N; t++) //свободные
                Console.Write(list2[t]+"  ");
            Console.WriteLine();
            Console.WriteLine("Транспонированная матрица для двойственной задачи:");
            double[,] transMatrix = new double[perem, N];
            transMatrix = transposeMatrix(matrix2);
            for (int t = 0; t < perem; t++) {
                for (int m = 0; m < N; m++)
                    Console.Write(transMatrix[t, m] + "  ");
                Console.WriteLine();
            }
            Console.WriteLine("Свободные коэфициенты для двойственной задачи:");
            for(int t=0;t<list.Count;t++)
                Console.Write(list[t]+"  ");
            int forlist3 = 0;
            Console.WriteLine("\n\nБазисные переменные из последней симплекс таблицы:");
            List<int> list3 = new List<int>();
            for (int k = 0; k < N; k++) // Сохранение базисных переменных
            {
                forlist3 = (int) matrix[k + 2, 1];
                list3.Add(forlist3);
                Console.Write(list3[k] + " ");
            }
            Console.WriteLine("");
            double max_dohod = matrix[N + 2, perem + 2];//ответ прибыль 53
            int numBasisPerem = N; double[,] matrixBasis;
            matrixBasis = new double[numBasisPerem, numBasisPerem];//строк, столбцов
            for (int t = 0; t < numBasisPerem; t++) {
                for (int k = 0; k < numBasisPerem; k++) {
                    if (k == t) {
                        matrixBasis[t, k] = 1;
                    } else {
                        matrixBasis[t, k] = 0; }
                }
            }
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
            Console.WriteLine(); Console.WriteLine("Обратная Матрица D");
            inversion(matrixD, N);
            for (int d = 0; d < N; d++) {
                for (int t = 0; t < N; t++)
                    Console.Write(matrixD[d, t] + "\t");
                Console.WriteLine();
            }
            list3.Clear();//базисный вектор 5 3 0 0
            Console.WriteLine("Базисиный вектор");
            for (int d = 0; d < N; d++) {
                int tmp = (int) matrix[d + 2, 0];
                list3.Add(tmp);
                Console.Write(list3[d] + "  ");
            }
            Console.WriteLine();double result = 0;
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
            Console.WriteLine("Результат для Fmax: " + max_dohod);
        }

        /// <summary>
        /// Расчет относительных оценок и оптимального плана.
        /// </summary>
        /// <param name="matrix"></param>
        /// <param name="a"></param>
        /// <param name="N"></param>
        /// <param name="perem"></param>
        /// <returns></returns>
        public static double delta_computing(double[,] matrix, int a, int N, int perem)
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
        public static int razresh_column(double[,] matrix, int N, int perem)
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
        public static int razresh_string(double[,] matrix, int razreshColumn, int N, int perem) {
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
        public static double[,] new_simpleks_matrix(double[,] matrix, int strSipleks, int colum, int razreshColumn, int razreshString)
        {
            double[,] matrix2;
            matrix2 = new double[strSipleks, colum];
            for (int i = 0; i < strSipleks; i++)
                for (int j = 0; j < colum; j++)
                    matrix2[i, j] = matrix[i, j];
            matrix2[razreshString, 0] = matrix[0, razreshColumn];
            matrix2[0, razreshColumn] = matrix[razreshString, 0];
            matrix2[razreshString, 1] = matrix[1, razreshColumn];
            matrix2[1, razreshColumn] = matrix[razreshString, 1];
            double razreshElemetn = matrix[razreshString, razreshColumn];
            for (int k = 2; k <= colum - 1; k++) //разрешающая строка (просмотр столбцов) было к = 4
            {
                if (k != razreshColumn) {
                    matrix2[razreshString, k] = matrix[razreshString, k] / razreshElemetn;
                }
            }
            for (int k = 2; k <= strSipleks - 1; k++) //разрешающая столбец (просмотр строк) было к = 6
            {
                if (k != razreshString) {
                    matrix2[k, razreshColumn] = (-1) * (matrix[k, razreshColumn] / razreshElemetn);
                }
            }
            matrix2[razreshString, razreshColumn] = 1 / razreshElemetn;
            for (int str = 2; str < strSipleks; str++)
                for (int col = 2; col < colum; col++) //столбцы  было col < 5
                    if (str != razreshString && col != razreshColumn)
                        matrix2[str, col] = (matrix[str, col] * razreshElemetn - matrix[razreshString, col] * matrix[str, razreshColumn]) / razreshElemetn;
            return matrix2;
        }
        private static void inversion(double[,] A, int N)
        {
            double temp;double[,] E = new double[N, N];
            for (int i = 0; i < N; i++)
                for (int j = 0; j < N; j++)
                {
                    E[i, j] = 0f;
                    if (i == j)
                        E[i, j] = 1f;
                }
            for (int k = 0; k < N; k++) {
                temp = A[k, k];
                for (int j = 0; j < N; j++) {
                    A[k, j] /= temp;
                    E[k, j] /= temp;
                }
                for (int i = k + 1; i < N; i++) {
                    temp = A[i, k];
                    for (int j = 0; j < N; j++) {
                        A[i, j] -= A[k, j] * temp;
                        E[i, j] -= E[k, j] * temp;
                    }
                }
            }
            for (int k = N - 1; k > 0; k--) {
                for (int i = k - 1; i >= 0; i--) {
                    temp = A[i, k];
                    for (int j = 0; j < N; j++) {
                        A[i, j] -= A[k, j] * temp;
                        E[i, j] -= E[k, j] * temp;
                    }}}

            for (int i = 0; i < N; i++) {
                for (int j = 0; j < N; j++) {
                    A[i, j] = E[i, j];
                }
            }
        }

     private static double[,] transposeMatrix(double [, ] m)
     {
            double[,] temp = new double[m.GetLength(1), m.GetLength(0)];
            for (int i = 0; i < m.GetLength(0); i++)
                for (int j = 0; j < m.GetLength(1); j++)
                    temp[j, i] = m[i, j];
            return temp;
        }
    }
}