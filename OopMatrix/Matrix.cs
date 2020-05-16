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
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
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
                    if (one[i, j] != two[i, j])
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
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    TransposeMatrix[j, i] = matrix[i, j];
                }
            }
            return TransposeMatrix;
        }
        public Matrix Transpose(Matrix matr)
        {
            Matrix TransposeMatrix = new Matrix(matr.cols, matr.rows);
            for (int i = 0; i < matr.rows; i++)
            {
                for (int j = 0; j < matr.cols; j++)
                {
                    TransposeMatrix[j, i] = matr[i, j];
                }
            }
            return TransposeMatrix;
        }
        #endregion
        #region Нахождение обратной матрицы
        public double Determinant(Matrix matr)
        {
            if (!IsSquare(matr))
                throw new NoSquareException();
            if(matr.IfSquareThenGetSize() == 1)
                return matrix[0, 0];

            if (matr.IfSquareThenGetSize() == 2)
                return (matr[0, 0] * matr[1, 1]) - (matr[0, 1] * matr[1, 0]);
            double sum = 0.0;
            for(int i = 0;i < matr.cols; i++)
            {
                sum += ChangeSign(i) * matr[0, i] * Determinant(CreateSubMatrix(matr, 0, i));
            }
            return sum;

        }
        public Matrix Cofactor(Matrix matr)
        {
            if (!IsSquare(matr))
                throw new NoSquareException();
            Matrix mat = new Matrix(matr.rows, matr.cols);
            for(int i = 0;i < matr.rows; i++)
            {
                for(int j = 0;j < matr.cols; j++)
                {
                    mat[i, j] = ChangeSign(i) * ChangeSign(j) 
                        * Determinant(CreateSubMatrix(matr, i, j));
                }
            }
            return mat;
        }
        public Matrix Inverse(Matrix matr)
        {
            if (!IsSquare(matr))
                throw new NoSquareException();
            return (Transpose(Cofactor(matr)).MultiplyByConstant(1.0 / Determinant(matr)));
        }
        #endregion
        #region Вывод и заполнение матрицы
        // вывод матрицы
        public void Print()
        {
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
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
        private void SetValueAt(int row, int col, double value)
        {
            matrix[row, col] = value;
        }
        private bool IsSquare()
        {
            return rows == cols;
        }
        private bool IsSquare(Matrix matr)
        {
            return matr.rows == matr.cols;
        }
            
        private Matrix MultiplyByConstant(double constant)
        {
            Matrix MultipliedMatrix = new Matrix(rows, cols);
            for(int i = 0;i < rows; i++)
            {
                for(int j = 0;j < cols; j++)
                {
                    MultipliedMatrix[i, j] *= constant;
                }
            }
            return MultipliedMatrix;
        }
        private Matrix CreateSubMatrix(Matrix matr, int ExcludingRow, int ExcludingCol)
        {
            Matrix SubMatrix = new Matrix(rows - 1, cols - 1);
            int row = -1;
            for(int i = 0;i < rows; i++)
            {
                if (i == ExcludingRow)
                    continue;
                row++;
                int col = -1;
                for(int j = 0;j < cols; j++)
                {
                    if (j == ExcludingCol)
                        continue;
                    SubMatrix[row, ++col] = matr[i, j];
                }
            }
            return SubMatrix;
        }
        private int ChangeSign(int index)
        {
            if (index % 2 == 0)
                return 1;
            return -1;
        }
        private int IfSquareThenGetSize()
        {
            if (!IsSquare())
                throw new NoSquareException();
            return IsSquare() ? rows : -1; 
        }
        #endregion
    }
}
