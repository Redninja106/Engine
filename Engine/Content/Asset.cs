using Engine.Debugging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Content;
public abstract class Asset(string file) : IInspectable
{
    public string File => file;

    public abstract void Layout();
}
