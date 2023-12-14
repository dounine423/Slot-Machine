using RAFFLE.Schema;
using RAFFLE.UI;
using RAFFLE.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media.Imaging;

namespace RAFFLE.Manager
{
    public static class DBMgr
    {
        public static SQLiteConnection sqliteConn = new SQLiteConnection("Data Source=slot_machine.db;Version=3;New=True;Compress=True");
		public static bool LogInOutDB(bool bLogInOut)
        {
            bool bResult = false;
            if (bLogInOut)
            {
                try
                {
                    sqliteConn.Open();
                    bResult = true;
                }
                catch (Exception ex)
                {
                    bResult = false;
                    MessageBox.Show(ex.Message);
                }
                return bResult;
            }
            else
            {
                try
                {
                    sqliteConn.Close();
                    bResult = true;
                }
                catch (Exception)
                {
                    bResult = false;
                }
                return bResult;
            }
        }

        public static string ReadPIN(string username)
        {
            string pin = "";
            SQLiteCommand sqlite_cmd;
            SQLiteDataReader sqlite_reader;
            sqlite_cmd = sqliteConn.CreateCommand();
            sqlite_cmd.CommandText = String.Format("select password from tbl_user where username = '{0}'", username);
            sqlite_reader = sqlite_cmd.ExecuteReader();
            while (sqlite_reader.Read())
            {
                pin = sqlite_reader.GetString(0);
            }
            return pin;
        }

        public static void UpdatePIN(string username, string new_pin, string new_username)
        {
            try
            {
                SQLiteCommand sqlite_cmd;
                sqlite_cmd = sqliteConn.CreateCommand();
                sqlite_cmd.CommandText = String.Format("update tbl_user set password = '{0}', username = '{1}' where username = '{2}'", new_pin, new_username, username);
                sqlite_cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                MsgHelper.ShowMessage(MsgType.Other, "Faild update username and password");
            }
        }

        public static int validateUsername(string username)
        {
            int result = 0;
            SQLiteCommand sqlite_cmd;
            SQLiteDataReader sqlite_reader;
            sqlite_cmd = sqliteConn.CreateCommand();
            sqlite_cmd.CommandText = String.Format("select count(username) from tbl_user where username = '{0}'", username);
            sqlite_reader = sqlite_cmd.ExecuteReader();
            while (sqlite_reader.Read())
            {
                result = sqlite_reader.GetInt16(0);
            }
            if (result == 0)
            {
                result = -1;
            }
            return result;
        }
        public static void InsertSetting()
        {
            SQLiteCommand cmd = new SQLiteCommand();
            cmd = sqliteConn.CreateCommand();
            string query = String.Format("Insert into tbl_setting (imgPath, location, description, rate, total, profit, createdAt) values ('{0}', '{1}', '{2}', {3}, {4}, {5},'{6}');", SettingSchema.ImgPath, SettingSchema.Location, SettingSchema.Description, SettingSchema.Rate, SettingSchema.Total, SettingSchema.Profit, SettingSchema.CreatedAt);
            cmd.CommandText = query;
            cmd.ExecuteNonQuery();
        }

        public static Boolean CheckSetting()
        {
            SQLiteCommand cmd = new SQLiteCommand();
            cmd = sqliteConn.CreateCommand();
            string query = "Select count(imgPath) from  tbl_setting";
            cmd.CommandText = query;
            cmd.CommandType = CommandType.Text;
            int rowCount = 0;
            rowCount = Convert.ToInt32(cmd.ExecuteScalar());
            if (rowCount == 0)
                return false;
            else
                return true;
        }

        public static void ReadSetting()
        {
            SQLiteCommand cmd = new SQLiteCommand();
            cmd = sqliteConn.CreateCommand();
            string query = "Select * FROM tbl_setting ORDER BY id DESC LIMIT 1";
            cmd.CommandText = query;
            SQLiteDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                SettingSchema.Id = reader.GetInt32(0);
                SettingSchema.ImgPath = reader.GetString(1);
                SettingSchema.Location = reader.GetString(2);
                SettingSchema.Description = reader.GetString(3);
                SettingSchema.Profit = reader.GetInt32(4);
            }
        }

        public static void UpdateSetting(String state, int price)
        {
            SQLiteCommand sqlite_cmd;
            sqlite_cmd = sqliteConn.CreateCommand();

            if (state == "success")
                sqlite_cmd.CommandText = string.Format("UPDATE tbl_setting SET profit = profit - {0} WHERE id = '{1}'", price, SettingSchema.Id);
            if (state == "fail")
                sqlite_cmd.CommandText = string.Format("UPDATE tbl_setting SET profit = profit + {0} WHERE id = '{1}'", price, SettingSchema.Id);

            sqlite_cmd.ExecuteNonQuery();
        }

        public static void ClearSetting()
        {
            SQLiteCommand cmd = new SQLiteCommand();
            cmd = sqliteConn.CreateCommand();
            string query = "DELETE FROM tbl_setting";
            cmd.CommandText = query;
            cmd.ExecuteNonQuery();
        }

        public static void ReadStatistic()
        {
            SQLiteCommand cmd = new SQLiteCommand();
            cmd = sqliteConn.CreateCommand();
            string query = "Select * FROM tbl_statistic";
            cmd.CommandText = query;
            SQLiteDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                StatisticSchema.Total = reader.GetInt32(0);
                StatisticSchema.One = reader.GetInt32(1);
                StatisticSchema.Ten = reader.GetInt32(2);
                StatisticSchema.Hundred = reader.GetInt32(3);
                StatisticSchema.Thousand = reader.GetInt32(4);
                StatisticSchema.Million = reader.GetInt32(5);
            }
        }

        public static void UpdateStatistic(String state)
        {
            SQLiteCommand sqlite_cmd;
            sqlite_cmd = sqliteConn.CreateCommand();
            if(state == "none")
                sqlite_cmd.CommandText = String.Format("update tbl_statistic set total = total + 1");
            if (state == "one")
                sqlite_cmd.CommandText = String.Format("update tbl_statistic set total = total + 1, one = one + 1");
            if (state == "ten")
                sqlite_cmd.CommandText = String.Format("update tbl_statistic set total = total + 1, ten = ten + 1");
            if (state == "hundred")
                sqlite_cmd.CommandText = String.Format("update tbl_statistic set total = total + 1, hundred = hundred + 1");
            if (state == "thousand")
                sqlite_cmd.CommandText = String.Format("update tbl_statistic set total = total + 1, thousand = thousand + 1");
            if (state == "million")
                sqlite_cmd.CommandText = String.Format("update tbl_statistic set total = total + 1, million = million + 1");
            sqlite_cmd.ExecuteNonQuery();
        }

        public static void InsertHistory(HistorySchema history)
        {
            
            SQLiteCommand cmd = new SQLiteCommand();
            cmd = sqliteConn.CreateCommand();
            string query = String.Format("Insert into tbl_history (price, rate, settingId, winnerNum, isWinner, createdAt) values ({0}, {1}, {2}, {3}, {4}, '{5}');", history.Price, history.Rate, SettingSchema.Id, history.WinnerNum, history.IsWinner, history.CreatedAt);
            cmd.CommandText = query;
            cmd.ExecuteNonQuery();
        }

        public static List<HistoryTableSchema> LoadHistoryData()
        {
            List<HistoryTableSchema> lstHistory = new List<HistoryTableSchema>();

            SQLiteCommand cmd = new SQLiteCommand();
            cmd = sqliteConn.CreateCommand();
            string query = "SELECT * FROM tbl_history LEFT JOIN tbl_setting ON tbl_history.settingId = tbl_setting.id";
            cmd.CommandText = query;
            SQLiteDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                HistoryTableSchema historyEntity = new HistoryTableSchema();

                historyEntity.No = reader.GetInt32(0);
                historyEntity.Winner = reader.GetInt32(4);
                if (reader.GetInt32(5) == 1)
                    historyEntity.IsWinner = "Yes";
                else
                    historyEntity.IsWinner = "No";
                historyEntity.Price = reader.GetInt32(1);
                historyEntity.Rate = reader.GetInt32(2);
                historyEntity.WinnerPrice = historyEntity.Price * historyEntity.Rate;
                historyEntity.Location = reader.GetString(9);
                historyEntity.Description = reader.GetString(10);
                historyEntity.CreatedAt = reader.GetString(6);

                lstHistory.Add(historyEntity);
            }
            return lstHistory;
        }

        public static void ClearHistoryData()
        {
            SQLiteCommand cmd = new SQLiteCommand();
            cmd = sqliteConn.CreateCommand();
            string query = "DELETE FROM tbl_history";
            cmd.CommandText = query;
            cmd.ExecuteNonQuery();
        }
    }
}