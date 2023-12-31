﻿using System.Windows;
using Wpf.Ui.Controls;
using RAFFLE.Utils;
using RAFFLE.Manager;

namespace RAFFLE.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class AccountSetting : UiWindow
    {
        public AccountSetting()
        {
            InitializeComponent();
        }

        private void UiWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (MsgHelper.ShowMessage(MsgType.AppExit, ""))
                e.Cancel = true;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text;
            int bUsername = DBMgr.validateUsername(username);
            if (bUsername < 0)
            {
                MsgHelper.ShowMessage(MsgType.Other, "Invalid Username");
                return;
            }

            string pin = DBMgr.ReadPIN(username);
            if (pin != txtCurPIN.Password) 
            {
                MsgHelper.ShowMessage(MsgType.Other, "Invalid Password");
                return;
            }

            if ((txtNewPIN.Password != txtNewPINConfirm.Password) || (txtNewPIN.Password == "" || txtNewPINConfirm.Password == ""))
            {
                MsgHelper.ShowMessage(MsgType.Other, "Password don't match");
                return;
            }
            string new_username = txtNewUsername.Text == "" ? username : txtNewUsername.Text;
            string new_pin = txtNewPIN.Password;

            DBMgr.UpdatePIN(username, new_pin, new_username);

            Builder.RaiseEvent(EventRaiseType.AccountSetting_Closed);
        }

        private void btnSetting_Click(object sender, RoutedEventArgs e)
        {
            Builder.RaiseEvent(EventRaiseType.AccountSetting_Closed);
        }
    }
}
