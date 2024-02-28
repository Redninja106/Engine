using Engine.Content;
using Engine.Debugging;
using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vortice.Direct3D11;

namespace Engine.Graphics;

public abstract class ShaderParameter : IInspectable
{
    public string Name { get; }

    public abstract void Layout();

    public abstract void BindVS(ID3D11DeviceContext4 context);
    public abstract void BindPS(ID3D11DeviceContext4 context);
}

public class ConstantBufferShaderParameter<T> : ShaderParameter
    where T : unmanaged
{
    private T value;
    public ref T Value => ref value;

    public override void Layout()
    {
        throw new NotImplementedException();
    }
    public override void BindPS(ID3D11DeviceContext4 context)
    {
        throw new NotImplementedException();
    }

    public override void BindVS(ID3D11DeviceContext4 context)
    {
        throw new NotImplementedException();
    }
}

public class TextureShaderParameter(int slot) : ShaderParameter
{
    public int slot = slot;
    public Sampler? Sampler;
    public Texture? Texture;

    public override void Layout()
    {
        if (Texture is null)
        {

        }
        else
        {
            if (ImGui.TreeNode($"texture: {Texture.Width}x{Texture.Height}"))
            {
                float aspect = Texture.Width / (float)Texture.Height;
                ImGui.Image(Texture.View.NativePointer, new(200, 200 / aspect));
            }
        }
    }

    public override void BindPS(ID3D11DeviceContext4 context)
    {
        context.PSSetShaderResource(slot, Texture?.View!);
        context.PSSetSampler(slot, Sampler?.SamplerState);
    }

    public override void BindVS(ID3D11DeviceContext4 context)
    {
        context.PSSetShaderResource(slot, Texture?.View!);
        context.PSSetSampler(slot, Sampler?.SamplerState);
    }
}


public class Sampler
{
    public static Sampler DefaultLinear { get; } = new(SamplerDescription.LinearClamp);
    public static Sampler DefaultPoint { get; } = new(SamplerDescription.PointClamp);

    public Sampler(SamplerDescription desc)
    {
        SamplerState = App.Graphics.Device.CreateSamplerState(desc);
    }

    public ID3D11SamplerState SamplerState { get; set; }
}