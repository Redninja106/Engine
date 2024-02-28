using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Vortice.Direct3D;
using Vortice.Direct3D11;
using Vortice.DXGI;

namespace Engine.Graphics;
public class MeshRenderer : DrawableComponent
{
    Mesh mesh;
    Material material;

    public MeshRenderer(Mesh mesh, Material material)
    {
        this.mesh = mesh;
        this.material = material;
    }

    public override void Draw(RenderContext context)
    {
        var d3dContext = App.Graphics.ImmediateContext;

        context.matrixBuffer.SetWorld(Transform);
        context.matrixBuffer.Upload();

        material.Bind(context);

        d3dContext.IASetPrimitiveTopology(PrimitiveTopology.TriangleList);
        mesh.Draw(d3dContext);
    }
}
