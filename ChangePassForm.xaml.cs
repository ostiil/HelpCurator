using DP.Model;
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

namespace DP
{
    /// <summary>
    /// Логика взаимодействия для ChangePassForm.xaml
    /// </summary>
    public partial class ChangePassForm : Window
    {
        public ChangePassForm()
        {
            InitializeComponent();
        }

        private void AutorizeBtn_Click(object sender, RoutedEventArgs e)
        {
            using (Context context = new Context())
            {
                var check = context.autorize.FirstOrDefault(x => x.Password == OldPassTb.Password);
                if (check != null)
                {
                    check.Password = NewPassTb.Password;
                    Autorize autorize = new Autorize();
                    autorize = check;
                    context.Update(autorize);
                    context.SaveChanges();
                    MessageBox.Show("Пароль обновлен", "Уведоление", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Старый пароль не найден", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
