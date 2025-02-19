using kursovaya;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Kursovaya
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        // Обработчик для кнопки "Регистрация"
        private void OnRegistrationButtonClick(object sender, RoutedEventArgs e)
        {
            // Открыть окно регистрации
            RegistrationWindow registrationWindow = new RegistrationWindow();
            registrationWindow.Show();   // Показываем окно регистрации
            this.Close();                // Закрываем окно входа
        }

        // Пример обработчика для кнопки "Войти", если нужно добавить логику
        private void OnLoginButtonClick(object sender, RoutedEventArgs e)
        {
            string login = LoginTextBox.Text; // Получаем логин из текстового поля
            string password = PasswordBox.Password; // Получаем пароль из поля

            // Проверка на пустые поля
            if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Введите логин и пароль!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Строка подключения к базе данных
            string connectionString = "Server=192.168.10.200;Database=baburin_practice;User Id=student;Password=PassWord123!";

            // SQL запрос для проверки логина и пароля
            string query = "SELECT COUNT(*) FROM Clients WHERE login = @Login AND password = @Password";

            try
            {
                // Создаем подключение и выполняем запрос
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Login", login);
                    command.Parameters.AddWithValue("@Password", password);

                    connection.Open();
                    int result = (int)command.ExecuteScalar(); // Получаем количество строк, которые соответствуют запросу

                    if (result > 0)  // Если результат больше 0, значит пользователь найден
                    {
                        MessageBox.Show("Вы успешно вошли!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                        // Открытие главного окна
                        MainWindow mainWindow = new MainWindow(); // Главное окно
                        mainWindow.Show();
                        this.Close();  // Закрыть окно входа
                    }
                    else
                    {
                        MessageBox.Show("Неверный логин или пароль!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }

                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при подключении к базе данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}