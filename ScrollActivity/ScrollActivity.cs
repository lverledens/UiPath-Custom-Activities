using System.Activities;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace ScrollActivity
{
    public class ScrollActivity : CodeActivity
    {
        private const int MOUSEEVENTF_WHEEL = 0x800; // The amount of movement is specified in mouseData.

        [Category("Input")]
        [DefaultValue(false)]
        [RequiredArgument]
        [Description("Scroll up (True) or down (False)")]
        public InArgument<bool> up { get; set; }

        [Category("Input")]
        [DefaultValue(100)]
        [Description("Amount of pixels to scroll")]
        public InArgument<int> amount { get; set; }

        [DllImport("user32.dll", EntryPoint = "mouse_event")]
        private static extern void mouse_event(uint dwFlags, int dx, int dy, int dwData, uint dwExtraInfo);

        protected override void Execute(CodeActivityContext context)
        {
            if (up.Get(context))
            {
                mouse_event(MOUSEEVENTF_WHEEL, 0, 0, (amount.Get(context) * 1), 0);
            }
            else
            {
                mouse_event(MOUSEEVENTF_WHEEL, 0, 0, -(amount.Get(context) * 1), 0);
            }
        }
    }
}
