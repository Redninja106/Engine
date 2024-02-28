using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Debugging;
public class SceneViewer : DebugWindow
{
    protected override Key KeyBind => Key.F1;

    public override void OnLayout()
    {
        var scene = App.Scenes.Current;

        if (scene is null)
        {
            ImGui.Text("No active scene!");
            return;
        }

        foreach (var actor in scene.Actors)
        {
            if (ImGui.TreeNode(actor.ToString()))
            {
                ImGui.TreePop();
            }
        }
    }
}
