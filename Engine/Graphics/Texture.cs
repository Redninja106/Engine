using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vortice.Direct3D11;

namespace Engine.Graphics;
public class Texture
{
    public static Texture White1x1 { get; }

    private ID3D11Texture2D texture;
    public int Width { get; }
    public int Height { get; }
    public ID3D11ShaderResourceView View { get; set; }
    public Sampler DefaultSampler { get; set; }

    static Texture()
    {
        White1x1 = new(1, 1);
        White1x1.DefaultSampler = Sampler.DefaultPoint;
        White1x1.Update([0xFF, 0xFF, 0xFF, 0xFF]);
    }

    internal Texture(int width, int height)
    {
        Width = width;
        Height = height;

        Texture2DDescription desc = new()
        {
            Width = width,
            Height = height,
            ArraySize = 1,
            BindFlags = BindFlags.ShaderResource,
            CPUAccessFlags = CpuAccessFlags.None,
            SampleDescription = new(1, 0),
            Format = Vortice.DXGI.Format.R8G8B8A8_UNorm,
            MipLevels = 1,
            MiscFlags = ResourceOptionFlags.None,
            Usage = ResourceUsage.Default,
        };

        texture = App.Graphics.Device.CreateTexture2D(desc);

        View = App.Graphics.Device.CreateShaderResourceView(texture);
    }

    public void Update(byte[] data)
    {
        App.Graphics.ImmediateContext.UpdateSubresource(data, texture, rowPitch: Width * 4);
    }
}
