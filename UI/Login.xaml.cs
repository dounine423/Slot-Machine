﻿
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Wpf.Ui.Controls;
using RAFFLE.Utils;
using RAFFLE.Manager;

namespace RAFFLE.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class Login : UiWindow
    {
        public Login()
        {
            InitializeComponent();
            txtUsername.Focus();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text;
            string pin = DBMgr.ReadPIN(username);
            if (txtPassword.Password != pin)
            {
                Wpf.Ui.Controls.MessageBox messageBox = new Wpf.Ui.Controls.MessageBox();
                messageBox.Title = "Error";

                messageBox.Content = new TextBlock
                {
                    Margin = new Thickness(0, 8, 0, 0),
                    TextWrapping = TextWrapping.WrapWithOverflow,
                    TextAlignment = TextAlignment.Center,
                    FontSize = 20,
                    Text = "Invalid Authorization"
                };

                messageBox.Height = 170;
                messageBox.Topmost = false;
                messageBox.ButtonRightName = "Exit";
                messageBox.ButtonLeftName = "Retry";
                messageBox.ShowTitle = true;
                messageBox.ButtonLeftClick += (_, _) => messageBox.Hide();
                messageBox.ButtonRightClick += (_, _) => Builder.RaiseEvent(EventRaiseType.AppExit);
                messageBox.ShowDialog();
            }
            else
            {
                Builder.RaiseEvent(EventRaiseType.LoginSuccess);
            }
        }

        private void UiWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (MsgHelper.ShowMessage(MsgType.AppExit, ""))
                e.Cancel = true;
        }

        private void txtPassword_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                string username = txtUsername.Text;
                string pin = DBMgr.ReadPIN(username);
                if (txtPassword.Password != pin)
                {
                    Wpf.Ui.Controls.MessageBox messageBox = new Wpf.Ui.Controls.MessageBox();
                    messageBox.Title = "Error";

                    messageBox.Content = new TextBlock
                    {
                        Margin = new Thickness(0, 8, 0, 0),
                        TextWrapping = TextWrapping.WrapWithOverflow,
                        TextAlignment = TextAlignment.Center,
                        FontSize = 20,
                        Text = "Invalid Authorization"
                    };
                    messageBox.Height = 170;
                    messageBox.Topmost = false;
                    messageBox.ButtonRightName = "Exit";
                    messageBox.ButtonLeftName = "Retry";
                    messageBox.ShowTitle = true;
                    messageBox.ButtonLeftClick += (_, _) => messageBox.Hide();
                    messageBox.ButtonRightClick += (_, _) => Builder.RaiseEvent(EventRaiseType.AppExit);
                    messageBox.ShowDialog();
                }
                else
                {
                    Builder.RaiseEvent(EventRaiseType.LoginSuccess);
                }
            }
        }

        private void btnSetting_Click(object sender, RoutedEventArgs e)
        {
            Builder.RaiseEvent(EventRaiseType.AccountSetting);
        }
    }
}
