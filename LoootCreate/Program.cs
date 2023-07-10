using System;
using System.Net;
using System.Timers;
using LoootCreate.Models;
using Microsoft.Data.Sqlite;
using Newtonsoft.Json;

namespace LoootCreate
{
    public class Program
    {
        public static void Main(string[] agrs)
        {
            Console.WriteLine("Start Looot Process");

            DBInit();

            new TeleBot().Init();

            Timer timer = new System.Timers.Timer();
            timer.Interval = 1000 * 60 * 60; // 1 시간
            timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
            //timer.Start();

            Console.WriteLine("Press Enter to exit");
            Console.ReadLine();
        }

        private static void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Console.WriteLine($"{DateTime.Now.ToString("yyyy/MM/dd HH:MM:ss")} \t timer Process ");
        }

        private static void DBInit()
        {
            int index = 1;

            //string strConn = @"Data Source=LoootDB.db";
            string strConn = SqLiteBaseRepository.DBConnectionString;

            using var conn = new SqliteConnection(strConn);
            conn.Open();

            string sql = null;
            sql = "SELECT drwno, confirm FROM lottery order by drwno desc limit 1";

            //SQLiteDataReader를 이용하여 연결 모드로 데이타 읽기
            SqliteDataReader rdr = new SqliteCommand(sql, conn).ExecuteReader();

            bool confirm = false;
            if (true == rdr.HasRows)
            {
                while (rdr.Read())
                {
                    index = (int)((long)rdr["drwNo"]);
                    confirm = bool.Parse(rdr["confirm"].ToString());

                    if (confirm == true) index++;
                }
            }

            rdr.Close();

            WebClient webClient = new WebClient();
            while (true)
            {
                sql = null;
                string url = ConstData.LOTTERY_API_URL + index++;
                string jsondata = webClient.DownloadString(url);

                LotteryModel lottery = JsonConvert.DeserializeObject<LotteryModel>(jsondata, new CustomBooleanJsonConverter());

                // false면 일단 중단한다.
                if (lottery.returnValue == false)
                {
                    Console.WriteLine("End : get api ");
                    break;
                }

                // db에 넣기
                sql = $"INSERT INTO lottery (drwno, drwnodate, totsellamnt, firstwinamnt,firstprzwnerco,drwtno1,drwtno2,drwtno3,drwtno4,drwtno5,drwtno6,bnusno,confirm)" +
                   $" VALUES ({lottery.drwNo},'{lottery.drwNoDate.ToString("yyyy-MM-dd")}',{lottery.totSellamnt},{lottery.firstWinamnt},{lottery.firstPrzwnerCo}," +
                   $"{lottery.drwtNo1},{lottery.drwtNo2},{lottery.drwtNo3},{lottery.drwtNo4},{lottery.drwtNo5},{lottery.drwtNo6},{lottery.bnusNo},'True')";
                SqliteCommand cmd = new SqliteCommand(sql, conn);
                int row = cmd.ExecuteNonQuery();
                Console.WriteLine($"{lottery.drwNo} : {lottery.drwNoDate.ToString("yyyy-MM-dd")}");
            }
            conn.Close();
        }
    }
}