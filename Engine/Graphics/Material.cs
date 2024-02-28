using Engine.Debugging;
using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vortice.Direct3D11;

namespace Engine.Graphics;

/// <summary>
/// The mediator between the shader world and the cpu world. 
/// <para>Handles the shaders in a pipeline and their parameters. Inherit to make shaders more usable.</para>
/// </summary>
public class Material : IInspectable
{
    public VertexShader VertexShader { get; private set; }
    public ShaderParameters VertexShaderParameters { get; private set; }

    public PixelShader PixelShader { get; private set; }
    public ShaderParameters PixelShaderParameters { get; private set; }

    public Material(VertexShader vertexShader, PixelShader pixelShader)
    {
        VertexShader = vertexShader;
        PixelShader = pixelShader;

        VertexShaderParameters = new();
        PixelShaderParameters = new();
    }

    public void Layout()
    {
    }

    public virtual void Bind(RenderContext context)
    {
        VertexShader.Bind(context.DeviceContext);
        VertexShaderParameters.BindVS(context.DeviceContext);

        PixelShader.Bind(context.DeviceContext);
        PixelShaderParameters.BindPS(context.DeviceContext); 
    }
}