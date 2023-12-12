using System.Windows.Media.Imaging;

namespace RAFFLE.Schema
{
    public static class SettingSchema
    {
        public static int Id { get; set; }
        public static string ImgPath { get; set; }
        public static string Location { get; set; }
        public static string Description { get; set; }
        public static int Profit { get; set; }
        public static string CreatedAt { get; set; }
    }
}
