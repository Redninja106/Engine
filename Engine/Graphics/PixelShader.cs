using System;
using System.Diagnostics.CodeAnalysis;
using Vortice.Direct3D11;

namespace Engine.Graphics;

public class PixelShader : Shader
{
    public ID3D11PixelShader Shader { get; private set; }

    public PixelShader(string fileName, string entryPoint = "main") : base(fileName, entryPoint, "ps_5_0")
    {
    }

    protected override void CreateShader(ReadOnlyMemory<byte> bytecode)
    {
        Shader?.Dispose();
        Shader = App.Graphics.Device.CreatePixelShader(bytecode.Span);
    }

    public override void Bind(ID3D11DeviceContext context)
    {
        context.PSSetShader(Shader);
    }

    public override void Dispose()
    {
        Shader?.Dispose();
        base.Dispose();
    }
}