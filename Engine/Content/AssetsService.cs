using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Assimp.Unmanaged;
using Engine.Graphics;
using StbImageSharp;

namespace Engine.Content;
public class AssetsService
{
    Assimp.AssimpContext context = new();

    private List<Asset> loadedAssets = new();

    public IEnumerable<Asset> LoadedAssets => loadedAssets;

    public AssetsService()
    {
        App.Debug.RegisterWindow(new AssetBrowser());
    }

    public Mesh LoadMesh(string file)
    {
        Assimp.Scene scene = context.ImportFile(file, Assimp.PostProcessSteps.Triangulate);
        Assimp.Mesh assimpMesh = scene.Meshes.Single();

        MeshData data = new();
        data.positions = assimpMesh.Vertices.Select(v => new Vector3(v.X, v.Y, v.Z)).ToArray();
        data.normals = (assimpMesh.HasNormals ? assimpMesh.Normals : null)?.Select(v => new Vector3(v.X, v.Y, v.Z))?.ToArray();
        data.uvs = (assimpMesh.HasTextureCoords(0) ? assimpMesh.TextureCoordinateChannels[0] : null)?.Select(v => new Vector2(v.X, v.Y))?.ToArray();
        data.indices = assimpMesh.GetUnsignedIndices();

        return new Mesh(data);
    }

    public Texture LoadTexture(string file)
    {
        using var fs = new FileStream(file, FileMode.Open);
        var image = ImageResult.FromStream(fs, ColorComponents.RedGreenBlueAlpha);
        
        Texture result = new Texture(image.Width, image.Height);
        result.Update(image.Data);
        result.DefaultSampler = Sampler.DefaultLinear;

        loadedAssets.Add(new TextureAsset(file, result));

        return result;
    }

    public VertexShader LoadVertexShader(string file)
    {
        return new(file);
    }

    public PixelShader LoadPixelShader(string file)
    {
        return new(file);
    }
}
