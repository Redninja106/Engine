using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Debugging;
public abstract class DebugWindow
{
    private bool open;

    protected virtual ImGuiWindowFlags WindowFlags => ImGuiWindowFlags.None;
    protected virtual Key KeyBind => Key.Unknown;
    protected virtual string Title => ToString() ?? "";

    public bool Open { get => open; set => open = value; }
    
    private bool wantFocus = false;

    public abstract void OnLayout();

    public void Update()
    {
        if (App.Input.IsKeyDown(KeyBind))
            open = !open;

        if (wantFocus)
        {
            Open = true;
            ImGui.SetNextWindowFocus();
            wantFocus = false;
        }

        if (open && ImGui.Begin(Title, ref open, WindowFlags))
        {
            OnLayout();
        }
        ImGui.End();
    }

    public void Focus()
    {
        wantFocus = true;
    }
}
