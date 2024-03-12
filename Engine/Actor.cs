using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Debugging;
using Engine.Graphics;
using ImGuiNET;

namespace Engine;
public sealed class Actor : IInspectable
{
    private static ulong nextId = 1;

    private ulong id;
    public ulong ID => id;
    public string? Name { get; set; }
    public Transform Transform { get; }
    private List<Component> components = [];

    public IEnumerable<Component> Components => components;

    public Actor(string? name, ReadOnlySpan<Component> components) : this(components)
    {
        this.Name = name;
    }

    public Actor(ReadOnlySpan<Component> components)
    {
        id = nextId++;

        Transform = new();

        foreach (var component in components)
        {
            component.Actor = this;
        }

        this.components.AddRange(components);

        foreach (var component in components)
        {
            component.Initialize();
        }

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

    public void Layout()
    {
        Transform.Layout();
        foreach (var component in components)
        {
            bool open = true;
            if (ImGui.CollapsingHeader(component.ToString(), ref open, ImGuiTreeNodeFlags.None))
            {
                component.Layout();
            }
        }
    }

    public override string? ToString()
    {
        return this.Name ?? base.ToString();
    }

    public T? GetComponent<T>() where T : Component
    {
        return components.OfType<T>().FirstOrDefault();
    }
}
