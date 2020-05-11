using System;

namespace OopMatrix
{
    class Matrix
    {
        #region Приватные поля
        // приватные поля
        private double[,] matrix;
        private int rows;
        private int cols;
        #endregion
        #region Геттеры и сеттеры
        // геттеры, сеттеры
        public int Rows
        {
            get
            {
                return rows;
            }
            set
            {
                if (value > 0)
                    rows = value;
            }
        }
        public int Cols
        {
            get
            {
                return cols;
            }
            set
            {
                if (value > 0)
                    cols = value;
            }
        }
        #endregion
        #region Конструкторы
        // конструкторы
        public Matrix()
        {
            rows = 0;
            cols = 0;
            matrix = null;
        }
        public Matrix(int n)
        {
            rows = n;
            cols = n;
            matrix = new double[rows, cols];
        }
        public Matrix(int rows, int cols)
        {
            this.rows = rows;
            this.cols = cols;
            matrix = new double[rows, cols];
        }
        public Matrix(Matrix other)
        {
            this.rows = other.rows;
            this.cols = other.cols;
            for(int i = 0;i < rows; i++)
            {
                for(int j = 0;j < cols; j++)
                {
                    this[i, j] = other[i, j];
                }
            }
        }
        #endregion
        #region Перегруженные операторы
        // перегруженые операторы
        public double this[int i, int j]
        {
            get { return matrix[i, j]; }
            set { matrix[i, j] = value; }
        }
        public static Matrix operator +(Matrix one, Matrix two)
        {
            if(one.rows != two.rows &&
               one.cols != two.cols)
            {
                throw new ArgumentException();
            }
            Matrix res = new Matrix(one.rows, two.cols);
            for(int i = 0;i < one.rows; i++)
            {
                for(int j = 0;j < two.cols; j++)
                {
                    res[i, j] = one[i, j] + two[i, j];
                }
            }
            return res;
        }
        
        public static Matrix operator -(Matrix one, Matrix two)
        {
            if (one.rows != two.rows &&
               one.cols != two.cols)
            {
                throw new ArgumentException();
            }
            Matrix res = new Matrix(one.rows, two.cols);
            for (int i = 0; i < one.rows; i++)
            {
                for (int j = 0; j < two.cols; j++)
                {
                    res[i, j] = one[i, j] - two[i, j];
                }
            }
            return res;
        }
        public static bool operator !=(Matrix one, Matrix two) 
        {
            if (one.rows != two.rows &&
               one.cols != two.cols)
            {
                return true;
            }
            for (int i = 0; i < one.rows; i++)
            {
                for (int j = 0; j < two.cols; j++)
                {
                    if(one[i, j] != two[i, j])
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public static bool operator ==(Matrix one, Matrix two)
        {
            if (one.rows == two.rows &&
               one.cols == two.cols)
            {
                return true;
            }
            for (int i = 0; i < one.rows; i++)
            {
                for (int j = 0; j < two.cols; j++)
                {
                    if (one[i, j] == two[i, j])
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        // переопределенные Equals() и HashCode()
        public override bool Equals(object obj)
        {
            Matrix other = (Matrix)obj;
            if (this.rows != other.rows &&
               this.cols != other.cols)
            {
                return false;
            }
            for (int i = 0; i < this.rows; i++)
            {
                for (int j = 0; j < other.cols; j++)
                {
                    if (this[i, j] == other[i, j])
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        #endregion
        #region Транспонирование матрицы
        // нахождение транспонированной матрицы
        public Matrix Transpose()
        {
            Matrix TransposeMatrix = new Matrix(cols, rows);
            for(int i = 0;i < rows; i++)
            {
                for(int j = 0;j < cols; j++)
                {
                    TransposeMatrix[j, i] = matrix[i, j];
                }
            }
            return TransposeMatrix;
        }
        #endregion
        #region Нахождение обратной матрицы
        public Matrix Inverse(Matrix mA, uint round = 0)
        {
            if (mA.rows != mA.cols) throw new ArgumentException("Обратная матрица существует только для квадратных, невырожденных, матриц.");
            Matrix matrix = new Matrix(mA.rows);
            double determinant = Determinant(mA);

            if (determinant == 0) return matrix; //Если определитель == 0 - матрица вырожденная

            for (int i = 0; i < mA.rows; i++)
            {
                for (int t = 0; t < mA.cols; t++)
                {
                    Matrix tmp = mA.Exclude(i, t);
                    matrix[t, i] = round == 0 ? 
                        (1 / determinant) * Determinant(tmp) : 
                        Math.Round(((1 / determinant) * Determinant(tmp)), (int)round, MidpointRounding.ToEven);
                }
            }
            return matrix;
        }
        public double Determinant(Matrix mA)
        {
            if (mA.rows != mA.cols) throw new ArgumentException("Вычисление определителя возможно только для квадратных матриц.");
            Matrix matrix = mA.Clone();
            double det = 1;
            int order = mA.rows;

            for (int i = 0; i < order - 1; i++)
            {
                double[] masterRow = matrix.GetRow(i);
                det *= masterRow[i];
                if (det == 0) return 0;
                for (int t = i + 1; t < order; t++)
                {
                    double[] slaveRow = matrix.GetRow(t);
                    double[] tmp = MulArrayConst(masterRow, slaveRow[i] / masterRow[i]);
                    double[] source = matrix.GetRow(t);
                    matrix.SetRow(SubArray(source, tmp), t);
                }
            }
            det *= matrix[order - 1, order - 1];

            return det;
        }


        #endregion
        #region Вывод и заполнение матрицы
        // вывод матрицы
        public void Print()
        {
            for(int i = 0;i < rows; i++)
            {
                for(int j = 0;j < cols; j++)
                {
                    Console.Write(matrix[i, j] + "\t");
                }
                Console.WriteLine();
            }
        }
        // рандомное заполение
        public void RandomFill(double min, double max)
        {
            Random random = new Random();
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    matrix[i, j] = Math.Round(random.NextDouble() * (max - min) + min, 2);
                }
            }
        }
        #endregion
        #region Вспомогательные функции
        public Matrix Exclude(int row, int column)
        {
            if (row > rows || column > cols) throw new IndexOutOfRangeException("Строка или столбец не принадлежат матрице.");
            Matrix ret = new Matrix(rows - 1, cols - 1);
            int offsetX = 0;
            for (int i = 0; i < rows; i++)
            {
                int offsetY = 0;
                if (i == row) { offsetX++; continue; }
                for (int t = 0; t < cols; t++)
                {
                    if (t == column) { offsetY++; continue; }
                    ret[i - offsetX, t - offsetY] = this[i, t];
                }
            }
            return ret;
        }
        public Matrix Clone()
        {
            Matrix clone = new Matrix(rows, cols);
            for(int i = 0;i < rows; i++)
            {
                for(int j = 0;j < cols; j++)
                {
                    clone[i, j] = matrix[i, j];
                }
            }
            return clone;
        }
        public double[] GetRow(int row)
        {
            if (row >= rows) throw new IndexOutOfRangeException("Индекс строки не принадлежит массиву.");
            double[] ret = new double[cols];
            for (int i = 0; i < cols; i++)
                ret[i] = matrix[row, i];

            return ret;
        }
        public static double[] MulArrayConst(double[] array, double number)
        {
            double[] ret = (double[])array.Clone();
            for (int i = 0; i < ret.Length; i++)
                ret[i] *= number;
            return ret;
        }
        public void SetRow(double[] rowValues, int row)
        {
            if (row >= rows) throw new IndexOutOfRangeException("Индекс строки не принадлежит массиву.");
            for (int i = 0; i < (cols > rowValues.Length ? rowValues.Length : cols); i++)
                matrix[row, i] = rowValues[i];
        }
        public double[] SubArray(double[] A, double[] B)
        {
            double[] ret = (double[])A.Clone();
            for (int i = 0; i < (A.Length > B.Length ? A.Length : B.Length); i++)
                ret[i] -= B[i];
            return ret;
        }
        #endregion
    }
}
