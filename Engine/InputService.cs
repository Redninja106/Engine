using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine;
public class InputService
{
    public bool IsKeyDown(Key key)
    {
        return App.Window.PressedKeys.Contains(key);
    }

    public bool IsKeyJustPressed(Key key)
    {
        return !App.Window.LastPressedKeys.Contains(key) && App.Window.PressedKeys.Contains(key);
    }
    public bool IsKeyJustReleased(Key key)
    {
        return App.Window.LastPressedKeys.Contains(key) && !App.Window.PressedKeys.Contains(key);
    }
}
