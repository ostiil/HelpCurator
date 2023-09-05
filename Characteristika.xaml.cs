using System.Linq;
using System.Windows;
using Microsoft.Office.Interop.Word;
using Word = Microsoft.Office.Interop.Word;
using System.IO;
using DP.Model;
using System.Reflection;

namespace DP
{
    /// <summary>
    /// Логика взаимодействия для Characteristika.xaml
    /// </summary>
    public partial class Characteristika : System.Windows.Window
    {
        
        public Characteristika()
        {
            InitializeComponent();
            using (Context context = new Context())
            {
                Charact.ItemsSource = context.student.ToList();
            }   
        }

        private void OpenWord_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //каталог старта программы
                var appDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                //относительный путь
                var relativePath = @"\Resource\shablon.docx";
                var fullPath = Path.Combine(appDir + relativePath);
                Word._Application oWord = new Word.Application();
                _Document oDoc = GetWord(fullPath, oWord);
                oDoc.Close();
                oWord.Quit();
             }
            catch
            {
                MessageBox.Show("Закройте Word и повторите попытку", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
            }


}
        public _Document GetWord(string path, Microsoft.Office.Interop.Word._Application word)
        {
            _Document oDoc = word.Documents.Add(path);
            Template(oDoc);
            return oDoc;
        }

        public void Template(Word._Document oDoc)
        {
            string birth = ((Student)Charact.SelectedItems[0]).Birth.ToString();
            string prikaz = ((Student)Charact.SelectedItems[0]).Date_enrollmant.ToString();
            string period = ((Student)Charact.SelectedItems[0]).Period.ToString();

            string[] massDate;
            massDate = birth.Split(" ");
            oDoc.Bookmarks["fio"].Range.Text = ((Student)Charact.SelectedItems[0]).Fio_student;
            oDoc.Bookmarks["birth"].Range.Text = massDate[0].ToString();
            oDoc.Bookmarks["prikaz"].Range.Text = ((Student)Charact.SelectedItems[0]).Order_of_enrollment;
            massDate = prikaz.Split(" ");
            oDoc.Bookmarks["datePrikaz"].Range.Text = massDate[0].ToString();
            oDoc.Bookmarks["fio2"].Range.Text = ((Student)Charact.SelectedItems[0]).Fio_student;
            oDoc.Bookmarks["spec"].Range.Text = ((Student)Charact.SelectedItems[0]).Specialnost;
            massDate = period.Split(" ");
            oDoc.Bookmarks["period"].Range.Text = massDate[0].ToString();
            oDoc.Bookmarks["mark"].Range.Text = 
                MotivationCB.Text;
            oDoc.Bookmarks["motivation"].Range.Text = MarksCB.Text;
            oDoc.Bookmarks["discipline"].Range.Text = PenaltyCB.Text;
            oDoc.Bookmarks["emotional"].Range.Text = EmotionalCB.Text;
            oDoc.Bookmarks["vneshVid"].Range.Text = AppearanceCB.Text;
            oDoc.Bookmarks["vred"].Range.Text = PECb.Text;
        }

        //private void Window_Closed(object sender, System.EventArgs e)
        //{
        //    oWord.Quit();
        //}
    }
}
