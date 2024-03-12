using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Vortice.D3DCompiler;
using Vortice.Direct3D;
using Vortice.Direct3D11;
using Vortice.Direct3D11.Shader;
using Vortice.DXGI;

namespace Engine.Graphics;

public class VertexShader : Shader
{
    public ID3D11VertexShader Shader { get; private set; }
    public ID3D11InputLayout InputLayout { get; private set; }

    public VertexShader(string fileName, string entryPoint = "main") : base(fileName, entryPoint, "vs_5_0")
    {
    }

    protected override void CreateShader(ReadOnlyMemory<byte> bytecode)
    {
        Shader?.Dispose();
        Shader = App.Graphics.Device.CreateVertexShader(bytecode.Span);

        CreateInputLayout(bytecode);
    }

    public override void Bind(ID3D11DeviceContext context)
    {
        context.VSSetShader(Shader);
        context.IASetInputLayout(InputLayout);
    }

    private void CreateInputLayout(ReadOnlyMemory<byte> bytecode)
    {
        InputLayout?.Dispose();

        var reflection = Compiler.Reflect<ID3D11ShaderReflection>(bytecode.Span);

        var inputElements = reflection.InputParameters.Select(ShaderParameterToInputElement).ToArray();

        InputLayout = App.Graphics.Device.CreateInputLayout(inputElements, bytecode.Span);
    }

    private InputElementDescription ShaderParameterToInputElement(ShaderParameterDescription shaderParameter)
    {
        VertexAttribute attribute = Enum.Parse<VertexAttribute>(shaderParameter.SemanticName, true);

        return new()
        {
            AlignedByteOffset = InputElementDescription.AppendAligned,
            Classification = InputClassification.PerVertexData,
            Format = GetParameterFormat(shaderParameter),
            SemanticIndex = shaderParameter.SemanticIndex,
            SemanticName = shaderParameter.SemanticName,
            Slot = (int)attribute,
        };

        // converted to switch exprs https://gist.github.com/mobius/b678970c61a93c81fffef1936734909f
        static Format GetParameterFormat(ShaderParameterDescription shaderParameter)
        {
            return shaderParameter.UsageMask switch
            {
                < RegisterComponentMaskFlags.ComponentY => shaderParameter.ComponentType switch
                {
                    RegisterComponentType.UInt32 => Format.R32_UInt,
                    RegisterComponentType.SInt32 => Format.R32_SInt,
                    RegisterComponentType.Float32 => Format.R32_Float,
                    _ => Format.Unknown
                },
                < RegisterComponentMaskFlags.ComponentZ => shaderParameter.ComponentType switch
                {
                    RegisterComponentType.UInt32 => Format.R32G32_UInt,
                    RegisterComponentType.SInt32 => Format.R32G32_SInt,
                    RegisterComponentType.Float32 => Format.R32G32_Float,
                    _ => Format.Unknown
                },
                < RegisterComponentMaskFlags.ComponentW => shaderParameter.ComponentType switch
                {
                    RegisterComponentType.UInt32 => Format.R32G32B32_UInt,
                    RegisterComponentType.SInt32 => Format.R32G32B32_SInt,
                    RegisterComponentType.Float32 => Format.R32G32B32_Float,
                    _ => Format.Unknown
                },
                RegisterComponentMaskFlags.All => shaderParameter.ComponentType switch
                {
                    RegisterComponentType.UInt32 => Format.R32G32B32A32_UInt,
                    RegisterComponentType.SInt32 => Format.R32G32B32A32_SInt,
                    RegisterComponentType.Float32 => Format.R32G32B32A32_Float,
                    _ => Format.Unknown
                },
                _ => Format.Unknown,
            };
        }
    }

}
