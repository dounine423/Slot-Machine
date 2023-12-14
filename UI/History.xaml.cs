using System.Collections.Generic;
using System.Windows;
using Wpf.Ui.Controls;
using RAFFLE.Schema;
using RAFFLE.Manager;
using RAFFLE.Utils;

namespace RAFFLE.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class History : UiWindow
    {
        List<HistoryTableSchema> lstHistory = new List<HistoryTableSchema>();

        public History()
        {
            InitializeComponent();
            Initialize();
        }

        private void UiWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (MsgHelper.ShowMessage(MsgType.AppExit, ""))
                e.Cancel = true;
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            Builder.RaiseEvent(EventRaiseType.HistoryExit);
        }

        private void Initialize()
        {
            lstHistory = DBMgr.LoadHistoryData();
            InitializeHistoryDataGrid();
        }

        private void InitializeHistoryDataGrid()
        {
            dgHistory.ItemsSource = lstHistory;
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            DBMgr.ClearSetting();
            DBMgr.ClearHistoryData();
            dgHistory.ItemsSource = null;
            lstHistory.Clear();
            InitializeHistoryDataGrid();
        }
    }
}
