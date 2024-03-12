using Engine.Debugging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine;
public abstract class Component : IInspectable
{
    public Actor Actor { get; internal set; }
    public Transform Transform => Actor.Transform;

    public abstract void Update(float deltaTime);

    public virtual void Initialize()
    {

    }

    public virtual void Layout()
    {
    }
}
