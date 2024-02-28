using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Debugging;
public class Inspector : DebugWindow
{
    private IInspectable? target;

    public override void OnLayout()
    {
        target?.Layout();
    }

    public void Inspect(IInspectable inspectable)
    {
        Focus();
        target = inspectable;
    }
}
