using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;

namespace vex
{
    public partial class FRM_render : Form
    {
        private readonly float CONTROLLEDROTATIONSCALING = 0.01f;
        private readonly float CONTROLLEDSCALESCALING = 0.0005f;
        //can't be used with block efficiency
        //private readonly int RASTERIZERDENSITY = 1;
        //8 is used in OpenGL so keep this const
        private readonly int RASTERIZERBLOCKSIZE = 8;
        //bounding box for shapes#
        //may have to initiate at lower scaling value
        private readonly float BOUNDINGBOXSIZE = 10;

        //diagnostics
        private int frames = 0;


        //fake infinity for depth buffer - just a whopping NDC value, increase if you hit problemos
        //or use double infinity value
        private readonly float INFINITY = 10000;

        //variables that define viewport width, height and camera distance
        private readonly float VPWIDTH = 1, VPHEIGHT = 1, CTOVPZ = 1;

        //clipping distance to help with z buffer - not being used rn (don't think i need to)
        private readonly float ZCLIPPING = 10;

        //catch all of renderable items that is passed to PB_main_Paint() - currently expected to just be lines and tris but who knows?
        private List<Renderable> render;

        //global zBuffer (genned in TIM_render, used in graphics) and frameBuffer
        float[,] zBuffer;
        //Image frameBuffer;

        //Points that define rotation line drawn

        //used as Points that mark where the line drawn by the mouse is, use this to calculate live rotation data
        private Point rotationBase, rotationHover;

        //time elapsed t / ms
        //private int t;

        //rotation angles - z is never rotated
        private float angleX, angleY;
        //base angles are adjusted on mouse up, so that when the rotation starts recording again, it is from the previous state
        private float baseAngleX, baseAngleY;

        //scaling for zooming
        private float scaleFactor;

        //test array of triangles
        private Triangle[] testTris;

        public FRM_render()
        {
            InitializeComponent();
            //t = 0;
            render = new List<Renderable>();
            baseAngleX = 0; baseAngleY = 0;
            scaleFactor = 1;
            //frameBuffer = new Bitmap(PB_main.Width, PB_main.Height);
        }

        private void FRM_render_Load(object sender, EventArgs e)
        {
            //points and triangles defined for a test cube - may have to be transformed for matrix operations
            //Vect3[] testCube = new Vect3[]
            //{
            //    new Vect3(-0.25f, -0.25f, -1),
            //    new Vect3(-0.25f, 0.25f, -1),
            //    new Vect3(0.25f, -0.25f, -1),
            //    new Vect3(0.25f, 0.25f, -1),
            //    new Vect3(-0.25f, -0.25f, -1.5f),
            //    new Vect3(-0.25f, 0.25f, -1.5f),
            //    new Vect3(0.25f, -0.25f, -1.5f),
            //    new Vect3(0.25f, 0.25f, -1.5f)
            //};
            //Vect3[] testCube = new Vect3[]
            //{
            //    //new Vect3(-0.25f, -0.25f, 0.25f),
            //    new Vect3(0, 0, 0),
            //    new Vect3(-0.25f, 0.25f, 0.25f),
            //    new Vect3(0.25f, -0.25f, 0.25f),
            //    new Vect3(0.25f, 0.25f, 0.25f),
            //    new Vect3(-0.25f, -0.25f, -0.25f),
            //    new Vect3(-0.25f, 0.25f, -0.25f),
            //    new Vect3(0.25f, -0.25f, -0.25f),
            //    new Vect3(0.25f, 0.25f, -0.25f)
            //};
            //testTris = new Triangle[]
            //{
            //    new Triangle(testCube[0], testCube[3], testCube[1], Color.MediumPurple),
            //    new Triangle(testCube[2], testCube[3], testCube[0], Color.MediumPurple),
            //    new Triangle(testCube[7], testCube[6], testCube[5], Color.Blue),
            //    new Triangle(testCube[4], testCube[5], testCube[6], Color.Blue),
            //    new Triangle(testCube[0], testCube[4], testCube[2], Color.Red),
            //    new Triangle(testCube[2], testCube[4], testCube[6], Color.Red),
            //    new Triangle(testCube[5], testCube[1], testCube[7], Color.White),
            //    new Triangle(testCube[7], testCube[1], testCube[3], Color.White),
            //    new Triangle(testCube[5], testCube[4], testCube[1], Color.Yellow),
            //    new Triangle(testCube[1], testCube[4], testCube[0], Color.Yellow),
            //    new Triangle(testCube[3], testCube[2], testCube[7], Color.Orange),
            //    new Triangle(testCube[7], testCube[2], testCube[6], Color.Orange)
            //    //new Triangle(new Vect3(0.25f, 0.25f, 0.25f), new Vect3(-0.25f, 0, -0.25f), new Vect3(0.25f, -0.25f, 0.25f), Color.Red),
            //    //new Triangle(new Vect3(-0.25f, 0.25f, 0.25f), new Vect3(-0.25f, -0.25f, 0.25f), new Vect3(0.25f, 0, -0.25f), Color.Blue)
            //};

            List<Triangle> testTriList = new List<Triangle>();
            //testTriList.AddRange(Construct.Square(new Vect3(-0.1f, -0.1f, -0.1f), 0.2f, Color.Blue));
            testTriList.AddRange(Construct.Cube(new Vect3(0.1f, 0.1f, 0.1f), 0.1f, Color.Blue));
            testTriList.AddRange(Construct.Cube(new Vect3(-0.1f, -0.1f, -0.1f), 0.1f, Color.Red));
            //testTriList.AddRange(Construct.Cube(new Vect3(0.1f, -0.1f, -0.2f), 0.5f, Color.Red));
            testTris = testTriList.ToArray();

            //set up mousewheel handler
            MouseWheel += new MouseEventHandler(Scroll);
        }

        private void PB_main_Paint(object sender, PaintEventArgs e)
        {
            //Graphics g = Graphics.FromImage(frameBuffer);
            //get graphics from PB - not framebuffer because bmp is inefficient
            Graphics g = e.Graphics;
            //for each "thing"
            //this section handles surfaces

            foreach (Renderable entity in render)
            {
                //make sure its not 1d (a line)
                if (entity.Is2D)
                {
                    //covers every pixel for triangles - doesn't bother with whole screen
                    Rectangle rectangle = BoundingBox((Triangle)entity);
                    //2d - xScroll is value from 0 to width of bounding box, added to init x value to create screen coords
                    //same goes for yScroll
                    //RASTERIZERBLOCKSIZE is the size of blocks that can be drawn all at once
                    //this method scrolls through each 8x8 and then, if it finds that all four vertices are not in the triangle
                    //and of the right depth, it will go into individual pixel mode

                    for (int xScroll = 0; xScroll < rectangle.Width; xScroll += RASTERIZERBLOCKSIZE)
                    {
                        for (int yScroll = 0; yScroll < rectangle.Height; yScroll += RASTERIZERBLOCKSIZE)
                        {

                            int x = rectangle.Left + xScroll;
                            int y = rectangle.Top + yScroll;
                            //IsInsideTriangle bit checks that any of the corners are inside before painting any of the block
                            //!(SameSide) bit makes sure that blocks where all four corners are outside but have some part of them
                            //inside the triangle aren't ignored


                            if ((IsInsideTriangle(new Vect3(x, y, -1), (Triangle)entity)
                                || IsInsideTriangle(new Vect3(x + RASTERIZERBLOCKSIZE, y, -1), (Triangle)entity)
                                || IsInsideTriangle(new Vect3(x, y + RASTERIZERBLOCKSIZE, -1), (Triangle)entity)
                                || IsInsideTriangle(new Vect3(x + RASTERIZERBLOCKSIZE, y + RASTERIZERBLOCKSIZE, -1), (Triangle)entity))
                                || !(SameSide((Triangle)entity, new PointF(x, y), new PointF(x + RASTERIZERBLOCKSIZE, y))
                                && SameSide((Triangle)entity, new PointF(x + RASTERIZERBLOCKSIZE, y), new PointF(x, y + RASTERIZERBLOCKSIZE))
                                && SameSide((Triangle)entity, new PointF(x, y + RASTERIZERBLOCKSIZE), new PointF(x + RASTERIZERBLOCKSIZE, y + RASTERIZERBLOCKSIZE))))
                            {



                                //general purpose depth var - used throughout
                                float z;
                                //checks for array exceptions
                                bool safe = true;
                                safe &= x + RASTERIZERBLOCKSIZE < PB_main.Width;
                                safe &= y + RASTERIZERBLOCKSIZE < PB_main.Height;

                                //handles block
                                //CHecks that all four corners are inside the triangle before painting the block
                                //also checks that all the depths are valid

                                if (safe
                                    && IsInsideTriangle(new Vect3(x, y, -1), (Triangle)entity)
                                    && IsInsideTriangle(new Vect3(x + RASTERIZERBLOCKSIZE, y, -1), (Triangle)entity)
                                    && IsInsideTriangle(new Vect3(x, y + RASTERIZERBLOCKSIZE, -1), (Triangle)entity)
                                    && IsInsideTriangle(new Vect3(x + RASTERIZERBLOCKSIZE, y + RASTERIZERBLOCKSIZE, -1), (Triangle)entity)
                                    && GetZ(new PointF(x, y), (Triangle)entity) < zBuffer[x, y]
                                    && GetZ(new PointF(x + RASTERIZERBLOCKSIZE, y), (Triangle)entity) < zBuffer[x + RASTERIZERBLOCKSIZE, y]
                                    && GetZ(new PointF(x, y + RASTERIZERBLOCKSIZE), (Triangle)entity) < zBuffer[x, y + RASTERIZERBLOCKSIZE]
                                    && GetZ(new PointF(x + RASTERIZERBLOCKSIZE, y + RASTERIZERBLOCKSIZE), (Triangle)entity) < zBuffer[x + RASTERIZERBLOCKSIZE, y + RASTERIZERBLOCKSIZE])
                                {
                                    //just does the main part of all of this
                                    g.FillRectangle(new SolidBrush(entity.Color), new Rectangle(x, y, RASTERIZERBLOCKSIZE, RASTERIZERBLOCKSIZE));
                                    //its only after commenting that I realise how ridic this all is
                                    //this part scrolls through the whole block and gets depth values for each pixel, applies
                                    //them to the z buffer
                                    //this remains kind of efficient because the painting is the intensive part
                                    //COULD BE IMPROVED PROBABLY

                                    for (int zX = x; zX < x + RASTERIZERBLOCKSIZE; zX++)
                                    {
                                        for (int zY = y; zY < y + RASTERIZERBLOCKSIZE; zY++)
                                        {
                                            z = GetZ(new PointF(zX, zY), (Triangle)entity);
                                            if (zX < PB_main.Width && zY < PB_main.Height
                                                && zX > 0 && zY > 0)
                                            {
                                                zBuffer[zX, zY] = z;
                                            }
                                        }
                                    }

                                }
                                //handles subpixels - goes through every pixel in the 8x8 and checks and paints individually
                                //this happens if >1 but <3 corners are in the triangle, or all the points of the box are not
                                //on the same side of every triangle edge
                                else
                                {
                                    //subX for subpixel 

                                    for (int subX = x; subX < x + RASTERIZERBLOCKSIZE; subX++)
                                    {
                                        for (int subY = y; subY < y + RASTERIZERBLOCKSIZE; subY++)
                                        {
                                            //just checks there will be no exceptions - some of these will be sliced by the edge of the screen
                                            if (subX < PB_main.Width && subY < PB_main.Height
                                                && subX > 0 && subY > 0)
                                            {
                                                //i told you it would be used
                                                z = GetZ(new PointF(subX, subY), (Triangle)entity);
                                                //standard check for any pixel
                                                if (IsInsideTriangle(new Vect3(subX, subY, -1), (Triangle)entity)
                                                    && z < zBuffer[subX, subY])
                                                {
                                                    //if it is visible, change zBuffer and draw single pixel
                                                    zBuffer[subX, subY] = z;
                                                    g.FillRectangle(new SolidBrush(entity.Color), new Rectangle(subX, subY, 1, 1));
                                                }
                                            }
                                        }
                                    }

                                }
                                //if (z < zBuffer[x, y])
                                //{
                                //    //r = Convert.ToInt32(entity.Color.R - (255 * (z / 1)));
                                //    //g = Convert.ToInt32(entity.Color.G - (255 * (z / 1)));
                                //    //b = Convert.ToInt32(entity.Color.B - (255 * (z / 1)));
                                //    //if (r < 0) { r = 0; } else if (r > 255) { r = 255; }
                                //    //if (g < 0) { g = 0; } else if (g > 255) { g = 255; }
                                //    //if (b < 0) { b = 0; } else if (b > 255) { b = 255; }
                                //    //frameBuffer.SetPixel(x, y, Color.FromArgb(r, g, b));

                                //    g.FillRectangle(new SolidBrush(entity.Color), x, y, 1, 1);
                                //    zBuffer[x, y] = z;
                                //}                    
                            }
                        }
                    }
                }
            }

            //and this lil boy does wireframes
            foreach (Renderable entity in render)
            {
                //go through all combinations of vertices
                foreach (Vect3 vertex in entity.Vertices)
                {
                    foreach (Vect3 connectToVertex in entity.Vertices)
                    {
                        //das a smart operator don't you worry
                        if (vertex != connectToVertex)
                        {
                            //draw dat line
                            g.DrawLine(Pens.Lime, vertex.X, vertex.Y, connectToVertex.X, connectToVertex.Y);
                        }
                    }
                }
            }
            frames++;
            //dont need this cus drawing directly on pb
            //draw the image onto pb starting at (0,0)
            //e.Graphics.DrawImage(frameBuffer, new Point(0, 0));
        }

        new private void Scroll(object sender, MouseEventArgs e)
        {
            //e.Delta is difference in scroll "value"
            //const scales for reasonable interaction
            scaleFactor += e.Delta * CONTROLLEDSCALESCALING;
            //make sure it doesnt go -ve
            if (scaleFactor < 0)
            {
                scaleFactor = 0;
            }
        }

        //TIM_rotate tracks controlled rotation
        private void TIM_rotate_Tick(object sender, EventArgs e)
        {
            //difference in x and y used to generate 
            int diffX, diffY;

            //find second point on line, P1 being where mouse was clicked
            rotationHover = PB_main.PointToClient(MousePosition);
            //find 2 smaller sides of triangle they make
            diffX = rotationHover.X - rotationBase.X; diffY = rotationHover.Y - rotationBase.Y;

            //using arbritary const scaler, find angle... base angle is from previous state
            angleX = diffY * CONTROLLEDROTATIONSCALING + baseAngleX;
            angleY = diffX * CONTROLLEDROTATIONSCALING + baseAngleY;
        }

        //TIM_render is processing
        private void TIM_render_Tick(object sender, EventArgs e)
        {
            Renderable[] projected;
            //frameBuffer = new Bitmap(PB_main.Width, PB_main.Height);
            zBuffer = new float[PB_main.Width, PB_main.Height];
            Matrix combined, translateToFrustrum;
            {
                //cheeky matrix definition
                //takes from origin, translates to into camera view
                translateToFrustrum = new Matrix(new float[,]
                {
                    { 1, 0, 0, 0 },
                    { 0, 1, 0, 0 },
                    { 0, 0, 1, -1.25f },
                    { 0, 0, 0, 1 }
                });
                //angleY = Convert.ToSingle((t / 72000f) * 2 * Math.PI);

                //rotation matrices
                Matrix rotateY = new Matrix(new float[,]
                {
                    { Convert.ToSingle(Math.Cos(angleY)), 0, Convert.ToSingle(Math.Sin(angleY)), 0 },
                    { 0, 1, 0, 0 },
                    { Convert.ToSingle(-Math.Sin(angleY)), 0, Convert.ToSingle(Math.Cos(angleY)), 0 },
                    { 0, 0, 0, 1 }
                });

                //angleX = Convert.ToSingle((t / 72000f) * 2 * Math.PI);

                Matrix rotateX = new Matrix(new float[,]
                {
                    { 1, 0, 0, 0 },
                    { 0, Convert.ToSingle(Math.Cos(angleX)), Convert.ToSingle(-Math.Sin(angleX)), 0 },
                    { 0, Convert.ToSingle(Math.Sin(angleX)), Convert.ToSingle(Math.Cos(angleX)), 0 },
                    { 0, 0, 0, 1 }
                });

                //scaling matrix

                Matrix scale = new Matrix(new float[,]
                {
                    { scaleFactor, 0, 0, 0 },
                    { 0, scaleFactor, 0, 0 },
                    { 0, 0, scaleFactor, 0 },
                    { 0, 0, 0, scaleFactor }
                });

                //combined matrix to make syntax cleaner
                combined = scale.Multiply(rotateY.Multiply(rotateX));

                //keep that time in sync - p much only autorotate for now so not used prob
                //t += 100;
            }
            //clean that dirty array
            render = new List<Renderable>();

            //initialise zBuffer
            //INFINITY is just a big no. don't tell anyone
            for (int x = 0; x < zBuffer.GetLength(0); x++)
            {
                for (int y = 0; y < zBuffer.GetLength(1); y++)
                {
                    zBuffer[x, y] = INFINITY;
                }
            }

            //array of 2d triangles - still NDC
            projected = new Renderable[testTris.Length];
            for (int i = 0; i < testTris.Length; i++)
            {
                projected[i] = (Triangle)testTris[i].Transform(combined)
                    .Transform(translateToFrustrum)
                    .Project(VPWIDTH, VPHEIGHT, CTOVPZ, ZCLIPPING);
            }

            //projected = new Renderable[] { (Line)(new Line(new Vect3(-0.25f, -0.25f, -0.25f), new Vect3(0.25f, 0.25f, 0.25f))).Transform(rotateX).Transform(rotateY).Transform(translateToFrustrum).Project(1,1,1,10) };

            //add NDC converts to render list
            foreach (Renderable entity in projected)
            {
                render.Add(ToScreenFromNDC(entity));
            }

            //add axis
            List<Line> axes = new List<Line>();

            axes.Add(new Line(new Vect3[] { new Vect3(0, 0, 0), new Vect3(0.1f, 0, 0) }));
            axes.Add(new Line(new Vect3[] { new Vect3(0, 0, 0), new Vect3(0, 0.1f, 0) }));
            axes.Add(new Line(new Vect3[] { new Vect3(0, 0, 0), new Vect3(0, 0, 0.1f) }));

            //ugly
            foreach (Line axis in axes)
            {
                render.Add(ToScreenFromNDC(axis.Transform(combined).Transform(translateToFrustrum).Project(VPWIDTH, VPHEIGHT, CTOVPZ, ZCLIPPING)));
            }

            //gonna make that bmp - oh no we don't gonna
            //Graphics graph = Graphics.FromImage(frameBuffer);

            //nice
            //            PB_main.Refresh();
            TSC_main.ContentPanel.Update();
            PB_main.Refresh();
        }

        //gets paintable equivalent for renderable NDC 2d shape
        private Renderable ToScreenFromNDC(Renderable input)
        {
            //replacement non-NDC list of vertices
            List<Vect3> newVerts;
            Point centre;

            //factor by which NDC are scaled to match screen            
            float scalingFactor;

            centre = new Point(PB_main.Width / 2, PB_main.Height / 2);

            //determine scaling factor based on largest square in rectangular screen
            if (centre.X <= centre.Y)
            {
                scalingFactor = centre.X / 0.5f;
            }
            else
            {
                scalingFactor = centre.Y / 0.5f;
            }

            newVerts = new List<Vect3>();

            //for each vertex of the shape (probably triangle or line), convert that 
            //scaling factor -ve cus screen is upside down for y :(
            //honestly probably doing the -ve y fix several times but who cares
            foreach (Vect3 vect in input.Vertices)
            {
                newVerts.Add(new Vect3(centre.X + (scalingFactor * vect.X), centre.Y + (-scalingFactor * vect.Y), vect.Z));
            }
            input.Vertices = newVerts.ToArray();
            return input;
        }
        //keeping this cus it feels useful
        private Point ToScreenFromNDC(PointF input)
        {
            Point centre;

            //factor by which NDC are scaled to match screen            
            float scalingFactor;

            centre = new Point(PB_main.Width / 2, PB_main.Height / 2);

            //determine scaling factor based on largest square in rectangular screen
            if (centre.X <= centre.Y)
            {
                scalingFactor = centre.X / 0.5f;
            }
            else
            {
                scalingFactor = centre.Y / 0.5f;
            }

            return new Point(Convert.ToInt32(centre.X + (scalingFactor * input.X)), Convert.ToInt32(centre.Y + (-scalingFactor * input.Y)));
        }

        //resets buffers
        private void PB_main_Resize(object sender, EventArgs e)
        {
            //frameBuffer = new Bitmap(PB_main.Width, PB_main.Height);
            zBuffer = new float[PB_main.Width, PB_main.Height];
        }

        //debug method
        //private void FRM_render_KeyPress(object sender, KeyPressEventArgs e)
        //{
        //    //ENTER
        //    if (e.KeyChar == (char)13)
        //    {
        //        Point mouse = PB_main.PointToClient(MousePosition);
        //        MessageBox.Show(mouse.X.ToString() + ", " + mouse.Y.ToString() + ": " + zBuffer[mouse.X, mouse.Y].ToString());
        //        float[] bary = GetBary(testTris[0], new Vect3(mouse.X, mouse.Y, -1));
        //        MessageBox.Show(bary[0] + ", " + bary[1] + ", " + bary[2]);
        //    }
        //}

        private void PB_main_MouseDown(object sender, MouseEventArgs e)
        {
            //set rotation base
            rotationBase = PB_main.PointToClient(MousePosition);
            //start the rotation timer to correct rotation every tick
            TIM_rotate.Enabled = true;
        }

        private void PB_main_MouseUp(object sender, MouseEventArgs e)
        {
            //turn off timer - stationary now
            TIM_rotate.Enabled = false;
            //save state
            baseAngleX = angleX; baseAngleY = angleY;
        }

        //self explanatory really (deals with projected screen coords)
        private bool IsInsideTriangle(Vect3 loc, Triangle container)
        {
            //looks awfully familiar
            //i dont know what this means
            //help
            if (Construct.EdgeFunction(container.Vertices[0], container.Vertices[1], loc)
                && Construct.EdgeFunction(container.Vertices[1], container.Vertices[2], loc)
                && Construct.EdgeFunction(container.Vertices[2], container.Vertices[0], loc))
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        private void TIM_fps_Tick(object sender, EventArgs e)
        {
            Text = "Vex - " + frames.ToString() + " FPS";
            frames = 0;
        }

        private Rectangle BoundingBox(Triangle input)
        {
            //reps max values for each
            //ahahhaha
            float top, left, bottom, right;

            //some initial values to compare with
            bottom = top = input.Vertices[0].Y; left = right = input.Vertices[0].X;

            //maximise
            for (int i = 1; i <= 2; i++)
            {
                if (input.Vertices[i].Y > bottom)
                {
                    bottom = input.Vertices[i].Y;
                }
                else if (input.Vertices[i].Y < top)
                {
                    top = input.Vertices[i].Y;
                }

                if (input.Vertices[i].X > right)
                {
                    right = input.Vertices[i].X;
                }
                else if (input.Vertices[i].X < left)
                {
                    left = input.Vertices[i].X;
                }
            }
            //a nasty little correction thing
            //god bless no whitespace restrictions
            if (left < 0) { left = 0; }
            if (left > PB_main.Width) { left = PB_main.Width; }
            if (right < 0) { right = 0; }
            if (right > PB_main.Width) { right = PB_main.Width; }
            if (top < 0) { top = 0; }
            if (top > PB_main.Height) { top = PB_main.Height; }
            if (bottom < 0) { bottom = 0; }
            if (bottom > PB_main.Height) { bottom = PB_main.Height; }

            //ew
            return new Rectangle(Convert.ToInt32(left), Convert.ToInt32(top), Convert.ToInt32(right - left), Convert.ToInt32(bottom - top));
        }

        private float GetZ(PointF p, Triangle t)
        {
            //this is barycentric coords not a guy called barry
            float[] bary = GetBary(t, new Vect3(p.X, p.Y, -1));
            //minus needed - i don't know how many times this has been done
            return -Convert.ToSingle(Math.Pow(Math.Pow(t.Vertices[0].Z, -1) * bary[0]
                + Math.Pow(t.Vertices[1].Z, -1) * bary[1]
                + Math.Pow(t.Vertices[2].Z, -1) * bary[2], -1));
        }

        //get barycentric coords
        private float[] GetBary(Triangle t, Vect3 v)
        {
            float[] output = new float[3];
            Vect3 v0 = t.Vertices[1] - t.Vertices[0];
            Vect3 v1 = t.Vertices[2] - t.Vertices[0];
            Vect3 v2 = v - t.Vertices[0];
            float d00 = v0.Dot2D(v0);
            float d01 = v0.Dot2D(v1);
            float d11 = v1.Dot2D(v1);
            float d20 = v2.Dot2D(v0);
            float d21 = v2.Dot2D(v1);
            float denom = d00 * d11 - d01 * d01;
            output[1] = (d11 * d20 - d01 * d21) / denom;
            output[2] = (d00 * d21 - d01 * d20) / denom;
            output[0] = 1 - output[1] - output[2];
            return output;
        }

        private bool SameSide(Triangle t, PointF p1, PointF p2)
        {
            bool output = true;
            output &= Construct.EdgeFunction(t.Vertices[0], t.Vertices[1], new Vect3(p1.X, p1.Y, -1))
                == Construct.EdgeFunction(t.Vertices[0], t.Vertices[1], new Vect3(p2.X, p2.Y, -1));
            output &= Construct.EdgeFunction(t.Vertices[1], t.Vertices[2], new Vect3(p1.X, p1.Y, -1))
                == Construct.EdgeFunction(t.Vertices[1], t.Vertices[2], new Vect3(p2.X, p2.Y, -1));
            output &= Construct.EdgeFunction(t.Vertices[2], t.Vertices[0], new Vect3(p1.X, p1.Y, -1))
                == Construct.EdgeFunction(t.Vertices[2], t.Vertices[0], new Vect3(p2.X, p2.Y, -1));
            return output;
        }
        private void TSB_input_Click(object sender, EventArgs e)
        {
            string input;
            //possibly do regex for each type of object and then check with matching later
            //could implement overall validation regex
            Regex planeCheck = new Regex("^([+-]? *[0-9]+(\\.[0-9]+)?x)? *([+-]? *[0-9]+(\\.[0-9]+)?y)? *([+-]? *[0-9]+(\\.[0-9]+)?z)? *= *[+-]?[0-9]*(\\.[0-9]*)?$");
            input = TSTXT_input.Text;
            if (planeCheck.IsMatch(input))
            {

            }
            else
            {
                MessageBox.Show("Not Plane");
            }
        }
        private float[] ParsePlane(string input)
        {
            float[] output = new float[4];
            int backCursor = 0;
            for (int cursor = 0; cursor < input.Length; cursor++)
            {
                if (input[cursor] == 'x')
                {
                    output[0] = Convert.ToSingle(input.Substring(backCursor, cursor - backCursor + 1));
                    for (int innerCursor = cursor; innerCursor < input.Length; innerCursor++)
                    {
                        //inner cursor scrolls through the wastes inbetween numbers and gets the start of the next section
                        if (input[innerCursor] != ' ' && input[innerCursor] != '+' && input[innerCursor] != '-')
                        {
                            
                        }
                    }
                }
            }
        }
            //return output -- then put output into Construct.Plane() -- then add triangles to draw list
    }

    public abstract class Construct
    {
        //vector consts
        private readonly float VECTORARROW = 0.05f, VECTORARROWELEVATION = Convert.ToSingle(Math.PI / 6);
        public static Triangle[] Cube(Vect3 pos, float size, Color color)
        {
            List<Triangle> output = new List<Triangle>();
            Triangle[] outputArr;
            Triangle[] currFace;

            currFace = Square(new Vect3(0 - size / 2, 0 - size / 2, 0), size, color);
            output.AddRange(CloneAndTransform(currFace, 0, 0, 0, 0, 0, size / 2));
            output.AddRange(CloneAndTransform(currFace, Convert.ToSingle(Math.PI), 0, 0, 0, 0, -size / 2));
            output.AddRange(CloneAndTransform(currFace, Convert.ToSingle(Math.PI / 2), 0, 0, 0, -size / 2, 0));
            output.AddRange(CloneAndTransform(currFace, Convert.ToSingle(-Math.PI / 2), 0, 0, 0, size / 2, 0));
            output.AddRange(CloneAndTransform(currFace, 0, Convert.ToSingle(Math.PI / 2), 0, size / 2, 0, 0));
            output.AddRange(CloneAndTransform(currFace, 0, Convert.ToSingle(-Math.PI / 2), 0, -size / 2, 0, 0));

            outputArr = CloneAndTransform(output.ToArray(), 0, 0, 0, pos.X, pos.Y, pos.Z);
            return outputArr;
        }
        public static Line[] Vector(Vect3 pos, float i, float j, float k, Color color)
        {
            Vect3 v1, v2;
            List<Line> output = new List<Line>();
            output.Add(new Line(pos, new Vect3(pos.X + i, pos.Y + j, pos.Z + k), color));
            //output.Add(new Line(new Vect3(pos.X + i, pos.Y + j, pos.Z + k), new Vect3()))

            return output.ToArray();
        }
        public static Triangle[] Square(Vect3 pos, float size, Color color)
        {
            Vect3[] vertices;
            Triangle[] output;

            vertices = new Vect3[4];
            output = new Triangle[2];

            vertices[0] = pos;
            vertices[1] = new Vect3(pos.X, pos.Y + size, pos.Z);
            vertices[2] = new Vect3(pos.X + size, pos.Y, pos.Z);
            vertices[3] = new Vect3(pos.X + size, pos.Y + size, pos.Z);

            output[0] = GetTri(new Vect3[] { vertices[0], vertices[1], vertices[2] }, color);
            output[1] = GetTri(new Vect3[] { vertices[1], vertices[2], vertices[3] }, color);

            return output;
        }
        private static Triangle GetTri(Vect3[] vertices, Color color)
        {
            if (!EdgeFunction(vertices[0], vertices[1], vertices[2]))
            {
                return new Triangle(vertices[0], vertices[1], vertices[2], color);
            }
            else
            {
                return new Triangle(vertices[1], vertices[0], vertices[2], color);
            }
        }
        public static bool EdgeFunction(Vect3 v0, Vect3 v1, Vect3 P)
        {
            float value;

            value = (P.X - v0.X) * (v1.Y - v0.Y) - (P.Y - v0.Y) * (v1.X - v0.X);

            if (value >= 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private static Triangle[] CloneAndTransform(Triangle[] input, float rX, float rY, float rZ, float tX, float tY, float tZ)
        {
            List<Triangle> output = new List<Triangle>();
            Matrix rXM = new Matrix(new float[,]
            {
                { 1, 0, 0, 0 },
                { 0, Convert.ToSingle(Math.Cos(rX)), Convert.ToSingle(-Math.Sin(rX)), 0 },
                { 0, Convert.ToSingle(Math.Sin(rX)), Convert.ToSingle(Math.Cos(rX)), 0 },
                { 0, 0, 0, 1 }
            });
            Matrix rYM = new Matrix(new float[,]
            {
                { Convert.ToSingle(Math.Cos(rY)), 0, Convert.ToSingle(Math.Sin(rY)), 0 },
                { 0, 1, 0, 0 },
                { Convert.ToSingle(-Math.Sin(rY)), 0, Convert.ToSingle(Math.Cos(rY)), 0 },
                { 0, 0, 0, 1 }
            });
            Matrix rZM = new Matrix(new float[,]
            {
                { Convert.ToSingle(Math.Cos(rZ)), Convert.ToSingle(-Math.Sin(rZ)), 0, 0 },
                { Convert.ToSingle(Math.Sin(rZ)), Convert.ToSingle(Math.Cos(rZ)), 0, 0 },
                { 0, 0, 1, 0 },
                { 0, 0, 0, 1 }
            });
            Matrix t = new Matrix(new float[,]
            {
                { 1, 0, 0, tX },
                { 0, 1, 0, tY },
                { 0, 0, 1, tZ },
                { 0, 0, 0, 1 }
            });
            Matrix combined = rZM.Multiply(rYM.Multiply(rXM.Multiply(t)));
            foreach (Triangle tri in input)
            {
                output.Add((Triangle)tri.Transform(combined));
            }
            return output.ToArray();
        }
        private static Triangle[] Plane(float x, float y, float z, float eq, float bound, Color color)
        {
            Vect3[] corners = new Vect3[4];
            List<Triangle> output = new List<Triangle>();
            //max z, min x
            corners[0] = new Vect3(-bound, (eq - x*(-bound) - z*(bound)) / y, bound);
            //min z, min x
            corners[1] = new Vect3(-bound, (eq - x * (-bound) - z * (-bound)) / y, -bound);
            //max x, min z
            corners[2] = new Vect3(bound, (eq - x * (bound) - z * (-bound)) / y, -bound);
            //max x, max z
            corners[3] = new Vect3(bound, (eq - x * (bound) - z * (bound)) / y, bound);

            output.Add(new Triangle(new Vect3[] { corners[0], corners[2], corners[1] }, color));
            output.Add(new Triangle(new Vect3[] { corners[3], corners[2], corners[0] }, color));
            return output.ToArray();
        }
    }
}