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

        if (ImGui.Button("Add Actor"))
        {
            App.Scenes.Current!.AddActor([]);
        }

        ImGui.Separator();

        foreach (var actor in scene.Actors)
        {
            ImGui.PushID((int)actor.ID);
            bool tree = ImGui.TreeNode(actor.ToString());
            ImGui.SameLine();
            if (ImGui.SmallButton("select"))
            {
                App.Debug.Inspector.Inspect(actor);
            }

            if (tree)
            {
                ImGui.TreePop();
            }
            ImGui.PopID();
        }
    }
}
