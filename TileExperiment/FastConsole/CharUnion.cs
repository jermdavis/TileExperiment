using System;
using System.Runtime.InteropServices;

namespace TileExperiment.FastConsole
{

    [StructLayout(LayoutKind.Explicit)]
    public struct CharUnion
    {
        [FieldOffset(0)] public UInt16 UnicodeChar;
        [FieldOffset(0)] public byte AsciiChar;
    }

}