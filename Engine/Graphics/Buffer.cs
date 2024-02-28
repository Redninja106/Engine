using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vortice.Direct3D11;

namespace Engine.Graphics;
public class Buffer : IDisposable
{
    public ID3D11Buffer InternalBuffer { get; }
    public int SizeInBytes { get; }

    public Buffer(int sizeInBytes, BindFlags bindFlags)
    {
        SizeInBytes = sizeInBytes;
        InternalBuffer = App.Graphics.Device.CreateBuffer(sizeInBytes, bindFlags);
    }

    public void SetData(ReadOnlySpan<byte> bytes)
    {
        App.Graphics.ImmediateContext.UpdateSubresource(bytes, InternalBuffer);
    }

    public void Dispose()
    {
        InternalBuffer.Dispose();
    }
}
