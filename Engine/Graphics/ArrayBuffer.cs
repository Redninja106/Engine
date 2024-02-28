using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Vortice.Direct3D11;

namespace Engine.Graphics;
public class ArrayBuffer<T> : TypedBuffer<T>
    where T : unmanaged
{
    private readonly T[] elements;

    public int Length { get; }

    public ref T this[int index] => ref elements[index];
  
    public ArrayBuffer(int length) : base(length, BindFlags.ShaderResource)
    {
        Length = length;
        elements = new T[length];
    }

    public void Upload()
    {
        SetData(elements);
    }

}
