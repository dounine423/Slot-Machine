using RAFFLE.Schema;
using System;
using System.IO;
using System.Windows;
using Wpf.Ui.Controls;
using System.Printing;
using RAFFLE.Utils;
using RAFFLE.Manager;
using System.Globalization;
using System.Xml.Linq;
using System.Reflection;
using System.Windows.Media.Imaging;
using System.Security.Policy;
using System.Windows.Input;
using System.Windows.Controls;
using System.Drawing.Printing;
using System.Drawing;
using System.Windows.Media;

namespace RAFFLE.UI
{

    public partial class MainWindow : UiWindow
    {
        private BitmapImage img;
        public MainWindow()
        {
            InitializeComponent();

            string executablePath = Assembly.GetExecutingAssembly().Location;
            string curDir = Path.GetDirectoryName(executablePath);
            var uri = new Uri(curDir + "\\Invalid.png");
            img = new BitmapImage(uri);
            Img.Source = img;
        }

        public void UpdateState()
        {
            if (File.Exists(SettingSchema.ImgPath))
            {
                var uri = new Uri(SettingSchema.ImgPath);
                img = new BitmapImage(uri);
                Img.Source = img;
            }
            else {
                MsgHelper.ShowMessage(MsgType.Other, "The image file is damaged.");
                return;
            }
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            if (DBMgr.CheckSetting())
            {
                MsgHelper.ShowMessage(MsgType.FileCheck, "exites");
                return;
            }
            else {
                Builder.RaiseEvent(EventRaiseType.Setting);
            }
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            if (SettingSchema.ImgPath == null || SettingSchema.ImgPath == "" ||
                SettingSchema.Location == null || SettingSchema.Location == "" ||
                SettingSchema.Description == null || SettingSchema.Description == "")
            {
                MsgHelper.ShowMessage(MsgType.Other, "Invalid Setting");
                return;
            }
            else if (!File.Exists(SettingSchema.ImgPath)) {
                MsgHelper.ShowMessage(MsgType.Other, "Invalid Setting");
                return;
            }
            else
            {
                Builder.RaiseEvent(EventRaiseType.UserBoard);
            }
        }

        private void btnHistory_Click(object sender, RoutedEventArgs e)
        {
            Builder.RaiseEvent(EventRaiseType.History);
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Builder.RaiseEvent(EventRaiseType.AppExit);
        }

        private void UiWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (MsgHelper.ShowMessage(MsgType.AppExit, ""))
                e.Cancel = true;
        }
    }
}
