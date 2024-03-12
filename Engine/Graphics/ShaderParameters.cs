using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vortice.Direct3D11;

namespace Engine.Graphics;

public class ShaderParameters
{
    private ID3D11ShaderResourceView[] srvs = new ID3D11ShaderResourceView[128];
    private ID3D11SamplerState[] samplers = new ID3D11SamplerState[16];
    private ID3D11Buffer[] cbuffers = new ID3D11Buffer[14];

    public ShaderParameters()
    {

    }

    public void BindVS(ID3D11DeviceContext context)
    {
        context.VSSetShaderResources(0, srvs);
        context.VSSetSamplers(0, samplers);
        context.VSSetConstantBuffers(0, cbuffers);
    }

    public void BindPS(ID3D11DeviceContext context)
    {
        context.PSSetShaderResources(0, srvs);
        context.PSSetSamplers(0, samplers);
        context.PSSetConstantBuffers(0, cbuffers);
    }

    public void SetConstantBuffer<T>(int slot, ConstantBuffer<T> buffer)
        where T : unmanaged
    {
        cbuffers[slot] = buffer.InternalBuffer;
    }

    public void SetSampler(int slot, Sampler sampler)
    {
        samplers[slot] = sampler.SamplerState;
    }

    public void SetShaderResource(int slot, IShaderResource resource)
    {
        srvs[slot] = resource.View;
    }
}