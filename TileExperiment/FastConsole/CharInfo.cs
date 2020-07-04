﻿using System.Runtime.InteropServices;

namespace TileExperiment.FastConsole
{

    [StructLayout(LayoutKind.Explicit)]
    public struct CharInfo
    {
        [FieldOffset(0)] public CharUnion Char;
        [FieldOffset(2)] public short Attributes;
    }

}