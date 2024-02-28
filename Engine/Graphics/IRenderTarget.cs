using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vortice.Direct3D11;

namespace Engine.Graphics;
public interface IRenderTarget : IDisposable
{
    int Width { get; }
    int Height { get; }
    ID3D11RenderTargetView RenderTargetView { get; }
    ID3D11DepthStencilView? DepthStencilView { get; }
}
