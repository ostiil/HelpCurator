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
using DP.Model;

namespace DP
{
    /// <summary>
    /// Логика взаимодействия для AutorizeForm.xaml
    /// </summary>
    public partial class AutorizeForm : Window
    {
        public AutorizeForm()
        {
            InitializeComponent();
        }

        private void AutorizeBtn_Click(object sender, RoutedEventArgs e)
        {
            Autorize autorize = new Autorize();
            using (Context context = new Context())
            {
                
                var user = context.autorize.Where(u => u.Password == PassTb.Password).FirstOrDefault();
                if (PassTb.Password != null && user != null)
                {
                    MainWindow mainWindow = new MainWindow();
                    mainWindow.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Введите пароль или проверьте правильность ввода пароля", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                
            }
        }

        private void ChangePass_Click(object sender, RoutedEventArgs e)
        {
            ChangePassForm changePass = new ChangePassForm();
            changePass.ShowDialog();
        }
    }
}
