using System;
using System.IO;
using System.Windows.Controls;
using System.Windows;
using RAFFLE.Schema;
using System.Windows.Media.Imaging;
using System.Globalization;
using RAFFLE.Manager;

namespace RAFFLE.Utils
{
    public static class MsgHelper
    {
        public static bool ShowMessage(MsgType msgType, string msgTxt)
        {
            bool bRes = false;
            Wpf.Ui.Controls.MessageBox messageBox = new Wpf.Ui.Controls.MessageBox();

            switch (msgType)
            {
                case MsgType.FileCheck:
                    messageBox.Title = "Information";
                    messageBox.Content = new TextBlock
                    {
                        Margin = new Thickness(0, 8, 0, 0),
                        TextWrapping = TextWrapping.WrapWithOverflow,
                        TextAlignment = TextAlignment.Center,
                        FontSize = 18,
                        Text = "Do you want to load saved setting data?"
                    };
                    messageBox.Height = 200;
                    messageBox.Topmost = false;
                    messageBox.ButtonRightName = "Yes";
                    messageBox.ButtonLeftName = "No";
                    messageBox.ShowTitle = true;
                    messageBox.ButtonLeftClick += (_, _) =>
                    {
                        messageBox.Hide();
                        bRes = false;
                        Builder.RaiseEvent(EventRaiseType.Setting);
                    };
                    messageBox.ButtonRightClick += (_, _) => {
                        messageBox.Hide();
                        bRes = false;
                        DBMgr.ReadSetting();
                        Builder.uiMainWindow.UpdateState();  
                    };
                    break;

                case MsgType.Other:
                    messageBox.Title = "Warnning";
                    messageBox.Content = new TextBlock
                    {
                        Margin = new Thickness(0, 8, 0, 0),
                        TextWrapping = TextWrapping.WrapWithOverflow,
                        TextAlignment = TextAlignment.Center,
                        FontSize = 14,
                        Text = msgTxt,
                        Width = 400,
                        Height = 200
                    };
                    messageBox.Height = 200;
                    messageBox.Topmost = false;
                    messageBox.ButtonLeftName = "Retry";
                    messageBox.ButtonRightName = "Close";
                    messageBox.ShowTitle = true;
                    messageBox.ButtonLeftClick += (_, _) => { messageBox.Hide(); bRes = true; };
                    messageBox.ButtonRightClick += (_, _) => { messageBox.Hide(); bRes = true; };

                    break;

                case MsgType.AppExit:
                    messageBox.Title = "Warnig";
                    messageBox.Content = new TextBlock
                    {
                        Margin = new Thickness(0, 8, 0, 0),
                        TextWrapping = TextWrapping.WrapWithOverflow,
                        TextAlignment = TextAlignment.Center,
                        FontSize = 18,
                        Text = "Are you sure exit?"
                    };
                    messageBox.Height = 200;
                    messageBox.Topmost = false;
                    messageBox.ButtonRightName = "Yes";
                    messageBox.ButtonLeftName = "No";
                    messageBox.ShowTitle = true;
                    messageBox.ButtonLeftClick += (_, _) => { messageBox.Hide(); bRes = true; };
                    messageBox.ButtonRightClick += (_, _) =>
                    {

                        Builder.RaiseEvent(EventRaiseType.AppExit);
                    };
                    break;

                case MsgType.Notification:
                    messageBox.Title = "Information";
                    messageBox.Content = new TextBlock
                    {
                        Margin = new Thickness(0, 8, 0, 0),
                        TextWrapping = TextWrapping.WrapWithOverflow,
                        TextAlignment = TextAlignment.Center,
                        FontSize = 14,
                        Text = msgTxt,
                        Width = 400,
                        Height = 200
                    };
                    messageBox.Height = 200;
                    messageBox.Topmost = false;
                    messageBox.ButtonLeftName = "Retry";
                    messageBox.ButtonRightName = "Close";
                    messageBox.ShowTitle = true;
                    messageBox.ButtonLeftClick += (_, _) => { messageBox.Hide(); bRes = true; };
                    messageBox.ButtonRightClick += (_, _) => { messageBox.Hide(); bRes = true; };

                    break;

                default:
                    break;
            }
            messageBox.ShowDialog();

            return bRes;
        }


    }
}
