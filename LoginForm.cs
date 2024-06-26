﻿using Login.backend;
using Login.backend.query;
using Login.util;
using MySql.Data.MySqlClient;

namespace Login
{
    public partial class LoginForm : Form
    {

        public LoginForm(string email, string password)
        {
            InitializeComponent();
            Connection.Init();

            if (!string.IsNullOrEmpty(email))
            {
                userTextBox.Text = email;
            }

            if (!string.IsNullOrEmpty(password)) 
            { 
                passwordTextBox.Text = password;
            }
        }

        private async void loginButton_Click(object sender, EventArgs e)
        {
            MySqlConnection? connection = Connection.MySqlConnection;
            Button button = loginButton;

            if (button != null && connection != null)
            {
                string email = userTextBox.Text;
                string password = passwordTextBox.Text;

                if (string.IsNullOrEmpty(email) || !email.Contains("@"))
                {
                    Util.DebugLabel(userDebugLabel, "Insira um e-mail válido.");
                }

                if (string.IsNullOrEmpty(password))
                {
                    Util.DebugLabel(passwordDebugLabel, "Insira uma password válida.");
                }

                if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(password) && email.Contains("@"))
                {

                    if (SqlQuery.HasLogin(connection, email))
                    {
                        if (SqlQuery.VerifyLogin(connection, email, Util.HashPassword(password)))
                        {
                            if (SqlQuery.HasConfirmedEmail(connection, email))
                            {
                                Util.DebugLabel(loginDebugLabel, "Login efetuado com sucesso.");

                                await Task.Delay(1000);

                                new PrivateAreaForm().Show();
                                this.Hide();
                            }
                            else
                            {
                                Util.DebugLabel(loginDebugLabel, "Você precisa verificar o e-mail antes de fazer login.");

                                await Task.Delay(1000);

                                new ConfirmEmailForm().Show();
                                this.Hide();
                            }
                        }
                        else
                        {
                            Util.DebugLabel(loginDebugLabel, "Os parâmetros de login estão incorreto.");
                        }
                    }
                    else
                    {
                        Util.DebugLabel(loginDebugLabel, "Não existe nenhum login com essas credenciais.");
                    }
                }
            }
            else
            {
                Util.OutgoingError("Botão ou conexão com o banco de dados nulo.");
            }
        }

        private void registerButton_Click(object sender, EventArgs e)
        {
            RegisterForm form = new RegisterForm();

            form.Show();
            this.Hide();
        }

        private void userTextBox_TextChanged(object sender, EventArgs e)
        {
            userTextBox.Focus();
        }

        private void passwordTextBox_TextChanged(object sender, EventArgs e)
        {
            passwordTextBox.Focus();
        }
    }
}
