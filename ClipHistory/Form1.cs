using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Forms;

// TODO: Make delete from clipboard history actually delete from real clipboard? Maybe make it an option
// TODO: Give user the option to set their own key to summon the app
// TODO: Add icon so that it shows up in install and file explorer as well as Task Manager
namespace ClipHistory {
    public partial class Form1 : Form {
        
        // Needed to round corners of window 
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn
        (
            int nLeftRect,     // x-coordinate of upper-left corner
            int nTopRect,      // y-coordinate of upper-left corner
            int nRightRect,    // x-coordinate of lower-right corner
            int nBottomRect,   // y-coordinate of lower-right corner
            int nWidthEllipse, // height of ellipse
            int nHeightEllipse // width of ellipse
        );

        // Clipboard imports
        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool AddClipboardFormatListener(IntPtr hwnd);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool RemoveClipboardFormatListener(IntPtr hwnd);

        // Hooks imports
        private static int _hookHandle = 0;

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        private static extern int SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hInstance, int threadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        private static extern bool UnhookWindowsHookEx(int idHook);

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        private static extern int CallNextHookEx(int idHook, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        private static extern short GetKeyState(int nVirtKey);

        public delegate int HookProc(int nCode, IntPtr wParam, IntPtr lParam);

        // Hook constants
        private const int WH_KEYBOARD_LL = 13;
        private const int VK_SHIFT = 0x10;
        private const int VK_LCONTROL = 0xA2;
        private const int VK_RCONTROL = 0xA3;

        // What actually keeps track of history
        private int historyIndex = 0;
        private const int maxHistory = 20;
        private List<string> history = new List<string>();

        public Form1() {

            // Start form in the center of screen
            StartPosition = FormStartPosition.CenterScreen;
            InitializeComponent();

            // Makes the form have rounded corners
            FormBorderStyle = FormBorderStyle.None;
            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
            
            // Init clipboard listener and hooks
            AddClipboardFormatListener(Handle);
            SetHook();
            SetUp();

            // Start the application with the form minimized and hidden
            WindowState = FormWindowState.Minimized;
            Hide();
        }

        private void SetUp() {

            // Initialize the clipboard history with whatever is in there when the program is launched
            var text = Clipboard.GetText();
            history.Add(text);
            labelMain.Text = text;

            // Handles what happens when the form is minimized
            Resize += Form1_ResizeEnd;

            notifyIcon.Text = "ClipHistory";
            notifyIcon.BalloonTipIcon = ToolTipIcon.Info; //Shows the info icon so the user doesn't think there is an error.

            // Notify icon right click menu
            var contextMenu1 = new ContextMenu();
            var menuItem1 = new MenuItem();
            contextMenu1.MenuItems.AddRange(new MenuItem[] { menuItem1 });

            // Initialize menuItem1
            menuItem1.Index = 0;
            menuItem1.Text = "E&xit";
            menuItem1.Click += menuItem1_Click;

            notifyIcon.ContextMenu = contextMenu1;

            // Event handlers for when the form loses focus (makes it so it can be summoned with the keyboard hook again)
            Leave += Form1_LostFocus;
            LostFocus += Form1_LostFocus;
        }
        
        // Updates the UI when there is a clipboard change or the user moves up/down the history
        private void UpdateUI() {

            try {
                labelMain.Text = history[historyIndex];
            } catch (ArgumentOutOfRangeException) {
                labelMain.Text = "";
            }
            

            if (historyIndex > 0) {
                labelNext.Text = history[historyIndex - 1];
            } else {
                labelNext.Text = "";
            }

            if (historyIndex + 1 < history.Count) {
                labelPrevious.Text = history[historyIndex + 1];
            } else {
                labelPrevious.Text = "";
            }
            
        }

        // Handles movement up/down, selection of history, and minimizing the program
        private void Form1_Keydown(object sender, KeyEventArgs e) {

            if (e.KeyCode == Keys.Down && historyIndex < history.Count - 1) { // History up
                ++historyIndex;
            } else if (e.KeyCode == Keys.Up && historyIndex > 0) { // History down
                --historyIndex;
            } else if (e.KeyCode == Keys.Enter) { // Select history
                Clipboard.SetText(history[historyIndex]);
                historyIndex = 0;
                Hide();
                WindowState = FormWindowState.Minimized;
            } else if (e.KeyCode == Keys.Back && history.Count > 0){ // Remove history
                history.RemoveAt(historyIndex);
                if (historyIndex > 0) {
                    historyIndex -= 1;
                }

            } else if (e.KeyCode == Keys.Escape) { // Minimize window
                historyIndex = 0;
                Hide();
                WindowState = FormWindowState.Minimized;
                return;
            }

            UpdateUI();
        }

        // Subscribe to clipboard change
        const int WM_CLIPBOARDUPDATE = 0x031D;
        protected override void WndProc(ref Message m) {
            switch (m.Msg) {
                case WM_CLIPBOARDUPDATE:
                    IDataObject iData = Clipboard.GetDataObject();

                    // Grab data from the clipboard if the data is in text format
                    if (iData.GetDataPresent(DataFormats.Text)) {
                        string data = (string)iData.GetData(DataFormats.Text);
                        history.Insert(0, data);
                        if (history.Count >= maxHistory) {
                            history.RemoveAt(history.Count - 1);
                        }

                        UpdateUI();
                    }
                    break;

                default:
                    base.WndProc(ref m);
                    break;
            }
        }

        // Hides the form in the task-bar
        private void Form1_ResizeEnd(object sender, EventArgs e) {
            if (WindowState == FormWindowState.Minimized) {
                notifyIcon.Visible = true;
                ShowInTaskbar = false;
                AddClipboardFormatListener(this.Handle);
            }
        }

        private void Form1_LostFocus(object sender, EventArgs e) {
            WindowState = FormWindowState.Minimized;
        }

// MARK: Keyboard Hooks
        // Set system-wide hook.
        private void SetHook() {
            _hookHandle = SetWindowsHookEx(WH_KEYBOARD_LL, KbHookProc, (IntPtr)0, 0);
        }

        private int KbHookProc(int nCode, IntPtr wParam, IntPtr lParam) {
            if (nCode >= 0) {
                var hookStruct = (KbLLHookStruct)Marshal.PtrToStructure(lParam, typeof(KbLLHookStruct));
                
                bool ctrlDown = GetKeyState(VK_LCONTROL) < 0 || GetKeyState(VK_RCONTROL) < 0;
                bool shiftDown = GetKeyState(VK_SHIFT) < 0;

                // Focuses the window if CTRL + SHIFT + H is pressed
                if (shiftDown & ctrlDown & hookStruct.vkCode == 0x48) {                    
                    BringToFront();
                    Activate();
                    WindowState = FormWindowState.Normal;
                    Show();
                    
                }
            }

            // Pass to other keyboard handlers. Makes the Ctrl+V pass through.
            return CallNextHookEx(_hookHandle, nCode, wParam, lParam);
        }

        // Removes keyboard hook and clipboard listener when application is closed
        private void Form1_FormClosed(object sender, FormClosedEventArgs e) {
            UnhookWindowsHookEx(_hookHandle);
            RemoveClipboardFormatListener(this.Handle);
        }

        //Declare the wrapper managed MouseHookStruct class.
        [StructLayout(LayoutKind.Sequential)]
        public class KbLLHookStruct {
            public int vkCode;
            public int scanCode;
            public int flags;
            public int time;
            public int dwExtraInfo;
        }

// MARK: NotifyIcon Stuff
        private void notifyIcon_Click(object sender, EventArgs e) {
            this.BringToFront();
            this.Activate();
            this.WindowState = FormWindowState.Normal;
            this.Show();
        }

        private void menuItem1_Click(object sender, EventArgs e) {
            this.Close();
        }
    }
}
