using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace loginsqlalec
{
    class Program
    {
        static void Main(string[] args)
        {
            SqlConnection connection = new SqlConnection("Server=GMRMLTV;Database=alecrockpapersicssors;User Id=alecLoginUser;Password = 1234;");
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter adapter = new SqlDataAdapter();
            DataTable table = new DataTable();
            bool con = true;
            Console.WriteLine("1 to login 2 to register");
            string choice = Console.ReadLine();
            if (choice == "1")
            {
                Console.Clear();
                while (con)
                {
                    Console.WriteLine(" User please");
                    string user = Console.ReadLine();
                    Console.WriteLine(" pass please");
                    string pass = Console.ReadLine();

                    //cmd.CommandType = System.Data.CommandType.Text;
                    //cmd.CommandText = String.Format("SELECT * FROM Users WHERE Username = '{0}' AND Password = '{1}'", user, pass);
                    //cmd.Connection = connection;

                    cmd = new SqlCommand("usp_GetUser", connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Username", user));
                    cmd.Parameters.Add(new SqlParameter("@Password", pass));

                    adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(table);

                    if (table.Rows.Count > 0)
                    {
                        Console.WriteLine("welcome {0}", user);
                        RockPaperScissors(user, connection, cmd, table, adapter);
                        con = false;
                    }
                    else
                    {
                        Console.WriteLine("Go away {0}. Wahhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhh", user);

                    }



                }
            }
            else if (choice == "2")
            {
                Console.Clear();
                Console.WriteLine(" User please");
                string user = Console.ReadLine();
                Console.WriteLine(" pass please");
                string pass = Console.ReadLine();

                connection.Open();

                //cmd.CommandType = System.Data.CommandType.Text;
                // cmd.CommandText = String.Format("INSERT Users VALUES('{0}', '{1}', 0, 0, 0)", user, pass);
                // cmd.Connection = connection;
                cmd = new SqlCommand("usp_CreateUser", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@Username", user));
                cmd.Parameters.Add(new SqlParameter("@Password", pass));
                cmd.ExecuteNonQuery();

                adapter.SelectCommand = cmd;
               
                connection.Close();
                RockPaperScissors(user, connection, cmd, table, adapter);
                Console.Clear();


            }
            Console.ReadKey();
        }
        static void RockPaperScissors(String user, SqlConnection connect, SqlCommand cmd, DataTable table, SqlDataAdapter adapter)
        {
            
            while (true)
            {
                Console.WriteLine("1 to play rock paper scissors 2 to sees stats for rock paper scissors 3 to play the guessing game 4 to see stats for guessing game");
                string choice2 = Console.ReadLine();
                Random random = new Random();
                if (choice2 == "1")
                {
                    Console.Clear();
                   
                    int Computerfight = random.Next(1, 4);
                    Console.WriteLine("1 for rock 2 for paper and 3 for scissors ");
                    int weapon = int.Parse(Console.ReadLine());
                    if (weapon == Computerfight)
                    {
                        connect.Open();
                        Console.WriteLine("Tie!");
                        cmd.CommandText = String.Format("UPDATE Users SET rockties = rockties + 1 WHERE Username = '{0}'", user);
                        
                        connect.Close();
                    }
                    else if (weapon == Computerfight + 1 || weapon == Computerfight - 2)
                    {
                        connect.Open();
                        Console.WriteLine("You WIN!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                        cmd.CommandText = String.Format("UPDATE Users SET rockwins = rockwins + 1 WHERE Username = '{0}'", user);
                        
                        connect.Close();
                    }
                    else
                    {
                        connect.Open();
                        Console.WriteLine("you lOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOse you big {0}face", user);
                        cmd.CommandText = String.Format("UPDATE Users SET rocklosses = rocklosses + 1 WHERE Username = '{0}'", user);
                        
                        connect.Close();
                    }
                }
                else if (choice2 == "2")
                {
                    Console.Clear();
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.CommandText = String.Format("SELECT	Users.rockwins, Users.rocklosses, Users.rockties FROM Users WHERE Username = '{0}'", user);
                    cmd.Connection = connect;

                    connect.Open();
                    adapter.SelectCommand = cmd;
                    table = new DataTable();
                    adapter.Fill(table);

                    connect.Close();

                    int wins = int.Parse(table.Rows[0]["rockwins"].ToString());
                    int losses = int.Parse(table.Rows[0]["rocklosses"].ToString());
                    int ties = int.Parse(table.Rows[0]["rockties"].ToString());
                    Console.WriteLine("Wins: {0} Losses : {1} Ties : {2}", wins, losses, ties);


                }
                else if(choice2 == "3")
                {
                    Console.WriteLine("You have 10 chances to think of the correct number. It is between 1 and 100");
                   int number = random.Next(1, 101);
                    int j = 0;
                for(int i = 0; i < 10; i++)
                    {
                        int choice3 = int.Parse(Console.ReadLine());
                        if(number == choice3)
                        {
                            
                            
                            connect.Open();
                            Console.WriteLine("You WIN!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                            cmd.CommandText = String.Format("UPDATE Users SET guesswins = guesswins + 1 WHERE Username = '{0}'", user);
                          
                            connect.Close();
                            i = 10;
                            break;
                        }
                        if(number > choice3)
                        {
                            Console.WriteLine("Higher!");
                            j++;
                        }
                        else
                        {
                            Console.WriteLine("Lower!");
                            j++;
                        }
                        if(j == 10)
                        {
                            connect.Open();
                            Console.WriteLine("you lOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOse you big {0}face", user);
                            cmd.CommandText = String.Format("UPDATE Users SET guesslosses = guesslosses + 1 WHERE Username = '{0}'", user);
                            cmd.ExecuteNonQuery();
                            connect.Close();
                        }
                    }
                    
                }
                else if (choice2 == "4")
                {
                    Console.Clear();
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.CommandText = String.Format("SELECT	Users.guesswins, Users.guesslosses FROM Users WHERE Username = '{0}'", user);
                    cmd.Connection = connect;

                    connect.Open();
                    adapter.SelectCommand = cmd;
                    table = new DataTable();
                    adapter.Fill(table);

                    connect.Close();

                    int wins = int.Parse(table.Rows[0]["guesswins"].ToString());
                    int losses = int.Parse(table.Rows[0]["guesslosses"].ToString());
                    Console.WriteLine("Wins: {0} Losses : {1}", wins, losses);
                }
            }
        }
    }
}