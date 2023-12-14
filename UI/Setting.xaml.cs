using System;
using System.Windows;
using System.IO;
using System.Windows.Media.Imaging;
using Wpf.Ui.Controls;
using RAFFLE.Schema;
using RAFFLE.Manager;
using Microsoft.Win32;
using RAFFLE.Utils;
using System.Reflection;

namespace RAFFLE.UI
{
    public partial class Setting : UiWindow
    {
        private BitmapImage img;
        private string imgPath;
        private string location;
        private string description;
        private int rate;
        public Setting()
        {
            InitializeComponent();
            imgPath = "";
            location = "";
            description = "";
            rate = 0;
            string executablePath = Assembly.GetExecutingAssembly().Location;
            string curDir = Path.GetDirectoryName(executablePath);
            var uri = new Uri(curDir + "\\Invalid.png");
            img = new BitmapImage(uri);
            SetImg.Source = img;
        }

        private void btnFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files(*.BMP; *.JPG; *JPEG; *.GIF; *PNG; *.png)| *.BMP; *.JPG; *.JPEG; *.GIF; *.PNG; *.png | All files(*.*) | *.*";
            Nullable<bool> result = openFileDialog.ShowDialog();
            if (result == true)
            {
                string sourceFilePath = openFileDialog.FileName;
                var uri = new Uri(sourceFilePath);
                img = new BitmapImage(uri);
                SetImg.Source = img;

                string exeDirectory = AppDomain.CurrentDomain.BaseDirectory;
                string picDirectory = Path.Combine(exeDirectory, "Pic");
                if (!Directory.Exists(picDirectory))
                {
                    Directory.CreateDirectory(picDirectory);
                }
                string destinationFilePath = Path.Combine(picDirectory, Path.GetFileName(sourceFilePath));
                imgPath = destinationFilePath;

                if (!File.Exists(destinationFilePath))
                {
                    File.Copy(sourceFilePath, destinationFilePath);
                }
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if ( imgPath == "")
            {
                MsgHelper.ShowMessage(MsgType.Other, "Please select an Image.");
                return;
            }
            else if( rate < 2)
            {
                MsgHelper.ShowMessage(MsgType.Other, "Please set the rate as higher more than 2.");
                return;
            }
            else if ( location == "")
            {
                MsgHelper.ShowMessage(MsgType.Other, "Please enter a your Location.");
                return;
            }
            else if (description == "")
            {
                MsgHelper.ShowMessage(MsgType.Other, "Please enter a Description.");
                return;
            }
            else {
                SettingSchema.ImgPath = imgPath;
                SettingSchema.Location = location;
                SettingSchema.Description = description;
                SettingSchema.Rate = rate;
                SettingSchema.Profit = 0;
                SettingSchema.Total = 0;
                SettingSchema.CreatedAt = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
                DBMgr.InsertSetting();
                Builder.RaiseEvent(EventRaiseType.SettingExit);
                Builder.uiMainWindow.UpdateState();
            }   
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Builder.RaiseEvent(EventRaiseType.SettingExit);
        }

        private void UiWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (MsgHelper.ShowMessage(MsgType.AppExit, ""))
                e.Cancel = true;
        }

        private void rate_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            string rateStr = rateInput.Text;
            try
            {
                int value = int.Parse(rateInput.Text);
                rate = value;
            }
            catch(Exception error)
            {
                Console.WriteLine(error.Message);
                rateInput.Text = "";
            }
        }

        private void description_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            description = descriptionInput.Text;
        }

        private void location_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            location = locationInput.Text;
        }
    }
}