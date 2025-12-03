using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace RegIN_Прохоров_Ожгибесов.Classes
{
    public class User
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public byte[] Image = new byte[0];
        public string Pincode { get; set; }
        public DateTime DateUpdate { get; set; }
        public DateTime DateCreate { get; set; }

        public bool HasPin => !string.IsNullOrEmpty(Pincode);

        public CorrectLogin HandelCorrectLogin;
        public InCorrectLogin HandlerInCorrectLogin;

        public delegate void CorrectLogin();
        public delegate void InCorrectLogin();
        public void GetUserLogin(string Login)
        {
            Id = -1;
            this.Login = String.Empty;
            Password = String.Empty;
            Name = String.Empty;
            this.Image = new byte[0];

            MySqlConnection mySqlConnection = WorkingDB.OpenConnection();
            if (WorkingDB.OpenConnection(mySqlConnection))
            {
                MySqlDataReader userQuery = WorkingDB.Query($"SELECT * FROM `users` WHERE `Login` = '{Login}'", mySqlConnection);

                if (userQuery.HasRows)
                {
                    userQuery.Read();
                    Id = userQuery.GetInt32(0);
                    this.Login = userQuery.GetString(1);
                    this.Name = userQuery.GetString(3);
                    Password = userQuery.GetString(2);
                    if (!userQuery.IsDBNull(4))
                    {
                        this.Image = new byte[64 * 1024];
                        userQuery.GetBytes(4, 0, Image, 0, Image.Length);
                    }
                    if (!userQuery.IsDBNull(7))
                    {
                        Pincode = userQuery.GetString(7);
                    }
                    DateUpdate = userQuery.GetDateTime(5);
                    DateCreate = userQuery.GetDateTime(6);
                    HandelCorrectLogin.Invoke();
                }
                else
                    HandlerInCorrectLogin.Invoke();
            }
            else
                HandlerInCorrectLogin.Invoke();

            WorkingDB.CloseConnection(mySqlConnection);
        }

        public void SetUser()
        {
            MySqlConnection mySqlConnection = WorkingDB.OpenConnection();
            if (WorkingDB.OpenConnection(mySqlConnection))
            {
                MySqlCommand mySqlCommand = new MySqlCommand("INSERT INTO `users`(`Login`, `Password`, `Name`, `Image`, `DateUpdate`, `DateCreate`) VALUES(@Login, @Password, @Name, @Image, @DateUpdate,@DateCreate)", mySqlConnection);
            
                mySqlCommand.Parameters.AddWithValue("@Login", this.Login);
                mySqlCommand.Parameters.AddWithValue("@Password", Password);
                mySqlCommand.Parameters.AddWithValue("@Name", Name);
                mySqlCommand.Parameters.AddWithValue("@Image", this.Image);
                mySqlCommand.Parameters.AddWithValue("@DateUpdate", DateUpdate);
                mySqlCommand.Parameters.AddWithValue("@DateCreate", DateCreate);
          
                mySqlCommand.ExecuteNonQuery();
            }
            WorkingDB.CloseConnection(mySqlConnection );
        }

        public void CrateNewPassword()
        {
            if(this.Login != String.Empty)
            {
                Password = GeneratePass();
                MySqlConnection mySqlConnection = WorkingDB.OpenConnection();
                if (WorkingDB.OpenConnection(mySqlConnection))
                {
                    WorkingDB.Query($"UPDATE `users` SET `Password` = '{Password}' WHERE `Login` = '{this.Login}'", mySqlConnection);
                }
                WorkingDB.CloseConnection(mySqlConnection);
                SendMail.SendMessage($"Your account password has been changed.\n New password {Password}",this.Login);
            }
        }

        public void SetPin(string pin)
        {
            MySqlConnection mySqlConnection = WorkingDB.OpenConnection();

            if (WorkingDB.OpenConnection(mySqlConnection))
            {
                WorkingDB.Query($"UPDATE `users` SET `Pincode` = '{pin}' WHERE `Login` = '{this.Login}'", mySqlConnection);
            }
            WorkingDB.CloseConnection(mySqlConnection);
            SendMail.SendMessage($"Your account password has been changed.\n New password {Password}", this.Login);
        }

        public string GeneratePass()
        {
            List<char> NewPassword = new List<char>();
            Random rnd = new Random();
            char[] ArrNumbers = { '1', '2', '3', '4', '5', '6', '7', '8', '9' };
            char[] ArrSymbols = { '|', '-', '_', '!', '@', '#', '$', '&', '+', '=', '+' };
            char[] ArrUppercase = { 'q', 'w', 'e', 'r', 't', 's', 'y', 'u', 'i', 'o', 'p', 'a', 's', 'd',
    'g', 'h', 'j', 'k', 'l', 'z', 'x', 'c', 'v', 'b', 'n', 'm' };
            for (int i = 0; i < 1; i++)
            {
                NewPassword.Add(ArrNumbers[rnd.Next(0, ArrNumbers.Length)]);
            }
            for (int i = 0; i < 1; i++)
            {
                NewPassword.Add(ArrSymbols[rnd.Next(0, ArrSymbols.Length)]);
            }
            for (int i = 0; i < 2; i++)
            {
                NewPassword.Add(char.ToUpper(ArrUppercase[rnd.Next(0, ArrUppercase.Length)]));
            }
            for (int i = 0; i < 6; i++)
            {
                NewPassword.Add(ArrUppercase[rnd.Next(0, ArrUppercase.Length)]);
            }
            for (int i = 0; i < NewPassword.Count; i++)
            {
                int RandomSymbol = rnd.Next(0, NewPassword.Count);
                char Symbol = NewPassword[RandomSymbol];
                NewPassword[RandomSymbol] = NewPassword[i];
                NewPassword[i] = Symbol;
            }
            string NPassword = "";
            for (int i = 0; i < NewPassword.Count; i++)
            {
                NPassword += NewPassword[i];
            }
            return NPassword;
        }
    }
}
