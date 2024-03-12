using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Vortice.Direct3D11;

namespace Engine.Graphics;
public class Buffer<T> : RawBuffer
    where T : unmanaged
{
    public int Length { get; }

    public Buffer(int size, BindFlags bindFlags, int? structuredBufferByteStride = null) : base(size * Unsafe.SizeOf<T>(), bindFlags, structuredBufferByteStride)
    {
        Length = size;
    }

    public void SetData(ReadOnlySpan<T> data)
    {
        var bytes = MemoryMarshal.AsBytes(data);
        SetData(bytes);
    }

    public static Buffer<T> CreateStructured(int length)
    {
        return new(length, BindFlags.ShaderResource, Unsafe.SizeOf<T>());
    }
}
