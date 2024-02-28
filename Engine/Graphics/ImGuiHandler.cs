using GLFW;
using ImGuiNET;
using SharpGen.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Graphics;
internal static class ImGuiHandler
{
    private static nint glfwWndProc;

    public unsafe static void Initialize()
    {
        ImGui.SetCurrentContext(ImGui.CreateContext());
        ImGui.ImGui_ImplWin32_Init(App.Window.Hwnd);
        ImGui.ImGui_ImplDX11_Init((void*)App.Graphics.Device.NativePointer, (void*)App.Graphics.ImmediateContext.NativePointer);

        glfwWndProc = SetWindowLongPtrA(App.Window.Hwnd, -4, (nint)(delegate* unmanaged[Stdcall] <nint, uint, nint, nint, nint>)&WndProcIntercept);
    }

    public static void BeginFrame()
    {
        ImGui.ImGui_ImplWin32_NewFrame();
        ImGui.ImGui_ImplDX11_NewFrame();
        ImGui.NewFrame();
    }

    public static void EndFrame()
    {
        ImGui.EndFrame();
        ImGui.Render();
        ImGui.ImGui_ImplDX11_RenderDrawData(ImGui.GetDrawData());
    }

    public static void Destroy()
    {
        ImGui.ImGui_ImplWin32_Shutdown();
        ImGui.ImGui_ImplDX11_Shutdown();
    }

    [UnmanagedCallersOnly(CallConvs = [typeof(CallConvStdcall)])]
    private static unsafe nint WndProcIntercept(nint hwnd, uint msg, nint wParam, nint lParam)
    {
        if (ImGui.ImGui_ImplWin32_WndProcHandler((void*)hwnd, msg, wParam, lParam) > 0)
        {
            return 0;
        }

        return CallWindowProcA(glfwWndProc, hwnd, msg, wParam, lParam);
    }

    [DllImport("User32.dll")]
    private static extern nint SetWindowLongPtrA(nint hwnd, int index, nint newLong);
    // [DllImport("User32.dll")]
    // private static extern nint GetWindowLongPtrA(nint hwnd, int index);
    [DllImport("User32.dll")]
    private static extern nint CallWindowProcA(nint prevWndFunc, nint hwnd, uint msg, nint wParam, nint lParam);
}
