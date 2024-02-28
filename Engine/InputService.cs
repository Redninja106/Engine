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

    public bool IsKeyPressed(Key key)
    {
        return App.Window.PressedKeys.Contains(key);
    }
}
