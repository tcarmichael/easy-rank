using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace EasyRank__
{
    public class MessageSender
    {
        [DllImport("user32.dll")]
        public static extern IntPtr PostMessage(IntPtr hWnd, uint Msg, UIntPtr wParam, uint lParam);

        const uint WM_KEYDOWN = 0x100;
        const uint WM_KEYUP = 0x0101;

        private static readonly Dictionary<char, byte> SCAN_CODE_LOOKUP = new Dictionary<char, byte>
        {
            { 'Q', 0x10 },
            { 'W', 0x11 },
            { 'E', 0x12 },
            { 'R', 0x13 },
            { 'T', 0x14 },
            { 'Y', 0x15 },
            { 'U', 0x16 },
            { 'I', 0x17 },
            { 'O', 0x18 },
            { 'P', 0x19 },
            { 'A', 0x1E },
            { 'S', 0x1F },
            { 'D', 0x20 },
            { 'F', 0x21 },
            { 'G', 0x22 },
            { 'H', 0x23 },
            { 'J', 0x24 },
            { 'K', 0x25 },
            { 'L', 0x26 },
            { 'Z', 0x2C },
            { 'X', 0x2D },
            { 'C', 0x2E },
            { 'V', 0x2F },
            { 'B', 0x30 },
            { 'N', 0x31 },
            { 'M', 0x32 },
        };

        private readonly IntPtr hWnd;
        private readonly char keyCode;

        private readonly uint lParamDown;
        private readonly uint lParamDownRepeat;
        private readonly uint lParamUp;

        public MessageSender(IntPtr hWnd, char keyCode)
        {
            this.hWnd = hWnd;
            this.keyCode = char.ToUpper(keyCode);

            var scanCode = SCAN_CODE_LOOKUP[keyCode];

            // Construct the lParam values.
            // See: https://docs.microsoft.com/en-us/windows/desktop/inputdev/wm-keydown.

            // The first KEYDOWN message only sends the scan code.
            lParamDown = (uint)(scanCode << 16);

            // Repeated KEYDOWN messages set the repeat count and previous key state to 1.
            lParamDownRepeat = lParamDown | (1u) | (1u << 30);

            // KEYUP messages set the transition state to 1.
            lParamUp = lParamDownRepeat | (1u << 31);
        }

        public void SendDown(bool isRepeat)
        {
            var lParam = (isRepeat) ? lParamDownRepeat : lParamDown;
            PostMessage(hWnd, WM_KEYDOWN, (UIntPtr)keyCode, lParam);
        }

        public void SendUp()
        {
            PostMessage(hWnd, WM_KEYDOWN, (UIntPtr)keyCode, lParamUp);
        }
    }
}
