using System.Windows.Media.Imaging;

namespace RAFFLE.Schema
{
    public class HistoryTableSchema
    {
        public int No { get; set; }
        public int Winner { get; set; }
        public string IsWinner { get; set; }
        public int Price { get; set; }
        public int Rate { get; set; }
        public int WinnerPrice { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public string CreatedAt { get; set; }
    }
}
