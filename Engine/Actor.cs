using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Graphics;

namespace Engine;
public sealed class Actor
{
    private static ulong nextId = 1;

    private ulong id;
    public ulong ID => id;
    public Transform Transform { get; }
    private List<Component> components = [];

    public IEnumerable<Component> Components => components;

    public Actor(ReadOnlySpan<Component> components)
    {
        id = nextId++;

        Transform = new();

        foreach (var component in components)
        {
            component.Actor = this;
        }

        this.components.AddRange(components);
    }

    public void Update(float deltaTime)
    {
        foreach (var component in components)
        {
            component.Update(deltaTime);
        }
    }

    public void Draw(RenderContext context)
    {
        foreach (var component in components)
        {
            if (component is DrawableComponent drawable)
            {
                drawable.Draw(context);
            }
        }
    }
}
