using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace vex
{

    //makes renderable
    public interface Renderable
    {
        //has to have some corners obvs
        Vect3[] Vertices { get; set; }

        Color Color { get; set; }

        bool Is2D { get; set; }

        //has to be able to be projected to 2d - has to implement
        Renderable Project(float viewportWidth, float viewPortHeight, float viewPortCameraDistance, float zClippingDepth);

        //anything that has vertices should be able to be transformed by 4*4 matrix
        Renderable Transform(Matrix transform);
    }

    public class Triangle : Renderable
    {
        //ooh boy is that a long constant
        private readonly string BADLYDIMENSIONEDCONSTRUCTORARRAYEXCEPTIONMESSAGE = "The constructor array passed is not of length 3 or more.";

        //triangle vertices - if there aren't only 3, something's gone wrong
        public Vect3[] Vertices { get; set; }

        public Color Color { get; set; }

        public bool Is2D { get { return true; } set { } }

        //two constructors - i'll never use the second one probably but I might who knows 
        //i like polymorphism
        public Triangle(Vect3 _p1, Vect3 _p2, Vect3 _p3, Color _color)
        {
            Vertices = new Vect3[3];
            Vertices[0] = _p1; Vertices[1] = _p2; Vertices[2] = _p3;
            Color = _color;
        }
        public Triangle(Vect3[] _vertices, Color _color)
        {
            //error checking
            if (!(_vertices.Length >= 3))
            {
                throw new ArgumentException(BADLYDIMENSIONEDCONSTRUCTORARRAYEXCEPTIONMESSAGE);
            }
            Vertices = _vertices;
            Color = _color;
        }

        //project - callback to like that interface and that
        //viewPort width and height are the NDC canvas dimensions
        //viewportCameraDistance is the distance from the canvas to the camera
        //zClippingDepth is the distance from the camera at which "well now I'm not doing it"
        public Renderable Project(float viewportWidth, float viewPortHeight, float viewPortCameraDistance, float zClippingDepth)
        {
            return new Triangle(Vertices[0].Project(viewportWidth, viewPortHeight, viewPortCameraDistance, zClippingDepth),
                Vertices[1].Project(viewportWidth, viewPortHeight, viewPortCameraDistance, zClippingDepth),
                Vertices[2].Project(viewportWidth, viewPortHeight, viewPortCameraDistance, zClippingDepth), Color);
        }

        //transform
        public Renderable Transform(Matrix transform)
        {
            Triangle output;

            output = new Triangle(Vect3.FromColumnMatrix(Vertices[0].ToColumnMatrix().Multiply(transform)),
                Vect3.FromColumnMatrix(Vertices[1].ToColumnMatrix().Multiply(transform)),
                Vect3.FromColumnMatrix(Vertices[2].ToColumnMatrix().Multiply(transform)), Color);

            return output;
        }
    }

    public class Line : Renderable
    {
        //^^
        private readonly string BADLYDIMENSIONEDCONSTRUCTORARRAYEXCEPTIONMESSAGE = "The constructor array passed is not of length 3 or more.";

        //line vertices - if there aren't only 2, something's gone wrong
        public Vect3[] Vertices { get; set; }

        public Color Color { get; set; }

        public bool Is2D { get { return false; } set { } }

        //^^
        public Line(Vect3 _p1, Vect3 _p2, Color _color)
        {
            Vertices = new Vect3[2];
            Vertices[0] = _p1; Vertices[1] = _p2;
            Color = _color;
        }
        public Line(Vect3[] _vertices)
        {
            //error checking
            if (!(_vertices.Length >= 2))
            {
                throw new ArgumentException(BADLYDIMENSIONEDCONSTRUCTORARRAYEXCEPTIONMESSAGE);
            }
            Vertices = _vertices;
        }

        //^^
        public Renderable Project(float viewportWidth, float viewPortHeight, float viewPortCameraDistance, float zClippingDepth)
        {
            return new Line(Vertices[0].Project(viewportWidth, viewPortHeight, viewPortCameraDistance, zClippingDepth),
                Vertices[1].Project(viewportWidth, viewPortHeight, viewPortCameraDistance, zClippingDepth), Color);
        }

        //^^
        public Renderable Transform(Matrix transform)
        {
            Line output;

            output = new Line(Vect3.FromColumnMatrix(Vertices[0].ToColumnMatrix().Multiply(transform)),
                Vect3.FromColumnMatrix(Vertices[1].ToColumnMatrix().Multiply(transform)), Color);

            return output;
        }
    }
}
