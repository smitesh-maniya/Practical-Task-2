using System.Data;
using System.Diagnostics;
using MySql.Data.MySqlClient;
using System.Text;

namespace PracticeSession2
{
    class Program
    {
        static string connectionString = "server=localhost; port=3306; uid=root;" +
                                        "pwd=; database=auth; charset=utf8; sslMode=none;";
        static string[] strs = new string[] { "ABCDEFGHIJKLMNOPQRSTUVWXYZ", "abcdefghijklmnopqrstuvwxyz", "~`!@#$%^&*()-_+=", "1234567890" };

        
        
        static void Main(string[] args)
        {
            trancateTable("passworddetail");

            Stopwatch sw = new Stopwatch();
            sw.Start();

            Console.WriteLine("Records are inserting...");
            List<Task> tasks = new List<Task>();
            int totalRecords = 1000000;
            int currentTasks = 6;
            for (int i = 1; i <= currentTasks; i++)
            {
                Task t = AddRecords(totalRecords / currentTasks);
                tasks.Add(t);
            }
            if(totalRecords % currentTasks > 0)
            {
                Task t1 = AddRecords(totalRecords % currentTasks);
                tasks.Add(t1);
            }
            try
            {
                Task.WaitAll(tasks.ToArray());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            sw.Stop();
            Console.WriteLine("time: "+sw.Elapsed);
            Console.WriteLine("Task Completed");
            Console.ReadLine();
           
        }

        private static void trancateTable(string table)
        {
            MySqlConnection sqlConnection = new MySqlConnection(connectionString);
            sqlConnection.Open();
            string query = $"TRUNCATE TABLE {table}";
            MySqlCommand cmd = new MySqlCommand(query, sqlConnection);
            cmd.ExecuteNonQuery();
            //sqlConnection.Close();
        }

        private static async Task AddRecords(int totalItretion)
        {
            MySqlConnection sqlConnection = new MySqlConnection(connectionString);
            sqlConnection.Open();
            for (int i=0; i< totalItretion; i++){
                try
                {
                    //string password = generatePassword();
                    string query = $"INSERT INTO `passworddetail` (`password`) VALUES ('{generatePassword()}');";
                    MySqlCommand cm = new MySqlCommand(query,sqlConnection);
                    cm.CommandType = System.Data.CommandType.Text;
                    await cm.ExecuteNonQueryAsync();
                    
                }catch(Exception ex)
                {
                    i--;
                }
            }
            sqlConnection.Close();
        }

        private static string generatePassword()
        {
            var rnd = new Random();
            StringBuilder password= new StringBuilder();

            int upperCase = rnd.Next(4,7);
            int loweCase = rnd.Next(4,7);
            int numbers = rnd.Next(1,4);
            int symbols = 20-upperCase-loweCase-numbers;

            for(int i=0; i<upperCase; i++)
            {
                password.Append(strs[0][rnd.Next(strs[0].Length-1)]);
            }
            for (int i = 0; i < loweCase; i++)
            {
                password.Append(strs[1][rnd.Next(strs[1].Length - 1)]);
            }
            for (int i = 0; i < symbols; i++)
            {
                password.Append(strs[2][rnd.Next(strs[2].Length - 1)]);
            }
            for (int i = 0; i < numbers; i++)
            {
                password.Append(strs[3][rnd.Next(strs[3].Length - 1)]);
            }

            return password.ToString();
        }

    }
}