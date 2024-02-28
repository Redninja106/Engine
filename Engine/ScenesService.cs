using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine;
public class ScenesService
{
    public Scene? Current { get; private set; }

    public ScenesService()
    {
    }

    public void Start(Scene scene)
    {
        Current = scene;
    }
}
