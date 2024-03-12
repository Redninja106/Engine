using Engine;
using Engine.Graphics;
using SharpGen.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace TestApp;
internal class Ground : Component
{
    private HeightMap heightMap;

    public override void Initialize()
    {
        heightMap = new HeightMap(128, 128, (x, y) => MathF.Sin(x * 100) + MathF.Sin(y * 100));

        Mesh mesh = new(GenerateMesh());
        Actor.GetComponent<MeshRenderer>()!.mesh = mesh;
    }

    float Height(float x, float z)
    {
        return MathF.Sin(x) + MathF.Sin(z);
    }

    public override void Update(float deltaTime)
    {
    }

    private MeshData GenerateMesh()
    {
        MeshBuilder b = new();
        int width = 128, length = 128;
        float size = 50f;
        float scale = size / width;

        void AddVertex(float x, float z)
        {
            float u = x / width;
            float v = z / width;
            b.Push(new(x * scale, heightMap.Sample(u, v), z * scale), new(u, v), new(0, 0, 0));
        }

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < length; z++)
            {
                AddVertex(x, z);
                AddVertex(x, z + 1);
                AddVertex(x + 1, z);

                AddVertex(x + 1, z);
                AddVertex(x, z + 1);
                AddVertex(x + 1, z + 1);
            }
        }

        b.ComputeNormals();

        return b.GetMeshData();
    }
}

class HeightMap
{
    private float[] values;
    public int Width { get; }
    public int Height { get; }

    public HeightMap(int width, int height, Func<float, float, float>? initialValueProvider)
    {
        values = new float[width * height];
        this.Width = width;
        this.Height = height;

        if (initialValueProvider is not null)
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    this[x, y] = initialValueProvider(x / (float)width, y / (float)height);
                }
            }
        }
    }

    public float Sample(float u, float v)
    {
        float x = u * (Width-1);
        float y = v * (Height-1);

        int lx = (int)MathF.Floor(x);
        int hx = (int)MathF.Ceiling(x);

        int ly = (int)MathF.Floor(y);
        int hy = (int)MathF.Ceiling(y);

        if (ly == hy && lx == hx)
        {
            return this[ly, lx];
        }

        // bilinear
        float highValue = float.Lerp(this[lx, hy], this[hx, hy], x - lx);
        float lowValue = float.Lerp(this[lx, ly], this[hx, ly], x - lx);
        float result = float.Lerp(highValue, lowValue, y - ly);

        return result;
    }

    public ref float this[int x, int y]
    {
        get
        {
            if ((uint)x >= Width || (uint)y >= Height)
                throw new IndexOutOfRangeException();
            return ref values[y * Width + x];
        }
    }
}

class MeshBuilder
{
    List<Vector3> positions = [];
    List<Vector2> uvs = [];
    List<Vector3> normals = [];

    public void Push(Vector3 position, Vector2 uv, Vector3 normal)
    {
        positions.Add(position);
        uvs.Add(uv);
        normals.Add(normal);
    }

    public void ComputeNormals()
    {
        int[] counts = new int[positions.Count];

        for (int i = 0; i < positions.Count / 3; i++)
        {
            Vector3 v1 = positions[i * 3 + 0];
            Vector3 v2 = positions[i * 3 + 1];
            Vector3 v3 = positions[i * 3 + 2];

            Vector3 a = v2 - v1;
            Vector3 b = v3 - v1;

            Vector3 normal = Vector3.Normalize(Vector3.Cross(a, b));

            normals[i * 3 + 0] += normal;
            normals[i * 3 + 1] += normal;
            normals[i * 3 + 2] += normal;

            counts[i * 3 + 0]++; 
            counts[i * 3 + 1]++; 
            counts[i * 3 + 2]++;
        }

        for (int i = 0; i < counts.Length; i++)
        {
            normals[i] = Vector3.Normalize(normals[i]* 1f / counts[i]);
        }
    }

    public MeshData GetMeshData()
    {
        return new()
        {
            normals = [.. normals],
            positions = [.. positions],
            uvs = [.. uvs],
        };
    }
}
