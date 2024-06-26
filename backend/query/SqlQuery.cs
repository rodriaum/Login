﻿using Login.util;
using MySql.Data.MySqlClient;

namespace Login.backend.query
{
    internal class SqlQuery
    {
        public static bool CreateLogin(MySqlConnection connection, string email, string password, string code)
        {
            using (MySqlCommand command = new MySqlCommand("INSERT INTO login (email, password, code, checked) VALUES (@Email, @Password, @Code, @Checked)", connection))
            {
                command.Parameters.AddWithValue("@Email", email);
                command.Parameters.AddWithValue("@Password", password);
                command.Parameters.AddWithValue("@Code", code);
                command.Parameters.AddWithValue("@Checked", false);

                try
                {
                    return command.ExecuteNonQuery() > 0;
                }
                catch (Exception ex)
                {
                    Util.OutgoingError("Criar novo registro.\nErro: " + ex.Message);
                }
            }

            return false;
        }

        public static bool ChangePassword(MySqlConnection connection, string email, string newPassword)
        {
            using (MySqlCommand command = new MySqlCommand("UPDATE login SET password = @Password WHERE email = @Email", connection))
            {
                command.Parameters.AddWithValue("@Email", email);
                command.Parameters.AddWithValue("@Password", newPassword);

                try
                {
                    return command.ExecuteNonQuery() > 0;
                }
                catch (Exception ex)
                {
                    Util.OutgoingError("Alterar password.\nErro: " + ex.Message);
                }
            }

            return false;
        }

        public static bool RemoveLoginByEmail(MySqlConnection connection, string email)
        {
            using (MySqlCommand command = new MySqlCommand("DELETE FROM login WHERE email = @Email", connection))
            {
                command.Parameters.AddWithValue("@Email", email);

                try
                {
                    return command.ExecuteNonQuery() > 0;
                }
                catch (Exception ex)
                {
                    Util.OutgoingError("Remover login com base no e-mail.\nErro: " + ex.Message);
                }
            }

            return false;
        }

        public static bool ConfirmEmail(MySqlConnection connection, string email)
        {
            using (MySqlCommand command = new MySqlCommand("UPDATE login SET checked = 1 WHERE email = @Email", connection))
            {
                command.Parameters.AddWithValue("@Email", email);

                try
                {
                    return command.ExecuteNonQuery() > 0;
                }
                catch (Exception ex)
                {
                    Util.OutgoingError("Confirmar o e-mail.\nErro: " + ex.Message);
                }
            }

            return false;
        }

        public static bool VerifyLogin(MySqlConnection connection, string email, string password)
        {
            using (MySqlCommand command = new MySqlCommand("SELECT password FROM login WHERE email = @Email", connection))
            {
                command.Parameters.AddWithValue("@Email", email);

                try
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read() && reader["password"].ToString() == password)
                            return true;
                    }
                }
                catch (Exception ex)
                {
                    Util.OutgoingError("Verificar o login.\nErro: " + ex.Message);
                }
            }

            return false;
        }

        public static bool VerifyConfirmationEmail(MySqlConnection connection, string email, string code)
        {
            using (MySqlCommand command = new MySqlCommand("SELECT code FROM login WHERE email = @Email", connection))
            {
                command.Parameters.AddWithValue("@Email", email);

                try
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read() && reader["code"].ToString() == code)
                            return true;
                    }
                }
                catch (Exception ex)
                {
                    Util.OutgoingError("Verificar o e-mail de confirmação\nErro: " + ex.Message);
                }
            }

            return false;
        }

        public static bool HasLogin(MySqlConnection connection, string email)
        {
            using (MySqlCommand command = new MySqlCommand("SELECT COUNT(*) FROM login WHERE email = @Email", connection))
            {
                command.Parameters.AddWithValue("@Email", email);

                try
                {
                    return (long)command.ExecuteScalar() > 0;
                }
                catch (Exception ex)
                {
                    Util.OutgoingError("Verificar se tem login.\nErro: " + ex.Message);
                }
            }

            return false;
        }

        public static bool HasConfirmedEmail(MySqlConnection connection, string email)
        {
            using (MySqlCommand command = new MySqlCommand("SELECT COUNT(*), checked FROM login WHERE email = @Email", connection))
            {
                command.Parameters.AddWithValue("@Email", email);

                try
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                            return Convert.ToBoolean(reader["checked"].ToString());
                    }
                }
                catch (Exception ex)
                {
                    Util.OutgoingError("Verificar se tem e-mail confirmado.\nErro: " + ex.Message);
                }
            }

            return false;
        }

        /*
        public static bool ConfirmationCodeExists(MySqlConnection connection, string code)
        {
            using (MySqlCommand command = new MySqlCommand("SELECT COUNT(*) FROM login WHERE code = @Code", connection))
            {
                command.Parameters.AddWithValue("@Code", code);

                try
                {
                    if ((long)command.ExecuteScalar() > 0)
                        return true;
                }
                catch (Exception ex)
                {
                    Util.OutgoingError("Verificar se o código de verificação existe.\nErro: " + ex.Message);
                }
            }

            return false;
        }
        */

        public static string GetEmailByConfirmCode(MySqlConnection connection, string code)
        {
            using (MySqlCommand command = new MySqlCommand("SELECT email FROM login WHERE code = @Code", connection))
            {
                command.Parameters.AddWithValue("@Code", code);

                try
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                            return (string)reader["email"];
                    }
                }
                catch (Exception ex)
                {
                    Util.OutgoingError("Obter o código de confirmação a partir do e-mail.\nErro: " + ex.Message);
                }
            }

            return "";
        }

        public static string GetConfirmCodeByEmail(MySqlConnection connection, string email)
        {
            using (MySqlCommand command = new MySqlCommand("SELECT COUNT(*) FROM login WHERE email = @Email", connection))
            {
                command.Parameters.AddWithValue("@Email", email);

                try
                {  
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                            return (string)reader["code"];
                    }
                }
                catch (Exception ex)
                {
                    Util.OutgoingError("Obter o código de confirmação a partir do e-mail.\nErro: " + ex.Message);
                }
            }

            return "";
        }

        public static List<string> GetEmails(MySqlConnection connection)
        {
            List<string> list = new List<string>();

            using (MySqlCommand command = new MySqlCommand("SELECT email FROM login", connection))
            {
                try
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add((string)reader["email"]);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Util.OutgoingError("Obter a lista de e-mails.\nErro: " + ex.Message);
                }
            }

            return list;
        }
    }
}