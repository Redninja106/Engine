using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Graphics;
public abstract class RenderDriver
{
    public abstract void Render(Scene scene, Camera camera);
}
