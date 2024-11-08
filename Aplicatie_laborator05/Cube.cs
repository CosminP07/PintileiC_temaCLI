using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace ConsoleApp3
{
    public class Cube
    {
        private List<Vector3> vertices;  // Vârfurile cubului
        private Color[] faceColors;      // Culorile pentru fețele cubului
        private bool visibility;         // Vizibilitatea cubului

        private const string FILENAME = "cube_coordinates.txt"; // Fișierul care conține coordonatele vârfurilor

        public Cube()
        {
            vertices = LoadVertices(FILENAME);
            faceColors = new Color[6] { Color.Red, Color.Green, Color.Blue, Color.Yellow, Color.Magenta, Color.Cyan };
            visibility = true;
        }

        public void ToggleVisibility()
        {
            visibility = !visibility;
        }

        // Citirea vârfurilor din fișier
        private List<Vector3> LoadVertices(string filename)
        {
            List<Vector3> verts = new List<Vector3>();
            var lines = File.ReadLines(filename);
            foreach (var line in lines)
            {
                if (line.StartsWith("v"))
                {
                    string[] parts = line.Split(' ');
                    float x = float.Parse(parts[1]);
                    float y = float.Parse(parts[2]);
                    float z = float.Parse(parts[3]);
                    verts.Add(new Vector3(x, y, z));
                }
            }
            return verts;
        }

        // Desenarea cubului
        public void Draw()
        {
            if (!visibility) return;

            GL.Begin(PrimitiveType.Quads);
            for (int i = 0; i < 6; i++)
            {
                GL.Color4(faceColors[i]); // Aplică culoarea feței

                // Desenăm fiecare față a cubului, având vârfurile din lista `vertices`
                DrawFace(i);
            }
            GL.End();
        }

        // Desenarea unei fețe a cubului, folosind vârfurile corespunzătoare
        private void DrawFace(int faceIndex)
        {
            switch (faceIndex)
            {
                case 0: // Fața din față
                    GL.Vertex3(vertices[0]); // v0
                    GL.Vertex3(vertices[1]); // v1
                    GL.Vertex3(vertices[2]); // v2
                    GL.Vertex3(vertices[3]); // v3
                    break;
                case 1: // Fața din spate
                    GL.Vertex3(vertices[4]); // v4
                    GL.Vertex3(vertices[5]); // v5
                    GL.Vertex3(vertices[6]); // v6
                    GL.Vertex3(vertices[7]); // v7
                    break;
                case 2: // Fața de sus
                    GL.Vertex3(vertices[0]); // v0
                    GL.Vertex3(vertices[1]); // v1
                    GL.Vertex3(vertices[5]); // v5
                    GL.Vertex3(vertices[4]); // v4
                    break;
                case 3: // Fața de jos
                    GL.Vertex3(vertices[3]); // v3
                    GL.Vertex3(vertices[2]); // v2
                    GL.Vertex3(vertices[6]); // v6
                    GL.Vertex3(vertices[7]); // v7
                    break;
                case 4: // Fața din stânga
                    GL.Vertex3(vertices[0]); // v0
                    GL.Vertex3(vertices[3]); // v3
                    GL.Vertex3(vertices[7]); // v7
                    GL.Vertex3(vertices[4]); // v4
                    break;
                case 5: // Fața din dreapta
                    GL.Vertex3(vertices[1]); // v1
                    GL.Vertex3(vertices[2]); // v2
                    GL.Vertex3(vertices[6]); // v6
                    GL.Vertex3(vertices[5]); // v5
                    break;
            }
        }

        // Schimbarea culorii unei fețe
        public void ChangeFaceColor(int faceIndex, Color color)
        {
            if (faceIndex >= 0 && faceIndex < 6)
            {
                faceColors[faceIndex] = color;
            }
        }
    }
}
