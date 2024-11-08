using OpenTK;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;

namespace ConsoleApp3
{
    class Triangle
    {
        private List<Vertex> vertices;  // Listă de vârfuri (vertexuri) care includ coordonate și culori
        private bool visibility;

        // Constructorul clasei Triangle
        public Triangle()
        {
            vertices = InitializeVertices();
            visibility = true;
        }

        // Structură pentru a ține coordonatele și culoarea fiecărui vertex
        public class Vertex
        {
            public Vector3 Position;
            public Color Color;

            public Vertex(Vector3 position, Color color)
            {
                Position = position;
                Color = color;
            }
        }


        // Inițializarea coordonatelor fixe ale triunghiului
        private List<Vertex> InitializeVertices()
        {
            List<Vertex> verts = new List<Vertex>();

            // Setăm coordonatele triunghiului manual (sunt coordonate arbitrare)
            verts.Add(new Vertex(new Vector3(-1.0f, 1.0f, 0.0f), RandomColor()));
            verts.Add(new Vertex(new Vector3(1.0f, 1.0f, 0.0f), RandomColor()));
            verts.Add(new Vertex(new Vector3(0.0f, 2.0f, 0.0f), RandomColor()));

            return verts;
        }

        // Schimbăm culoarea unui vertex folosind un RGB random
        public void RandomizeColors()
        {
            Random rand = new Random();
            for (int i = 0; i < vertices.Count; i++)
            {
                vertices[i].Color = Color.FromArgb(rand.Next(256), rand.Next(256), rand.Next(256));
            }
        }

        // Afișăm culorile RGB ale fiecărui vertex în consolă
        public void DisplayVertexColors()
        {
            foreach (var vertex in vertices)
            {
                Console.WriteLine($"Vertex Position: {vertex.Position} -> RGB: {vertex.Color.R}, {vertex.Color.G}, {vertex.Color.B}");
            }
        }

        // Desenăm triunghiul folosind culorile specifice
        public void Draw()
        {
            if (!visibility) return;

            GL.Begin(PrimitiveType.Triangles);

            // Desenăm triunghiul
            DrawTriangle();

            GL.End();
        }

        private void DrawTriangle()
        {
            // Folosim culorile corespunzătoare pentru fiecare vertex
            GL.Color3(vertices[0].Color);
            GL.Vertex3(vertices[0].Position);
            GL.Color3(vertices[1].Color);
            GL.Vertex3(vertices[1].Position);
            GL.Color3(vertices[2].Color);
            GL.Vertex3(vertices[2].Position);
        }

        // Metoda de schimbare a vizibilității triunghiului
        public void ToggleVisibility()
        {
            visibility = !visibility;
        }

        // Metoda de generare a culorilor aleatorii pentru vârfuri
        private Color RandomColor()
        {
            Random rand = new Random();
            return Color.FromArgb(rand.Next(256), rand.Next(256), rand.Next(256));
        }
    }


}
