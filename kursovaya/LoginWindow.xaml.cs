using System;
using System.Collections.Generic;
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
            // Логика входа, например проверка логина и пароля
        }
    }
}
