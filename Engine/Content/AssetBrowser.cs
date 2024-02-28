using Engine.Debugging;
using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Content;
public class AssetBrowser : DebugWindow
{
    public AssetBrowser()
    {
    }

    protected override Key KeyBind => Key.F3;

    public override void OnLayout()
    {
        foreach (var asset in App.Assets.LoadedAssets)
        {
            // have to use the selected overload cause igSelectable is not in cimgui.dll?
            bool selected = false;
            if (ImGui.Selectable(asset.File, ref selected))
            {
                App.Debug.Inspector.Inspect(asset);
            }
        }
    }
}
