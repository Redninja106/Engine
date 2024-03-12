using Vortice.Direct3D11;

namespace Engine.Graphics;

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