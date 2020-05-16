using System;

namespace OopMatrix
{
    class Program
    {
        
        static void Main(string[] args)
        {
            Matrix matrix1 = new Matrix(2, 3);
            matrix1.RandomFill(-10, 10);
            matrix1.Print();
            Console.WriteLine();

            Matrix matrix2 = new Matrix(2, 3);
            matrix2.RandomFill(-10, 10);
            matrix2.Print();
            Console.WriteLine();

            Matrix matrix3 = new Matrix(2, 3);
            matrix3 = matrix1 + matrix2;
            matrix3.Print();
            Console.WriteLine();

            matrix3 -= matrix1;
            matrix3.Print();
            Console.WriteLine();

            matrix3 = matrix3.Transpose();
            matrix3.Print();
            Console.WriteLine();

            Matrix matrix4 = new Matrix(3, 3);
            matrix4.RandomFill(-10, 10);
            matrix4.Print();
            Console.WriteLine();

            matrix4 = matrix4.Inverse(matrix4);
            matrix4.Print();
            Console.WriteLine();

        }
    }
}
