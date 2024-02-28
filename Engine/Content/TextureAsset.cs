using Engine.Debugging;
using Engine.Graphics;
using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Content;
internal class TextureAsset(string file, Texture texture) : Asset(file)
{
    public override void Layout()
    {
        ImGui.Text($"Texture: {File}");
        ImGui.Text($"{texture.Width}x{texture.Height}");

        float aspectRatio = texture.Width / (float)texture.Height;

        ImGui.Image(texture.View.NativePointer, new(200, 200 / aspectRatio));
        ImGui.EndChild();
    }
}
