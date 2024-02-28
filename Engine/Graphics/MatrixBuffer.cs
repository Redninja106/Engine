using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Vortice.Direct3D11;

namespace Engine.Graphics;

public class MatrixBuffer : ConstantBuffer<MatrixBufferData>
{
    public ref Matrix4x4 World => ref Value.world;
    public ref Matrix4x4 View => ref Value.view;
    public ref Matrix4x4 Proj => ref Value.proj;

    public MatrixBuffer()
    {

    }

    public void SetViewProj(Camera camera)
    {
        Proj = Matrix4x4.Transpose(camera.CreateProjectionMatrix());
        View = Matrix4x4.Transpose(camera.CreateViewMatrix());
    }

    public void SetWorld(Transform transform)
    {
        World = Matrix4x4.Transpose(transform.CreateLocalToWorldMatrix());
    }
}

public struct MatrixBufferData
{
    public Matrix4x4 world;
    public Matrix4x4 view;
    public Matrix4x4 proj;
}