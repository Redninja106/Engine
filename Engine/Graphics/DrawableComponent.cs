using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Graphics;
public abstract class DrawableComponent : Component
{
    public abstract void Draw(RenderContext camera);

    public override void Update(float deltaTime)
    {
    }
}
