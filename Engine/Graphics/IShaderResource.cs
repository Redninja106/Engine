using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vortice.Direct3D11;

namespace Engine.Graphics;
public interface IShaderResource
{
    ID3D11ShaderResourceView View { get; }
}
