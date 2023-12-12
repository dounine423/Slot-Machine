using RAFFLE.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAFFLE
{
    public static class Builder
    {
        public static Login uiLogin;
        public static AccountSetting uiAccountSetting;
        public static MainWindow uiMainWindow;
        public static Setting uiSetting;
        public static UserBoard uiUserBoard;
        public static History uiHistory;

        public static void RaiseEvent(EventRaiseType type)
        {
            switch (type)
            {
                case EventRaiseType.Init:
                    break;

                case EventRaiseType.Login:
                    uiLogin = new Login();
                    uiLogin.Visibility = System.Windows.Visibility.Visible;
                    break;

                case EventRaiseType.LoginSuccess:
                    uiLogin.Visibility = System.Windows.Visibility.Hidden;
                    uiLogin = null;
                    uiMainWindow = new MainWindow();
                    uiMainWindow.Show();
                    uiMainWindow.Visibility = System.Windows.Visibility.Visible;

                    break;

                case EventRaiseType.AccountSetting:
                    uiLogin.Visibility = System.Windows.Visibility.Hidden;
                    if (uiLogin != null)
                    {
                        uiLogin = null;
                    }
                    if (uiAccountSetting == null)
                    {
                        uiAccountSetting = new AccountSetting();
                    }
                    uiAccountSetting.Visibility = System.Windows.Visibility.Visible;
                    break;

                case EventRaiseType.AccountSetting_Closed:
                    uiAccountSetting.Visibility = System.Windows.Visibility.Hidden;
                    if (uiAccountSetting != null)
                    {
                        uiAccountSetting = null;
                    }
                    if (uiLogin == null)
                    {
                        uiLogin = new Login();
                    }
                    uiLogin.Visibility = System.Windows.Visibility.Visible;
                    break;

                case EventRaiseType.MainWindow:
                    uiMainWindow = new MainWindow();
                    uiMainWindow.Show();
                    uiMainWindow.Visibility = System.Windows.Visibility.Visible;
                    break;

                case EventRaiseType.Setting:
                    uiSetting = new Setting();
                    uiSetting.Show();
                    uiSetting.Visibility = System.Windows.Visibility.Visible;
                    break;

                case EventRaiseType.UserBoard:
                    uiUserBoard = new UserBoard();
                    uiUserBoard.Show();
                    uiUserBoard.WindowState = System.Windows.WindowState.Maximized;
                    uiUserBoard.Visibility = System.Windows.Visibility.Visible;
                    break;

                case EventRaiseType.History:
                    uiHistory = new History();
                    uiHistory.Show();
                    uiHistory.Visibility = System.Windows.Visibility.Visible;
                    break;

                case EventRaiseType.SettingExit:
                    uiSetting.Visibility = System.Windows.Visibility.Hidden;
                    uiSetting = null;
                    break;

                case EventRaiseType.UserBoardExit:
                    uiUserBoard.Visibility = System.Windows.Visibility.Hidden;
                    uiUserBoard = null;
                    break;

                case EventRaiseType.HistoryExit:
                    uiHistory.Visibility = System.Windows.Visibility.Hidden;
                    uiHistory = null;
                    break;

                case EventRaiseType.AppExit:
                    Process.GetCurrentProcess().Kill();
                    break;
            }
        }
    }
    public enum EventRaiseType
    {
        Init = 0x0001,
        Login = 0x0002,
        AccountSetting = 0x0010,
        AccountSetting_Closed = 0x0011,
        LoginSuccess = 0x0003,
        MainWindow = 0x0101,
        Setting = 0x0102,
        UserBoard = 0x0111,
        History = 0x0103,
        SettingExit = 0x0401,
        UserBoardExit = 0x0402,
        HistoryExit = 0x0403,
        AppExit = 0x0301
    }
}
