using Engine.Content;
using Engine.Debugging;
using Engine.Graphics;
using GLFW;
using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vortice.Direct3D11;

namespace Engine;
public static class App
{
    public static GraphicsService Graphics { get; private set; }
    public static WindowService Window { get; private set; }
    public static ScenesService Scenes { get; private set; }
    public static AssetsService Assets { get; private set; }
    public static InputService Input { get; private set; }
    public static DebugService Debug { get; private set; }

    public static class AppHost
    {
        public static void InitializeServices()
        {
            Glfw.WindowHint(Hint.ClientApi, ClientApi.None);
            Debug = new();
            Window = new(1920, 1080, "Engine");
            Graphics = new();
            Assets = new();
            Scenes = new();
            Input = new();
        }

        public static void Run()
        {
            ImGuiHandler.Initialize();

            DateTime lastFrameTime = DateTime.UtcNow;
            while (!Window.ShouldClose)
            {
                DateTime frameTime = DateTime.UtcNow;

                Window.NewFrame();
                Glfw.PollEvents();
                ImGuiHandler.BeginFrame();
                Debug.UpdateWindows();

                TimeSpan deltaTime = frameTime - lastFrameTime;
                Scenes.Current?.Update((float)deltaTime.TotalSeconds);

                Graphics.ImmediateContext.ClearRenderTargetView(Graphics.WindowRenderTarget.RenderTargetView, new(0, 0, 0));
                if (Graphics.WindowRenderTarget.DepthStencilView is not null)
                {
                    Graphics.ImmediateContext.ClearDepthStencilView(Graphics.WindowRenderTarget.DepthStencilView, DepthStencilClearFlags.Depth | DepthStencilClearFlags.Stencil, 1f, 0);
                }

                foreach (var camera in Scenes.Current?.ActiveCameras ?? [])
                {
                    camera.RenderDriver.Render(Scenes.Current!, camera);
                }

                ImGuiHandler.EndFrame();

                Graphics.Present();
                lastFrameTime = frameTime;
            }

            ImGuiHandler.Destroy();
        }
    }
}
