using System;
using System.Drawing;
using System.IO;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace OpenTK_immediate_mode
{
    class ImmediateMode : GameWindow
    {
        private const int XYZ_SIZE = 75; // Dimensiunea axelor pentru desenarea axelor de coordonate
        private Vector3[] triangleVertices; // Coordonatele vertexurilor triunghiului
        private Color triangleColor = Color.White; // Culoarea triunghiului, inițial setată la alb

        // Constructorul clasei ImmediateMode, configurând fereastra OpenGL
        public ImmediateMode() : base(800, 600, new GraphicsMode(32, 24, 0, 8))
        {
            VSync = VSyncMode.On; // Activează sincronizarea verticală (pentru a evita "tearing"-ul imaginii)

            Console.WriteLine("OpenGL versiunea: " + GL.GetString(StringName.Version)); // Afișează versiunea OpenGL în consolă
            Title = "OpenGL versiunea: " + GL.GetString(StringName.Version) + " (mod imediat)"; // Setează titlul ferestrei
        }

        // Metoda de inițializare a resurselor OpenGL, apelată o dată la pornire
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            GL.ClearColor(Color.Blue); // Setează culoarea de fundal pentru fereastra 3D
            GL.Enable(EnableCap.DepthTest); // Activează testul de adâncime pentru a afișa corect obiectele 3D
            GL.DepthFunc(DepthFunction.Less); // Specifică funcția de comparație pentru adâncime
            GL.Hint(HintTarget.PolygonSmoothHint, HintMode.Nicest); // Sugestie pentru a randa poligoanele cât mai neted

            // Încarcă coordonatele triunghiului dintr-un fișier text
            LoadTriangleVertices("triangle.txt");
        }

        // Metoda apelată când fereastra este redimensionată, pentru a actualiza viewport-ul și proiecția
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            GL.Viewport(0, 0, Width, Height); // Configurează viewport-ul să acopere întreaga fereastră

            double aspect_ratio = Width / (double)Height; // Calculează raportul de aspect al ferestrei

            // Creează o matrice de proiecție în perspectivă pentru a reda corect obiectele 3D
            Matrix4 perspective = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, (float)aspect_ratio, 1, 64);
            GL.MatrixMode(MatrixMode.Projection); // Comută la matricea de proiecție
            GL.LoadMatrix(ref perspective); // Încarcă matricea de proiecție

            // Configurează o matrice de vizualizare pentru a privi scena dintr-un punct extern
            Matrix4 lookat = Matrix4.LookAt(30, 30, 30, 0, 0, 0, 0, 1, 0);
            GL.MatrixMode(MatrixMode.Modelview); // Comută la matricea modelview
            GL.LoadMatrix(ref lookat); // Încarcă matricea de vizualizare
        }

        // Metoda de actualizare a logicii jocului, apelată înainte de fiecare cadru randat
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            KeyboardState keyboard = Keyboard.GetState(); // Obține starea curentă a tastaturii

            // Schimbă culoarea triunghiului când tasta 'C' este apăsată
            if (keyboard[Key.C])
            {
                Random rand = new Random(); // Inițializează un generator de numere aleatoare
                // Generază o culoare aleatoare
                triangleColor = Color.FromArgb(rand.Next(256), rand.Next(256), rand.Next(256));
                Console.WriteLine("Culoarea triunghiului a fost schimbată la: " + triangleColor);
            }

            // Închide aplicația dacă tasta 'Escape' este apăsată
            if (keyboard[Key.Escape])
            {
                Exit();
            }
        }

        // Metoda de randare a scenei, apelată pentru fiecare cadru
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            // Șterge buffer-ele de culoare și adâncime pentru a pregăti scena pentru randare
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            DrawAxes(); // Desenează axele coordonatelor
            DrawObjects(); // Desenează obiectele (în acest caz, triunghiul)

            SwapBuffers(); // Schimbă bufferele (se folosește "double buffering" pentru a evita pâlpâirile)
        }

        // Metodă pentru a desena axele de coordonate
        private void DrawAxes()
        {
            GL.LineWidth(3.0f); // Setează grosimea liniilor pentru axele coordonatelor

            // Desenează axa Ox (roșu)
            GL.Begin(PrimitiveType.Lines);
            GL.Color3(Color.Red); // Setează culoarea roșie
            GL.Vertex3(0, 0, 0); // Punctul de început (originea)
            GL.Vertex3(XYZ_SIZE, 0, 0); // Punctul final pe axa X
            GL.End();

            // Desenează axa Oy (galben)
            GL.Begin(PrimitiveType.Lines);
            GL.Color3(Color.Yellow); // Setează culoarea galbenă
            GL.Vertex3(0, 0, 0); // Originea
            GL.Vertex3(0, XYZ_SIZE, 0); // Punctul final pe axa Y
            GL.End();

            // Desenează axa Oz (verde)
            GL.Begin(PrimitiveType.Lines);
            GL.Color3(Color.Green); // Setează culoarea verde
            GL.Vertex3(0, 0, 0); // Originea
            GL.Vertex3(0, 0, XYZ_SIZE); // Punctul final pe axa Z
            GL.End();
        }

        // Metoda pentru a desena obiectele din scenă (în acest caz, doar triunghiul)
        private void DrawObjects()
        {
            if (triangleVertices != null) // Verifică dacă triunghiul a fost încărcat cu succes
            {
                DrawTriangle(); // Desenează triunghiul
            }
        }

        // Variabile pentru a reține valorile RGB anterioare ale vertexurilor
        private Color[] previousVertexColors = new Color[3]; // Array de culori anterioare pentru fiecare vertex

        // Metoda pentru a desena triunghiul folosind coordonatele și culorile definite
        private void DrawTriangle()
        {
            GL.Begin(PrimitiveType.Triangles); // Începe desenarea unui triunghi

            for (int i = 0; i < triangleVertices.Length; i++)
            {
                // Culoare gradient bazată pe indexul vertexului
                Color vertexColor = Color.FromArgb((triangleColor.R + i * 50) % 256,
                                                    (triangleColor.G + i * 50) % 256,
                                                    (triangleColor.B + i * 50) % 256);
                GL.Color3(vertexColor); // Setează culoarea pentru fiecare vertex
                GL.Vertex3(triangleVertices[i]); // Desenează vertexul triunghiului

                // Actualizează afișarea în consolă doar dacă culoarea s-a schimbat
                if (previousVertexColors[i] != vertexColor)
                {
                    // Salvează noua culoare ca valoare anterioară
                    previousVertexColors[i] = vertexColor;

                    // Poziționează cursorul în consola la linia corespunzătoare vertexului
                    Console.SetCursorPosition(0, i);
                    Console.WriteLine($"Vertex {i}: RGB({vertexColor.R}, {vertexColor.G}, {vertexColor.B})");
                }
            }

            GL.End(); // Încheie desenarea triunghiului
        }

        // Metoda pentru a încărca coordonatele triunghiului dintr-un fișier text
        private void LoadTriangleVertices(string filePath)
        {
            try
            {
                string[] lines = File.ReadAllLines(filePath); // Citește toate liniile din fișier
                triangleVertices = new Vector3[3]; // Creează un array de 3 vertexuri

                for (int i = 0; i < 3; i++)
                {
                    // Separă coordonatele după virgulă
                    string[] parts = lines[i].Split(',');
                    float x = float.Parse(parts[0]); // Coordonata X
                    float y = float.Parse(parts[1]); // Coordonata Y
                    float z = float.Parse(parts[2]); // Coordonata Z
                    triangleVertices[i] = new Vector3(x, y, z); // Stochează coordonatele într-un array de Vector3
                }

                Console.WriteLine("Coordonatele triunghiului au fost încărcate din fișier.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Eroare la încărcarea triunghiului: " + ex.Message); // Afișează un mesaj de eroare dacă nu reușește încărcarea
            }
        }

        // Metoda principală a programului
        [STAThread]
        static void Main(string[] args)
        {
            using (ImmediateMode example = new ImmediateMode())
            {
                example.Run(30.0, 0.0); // Rulează aplicația cu 30 FPS
            }
        }
    }
}
