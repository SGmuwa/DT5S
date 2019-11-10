using System;
using System.Collections.Generic;
using System.Linq;
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
            int countVars = GetNumber("Введите количество переменных (основных столбцов): ", new TryParseHandler<int>(int.TryParse));
            int countLimits = GetNumber("Введите количество ограничений (основных строк): ", new TryParseHandler<int>(int.TryParse));
            Console.WriteLine("Программа реализована для 2 переменных и 4 уравнений в системе.\n"
             + "Если в симплекс таблице элемент - буква, то необходимо ввести: -0\n"
             + "Формат записи координат: [строка, столбец].\n"
             + "Введите элементы симплекс таблицы по строкам! Для решения исходной задачи: ");
            double[,] matrix;
            double[,] subMatrix;
            matrix = new double[countLimits + 3, countVars + 3]; // Строк, столбцов
            subMatrix = new double[countLimits, countVars]; // Строк, столбцов
            for (int y = 0; y < countLimits + 3; y++)
                for (int x = 0; x < countVars + 3; x++) // Столбцы
                    matrix[y, x] = GetNumber($"[{y}, {x}]: ", new TryParseHandler<double>(double.TryParse));
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
            Console.WriteLine($"Входная таблица:\n{matrix.TableToString(null, (a) => Math.Round((double)a, 3))}\n"
                + "Решение исходной задачи симплекс-методом:");
            for (int y = 0; y < countVars; y++)
                if (matrix[countLimits + 2, y + 2] < 0)
                {
                    int targetColumn = PermitColumn(matrix, countLimits, countVars);
                    int targetString = PermitString(matrix, targetColumn, countLimits, countVars);
                    matrix = NewSimplexMatrix(matrix, countLimits + 3, countVars + 3, targetColumn, targetString);
                    Console.WriteLine($"Разрешающий элемент: [{targetString}, {targetColumn}], таблица:\n{matrix.TableToString(null, (a) => Math.Round((double)a, 3))}");
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
            matrixD = GenerateMatrixD(countLimits, countVars, baseVars, subMatrix, matrixBasis);
            Console.WriteLine($"\nСоставленная матрица D:\n{matrixD.TableToString()}");
            matrixD = matrixD.Inversion();
            Console.WriteLine($"\nОбратная Матрица D:\n{matrixD.TableToString()}");
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

		private static double[,] GenerateMatrixD(int countLimits, int countVars, IList<int> baseVars, double[,] subMatrix, double[,] matrixBasis)
		{
            double[,] output = new double[countLimits, countLimits]; // строк, столбцов
            int @in = 0;
            for (int indexBasis = 0; indexBasis < baseVars.Count; indexBasis++)
            {
                int tempIndex = baseVars[indexBasis];
                if (baseVars[indexBasis] <= countVars)
                    for (int c = 0; c < countLimits; c++)
                        output[c, @in] = subMatrix[c, tempIndex - 1];
                else // if (baseVars[indexBasis] > countVar)
                    for (int c = 0; c < countLimits; c++)
                        output[c, @in] = matrixBasis[c, tempIndex - 3];
                @in++;
            }
            return output;
		}

		/// <summary>
		/// Считывает с пользовательского интерфейса объект.
		/// </summary>
		/// <param name="message">Сообщение, которое надо напечатать пользователю.</param>
		/// <param name="parse">Метод считывания TryParse, с помощью которого будет преобразована строка в объект.</param>
		/// <typeparam name="T">Указывает, в какой тип надо преобразовывать объект.</typeparam>
		/// <returns>Считанный объект от пользователя.</returns>
		public static T GetNumber<T>(string message, TryParseHandler<T> parse)
		{
            while(true)
            {
                Console.WriteLine(message);
                if(parse(Console.ReadLine(), out T output))
                    return output;
                Console.WriteLine($"\"{output}\" не может быть прочитан с помощью {parse.Method}");
            }
		}

        /// <summary>
        /// Класс-делекат, который описывает интерфейс функций TryParse.
        /// </summary>
        /// <param name="inputStr">Входная строка, которую надо преобразовать в объект.</param>
        /// <param name="result">Указатель на результат выполнения.</param>
        /// <typeparam name="T">Требуемый тип искомого объекта.</typeparam>
        /// <returns>True, если чтение удачно. Иначе - false.</returns>
		public delegate bool TryParseHandler<T>(string inputStr, out T result);

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
            return inversion();
            
            double[,] inversion()
            {
                if(input.GetLength(1) != input.GetLength(0)) // Возможно излишняя проверка.
                    throw new NotSupportedException("Поддерживаются только квадратные матрицы.");
                double[,] matrixWidth = InitMatrixWidth(input);
                do
                {
                    Console.WriteLine(matrixWidth.TableToString());
                    List<int> badRows = SearchMaxNumbers(matrixWidth); // Ищет строку максимально плохую
                    Console.WriteLine($"badRows: {string.Join(", ", badRows)}");
                    if(badRows.Count() == 0)
                        break;
                    int badRow = 0;
                    (int, ISet<int>)? rowCompensation = null;
                    foreach(var br in badRows)
                    {
                        badRow = br;
                        rowCompensation = SearchRowForAddition(matrixWidth, badRow);
                        if(rowCompensation.HasValue)
                            break;
                    }
                    if(!rowCompensation.HasValue)
                        throw new Exception($"Тупик в вычислениях. Текущие индексы: {string.Join(", ", badRows)}. "
                            + $"Матрица:\n{matrixWidth.TableToString()}");
                    Console.WriteLine($"rowCompensation: ({rowCompensation.Value.Item1}, [{string.Join(", ", rowCompensation.Value.Item2)}])");
                    FixRow(matrixWidth, badRow, rowCompensation.Value.Item1, rowCompensation.Value.Item2);
                } while(true);
                MoveRowsIfNeed(matrixWidth); // Перемещает единицы на диагональ.
                Console.WriteLine($"MoveRowsIfNeed:\n{matrixWidth.TableToString()}");
                Normalize(matrixWidth); // Умножает каждую строку так, чтобы получились слева одни единицы.
                Console.WriteLine($"Normalize:\n{matrixWidth.TableToString()}");
                double[,] right = GetRightMatrix(matrixWidth);
                Console.WriteLine($"GetRightMatrix:\n{right.TableToString()}");
                return right;
            }

            double[,] InitMatrixWidth(double[,] input)
            {
                double[,] output = new double[input.GetLength(0), input.GetLength(1) * 2]; // Двойная матрица в ширину.
                for(int y = 0; y < output.GetLength(0); y++)
                {
                    for(int x = 0; x < output.GetLength(1) / 2; x++)
                        output[y, x] = input[y, x]; // Входная матрица слева
                    for(int x = output.GetLength(1) / 2; x < output.GetLength(1); x++)
                        output[y, x] = y == x - output.GetLength(1) / 2 ? 1 : 0; // Единичная матрица справа.
                }
                return output;
            }

            // Возвращает список строк, которые надо исправлять.
            List<int> SearchMaxNumbers(double[,] matrixWidth)
            {
                List<(int, int)> draft = new List<(int, int)>(matrixWidth.GetLength(0));
                for(int y = 0; y < matrixWidth.GetLength(0); y++)
                {
                    int count = GetNumbersInRow(y);
                    if(count > 1)
                        draft.Add((count, y));
                }
                draft.Sort((left, right) => left.Item1.CompareTo(right.Item1));
                return draft.Select(a => a.Item2).ToList();

                int GetNumbersInRow(int y)
                {
                    int output = 0;
                    for(int x = 0; x < matrixWidth.GetLength(1) / 2; x++)
                        if(matrixWidth[y, x] != 0.0)
                            output++;
                    return output;
                }
            }
            
            // Ищет строку, которую можно прибавить для исправления.
            // Также возвращает множество индексов, которые подойдут для последующего исправления.
            (int, ISet<int>)? SearchRowForAddition(double[,] matrixWidth, int badRow)
            {
                (ISet<int> indexesForFix, ISet<int> indexesNotTouch) = SearchIndexesForFix(badRow);
                Console.WriteLine($"indexesForFix({badRow}): [{string.Join(", ", indexesForFix)}]");
                for(int y = 0; y < matrixWidth.GetLength(0); y++)
                {
                    if(badRow == y)
                        continue;
                    ISet<int> indexesNotEmpty = SearchNotEmptyIndexes(y);
                    {
                        ISet<int> setForCheckIntersection = new HashSet<int>(indexesNotEmpty);
                        setForCheckIntersection.IntersectWith(indexesNotTouch);
                        if(setForCheckIntersection.Count > 0)
                            continue;
                    }
                    Console.WriteLine($"indexesNotEmpty[{y}]: [{string.Join(", ", indexesNotEmpty)}]");
                    indexesNotEmpty.IntersectWith(indexesForFix); // Пересечение.
                    Console.WriteLine($"indexesNotEmpty[{y}].IntersectWith(indexesForFix): [{string.Join(", ", indexesNotEmpty)}]");
                    indexesNotEmpty.ExceptWith(indexesNotTouch);
                    Console.WriteLine($"indexesNotEmpty[{y}].ExceptWith(indexesNotTouch): [{string.Join(", ", indexesNotEmpty)}]");
                    if(indexesNotEmpty.Count > 0)
                        return (y, indexesNotEmpty);
                }
                return null;

                HashSet<int> SearchNotEmptyIndexes(int y)
                {
                    HashSet<int> output = new HashSet<int>();
                    for(int x = 0; x < matrixWidth.GetLength(1) / 2; x++)
                        if(matrixWidth[y, x] != 0.0)
                            output.Add(x);
                    return output;
                }

                (HashSet<int>, HashSet<int>) SearchIndexesForFix(int badRow) // Возвращает индексы, которые надо починить. Вторым отправляет те, которые трогать нельзя.
                {
                    HashSet<int> bad = SearchNotEmptyIndexes(badRow);
                    HashSet<int> good = new HashSet<int>(Enumerable.Range(0, matrixWidth.GetLength(1) / 2));
                    good.ExceptWith(bad);
                    return (bad, good);
                }
            }

            // Исправляет плохую матрицу, складывая к плохой строке хорошую с некоторым коэффициентом.
            void FixRow(double[,] matrixWidth, int badRow, int rowForAdd, ISet<int> indexesGoodForAdd)
            {
                int indexColumnForAdd = indexesGoodForAdd.First();
                Console.WriteLine($"indexColumnForAdd: {indexColumnForAdd}");
                double CoefficientForAdd = -matrixWidth[badRow, indexColumnForAdd] / matrixWidth[rowForAdd, indexColumnForAdd];
                Console.WriteLine($"CoefficientForAdd: {CoefficientForAdd}");
                AddRow(badRow, rowForAdd, CoefficientForAdd);
                matrixWidth[badRow, indexColumnForAdd] = 0.0; // Нормализация. Необходимо, чтобы не оставалось хвостов после запятой.

                void AddRow(int badRow, int rowForAdd, double CoefficientForAdd)
                {
                    for(int x = 0; x < matrixWidth.GetLength(1); x++)
                        matrixWidth[badRow, x] += matrixWidth[rowForAdd, x] * CoefficientForAdd;
                }
            }

            // Делает единицы по диагонали.
            void Normalize(double[,] matrixWidth)
            {
                for(int y = 0; y < matrixWidth.GetLength(0); y++)
                {
                    DivideMatrixRow(matrixWidth, y, matrixWidth[y, y]);
                    matrixWidth[y, y] = 1.0; // Избавление от хвостов.
                }

                void DivideMatrixRow(double[,] matrix, int indexToDiv, double divisor)
                {
                    for(int x = 0; x < matrix.GetLength(1); x++)
                        matrix[indexToDiv, x] /= divisor;
                }
            }

            void MoveRowsIfNeed(double[,] matrixWidth)
            {
                for(int yx = 0; yx < matrixWidth.GetLength(0) && yx < matrixWidth.GetLength(1); yx++)
                    if(matrixWidth[yx, yx] == 0.0)
                        SwapRows(matrixWidth, yx, SearchOkRow(matrixWidth, yx, yx + 1));

                int SearchOkRow(double[,] matrixWidth, int columnNeed, int startRow = 0)
                {
                    for(int y = startRow; y < matrixWidth.GetLength(0); y++)
                        if(matrixWidth[y, columnNeed] != 0.0)
                            return y;
                    throw new Exception($"Не получилось расставить строки так, чтобы получилась единичная матрица.\n" +
                        $"Неправильная строка: {columnNeed}, таблица:\n{matrixWidth.TableToString()}");
                }

                void SwapRows(double[,] matrixWidth, int first, int second)
                {
                    for(int x = 0; x < matrixWidth.GetLength(1); x++)
                    {
                        double buffer = matrixWidth[first, x];
                        matrixWidth[first, x] = matrixWidth[second, x];
                        matrixWidth[second,x ] = buffer;
                    }
                }
            }
        
            // Возвращает правую часть матрицы.
            double[,] GetRightMatrix(double[,] matrixWidth)
            {
                double[,] output = new double[matrixWidth.GetLength(0), matrixWidth.GetLength(1) / 2];
                for(int y = 0; y < matrixWidth.GetLength(0); y++)
                    for(int x = matrixWidth.GetLength(1) / 2; x < matrixWidth.GetLength(1); x++)
                    {
                        Console.WriteLine($"output[y, x - matrixWidth.GetLength(1) / 2] = matrixWidth[y, x]; // output[{y}, {x - matrixWidth.GetLength(1) / 2}] = matrixWidth[{y}, {x}];");
                        output[y, x - matrixWidth.GetLength(1) / 2] = matrixWidth[y, x];
                    }
                return output;
            }
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
        /// <param name="input">Таблица.</param>
        /// <param name="format">Формат вывода каждого элемента.</param>
        /// <param name="renderForeach">Функция преобразования </param>
        /// <returns>Строковое представление каждого элемента массива в виде таблицы.</returns>
        internal static string TableToString(this Array input, string format = null, Func<dynamic, object> renderForeach = null)
        {
            if (input.Rank != 2)
                throw new NotSupportedException();
            Array array;
            if(renderForeach == null)
                array = input;
            else
            {
                array = new object[input.GetLength(0), input.GetLength(1)];
                for(int y = 0; y < array.GetLength(0); y++)
                    for(int x = 0; x < array.GetLength(1); x++)
                        array.SetValue(renderForeach(input.GetValue(y, x)), y, x);
            }
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