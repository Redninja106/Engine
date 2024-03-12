using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Vortice.Direct3D11;

namespace Engine.Graphics;
public class ConstantBuffer<T> : Buffer<T>
    where T : unmanaged
{
    private T value;

    public ref T Value => ref value;

    public ConstantBuffer() : base(1, BindFlags.ConstantBuffer, null)
    {
    }

    public virtual void Upload()
    {
        ReadOnlySpan<T> valueSpan = new(ref value);
        ReadOnlySpan<byte> bytes = MemoryMarshal.AsBytes(valueSpan);
        SetData(bytes);
    }
}
