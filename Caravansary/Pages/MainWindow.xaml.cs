﻿
namespace Caravansary
{
    using Caravansary.Core;
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Interop;

    public partial class MainWindow : Window, IWindow
    {
        


        public double MainWindowPositionX
        {
            get
            {
                return Data.MainWindowSettingsSave.MainWindowPositionX;
            }
            set
            {
                Data.MainWindowSettingsSave.MainWindowPositionX = value;
                Data.SaveWindowSettings();
            }
        }

        public double MainWindowPositionY
        {
            get
            {
                return Data.MainWindowSettingsSave.MainWindowPositionY;
            }
            set
            {
                Data.MainWindowSettingsSave.MainWindowPositionY = value;
                Data.SaveWindowSettings();
            }
        }

        public Window GetAssociatedWindow => window;

        public Action OnMouseWindowEnter;
        public Action OnMouseWindowLeave;

        public MainWindow()
        {

            LoadWindowSettings();



            Loaded += OnWindowLoaded;


        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);


            ModuleController.Instance.StopAllModules();
            KeyboardListener.Instance.UnHookKeyboard();
        }

        protected override void OnMouseEnter(MouseEventArgs e)
        {
            base.OnMouseEnter(e);
            OnMouseWindowEnter?.Invoke();

        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);
            OnMouseWindowLeave?.Invoke();

        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.LeftButton == MouseButtonState.Pressed && e.RightButton == MouseButtonState.Pressed)
                DragMove();
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            base.OnMouseUp(e);
            if (e.LeftButton == MouseButtonState.Released && e.RightButton == MouseButtonState.Released)
                SaveWindowPosition();
        }

        internal void OnWindowLoaded(object sender, RoutedEventArgs e)
        {
        }



        

        private void LoadWindowSettings()
        {
            Application.Current.MainWindow.ShowInTaskbar = ShowInTaskbar;

           WindowStartupLocation = WindowStartupLocation.Manual;
            double posX = MainWindowPositionX;
            double posY = MainWindowPositionY;

            if (posX != 0 || posY != 0)
            {
                WindowHelper.SetWindowPosition(this, posX, posY);
            }
            else
            {
                WindowHelper.ResetWindowPosition(this);
            }
        }

        private void SaveWindowPosition()
        {
            int x = (int)Left;
            int y = (int)Top;
            MainWindowPositionX = x;
            MainWindowPositionY = y;
        }



        public void MakeWindowNonClickThrough()
        {
            //var buttonHwndSource = (HwndSource)HwndSource.FromVisual(btn);
            //var buttonHwnd = buttonHwndSource.Handle;
            var windowHwnd = new WindowInteropHelper(this).Handle;
            WinApi.SetWindowExNotTransparent(windowHwnd);
        }

        public void MakeWindowClickThrough()
        {
            var windowHwnd = new WindowInteropHelper(this).Handle;

            WinApi.SetWindowExTransparent(windowHwnd);
        }
    }



}
