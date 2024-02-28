using SharpGen.Runtime;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vortice.Direct3D;
using Vortice.Direct3D11;
using Vortice.Direct3D11.Debug;
using Vortice.DXGI;

namespace Engine.Graphics;
public class GraphicsService : IService
{
    public ID3D11Device5 Device { get; set; } = null!;
    public ID3D11DeviceContext4 ImmediateContext { get; set; } = null!;
    public IDXGISwapChain1 SwapChain { get; set; } = null!;
    public IRenderTarget WindowRenderTarget => windowRenderTarget;
    public ID3D11Debug Debug { get; set; }
    public event Action? BeforeResize;
    public event Action? AfterResize;
    public Format BackBufferFormat { get; } = Format.R8G8B8A8_UNorm;
    public int SwapInterval { get; set; }

    private WindowRenderTarget windowRenderTarget;

    public GraphicsService()
    {
        var featureLevels = new[]
        {
            FeatureLevel.Level_11_1,
            FeatureLevel.Level_11_0,
            FeatureLevel.Level_10_1,
            FeatureLevel.Level_10_0,
            FeatureLevel.Level_9_3,
            FeatureLevel.Level_9_2,
            FeatureLevel.Level_9_1,
        };

        Result hr = D3D11.D3D11CreateDevice(null, DriverType.Hardware, DeviceCreationFlags.Debug, featureLevels, out ID3D11Device? device0);

        if (hr.Failure)
            throw new Exception();

        Debug = device0!.QueryInterface<ID3D11Debug>();
        Device = device0!.QueryInterface<ID3D11Device5>();

        ImmediateContext = Device!.ImmediateContext3.QueryInterface<ID3D11DeviceContext4>();

        CreateSwapChain(App.Window.Hwnd, App.Window.Width, App.Window.Height);
    }

    public void CreateSwapChain(nint hwnd, int width, int height)
    {
        using var dxgiDevice = Device.QueryInterface<IDXGIDevice>();

        using var adapter = dxgiDevice.GetAdapter();

        using var factory = adapter.GetParent<IDXGIFactory2>();

        SwapChainDescription1 swapChainDesc = new()
        {
            Format = BackBufferFormat,
            SampleDescription = new(1, 0),
            AlphaMode = AlphaMode.Ignore,
            BufferCount = 2,
            BufferUsage = Usage.RenderTargetOutput,
            Flags = SwapChainFlags.None,
            Height = height,
            Width = width,
            Scaling = Scaling.None,
            Stereo = false,
            SwapEffect = SwapEffect.FlipSequential,
        };

        SwapChain = factory.CreateSwapChainForHwnd(Device, hwnd, swapChainDesc);

        windowRenderTarget = new(Device, SwapChain);
    }

    public void Resize(int width, int height)
    {
        if (width == 0 || height == 0)
            return;

        BeforeResize?.Invoke();
        windowRenderTarget.DestroyRenderTargetView();

        SwapChain.ResizeBuffers(2, width, height);

        windowRenderTarget.CreateRenderTargetView(this.Device, this.SwapChain);
        AfterResize?.Invoke();
    }

    public void Present()
    {
        var hr = SwapChain.Present(SwapInterval);
        hr.CheckError();
    }
}

public class WindowRenderTarget : IRenderTarget
{
    public int Width { get; private set; }
    public int Height { get; private set; }
    public ID3D11RenderTargetView RenderTargetView { get; private set; }
    public ID3D11DepthStencilView? DepthStencilView { get; private set; }

    public WindowRenderTarget(ID3D11Device device, IDXGISwapChain1 SwapChain)
    {
        CreateRenderTargetView(device, SwapChain);
    }

    [MemberNotNull(nameof(RenderTargetView))]
    public void CreateRenderTargetView(ID3D11Device device, IDXGISwapChain1 SwapChain)
    {
        using var backBuffer = SwapChain.GetBuffer<ID3D11Texture2D>(0);
        var desc = new RenderTargetViewDescription(backBuffer, RenderTargetViewDimension.Texture2D, backBuffer.Description.Format);
        Width = backBuffer.Description.Width;
        Height = backBuffer.Description.Height;

        RenderTargetView = device.CreateRenderTargetView(backBuffer, desc);

        Texture2DDescription depthStencilDesc = new()
        {
            ArraySize = 1,
            Format = Format.D24_UNorm_S8_UInt,
            Width = Width,
            Height = Height,
            SampleDescription = new(1, 0),
            BindFlags = BindFlags.DepthStencil,
        };

        using var depthStencilTexture = device.CreateTexture2D(in depthStencilDesc);
        DepthStencilView = device.CreateDepthStencilView(depthStencilTexture, new(DepthStencilViewDimension.Texture2D, depthStencilDesc.Format));
    }

    public void DestroyRenderTargetView()
    {
        RenderTargetView?.Dispose();
    }

    public void Dispose()
    {
        DestroyRenderTargetView();
    }
}