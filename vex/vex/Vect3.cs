using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vex
{

    //general purpose class for position and direction vectors - this class should work for 2d vectors if z coord is set to 0 (but i like using -1 cus chaos)
    public class Vect3
    {
        public float X, Y, Z;

        public Vect3(float xIn, float yIn, float zIn)
        {
            X = xIn; Y = yIn; Z = zIn;
        }

        //gets magnitude - should also work for 2d since 0^2 = 0
        public float Mag()
        {
            return Convert.ToSingle(Math.Sqrt(Math.Pow(X, 2) + Math.Pow(Y, 2) + Math.Pow(Z, 2)));
        }

        //need that dot product
        public float Dot(Vect3 input)
        {
            return X * input.X + Y * input.Y + Z * input.Z;
        }
        //a 2d dot product
        public float Dot2D(Vect3 input)
        {
            return X * input.X + Y * input.Y;
        }

        //shallow == and matching != aww in love and that
        public static bool operator ==(Vect3 v1, Vect3 v2)
        {
            if (v1.X == v2.X && v1.Y == v2.Y && v1.Z == v2.Z)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool operator !=(Vect3 v1, Vect3 v2)
        {
            if (v1.X == v2.X && v1.Y == v2.Y && v1.Z == v2.Z)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        //define minus & plus while we're at it
        public static Vect3 operator -(Vect3 v1, Vect3 v2)
        {
            return new Vect3(v1.X - v2.X, v1.Y - v2.Y, v1.Z - v2.Z);
        }
        public static Vect3 operator +(Vect3 v1, Vect3 v2)
        {
            return new Vect3(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z);
        }

        //takes 3d vector and projects to NDC
        public Vect3 Project(float viewportWidth, float viewportHeight, float viewportCameraDistance, float zClippingDepth)
        {
            //for similar triangle projection: scales X and Y through ratio of Zs
            float scalingFactor;
            //output values
            float xOut, yOut;

            //-ve because camera looks down -ve Z direction
            scalingFactor = -viewportCameraDistance / Z;
            xOut = X * scalingFactor; yOut = Y * scalingFactor;

            return new Vect3(xOut, yOut, Z);
        }

        //converts to column matrix (with 1 at bottom for that good 4d transformation) or back
        public Matrix ToColumnMatrix()
        {
            return new Matrix(new float[,] { { X }, { Y }, { Z }, { 1 } });
        }
        public static Vect3 FromColumnMatrix(Matrix input)
        {
            return new Vect3(input.Peek(0, 0), input.Peek(1, 0), input.Peek(2, 0));
        }
        public Vect3 Clone()
        {
            return new Vect3(X, Y, Z);
        }
    }
}
