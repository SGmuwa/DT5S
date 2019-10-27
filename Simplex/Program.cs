import java.util.ArrayList;
import java.util.List;
import java.util.Scanner;
public class DvoystvZadacha {
    public static void main(String[] args) {
        int perem = 2;//Переменных2
        int N = 4; //Ограничений4
        System.out.println("Программа реализована для 2 переменных и 4 уравнений в системе");
        System.out.println("Если в симплекс таблице элемент - буква, то необходимо ввести: -0");
        Scanner sc1 = new Scanner(System.in);
        double[][] matrix;
        double[][] matrix2;
        matrix = new double[N + 3][perem + 3];//строк, столбцов
        matrix2 = new double[N][perem];//строк, столбцов
        List<Double> list = new ArrayList<Double>();
        List<Double> list2 = new ArrayList<Double>();
        Scanner sc4 = new Scanner(System.in);
        System.out.println("Введите элементы симплекс таблицы по строкам! Для решения исходной задачи: ");
        for (int i = 0; i < N + 3; i++) {
            for (int j = 0; j < perem + 3; j++) {//столбцы
                if (sc4.hasNextDouble()) {
                    matrix[i][j] = sc4.nextDouble();
                } else {
                    sc4.next();
                    System.out.println("Вы ввели не корректное число");
                    i--;}}}
        for (int i = 0; i < perem; i++) //Коэффициенты целевой функции
            list.add(matrix[0][i + 2]); 
        for (int i = 0; i < N; i++) //свободные
            list2.add(matrix[i + 2][perem + 2]); 
        for (int i = 0; i < N; i++) 
            for (int j = 0; j < perem; j++) 
                matrix2[i][j] = matrix[i + 2][j + 2]; 
        System.out.println("");System.out.println("");
        int flag;
        int tmpPerem = perem;
        int i = 1;
        int j = 0;
        while (tmpPerem != 0) {
            double delta = delta_computing(matrix, i++, N, perem);
            matrix[N + 3 - 1][j + 2] = delta;
            tmpPerem--;
            j++;
            double Q = delta_computing(matrix, 0, N, perem);//A0
            matrix[N + 3 - 1][perem + 3 - 1] = Q;
        }
        System.out.println("Решение исходной задачи симплекс-методом:");
        do {
            flag = 0;
            for (int k = 0; k < perem; k++) {
                if (matrix[N + 3 - 1][k + 2] < 0) {
                    flag = 1;
                }
            }
            if (flag == 1) {
                int razreshColumn = razresh_column(matrix, N, perem);
                int razreshString = razresh_string(matrix, razreshColumn, N, perem);
                matrix = new_simpleks_matrix(matrix, N + 3, perem + 3, razreshColumn, razreshString);
            }
            if(flag ==1)
            for (int d = 0; d < N + 3; d++) {
                for (int t = 0; t < perem + 3; t++) {
                    System.out.print(matrix[d][t] + "\t");
                }
                System.out.println();
            }
            System.out.println();
            System.out.println();
        } while (flag == 1);
        
        
        System.out.println("Коэффициенты целевой функции для двойственной задачи:");
        for (int t = 0; t < N; t++) //свободные
            System.out.print(list2.get(t)+"  ");
        System.out.println(); 
        System.out.println("Транспонированная матрица для двойственной задачи:");
        double[][] transMatrix = new double[perem][N];
        transMatrix = transposeMatrix(matrix2);
        for (int t = 0; t < perem; t++) {
            for (int m = 0; m < N; m++) 
                System.out.print(transMatrix[t][m] + "  ");
            System.out.println();
        } 
        System.out.println("Свободные коэфициенты для двойственной задачи:");
        for(int t=0;t<list.size();t++)
            System.out.print(list.get(t)+"  ");
        System.out.println(); int forlist3 = 0; System.out.println();
        System.out.println("Базисные переменные из последней симплекс таблицы:");
        List<Integer> list3 = new ArrayList<Integer>();
        for (int k = 0; k < N; k++) {//сохранение базисных переменных
            forlist3 = (int) matrix[k + 2][1];
            list3.add(forlist3); 
            System.out.print(list3.get(k) + " ");System.out.print("");
        }
        System.out.println("");
        double max_dohod = matrix[N + 2][perem + 2];//ответ прибыль 53 
        int numBasisPerem = N; double[][] matrixBasis;
        matrixBasis = new double[numBasisPerem][numBasisPerem];//строк, столбцов
        for (int t = 0; t < numBasisPerem; t++) {
            for (int k = 0; k < numBasisPerem; k++) {
                if (k == t) {
                    matrixBasis[t][k] = 1;
                } else {
                    matrixBasis[t][k] = 0; }}}

        int tempIndex = -1; double[][] matrixD;
        matrixD = new double[N][N];//строк, столбцов
        for (int t = 0; t < N; t++) {
            for (int k = 0; k < N; k++) 
                matrixD[t][k] = 0; 
        int in = 0;
        for (int indexBasis = 0; indexBasis < list3.size(); indexBasis++) {
            if (list3.get(indexBasis) <= perem) {
                tempIndex = list3.get(indexBasis);
                for (int c = 0; c < N; c++) {
                    matrixD[c][in] = matrix2[c][tempIndex - 1];
                }
                in++;
            } else if (list3.get(indexBasis) > perem) {
                tempIndex = list3.get(indexBasis);
                for (int c = 0; c < N; c++) {
                    matrixD[c][in] = matrixBasis[c][tempIndex - 3];
                }
                in++;}} 
        System.out.println("");
        System.out.println("Составленная матрица D");
        for (int d = 0; d < N; d++) {
            for (int t = 0; t < N; t++) 
                System.out.print(matrixD[d][t] + "\t");
            System.out.println();
        } 
        System.out.println(); System.out.println("Обратная Матрица D");
        inversion(matrixD, N);
        for (int d = 0; d < N; d++) {
            for (int t = 0; t < N; t++)
                System.out.print(matrixD[d][t] + "\t");
            System.out.println();
        } 
        list3.clear();//базисный вектор 5 3 0 0
        System.out.println("Базисиный вектор");
        for (int d = 0; d < N; d++) {
            int tmp = (int) matrix[d + 2][0];
            list3.add(tmp);
            System.out.print(list3.get(d)+"  ");
        }
        System.out.println();double result = 0;
        List<Double> list4 = new ArrayList<Double>();
        for (int t = 0; t < N; t++) {
            for (int k = 0; k < N; k++) 
                result += matrixD[k][t] * list3.get(k); 
            list4.add(result);
            result = 0;
        }
        result = 0; System.out.println("Базисный вектор (5,3,0,0) умножаем на обратную матрицу D");
        for (int t = 0; t < list4.size(); t++) 
            System.out.println("" + list4.get(t)); 
        for (int t = 0; t < N; t++) 
            result += list2.get(t) * list4.get(t); 
        System.out.println("Получившийся вектор умножаем на свободные коэфициенты прямой задачи. Результат для Gmin: " + result);
        System.out.println("Результат для Fmax: " + max_dohod);
    }

    public static double delta_computing(double matrix[][], int a, int N, int perem) {//Расчет относительных оценок и оптимального плана
        double delta = 0;
        if (a != 0) {
            for (int i = 2; i < N + 2 - 1; i++) 
                delta += matrix[i][0] * matrix[i][a + 1]; 
            delta = delta - matrix[0][a + 1];
        } else if (a == 0) {
            for (int i = 2; i < N + 2 - 1; i++)
                delta += matrix[i][0] * matrix[i][perem + 3 - 1]; 
        }
        return delta;
    }

    public static int razresh_column(double matrix[][], int N, int perem) {//нахождение разрешающего столбца
        double[] mas;
        mas = new double[perem];
        for (int i = 0; i < perem; i++) {
            mas[i] = matrix[N + 3 - 1][i + 2];
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
    public static int razresh_string(double matrix[][], int razreshColumn, int N, int perem) {//нахождение разрешающей строки
        double[] mas; mas = new double[N + 2]; mas[0] = -0; mas[1] = -0;
double min_elem = 0; int index_min_elem;
        for (int i = 2; i < N + 2; i++) 
            mas[i] = matrix[i][perem + 2] / matrix[i][razreshColumn]; 
        min_elem = mas[2]; index_min_elem = 2;
        for (int j = 3; j < N + 2; j++) 
            if (mas[j] > 0 && mas[j] < min_elem) {
                min_elem = mas[j];
                index_min_elem = j;
            }
        return index_min_elem;
    }
    public static double[][] new_simpleks_matrix(double matrix[][], int strSipleks, int colum, int razreshColumn, int razreshString) {
        double matrix2[][];
        matrix2 = new double[strSipleks][colum];
        for (int i = 0; i < strSipleks; i++) 
            for (int j = 0; j < colum; j++) 
                matrix2[i][j] = matrix[i][j];
        matrix2[razreshString][0] = matrix[0][razreshColumn];
        matrix2[0][razreshColumn] = matrix[razreshString][0]; 
        matrix2[razreshString][1] = matrix[1][razreshColumn];
        matrix2[1][razreshColumn] = matrix[razreshString][1]; 
        double razreshElemetn = matrix[razreshString][razreshColumn]; 
        for (int k = 2; k <= colum - 1; k++) //разрешающая строка (просмотр столбцов) было к = 4 
        {
            if (k != razreshColumn) {
                matrix2[razreshString][k] = matrix[razreshString][k] / razreshElemetn;
            }
        }
        for (int k = 2; k <= strSipleks - 1; k++) //разрешающая столбец (просмотр строк) было к = 6
        {
            if (k != razreshString) {
                matrix2[k][razreshColumn] = (-1) * (matrix[k][razreshColumn] / razreshElemetn);
            }
        }
        matrix2[razreshString][razreshColumn] = 1 / razreshElemetn;
        for (int str = 2; str < strSipleks; str++) 
            for (int col = 2; col < colum; col++) //столбцы  было col < 5
                if (str != razreshString && col != razreshColumn) 
                    matrix2[str][col] = (matrix[str][col] * razreshElemetn - matrix[razreshString][col] * matrix[str][razreshColumn]) / razreshElemetn;
        return matrix2;
    }
    public static void inversion(double[][] A, int N) {
        double temp;double[][] E = new double[N][N]; 
        for (int i = 0; i < N; i++) 
            for (int j = 0; j < N; j++) 
                E[i][j] = 0f; 
                if (i == j) 
                    E[i][j] = 1f; 
        for (int k = 0; k < N; k++) {
            temp = A[k][k]; 
            for (int j = 0; j < N; j++) {
                A[k][j] /= temp;
                E[k][j] /= temp;
            }
            for (int i = k + 1; i < N; i++) {
                temp = A[i][k]; 
                for (int j = 0; j < N; j++) {
                    A[i][j] -= A[k][j] * temp;
                    E[i][j] -= E[k][j] * temp;
                }}}
        for (int k = N - 1; k > 0; k--) {
            for (int i = k - 1; i >= 0; i--) {
                temp = A[i][k]; 
                for (int j = 0; j < N; j++) {
                    A[i][j] -= A[k][j] * temp;
                    E[i][j] -= E[k][j] * temp;
                }}}

        for (int i = 0; i < N; i++) {
            for (int j = 0; j < N; j++) {
                A[i][j] = E[i][j];
            }}}

 public static double[][] transposeMatrix(double [][] m){
        double[][] temp = new double[m[0].length][m.length];
        for (int i = 0; i < m.length; i++)
            for (int j = 0; j < m[0].length; j++)
                temp[j][i] = m[i][j];
        return temp;
    }
}