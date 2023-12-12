using System.Windows.Media.Imaging;

namespace RAFFLE.Schema
{
    public class HistorySchema
    {
        public int Price { get; set; }
        public int Rate { get; set; }
        public int SettingId { get; set; }
        public int WinnerNum { get; set; }
        public int IsWinner { get; set; }
        public string CreatedAt { get; set; }
    }
}
