using SharpGen.Runtime;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vortice.D3DCompiler;
using Vortice.Direct3D;
using Vortice.Direct3D11;
using Vortice.Direct3D11.Shader;

namespace Engine.Graphics;

public abstract class Shader : IDisposable
{
    public readonly string fileName;
    public readonly string entryPoint;
    public readonly string compilationProfile;

    public string Source { get; private set; }
    public ReadOnlyMemory<byte> ByteCode { get; private set; }

    public Shader(string fileName, string entryPoint, string compilationProfile)
    {
        // prepend "Shaders/" if it's missing
        if (!fileName.StartsWith("Shaders"))
        {
            fileName = Path.Combine("Shaders", fileName);
        }

#if DEBUG
        // if debugging load the original file instead

        var originalFile = Path.Combine("../../../", fileName);
        if (File.Exists(originalFile))
        {
            fileName = originalFile;
        }
#endif

        this.fileName = fileName;
        this.entryPoint = entryPoint;
        this.compilationProfile = compilationProfile;

        Reload();
    }

    protected abstract void CreateShader(ReadOnlyMemory<byte> bytecode);

    public abstract void Bind(ID3D11DeviceContext context);

    [MemberNotNull(nameof(Source), nameof(ByteCode))]
    public void Reload()
    {
        Source = LoadSourceFile(fileName);

        if (string.IsNullOrWhiteSpace(Source))
        {
            throw new Exception("Empty Shader!");
        }

        try
        {
            ByteCode = Compiler.Compile(Source, entryPoint, fileName, compilationProfile, ShaderFlags.Debug);
        }
        catch (Exception ex)
        {
            ReadOnlySpan<char> lineNumber = ex.Message.AsSpan()[(ex.Message.IndexOf("hlsl(") + 5)..];

            lineNumber = lineNumber[..lineNumber.IndexOf(',')];

            throw new Exception($"Shader compilation error: {ex.Message}. line: '{Source.Split("\n")[int.Parse(lineNumber.ToString()) - 1]}'");
        }

        CreateShader(ByteCode);
    }

    private string LoadSourceFile(string file)
    {
        var source = File.ReadAllText(file);

        source = ReplaceIncludes(source, file);

        return source;
    }

    private string ReplaceIncludes(string source, string fileName)
    {
        fileName = Path.GetFullPath(fileName);
        var dir = Path.GetDirectoryName(fileName)!;

        int index = source.IndexOf("#include");

        while (index != -1)
        {
            var firstQuote = index + source[index..].IndexOf('"');
            var includedFileBegin = firstQuote + 1;
            var secondQuote = includedFileBegin + source[includedFileBegin..].IndexOf('"');

            var included = source[includedFileBegin..secondQuote];

            var fullIncluded = Path.Combine(dir, included);

            if (fileName == fullIncluded)
                throw new("Recursive Include Alert!");

            if (!File.Exists(fullIncluded))
                throw new($"Included file {included} doesnt exist!");

            var includedSource = LoadSourceFile(fullIncluded);

            source = string.Concat(source[..index], includedSource, source[(secondQuote + 1)..]);

            index = source.IndexOf("#include");
        }

        return source;
    }

    public virtual void Dispose()
    {
        GC.SuppressFinalize(this);
    }

    public ShaderParameter[] CreateParameters()
    {
        using ID3D11ShaderReflection reflection = Compiler.Reflect<ID3D11ShaderReflection>(ByteCode.Span);

        List<ShaderParameter> parameters = [];

        foreach (var rsrc in reflection.BoundResources)
        {
            switch (rsrc.Type)
            {
                case ShaderInputType.ConstantBuffer:
                    // parameters.Add(new ConstantBufferShaderParameter());
                    break;
                case ShaderInputType.Texture:
                    parameters.Add(new TextureShaderParameter(rsrc.BindPoint));
                    break;
                case ShaderInputType.Sampler:
                    // samplers are not first-class parameters
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        return [..parameters];
    }

    ~Shader()
    {
        Dispose();
    }

    public class IncludeHandler : Include
    {
        public static readonly IncludeHandler Instance = new();

        private IncludeHandler()
        {
        }

        public void Close(Stream stream)
        {
            stream.Dispose();
        }

        public void Dispose()
        {

        }

        public Stream Open(IncludeType type, string fileName, Stream? parentStream)
        {
            return new FileStream(fileName, FileMode.Open);
        }
    }
}
