using Assimp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Vortice.Direct3D11;

namespace Engine.Graphics;
public sealed class BasicRenderDriver : RenderDriver
{
    ID3D11RasterizerState rsState;
    ID3D11DepthStencilState dsState;

    StructuredBuffer<PointLightData> lightBuffer;
    public MatrixBuffer matrixBuffer;

    public BasicRenderDriver()
    {
        rsState = App.Graphics.Device.CreateRasterizerState(RasterizerDescription.CullFront);
        dsState = App.Graphics.Device.CreateDepthStencilState(DepthStencilDescription.Default);
        lightBuffer = new(16);
        matrixBuffer = new();
    }

    public override void Render(Scene scene, Camera camera)
    {
        PointLight[] lights = scene.FindAll<PointLight>().ToArray();

        if (lightBuffer.Length < lights.Length)
        {
            lightBuffer.Dispose();
            lightBuffer = new(lights.Length);
        }

        for (int i = 0; i < lights.Length; i++)
        {
            lightBuffer[i] = new(lights[i]);
        }
        lightBuffer.Upload();

        var immediateContext = App.Graphics.ImmediateContext;
        immediateContext.RSSetState(rsState);
        immediateContext.OMSetDepthStencilState(dsState);
        immediateContext.RSSetViewport(0, 0, camera.renderTarget.Width, camera.renderTarget.Height);
        immediateContext.OMSetRenderTargets(camera.renderTarget.RenderTargetView, camera.renderTarget.DepthStencilView);

        matrixBuffer.SetViewProj(camera);

        RenderContext renderContext = new()
        {
            Camera = camera,
            matrixBuffer = this.matrixBuffer,
            pointLights = this.lightBuffer,
            DeviceContext = immediateContext,
        };

        scene.Draw(renderContext);
    }
}

public struct PointLightData(PointLight light)
{
    public Vector3 position = light.Transform.Position;
    public float radius = light.radius;
}

public class RenderContext
{
    public Camera Camera;
    public MatrixBuffer matrixBuffer;
    public StructuredBuffer<PointLightData> pointLights;
    public ID3D11DeviceContext DeviceContext;

}