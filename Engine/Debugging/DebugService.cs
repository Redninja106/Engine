using Engine.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Debugging;
public class DebugService
{
    private readonly List<DebugWindow> windows = new();

    public Inspector Inspector { get; }
    public SceneViewer SceneViewer { get; }

    public DebugService()
    {
        RegisterWindow(Inspector = new());
        RegisterWindow(SceneViewer = new());
    }

    public void Log(string message)
    {

    }

    public void RegisterWindow(DebugWindow window)
    {
        windows.Add(window);
    }

    internal void UpdateWindows()
    {
        foreach (var window in windows)
        {
            window.Update();
        }
    }

    public void Assert(bool condition)
    {
        if (!condition)
        {
            throw new Exception("Assert failed!");
        }
    }
}
