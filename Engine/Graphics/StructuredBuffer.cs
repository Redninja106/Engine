using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Vortice.Direct3D11;

namespace Engine.Graphics;
public class StructuredBuffer<T> : Buffer<T>
    where T : unmanaged
{
    private readonly T[] elements;

    public ref T this[int index] => ref elements[index];
  
    public StructuredBuffer(int length) : base(length, BindFlags.ShaderResource, Unsafe.SizeOf<T>())
    {
        elements = new T[length];
    }

    public void Upload()
    {
        SetData(elements);
    }
}
