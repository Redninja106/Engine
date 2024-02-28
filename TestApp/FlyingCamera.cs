using Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace TestApp;
internal class FlyingCamera : Component
{
    float xr, yr;

    public override void Update(float deltaTime)
    {
        float dxr = 0, dyr = 0;
        if (App.Input.IsKeyDown(Key.UpArrow))
        {
            dxr--;
        }
        if (App.Input.IsKeyDown(Key.DownArrow))
        {
            dxr++;
        }
        if (App.Input.IsKeyDown(Key.LeftArrow))
        {
            dyr++;
        }
        if (App.Input.IsKeyDown(Key.RightArrow))
        {
            dyr--;
        }

        xr += dxr * deltaTime * MathF.PI;
        yr += dyr * deltaTime * MathF.PI;

        Transform.Rotation = Quaternion.Concatenate(
            Quaternion.CreateFromAxisAngle(Vector3.UnitX, xr),
            Quaternion.CreateFromAxisAngle(Vector3.UnitY, yr)
            );

        Vector3 deltaPosition = Vector3.Zero;
        if (App.Input.IsKeyDown(Key.W))
        {
            deltaPosition += Vector3.UnitZ;
        }
        if (App.Input.IsKeyDown(Key.S))
        {
            deltaPosition -= Vector3.UnitZ;
        }
        if (App.Input.IsKeyDown(Key.A))
        {
            deltaPosition += Vector3.UnitX;
        }
        if (App.Input.IsKeyDown(Key.D))
        {
            deltaPosition -= Vector3.UnitX;
        }
        if (App.Input.IsKeyDown(Key.Space))
        {
            deltaPosition += Vector3.UnitY;
        }
        if (App.Input.IsKeyDown(Key.C))
        {
            deltaPosition -= Vector3.UnitY;
        }

        Transform.Position += Vector3.Transform(10 * deltaPosition * deltaTime, Transform.Rotation);
    }
}
