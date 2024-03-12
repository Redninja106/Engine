using Engine.Debugging;
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
    public TextureSlot Albedo { get; }
    public SamplerSlot Sampler { get; }
    public BufferSlot LightBuffer { get; }

    public BasicMaterial() : base(App.Assets.LoadVertexShader("Shaders/basicVS.hlsl"), App.Assets.LoadPixelShader("basicPS.hlsl"))
    {
        Albedo = new(PixelShaderParameters, 0);
        Sampler = new(PixelShaderParameters, 0);
        LightBuffer = new(PixelShaderParameters, 1);

        Sampler.Set(Graphics.Sampler.DefaultLinear);
    }

    public override void Bind(RenderContext context)
    {
        LightBuffer.Set(context.pointLights);

        base.Bind(context);
    }
}

public abstract class MaterialSlotBase
{
    protected readonly ShaderParameters parameters;
    protected readonly int slot;

    public MaterialSlotBase(ShaderParameters parameters, int slot)
    {
        this.parameters = parameters;
        this.slot = slot;
    }
}

public class TextureSlot : MaterialSlotBase
{
    public TextureSlot(ShaderParameters parameters, int slot) : base(parameters, slot)
    {
    }

    public void Set(Texture texture)
    {
        parameters.SetShaderResource(slot, texture);
    }
}

public class SamplerSlot : MaterialSlotBase
{
    public SamplerSlot(ShaderParameters parameters, int slot) : base(parameters, slot)
    {
    }

    public void Set(Sampler sampler)
    {
        parameters.SetSampler(slot, sampler);
    }
}

public class BufferSlot : MaterialSlotBase
{
    public BufferSlot(ShaderParameters parameters, int slot) : base(parameters, slot)
    {
    }

    public void Set(RawBuffer buffer)
    {
        parameters.SetShaderResource(slot, buffer);
    }
}

public class ConstantBufferSlot<T> : MaterialSlotBase
    where T : unmanaged
{
    public ConstantBufferSlot(ShaderParameters parameters, int slot) : base(parameters, slot)
    {
    }

    public void Set(ConstantBuffer<T> buffer)
    {
        parameters.SetConstantBuffer(slot, buffer);
    }
}
