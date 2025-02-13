using System;
using System.Data.SqlClient; // Добавлено для SqlConnection
using System.Text.RegularExpressions;
using System.Windows;

namespace Kursovaya
{
    public partial class RegistrationWindow : Window
    {
        public RegistrationWindow()
        {
            InitializeComponent();
        }

        // Обработчик для кнопки "Зарегистрироваться"
        private void OnRegisterButtonClick(object sender, RoutedEventArgs e)
        {
            string fullName = FullNameTextBox.Text;
            string login = LoginTextBox.Text;
            string password = PasswordBox.Password;
            string passportDetails = PassportTextBox.Text;

            // Проверка на пустые поля
            if (string.IsNullOrWhiteSpace(fullName) || string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(passportDetails))
            {
                MessageBox.Show("Все поля должны быть заполнены!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Разделяем ФИО на части (например, по пробелу)
            string[] nameParts = fullName.Split(' ');
            if (nameParts.Length < 2)
            {
                MessageBox.Show("Введите полное ФИО (имя и отчество).", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string firstName = nameParts[0];  // Имя
            string lastName = nameParts[1];   // Отчество

            // Простейшая проверка на уникальность логина
            if (login == "admin")
            {
                MessageBox.Show("Этот логин уже занят. Выберите другой.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Дополнительные проверки пароля
            if (password.Length < 6)
            {
                MessageBox.Show("Пароль должен быть не менее 6 символов.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Строка подключения к базе данных
            string connectionString = "Server=192.168.10.200;Database=baburin_practice;User Id=student;Password=PassWord123!";

            // SQL запрос для добавления пользователя в базу данных
            string query = "INSERT INTO Clients (name, surname, passport_details) VALUES (@Name, @Surname, @PassportDetails)";

            try
            {
                // Создаем подключение и выполняем запрос
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Name", firstName);
                    command.Parameters.AddWithValue("@Surname", lastName);
                    command.Parameters.AddWithValue("@PassportDetails", passportDetails);

                    connection.Open();
                    command.ExecuteNonQuery(); // Выполнение запроса
                    connection.Close();
                }

                MessageBox.Show("Регистрация успешна!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                // Закрываем окно регистрации и открываем окно входа
                LoginWindow loginWindow = new LoginWindow();
                loginWindow.Show(); // Показываем окно входа
                this.Close(); // Закрываем окно регистрации
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при регистрации: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Обработчик для кнопки "Войти"
        private void OnLoginButtonClick(object sender, RoutedEventArgs e)
        {
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
            this.Close();
        }

        // Обработчик для события TextChanged поля PassportTextBox
        private void PassportTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            string passportValue = PassportTextBox.Text;

            // Проверяем, состоит ли значение из 10 цифр
            if (passportValue.Length == 10 && Regex.IsMatch(passportValue, @"^\d{10}$"))
            {
                // Если введено 10 цифр, то всё в порядке
                PassportTextBox.Background = System.Windows.Media.Brushes.White;
            }
            else
            {
                // Если количество символов не равно 10 или введены не только цифры, меняем фон на красный
                PassportTextBox.Background = System.Windows.Media.Brushes.LightCoral;
            }
        }
    }
}
