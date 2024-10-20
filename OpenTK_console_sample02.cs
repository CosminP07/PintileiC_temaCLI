using System;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace OpenTK_console_sample02
{
    class SimpleWindow3D : GameWindow
    {
        const float rotation_speed = 180.0f;
        float angle;
        float rotationAngleX = 0;
        float rotationAngleY = 0;
        float mousePosX = 0;
        float mousePosY = 0;
        bool showCube = true;
        KeyboardState lastKeyPress;

        public SimpleWindow3D() : base(800, 600)
        {
            VSync = VSyncMode.On;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            GL.ClearColor(Color.Blue);
            GL.Enable(EnableCap.DepthTest);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, Width, Height);
            double aspect_ratio = Width / (double)Height;
            Matrix4 perspective = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, (float)aspect_ratio, 1, 64);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref perspective);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
            KeyboardState keyboard = OpenTK.Input.Keyboard.GetState();
            MouseState mouse = OpenTK.Input.Mouse.GetState();

            // Control prin tastatură: tastele A și D controlează rotația pe axa Y.
            if (keyboard[Key.A])
            {
                rotationAngleY -= rotation_speed * (float)e.Time;
            }
            if (keyboard[Key.D])
            {
                rotationAngleY += rotation_speed * (float)e.Time;
            }

            // Control prin tastatură: tastele W și S controlează rotația pe axa X.
            if (keyboard[Key.W])
            {
                rotationAngleX -= rotation_speed * (float)e.Time;
            }
            if (keyboard[Key.S])
            {
                rotationAngleX += rotation_speed * (float)e.Time;
            }

            // Control prin mouse: actualizarea poziției pe baza mișcării mouse-ului.
            mousePosX = mouse.X - (Width / 2.0f);
            mousePosY = mouse.Y - (Height / 2.0f);

            // Închiderea aplicației prin apăsarea tastei Escape.
            if (keyboard[Key.Escape])
            {
                Exit();
                return;
            }
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            // Configurarea matricei modelview.
            Matrix4 lookat = Matrix4.LookAt(15, 50, 15, 0, 0, 0, 0, 1, 0);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref lookat);

            // Aplicarea rotației controlate de tastatură.
            GL.Rotate(rotationAngleY, 0.0f, 1.0f, 0.0f);
            GL.Rotate(rotationAngleX, 1.0f, 0.0f, 0.0f);

            // Translația obiectului pe baza poziției mouse-ului.
            GL.Translate(mousePosX / 100.0f, -mousePosY / 100.0f, 0.0f);

            // Desenarea cubului și a axelor.
            if (showCube)
            {
                DrawCube();
                DrawAxes_OLD();
            }

            SwapBuffers();
        }

        private void DrawAxes_OLD()
        {
            GL.Begin(PrimitiveType.Lines);
            GL.Color3(Color.Red);
            GL.Vertex3(0, 0, 0);
            GL.Vertex3(20, 0, 0);
            GL.Color3(Color.Blue);
            GL.Vertex3(0, 0, 0);
            GL.Vertex3(0, 20, 0);
            GL.Color3(Color.Yellow);
            GL.Vertex3(0, 0, 0);
            GL.Vertex3(0, 0, 20);
            GL.End();
        }

        private void DrawCube()
        {
            GL.Begin(PrimitiveType.Quads);
            GL.Color3(Color.Silver);
            GL.Vertex3(-1.0f, -1.0f, -1.0f);
            GL.Vertex3(-1.0f, 1.0f, -1.0f);
            GL.Vertex3(1.0f, 1.0f, -1.0f);
            GL.Vertex3(1.0f, -1.0f, -1.0f);
            GL.Color3(Color.Honeydew);
            GL.Vertex3(-1.0f, -1.0f, -1.0f);
            GL.Vertex3(1.0f, -1.0f, 1.0f);
            GL.Vertex3(1.0f, 1.0f, 1.0f);
            GL.Vertex3(-1.0f, 1.0f, 1.0f);
            GL.Color3(Color.Moccasin);
            GL.Vertex3(-1.0f, -1.0f, -1.0f);
            GL.Vertex3(-1.0f, -1.0f, 1.0f);
            GL.Vertex3(-1.0f, 1.0f, 1.0f);
            GL.Vertex3(-1.0f, 1.0f, -1.0f);
            GL.Color3(Color.IndianRed);
            GL.Vertex3(-1.0f, -1.0f, 1.0f);
            GL.Vertex3(1.0f, -1.0f, 1.0f);
            GL.Vertex3(1.0f, 1.0f, 1.0f);
            GL.Vertex3(-1.0f, 1.0f, 1.0f);
            GL.Color3(Color.PaleVioletRed);
            GL.Vertex3(-1.0f, 1.0f, -1.0f);
            GL.Vertex3(-1.0f, 1.0f, 1.0f);
            GL.Vertex3(1.0f, 1.0f, 1.0f);
            GL.Vertex3(1.0f, 1.0f, -1.0f);
            GL.Color3(Color.ForestGreen);
            GL.Vertex3(1.0f, -1.0f, -1.0f);
            GL.Vertex3(1.0f, 1.0f, -1.0f);
            GL.Vertex3(1.0f, 1.0f, 1.0f);
            GL.Vertex3(1.0f, -1.0f, 1.0f);
            GL.End();
        }

        [STAThread]
        static void Main(string[] args)
        {
            using (SimpleWindow3D example = new SimpleWindow3D())
            {
                example.Run(30.0, 0.0);
            }
        }
    }
}
