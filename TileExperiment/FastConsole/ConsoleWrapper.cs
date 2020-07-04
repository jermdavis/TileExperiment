using System;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace TileExperiment.FastConsole
{

    //
    // Inspired by https://stackoverflow.com/a/2754674/847953
    //
    public class ConsoleWrapper : IDisposable
    {
        [DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        static extern SafeFileHandle CreateFileW(
            string fileName,
            [MarshalAs(UnmanagedType.U4)] uint fileAccess,
            [MarshalAs(UnmanagedType.U4)] uint fileShare,
            IntPtr securityAttributes,
            [MarshalAs(UnmanagedType.U4)] FileMode creationDisposition,
            [MarshalAs(UnmanagedType.U4)] int flags,
            IntPtr template);

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        static extern bool WriteConsoleOutputW(
          SafeFileHandle hConsoleOutput,
          CharInfo[] lpBuffer,
          Coord dwBufferSize,
          Coord dwBufferCoord,
          ref SmallRect lpWriteRegion);

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        static extern bool SetConsoleOutputCP(uint page);

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        static extern uint GetConsoleOutputCP();

        private SafeFileHandle h;
        private CharInfo[] buf;
        private SmallRect rect;
        private short _bufferWidth;
        private short _bufferHeight;

        public short Width => _bufferWidth;
        public short Height => _bufferHeight;

        public CharInfo[] Buffer => buf;

        public ConsoleWrapper(short bufferWidth, short bufferHeight)
        {
            _bufferWidth = bufferWidth;
            _bufferHeight = bufferHeight;

            //uint x = GetConsoleOutputCP();
            //bool b = SetConsoleOutputCP(1252);
            //bool b = SetConsoleOutputCP(850);
            //bool b = SetConsoleOutputCP(1250);

            h = CreateFileW("CONOUT$", 0x40000000, 2, IntPtr.Zero, FileMode.Open, 0, IntPtr.Zero);

            //uint x = GetConsoleOutputCP();
            //bool b = SetConsoleOutputCP(1252);
            //bool b = SetConsoleOutputCP(850);

            if (h.IsInvalid)
            {
                throw new InvalidOperationException("Unable to get handle to console window.");   
            }

            buf = new CharInfo[_bufferWidth * _bufferHeight];
            rect = new SmallRect() { Left = 0, Top = 0, Right = _bufferWidth, Bottom = _bufferHeight };
        }

        public void RenderBuffer()
        {
            if (!h.IsInvalid && !h.IsClosed)
            {
                WriteConsoleOutputW(h, buf, new Coord() { X = _bufferWidth, Y = _bufferHeight }, new Coord() { X = 0, Y = 0 }, ref rect);
            }
        }

        public void Dispose()
        {
            if( h!= null )
            {
                h.Dispose();
            }
        }
    }

}
