using System;
using System.Windows.Input;
using System.Windows;
using System.Windows.Threading;
using Wpf.Ui.Controls;
using RAFFLE.Schema;
using RAFFLE.Manager;
using RAFFLE.Utils;
using System.Windows.Media.Imaging;

namespace RAFFLE.UI
{
    public partial class UserBoard : UiWindow
    {
        private BitmapImage img;
        private DispatcherTimer timer;
        private int rate;
        public UserBoard()
        {
            InitializeComponent();

            lblCurTime.Text = "Current Time: " + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
            lblLocation.Text = "Location: " + SettingSchema.Location;
            lblDescription.Text = "Description: " + SettingSchema.Description;

            var uri = new Uri(SettingSchema.ImgPath);
            img = new BitmapImage(uri);
            Img.Source = img;

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += TimerElapsed;
            timer.Start();

            DBMgr.ReadStatistic();

            txtRate.MaxLength = 10;

            txtRate.TextChanged += txtRate_TextChanged;

            txtRate.LostFocus += txtRate_LostFocus;
        }
        
        private void TimerElapsed(object sender, EventArgs e)
        {
            lblCurTime.Text = "Current Time: " + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
        }

        private void txtRate_TextChanged(object sender, EventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(txtRate.Text, "[^0-9]"))
            {
                MsgHelper.ShowMessage(MsgType.Other, "Please enter only numbers.");
                txtRate.Clear();
            }

            if (txtRate.Text.StartsWith('0'))
            {
                MsgHelper.ShowMessage(MsgType.Other, "Please start with a non-zero number.");
                txtRate.Clear();
            }
        }

        private void txtRate_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtRate.Text != null && txtRate.Text != "")
                rate = Convert.ToInt32(txtRate.Text);
        }

        private void txtImpluse_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = true;
            char typedChar = e.Text[0];

            if (typedChar == '9')
            {
                txtImpluse.Text += "+";

                if (txtRate.Text == null || txtRate.Text == "")
                {
                    MsgHelper.ShowMessage(MsgType.Other, "Please enter a Rate.");
                    txtImpluse.Clear();
                }
                else if (rate < 2)
                {
                    MsgHelper.ShowMessage(MsgType.Other, "Rate must be greater than 1.");
                    txtImpluse.Clear();
                }
                else
                {
                    HistorySchema history = new HistorySchema();

                    history.Price = Convert.ToInt32(txtPrice.Text);
                    history.Rate = rate;

                    Random rnd = new Random();
                    history.WinnerNum = rnd.Next(1, 500);

                    if (winState1() && winState2() && winState3(history.WinnerNum))
                    {
                        DBMgr.UpdateSetting("success", Convert.ToInt32(txtPrice.Text));
                        DBMgr.ReadSetting();

                        history.IsWinner = 1;

                        if (rate >= 2 && rate < 10)
                            DBMgr.UpdateStatistic("one");
                        else if (rate >= 10 && rate < 100)
                            DBMgr.UpdateStatistic("ten");
                        else if (rate >= 100 && rate < 1000)
                            DBMgr.UpdateStatistic("hundred");
                        else if (rate >= 1000 && rate < 10000)
                            DBMgr.UpdateStatistic("thousand");
                        else DBMgr.UpdateStatistic("million");

                        DBMgr.ReadStatistic();
                    }
                    else
                    {
                        DBMgr.UpdateSetting("fail", Convert.ToInt32(txtPrice.Text));
                        DBMgr.ReadSetting();

                        history.IsWinner = 0;

                        DBMgr.UpdateStatistic("none");
                        DBMgr.ReadStatistic();
                    }

                    history.CreatedAt = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");

                    DBMgr.InsertHistory(history);

                    string text;

                    if (history.IsWinner == 1)
                    {
                        MsgHelper.ShowMessage(MsgType.Notification, "You are Winner!\n" + "Winner Number: " + history.WinnerNum + "\n" + "Winner Price: " + Convert.ToInt32(txtPrice.Text) * rate + " $");

                        text = "Winner\n" + "$ " + history.Price + "\n" + "Location: " + SettingSchema.Location + "\n" + "Description: " + SettingSchema.Description + "\n" + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString();
                    }
                    else
                    {
                        MsgHelper.ShowMessage(MsgType.Notification, "Sorry.\n" + "Number: " + history.WinnerNum);

                        text = "Sorry\n" + "Not A\n" + "Winner\n" + "Location: " + SettingSchema.Location + "\n" + "Description: " + SettingSchema.Description + "\n" + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString();
                    }

                    ThreadMgr.PrintText(text, 14);

                    txtImpluse.Clear();
                }
            }
            else {
                txtImpluse.Clear();
            }
        }

        private bool winState1(){
            int val1 = StatisticSchema.Total;
            float val2 = (float)val1;

            if (rate >= 2 && rate < 10)
            {
                int val3 = (int)((val2 / 10f) * 7);
                if (StatisticSchema.One < val3)
                    return true;
                else return false;
            }
            else if (rate >= 10 && rate < 100)
            {

                int val3 = (int)((val2 / 100f) * 7);
                if (StatisticSchema.Ten < val3)
                    return true;
                else return false;
            }
            else if (rate >= 100 && rate < 1000)
            {
                int val3 = (int)((val2 / 1000f) * 7);
                if (StatisticSchema.Hundred < val3)
                    return true;
                else return false;
            }
            else if (rate >= 1000 && rate < 10000)
            {
                int val3 = (int)((val2 / 10000f) * 7);
                if (StatisticSchema.Thousand < val3)
                    return true;
                else return false;
            }
            else
            {
                int val3 = (int)((val2 / 100000f) * 7);
                if (StatisticSchema.Million < val3)
                    return true;
                else return false;
            }
        }

        private bool winState2()
        {
            if (SettingSchema.Profit > Convert.ToInt32(txtPrice.Text) * rate)
                return true;
            else return false;
        }

        private bool winState3(int rnd)
        {
            if (rnd%2 == 1)
                return true;
            else return false;
        }

        private void UiWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Builder.RaiseEvent(EventRaiseType.UserBoardExit);
        }
    }
}