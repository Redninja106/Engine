using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Vortice.Direct3D11;

namespace Engine.Graphics;
public class TypedBuffer<T> : Buffer
    where T : unmanaged
{
    public int Length { get; }

    protected TypedBuffer(int size, BindFlags bindFlags) : base(size * Unsafe.SizeOf<T>(), bindFlags)
    {
        Length = size;
    }

    public void SetData(ReadOnlySpan<T> data)
    {
        var bytes = MemoryMarshal.AsBytes(data);
        SetData(bytes);
    }
}
