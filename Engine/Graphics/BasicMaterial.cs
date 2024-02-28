using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Vortice.Direct3D11;

namespace Engine.Graphics;
public class BasicMaterial : Material
{
    public BasicMaterial() : base(App.Assets.LoadVertexShader("Shaders/basicVS.hlsl"), App.Assets.LoadPixelShader("basicPS.hlsl"))
    {
    }

    public override void Bind(RenderContext context)
    {
        context.DeviceContext.VSSetConstantBuffer(0, context.matrixBuffer.InternalBuffer);
    }

    public Texture Albedo { get; set; } => this.PixelShaderParameters.Textures["albedo"];
}
