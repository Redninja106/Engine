using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Graphics;

public class ShaderParameters
{

    public TextureParameters Textures;
    public SamplerParameters Samplers;
    public BufferParameters BufferParameters;
    public ConstantBufferParameters ConstantBufferParameters;

}

public class TextureParameters
{
    public ref Texture this[string name] { get => throw null!; }
}

public class SamplerParameters
{
}

public class BufferParameters
{

}

public class ConstantBufferParameters
{

}