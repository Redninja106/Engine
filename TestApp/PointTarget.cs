using Engine;
using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace TestApp;
internal class PointTarget : Component
{
    Vector3 target;

    public override void Update(float deltaTime)
    {
        ImGui.DragFloat3("target", ref target);
        ImGui.DragFloat3("pos", ref Transform.Position);
        Transform.LookAt(target);
    }
}
