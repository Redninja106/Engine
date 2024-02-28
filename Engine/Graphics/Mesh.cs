using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Vortice.Direct3D11;
using Vortice.DXGI;

namespace Engine.Graphics;
public class Mesh
{
    public static Mesh Plane { get; }

    static Mesh()
    {
        var planeData = new MeshData()
        {
            positions = [new(1, 0, 1), new(1, 0, -1), new(-1, 0, 1), new(-1, 0, -1)],
            normals = [new(0, 1, 0), new(0, 1, 0), new(0, 1, 0), new(0, 1, 0)],
            uvs = [new(1, 1), new(1, 0), new(0, 1), new(0, 0)],
            indices = [0, 1, 2, 2, 1, 3]
        };

        Plane = new(planeData);
    }
    

    public Mesh(MeshData data)
    {
        AddVertexAttribute(VertexAttribute.Position, data.positions);
        AddVertexAttribute(VertexAttribute.UV, data.uvs);
        AddVertexAttribute(VertexAttribute.Normal, data.normals);

        if (data.indices != null)
        {
            indexBuffer = App.Graphics.Device.CreateBuffer(data.indices, BindFlags.IndexBuffer);
            indexCount = data.indices.Length;
        }
    }

    private readonly Dictionary<VertexAttribute, (ID3D11Buffer, int)> buffers = new();
    private readonly ID3D11Buffer? indexBuffer;
    private int? indexCount;
    private int vertexCount;

    private void AddVertexAttribute<T>(VertexAttribute attribute, T[]? data) where T : unmanaged
    {
        if (data is null)
            return;

        if (vertexCount == 0)
        {
            vertexCount = data.Length;
        }
        else 
        {
            App.Debug.Assert(vertexCount == data.Length);
        }

        var buffer = App.Graphics.Device.CreateBuffer(data, BindFlags.VertexBuffer);
        buffers.Add(attribute, (buffer, Unsafe.SizeOf<T>()));
    }

    public void Draw(ID3D11DeviceContext context)
    {
        foreach (var (attribute, (buffer, stride)) in buffers)
        {
            context.IASetVertexBuffer((int)attribute, buffer, stride);
        }
        context.IASetIndexBuffer(indexBuffer, Format.R32_UInt, 0);

        if (this.indexCount is int indexCount)
        {
            context.DrawIndexed(indexCount, 0, 0);
        }
        else
        {
            context.Draw(vertexCount, 0);
        }
    }
}

public struct MeshData
{
    public Vector3[] positions;
    public Vector2[]? uvs;
    public Vector3[]? normals;
    public uint[]? indices;
}
