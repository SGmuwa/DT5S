using System;

namespace Analitic_Hierarchy_Process
{
    class Program
    {
        public static void main(String[] args)
        {
            int n = 5; int k = 10;
            double SI = 1.12;
            System.out.println("Матрица парных суждений");
            Double[][] matrix1 ={
        {1.0,1.0,2.0,4.0,5.0},
        {null,1.0,3.0,5.0,5.0},
        {null,null,1.0,3.0,5.0},
        {null,null,null,1.0,5.0},
        {null,null,null,null,1.0},
        };
            for (int j = 0; j < 5; j++)
                for (int i = j + 1; i < 5; i++)
                {
                    matrix1[i][j] = (double)1 / matrix1[j][i];
                }
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    System.out.format(matrix1[i][j] + "\t" + "\t");
                }
                System.out.println();
            }
            double V1 = Math.pow(matrix1[0][0] * matrix1[0][1] * matrix1[0][2] * matrix1[0][3] * matrix1[0][4], (1.0 / 5));
            double V2 = Math.pow((1 / matrix1[0][1]) * matrix1[1][1] * matrix1[1][2] * matrix1[1][3] * matrix1[1][4], (1.0 / 5));
            double V3 = Math.pow((1 / matrix1[0][2]) * (1 / matrix1[1][2]) * matrix1[2][2] * matrix1[2][3] * matrix1[2][4], (1.0 / 5));
            double V4 = Math.pow((1 / matrix1[0][3]) * (1 / matrix1[1][3]) * (1 / matrix1[2][3]) * matrix1[3][3] * matrix1[3][4], (1.0 / 5));
            double V5 = Math.pow((1 / matrix1[0][4]) * (1 / matrix1[1][4]) * (1 / matrix1[2][4]) * (1 / matrix1[3][4]) * matrix1[4][4], (1.0 / 5));
            double sumVi = V1 + V2 + V3 + V4 + V5;
            double Wc1 = V1 / sumVi;
            double Wc2 = V2 / sumVi;
            double Wc3 = V3 / sumVi;
            double Wc4 = V4 / sumVi;
            double Wc5 = V5 / sumVi;
            System.out.println("Wci вектор приоритетов: " + Wc1 + ", " + Wc2 + ", " + Wc3 + ", " + Wc4 + ", " + Wc5 + ", ");
            double S1 = matrix1[0][0] + (1 / matrix1[0][1]) + (1 / matrix1[0][2]) + (1 / matrix1[0][3]) + (1 / matrix1[0][4]);
            double S2 = matrix1[0][1] + matrix1[1][1] + (1 / matrix1[1][2]) + (1 / matrix1[1][3]) + (1 / matrix1[1][4]);
            double S3 = matrix1[0][2] + matrix1[1][2] + matrix1[2][2] + (1 / matrix1[2][3]) + (1 / matrix1[2][4]);
            double S4 = matrix1[0][3] + matrix1[1][3] + matrix1[2][3] + matrix1[3][3] + (1 / matrix1[3][4]);
            double S5 = matrix1[0][4] + matrix1[1][4] + matrix1[2][4] + matrix1[3][4] + matrix1[4][4];
            double P1 = S1 * Wc1;
            double P2 = S2 * Wc2;
            double P3 = S3 * Wc3;
            double P4 = S4 * Wc4;
            double P5 = S5 * Wc5;
            double Ymax = P1 + P2 + P3 + P4 + P5;
            System.out.println("Максимальное среднее значение: " + Ymax);
            double IS = (Ymax - n) / (n - 1);
            double OS = IS / SI;
            System.out.println("Отношение согласованности: " + OS);
            System.out.println("");
            System.out.println("");

            ///Критерий K1
            System.out.println("Критерий К1");
            Double[][] matrixK1 ={
        {1.0,null,null,null,null},
        {3.0,1.0,null,null,null},
        {2.0,3.0,1.0,null,null},
        {3.0,5.0,3.0,1.0,null,},
        {3.0,2.0,2.0,3.0,1.0},
    };

            for (int i = 0; i < 5; i++)
                for (int j = i + 1; j < 5; j++)
                {
                    matrixK1[i][j] = (double)1 / matrixK1[j][i];
                }
            List<Double> listK1 = new ArrayList<Double>();
            listK1 = function(matrixK1, 5);

            System.out.println("");
            System.out.println("");

            //Критерий К2
            System.out.println("Критерий К2");
            Double[][] matrixK2 ={
        {1.0,null,null,null,null},
        {1.0,1.0,null,null,null},
        {3.0,3.0,1.0,null,null},
        {2.0,2.0,3.0,1.0,null},
        {2.0,2.0,3.0,2.0,1.0},

    };
            for (int i = 0; i < 5; i++)
                for (int j = i + 1; j < 5; j++)
                {
                    matrixK2[i][j] = (double)1 / matrixK2[j][i];
                }
            List<Double> listK2 = new ArrayList<Double>();
            listK2 = function(matrixK2, 5);

            System.out.println(""); System.out.println("");

            ///Критерий K3
            System.out.println("Критерий К3");
            Double[][] matrixK3 ={
        {1.0,null,null,null,null},
        {3.0,1.0,null,null,null},
        {1.0,3.0,1.0,null,null},
        {3.0,1.0,3.0,1.0,null},
        {3.0,1.0,3.0,1.0,1.0},

    };

            for (int i = 0; i < 5; i++)
                for (int j = i + 1; j < 5; j++)
                {
                    matrixK3[i][j] = (double)1 / matrixK3[j][i];
                }

            List<Double> listK3 = new ArrayList<Double>();
            listK3 = function(matrixK3, 5);
            System.out.println(""); System.out.println("");
            ///Критерий K4
            System.out.println("Критерий K4");
            Double[][] matrixK4 ={
        {1.0,null,null,null,null},
        {1.0,1.0,null,null,null},
        {1.0,1.0,1.0,null,null},
        {3.0,2.0,3.0,1.0,null},
        {1.0,1.0,1.0,2.0,1.0},

    };
            for (int i = 0; i < 5; i++)
                for (int j = i + 1; j < 5; j++)
                {
                    matrixK4[i][j] = (double)1 / matrixK4[j][i];
                }
            List<Double> listK4 = new ArrayList<Double>();
            listK4 = function(matrixK4, 5);

            System.out.println(""); System.out.println("");
            ///Критерий K5
            System.out.println("Критерий K5");
            Double[][] matrixK5 ={
        {1.0,null,null,null,null},
        {1.0,1.0,null,null,null},
        {1.0,1.0,1.0,null,null},
        {1.0,3.0,3.0,1.0,null},
        {1.0,1.0,1.0,3.0,1.0},
    };
            for (int i = 0; i < 5; i++)
                for (int j = i + 1; j < 5; j++)
                {
                    matrixK5[i][j] = (double)1 / matrixK5[j][i];
                }
            List<Double> listK5 = new ArrayList<Double>();
            listK5 = function(matrixK5, 5);
            System.out.println(""); System.out.println("");
            double W1 = Wc1 * listK1.get(1) + Wc2 * listK2.get(1) + Wc3 * listK3.get(1) + Wc4 * listK4.get(1) + Wc5 * listK5.get(1);
            double W2 = Wc1 * listK1.get(2) + Wc2 * listK2.get(2) + Wc3 * listK3.get(2) + Wc4 * listK4.get(2) + Wc5 * listK5.get(2);
            double W3 = Wc1 * listK1.get(3) + Wc2 * listK2.get(3) + Wc3 * listK3.get(3) + Wc4 * listK4.get(3) + Wc5 * listK5.get(3);
            double W4 = Wc1 * listK1.get(4) + Wc2 * listK2.get(4) + Wc3 * listK3.get(4) + Wc4 * listK4.get(4) + Wc5 * listK5.get(4);
            double W5 = Wc1 * listK1.get(5) + Wc2 * listK2.get(5) + Wc3 * listK3.get(5) + Wc4 * listK4.get(5) + Wc5 * listK5.get(5);
            System.out.println("W (вектор приоритетов):  " + W1 + ", " + W2 + ", " + W3 + ", " + W4 + ", " + W5);
        }

        public static List<Double> function(Double array[][], int k)
        {
            List<Double> list = new ArrayList<Double>();
            double V1 = 1; double V2 = 1; double V3 = 1; double V4 = 1; double V5 = 1;
            //Вычисление вектора приоритетов по данной матрице 
            //по каждой строке
            for (int i = 0; i < k; i++)
                for (int j = 0; j < k; j++)
                {
                    if (i == 0) V1 *= array[i][j];
                    if (i == 1) V2 *= array[i][j];
                    if (i == 2) V3 *= array[i][j];
                    if (i == 3) V4 *= array[i][j];
                    if (i == 4) V5 *= array[i][j];
                }
            V1 = Math.pow(V1, 1.0 / 5); V2 = Math.pow(V2, 1.0 / 5); V3 = Math.pow(V3, 1.0 / 5); V4 = Math.pow(V4, 1.0 / 5); V5 = Math.pow(V5, 1.0 / 5);
            //нормирующий коэффициент           
            double sumVi = V1 + V2 + V3 + V4 + V5;
            double Wa1 = V1 / sumVi;
            double Wa2 = V2 / sumVi;
            double Wa3 = V3 / sumVi;
            double Wa4 = V4 / sumVi;
            double Wa5 = V5 / sumVi;
            System.out.println("Вектор приоритетов Wki:");
            System.out.println("Wki (вектор приоритетов):  " + Wa1 + ", " + Wa2 + ", " + Wa3 + ", " + Wa4 + ", " + Wa5);
            double S1, S2, S3, S4, S5;
            S1 = 0; S2 = 0; S3 = 0; S4 = 0; S5 = 0;
            for (int i = 0; i < k; i++)
                for (int j = 0; j < k; j++)
                {
                    if (j == 0) S1 += array[i][j];
                    if (j == 1) S2 += array[i][j];
                    if (j == 2) S3 += array[i][j];
                    if (j == 3) S4 += array[i][j];
                    if (j == 4) S5 += array[i][j];
                }
            double Р1 = S1 * Wa1;
            double Р2 = S2 * Wa2;
            double Р3 = S3 * Wa3;
            double P4 = S4 * Wa4;
            double Р5 = S5 * Wa5;
            double Ymax = Р1 + Р2 + Р3 + P4 + Р5;

            double IS = (Ymax - k) / (k - 1);
            double OS = IS / 1.12;//Отклонение от согласованности (Индекс Согласованности (ИС)
            System.out.println("Ymax (максимальное среднее значение) = " + Ymax);
            System.out.println("ОС = " + OS);//Отношение согласованности (OC)
            list.add(OS);
            list.add(Wa1);
            list.add(Wa2);
            list.add(Wa3);
            list.add(Wa4);
            list.add(Wa5);
            return list;
        }
    }
}
