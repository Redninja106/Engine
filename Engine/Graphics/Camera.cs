using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Graphics;

public class Camera : Component
{
    public RenderDriver RenderDriver;
    public IRenderTarget renderTarget;
    public float FieldOfView { get; set; } = 60f;

    public Camera()
    {
        RenderDriver = new BasicRenderDriver();
        renderTarget = App.Graphics.WindowRenderTarget;
    }

    public override void Update(float deltaTime)
    {
    }

    public Matrix4x4 CreateProjectionMatrix()
    {
        return Matrix4x4.CreatePerspectiveFieldOfView(FieldOfView * (MathF.PI / 180f), renderTarget.Width / (float)renderTarget.Height, 0.01f, 100f);
    }

    public Matrix4x4 CreateViewMatrix()
    {
        return Matrix4x4.CreateLookAt(Transform.Position, Transform.Position + Transform.Forward, Transform.Up);
    }
}