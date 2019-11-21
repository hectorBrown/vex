using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vex
{
    public class Matrix
    {
        //try and read that
        private readonly string MULTIPLICATIONINVALIDDIMENSIONSEXCEPTIONMESSAGE = "The matrices passed do not have compatible dimensions.",
            OUTOFMATRIXBOUNDSEXCEPTIONMESSAGE = "The row and/or column values passed are outside of the bounds of the matrix.";
        private float[,] matrix;

        //creates empty matrix
        public Matrix(int rows, int columns)
        {
            matrix = new float[rows, columns];
        }
        //quickly creates populated matrix
        public Matrix(float[,] population)
        {
            matrix = population;
        }

        //allows access to row and column size outside object
        public int CountDimension(int dimension)
        {
            return matrix.GetLength(dimension);
        }

        //adjusts single value of matrix
        public void Adjust(int row, int column, float value)
        {
            //error checking
            if (!(row >= 0 && column >= 0 && row < CountDimension(0) && column < CountDimension(1)))
            {
                throw new ArgumentException(OUTOFMATRIXBOUNDSEXCEPTIONMESSAGE);
            }
            matrix[row, column] = value;
        }

        //views a part of the matrix
        public float Peek(int row, int column)
        {
            //error checking
            if (!(row >= 0 && column >= 0 && row < CountDimension(0) && column < CountDimension(1)))
            {
                throw new ArgumentException(OUTOFMATRIXBOUNDSEXCEPTIONMESSAGE);
            }

            return matrix[row, column];
        }

        //main event - multiplication method
        public Matrix Multiply(Matrix input)
        {
            float[,] rawResult;

            //error checking
            if (!(input.CountDimension(1) == CountDimension(0)))
            {
                throw new ArgumentException(MULTIPLICATIONINVALIDDIMENSIONSEXCEPTIONMESSAGE);
            }

            //creation of raw result array
            rawResult = new float[input.CountDimension(0), CountDimension(1)];

            //perform multiplication
            for (int row = 0; row < rawResult.GetLength(0); row++)
            {
                for (int column = 0; column < rawResult.GetLength(1); column++)
                {
                    //for cell, will be added to repeatedly
                    float value = 0;

                    //scroll along row for input or column for host matrix
                    for (int i = 0; i < input.CountDimension(1); i++)
                    {
                        value += input.Peek(row, i) * Peek(i, column);
                    }

                    rawResult[row, column] = value;
                }
            }

            //returns
            return new Matrix(rawResult);
        }
    }
}
