using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Graphics;
public class PointLight : Component
{
    public float radius = 1;

    public override void Update(float deltaTime)
    {
    }

    public override void Layout()
    {
        ImGui.DragFloat("radius", ref radius);
    }
}
