using GLFW;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Engine;
public class WindowService(int width, int height, string title) : IService
{
    private EngineWindow nativeWindow = new(width, height, title);

    public bool ShouldClose => Glfw.WindowShouldClose(nativeWindow.Window);

    public nint Hwnd => nativeWindow.Hwnd;
    public int Width => nativeWindow.Size.Width;
    public int Height => nativeWindow.Size.Height;

    public HashSet<Key> PressedKeys => nativeWindow.pressedKeys;

    private class EngineWindow(int width, int height, string title) : NativeWindow(width, height, title)
    {
        public new Window Window => base.Window;
        public readonly HashSet<Key> pressedKeys = []; 

        protected override void OnFramebufferSizeChanged(int width, int height)
        {
            App.Graphics?.Resize(width, height);
            base.OnFramebufferSizeChanged(width, height);
        }

        protected override void OnKey(Keys key, int scanCode, InputState state, ModifierKeys mods)
        {
            Key engineKey = key switch
            {
                Keys.LeftControl => Key.LeftControl,
                Keys.RightControl => Key.RightControl,
                Keys.LeftAlt => Key.LeftAlt,
                Keys.RightAlt => Key.RightAlt,
                Keys.LeftShift => Key.LeftShift,
                Keys.RightShift => Key.RightShift,
                Keys.GraveAccent => Key.Grave,
                Keys.Minus => Key.Minus,
                Keys.Equal => Key.Equal,
                Keys.Backspace => Key.Backspace,
                Keys.Tab => Key.Tab,
                Keys.LeftBracket => Key.LeftBracket,
                Keys.RightBracket => Key.RightBracket,
                Keys.Backslash => Key.Backslash,
                Keys.CapsLock => Key.CapsLock,
                Keys.SemiColon => Key.SemiColon,
                Keys.Apostrophe => Key.Apostrophe,
                Keys.Enter => Key.Enter,
                Keys.Comma => Key.Comma,
                Keys.Period => Key.Period,
                Keys.Slash => Key.Slash,
                Keys.Menu => Key.Menu,
                Keys.Up => Key.UpArrow,
                Keys.Down => Key.DownArrow,
                Keys.Left => Key.LeftArrow,
                Keys.Right => Key.RightArrow,
                Keys.F1 => Key.F1,
                Keys.F2 => Key.F2,
                Keys.F3 => Key.F3,
                Keys.F4 => Key.F4,
                Keys.F5 => Key.F5,
                Keys.F6 => Key.F6,
                Keys.F7 => Key.F7,
                Keys.F8 => Key.F8,
                Keys.F9 => Key.F9,
                Keys.F10 => Key.F10,
                Keys.F11 => Key.F11,
                Keys.F12 => Key.F12,
                Keys.Space => Key.Space,
                Keys.A => Key.A,
                Keys.B => Key.B,
                Keys.C => Key.C,
                Keys.D => Key.D,
                Keys.E => Key.E,
                Keys.F => Key.F,
                Keys.G => Key.G,
                Keys.H => Key.H,
                Keys.I => Key.I,
                Keys.J => Key.J,
                Keys.K => Key.K,
                Keys.L => Key.L,
                Keys.M => Key.M,
                Keys.N => Key.N,
                Keys.O => Key.O,
                Keys.P => Key.P,
                Keys.Q => Key.Q,
                Keys.R => Key.R,
                Keys.S => Key.S,
                Keys.T => Key.T,
                Keys.U => Key.U,
                Keys.V => Key.V,
                Keys.W => Key.W,
                Keys.X => Key.X,
                Keys.Y => Key.Y,
                Keys.Z => Key.Z,
                Keys.Alpha0 => Key.Key0,
                Keys.Alpha1 => Key.Key1,
                Keys.Alpha2 => Key.Key2,
                Keys.Alpha3 => Key.Key3,
                Keys.Alpha4 => Key.Key4,
                Keys.Alpha5 => Key.Key5,
                Keys.Alpha6 => Key.Key6,
                Keys.Alpha7 => Key.Key7,
                Keys.Alpha8 => Key.Key8,
                Keys.Alpha9 => Key.Key9,
                _ => Key.Unknown,
            };


            if (state == InputState.Press)
            {
                pressedKeys.Add(engineKey);
            }
            else if (state == InputState.Release)
            {
                pressedKeys.Remove(engineKey);
            }

            base.OnKey(key, scanCode, state, mods);
        }
    }
}
