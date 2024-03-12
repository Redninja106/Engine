using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vortice.Direct3D11;
using Vortice.DXGI;

namespace Engine.Graphics;
public class RawBuffer : IDisposable, IShaderResource
{
    public ID3D11Buffer InternalBuffer { get; }
    public int SizeInBytes { get; }
    public ID3D11ShaderResourceView View => view ??= CreateView();
    private int? structuredBufferByteStride;

    private ID3D11ShaderResourceView? view;

    public RawBuffer(int sizeInBytes, BindFlags bindFlags, int? structuredBufferByteStride)
    {
        SizeInBytes = sizeInBytes;
        InternalBuffer = App.Graphics.Device.CreateBuffer(
            sizeInBytes, 
            bindFlags, 
            miscFlags: structuredBufferByteStride != null ? ResourceOptionFlags.BufferStructured : 0, 
            structureByteStride: structuredBufferByteStride.GetValueOrDefault()
            );
        this.structuredBufferByteStride = structuredBufferByteStride;
    }

    public void SetData(ReadOnlySpan<byte> bytes)
    {
        App.Graphics.ImmediateContext.UpdateSubresource(bytes, InternalBuffer);
    }
    private ID3D11ShaderResourceView CreateView()
    {
        return App.Graphics.Device.CreateShaderResourceView(InternalBuffer, new(InternalBuffer, Format.Unknown, 0, SizeInBytes / structuredBufferByteStride!.Value));
    }

    public void Dispose()
    {
        InternalBuffer.Dispose();
        view?.Dispose();
    }
}
