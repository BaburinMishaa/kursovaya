using System;
using System.Data.SqlClient; // Для работы с SQL Server
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
            string fullName = FullNameTextBox.Text;  // Получаем ФИО логин пароль паспорт данные
            string login = LoginTextBox.Text;
            string password = PasswordBox.Password;
            string passportDetails = PassportTextBox.Text;

            // Проверка на пустые поля
            if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Логин и пароль не могут быть пустыми!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Разделяем ФИО на части (фамилия, имя, отчество)
            string[] nameParts = fullName.Split(' ');
            if (nameParts.Length < 2)
            {
                MessageBox.Show("Введите полное ФИО (фамилия, имя, отчество).", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string surname = nameParts[0];  // Фамилия
            string firstName = nameParts[1]; // Имя
            string lastName = nameParts.Length > 2 ? nameParts[2] : ""; // Отчество (если есть)

            // Строка подключения к базе данных
            string connectionString = "Server=192.168.10.200;Database=baburin_practice;User Id=student;Password=PassWord123!";

            // SQL запрос для добавления данных
            string query = "INSERT INTO Clients (surname, name, lastname, passport_details, login, password) " +
                          "VALUES (@Surname, @Name, @Lastname, @PassportDetails, @Login, @Password)";

            try
            {
                // Создаем подключение и выполняем запрос
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Surname", surname);
                    command.Parameters.AddWithValue("@Name", firstName);
                    command.Parameters.AddWithValue("@Lastname", lastName);
                    command.Parameters.AddWithValue("@PassportDetails", passportDetails);
                    command.Parameters.AddWithValue("@Login", login);
                    command.Parameters.AddWithValue("@Password", password);

                    connection.Open();
                    command.ExecuteNonQuery(); // Выполнение запроса
                    connection.Close();
                }

                MessageBox.Show("Регистрация успешна!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                // Открываем окно входа или другое действие
                LoginWindow loginWindow = new LoginWindow();
                loginWindow.Show();
                this.Close();  // Закрыть окно регистрации
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
