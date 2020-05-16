using System;
using System.Collections.Generic;
using System.Text;

namespace OopMatrix
{
    class NoSquareException : Exception
    {
        public NoSquareException()
            : base("Матрица должна быть квадратной!")
        {

        }
    }
}
