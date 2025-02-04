using System;
using System.Windows;
using System.Windows.Input;
using System.Runtime.InteropServices;
using System.Windows.Interop;

namespace Theme.Core.Controls
{
    public class WindowControl : Window
    {
        #region Constructor

        public WindowControl()
        {
            this.CommandBindings.Add(new CommandBinding(SystemCommands.CloseWindowCommand, this.OnCloseWindow));
            this.CommandBindings.Add(new CommandBinding(SystemCommands.MaximizeWindowCommand, this.OnMaximizeWindow, this.OnCanResizeWindow));
            this.CommandBindings.Add(new CommandBinding(SystemCommands.MinimizeWindowCommand, this.OnMinimizeWindow, this.OnCanMinimizeWindow));
            this.CommandBindings.Add(new CommandBinding(SystemCommands.RestoreWindowCommand, this.OnRestoreWindow, this.OnCanResizeWindow));

            this.SourceInitialized += Window_SourceInitialized;
        }

        #endregion

        #region Static

        static WindowControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WindowControl),
                new FrameworkPropertyMetadata(typeof(WindowControl)));

            #region InitializeDependency

            VisibilityMaximizedProperty = DependencyProperty.Register("VisibilityMaximized",
                typeof(bool), typeof(WindowControl),
                new PropertyMetadata(false, VisibilityMaximized_DependencyChange));

            VisibilityHideProperty = DependencyProperty.Register("VisibilityHide",
                typeof(bool), typeof(WindowControl),
                new PropertyMetadata(false, VisibilityHide_DependencyChange));

            ShowToolBarProperty = DependencyProperty.Register("ShowToolBar", 
                typeof(bool), typeof(WindowControl), new PropertyMetadata(true));

            #endregion
        }

        #endregion

        #region Commands

        private void OnCanResizeWindow(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = this.ResizeMode == ResizeMode.CanResize || this.ResizeMode == ResizeMode.CanResizeWithGrip;
        }

        private void OnCanMinimizeWindow(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = this.ResizeMode != ResizeMode.NoResize;
        }

        private void OnCloseWindow(object target, ExecutedRoutedEventArgs e)
        {
            SystemCommands.CloseWindow(this);
        }

        private void OnMaximizeWindow(object target, ExecutedRoutedEventArgs e)
        {
            SystemCommands.MaximizeWindow(this);
        }

        private void OnMinimizeWindow(object target, ExecutedRoutedEventArgs e)
        {
            SystemCommands.MinimizeWindow(this);
        }

        private void OnRestoreWindow(object target, ExecutedRoutedEventArgs e)
        {
            SystemCommands.RestoreWindow(this);
        }

        #endregion

        #region DependencyPropertys

        #region Propertys

        public bool VisibilityMaximized
        {
            get { return (bool)GetValue(VisibilityMaximizedProperty); }
            set { SetValue(VisibilityMaximizedProperty, value); }
        }

        public bool VisibilityHide
        {
            get { return (bool)GetValue(VisibilityHideProperty); }
            set { SetValue(VisibilityHideProperty, value); }
        }

        public bool ShowToolBar
        {
            get { return (bool)GetValue(ShowToolBarProperty); }
            set { SetValue(ShowToolBarProperty, value); }
        }

        #endregion

        #region Fields

        public static readonly DependencyProperty VisibilityMaximizedProperty;
        public static readonly DependencyProperty VisibilityHideProperty;
        public static readonly DependencyProperty ShowToolBarProperty;
        public static readonly DependencyProperty WindowResultProperty;

        #endregion

        #region Methods

        private static void VisibilityMaximized_DependencyChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as WindowControl;
            var value = (bool)e.NewValue;
        }


        private static void VisibilityHide_DependencyChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as WindowControl;
            var value = (bool)e.NewValue;
        }

        private static void WindowResultChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var window = d as Window;

            if (window != null && (bool?)e.NewValue == true)
                window.Close();
        }

        public static void SetWindowResult(Window target, bool? value)
        {
            target.SetValue(WindowResultProperty, value);
        }

        #endregion

        #endregion

        #region Windows Localization 

        #region Private

        void Window_SourceInitialized(object sender, EventArgs e)
        {
            IntPtr mWindowHandle = (new WindowInteropHelper(this)).Handle;
            HwndSource.FromHwnd(mWindowHandle).AddHook(new HwndSourceHook(WindowProc));
        }

        private static System.IntPtr WindowProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            switch (msg)
            {
                case 0x0024:
                    WmGetMinMaxInfo(hwnd, lParam);
                    break;
            }

            return IntPtr.Zero;
        }

        private static void WmGetMinMaxInfo(System.IntPtr hwnd, System.IntPtr lParam)
        {
            POINT lMousePosition;
            GetCursorPos(out lMousePosition);

            IntPtr lCurrentScreen = MonitorFromPoint(lMousePosition, MonitorOptions.MONITOR_DEFAULTTONEAREST);


            MINMAXINFO lMmi = (MINMAXINFO)Marshal.PtrToStructure(lParam, typeof(MINMAXINFO));

            MONITORINFO lCurrentScreenInfo = new MONITORINFO();
            if (GetMonitorInfo(lCurrentScreen, lCurrentScreenInfo) == false)
                return;

            lMmi.ptMaxPosition.X = lCurrentScreenInfo.rcWork.Left - lCurrentScreenInfo.rcMonitor.Left;
            lMmi.ptMaxPosition.Y = lCurrentScreenInfo.rcWork.Top - lCurrentScreenInfo.rcMonitor.Top;
            lMmi.ptMaxSize.X = lCurrentScreenInfo.rcWork.Right - lCurrentScreenInfo.rcWork.Left;
            lMmi.ptMaxSize.Y = lCurrentScreenInfo.rcWork.Bottom - lCurrentScreenInfo.rcWork.Top;

            Marshal.StructureToPtr(lMmi, lParam, true);
        }

        private void SwitchWindowState()
        {
            switch (WindowState)
            {
                case WindowState.Normal:
                    {
                        WindowState = WindowState.Maximized;
                        break;
                    }

                case WindowState.Maximized:
                    {
                        WindowState = WindowState.Normal;
                        break;
                    }
            }
        }

        #endregion

        #region DLLImport

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetCursorPos(out POINT lpPoint);

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr MonitorFromPoint(POINT pt, MonitorOptions dwFlags);

        [DllImport("user32.dll")]
        static extern bool GetMonitorInfo(IntPtr hMonitor, MONITORINFO lpmi);

        #endregion

        #region Public Struct

        enum MonitorOptions : uint
        {
            MONITOR_DEFAULTTONULL = 0x00000000,
            MONITOR_DEFAULTTOPRIMARY = 0x00000001,
            MONITOR_DEFAULTTONEAREST = 0x00000002
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int X;
            public int Y;

            public POINT(int x, int y)
            {
                this.X = x;
                this.Y = y;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MINMAXINFO
        {
            public POINT ptReserved;
            public POINT ptMaxSize;
            public POINT ptMaxPosition;
            public POINT ptMinTrackSize;
            public POINT ptMaxTrackSize;
        };


        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public class MONITORINFO
        {
            public int cbSize = Marshal.SizeOf(typeof(MONITORINFO));
            public RECT rcMonitor = new RECT();
            public RECT rcWork = new RECT();
            public int dwFlags = 0;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left, Top, Right, Bottom;

            public RECT(int left, int top, int right, int bottom)
            {
                this.Left = left;
                this.Top = top;
                this.Right = right;
                this.Bottom = bottom;
            }
        }

        #endregion

        #endregion
    }
}
