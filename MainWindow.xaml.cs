using Microsoft.Win32;
using System;
using System.Linq;
using System.Windows;
using System.Data;
using System.IO;
using ExcelDataReader;
using Microsoft.Office.Interop.Word;
using System.Windows.Controls;
using OfficeOpenXml;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using WordReport = Microsoft.Office.Interop.Word;
using DP.Model;
using LicenseContext = OfficeOpenXml.LicenseContext;
using System.Reflection;
using System.Drawing;
using Microsoft.Office.Interop.Excel;

namespace DP
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : System.Windows.Window
    {
        string? fileName;
        IExcelDataReader? edr;
        
        public MainWindow()
        {
            InitializeComponent();
            using (Context context = new Context())
            {
                StudentGrid.ItemsSource = context.student.ToList();
                GridEvent.ItemsSource = context.@event.Include(x => x.TypeEvent).ToList();
                GridAttendance.ItemsSource = context.attendance.ToList();
                TypeCBox.ItemsSource = context.typeEvent.ToList();
                TypeCBox.DisplayMemberPath = "Name_type";
                TypeCBox.SelectedValuePath = "Id_type";
            }

        }

        private void AddStudetn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Services userService = new Services();
            try
            {
                userService.AddStudent(Fio.Text, Specialnost.Text, datePicker.SelectedDate.Value, "", Adress.Text,
                Phone.Text, FioMother.Text, PhoneMother.Text, FioFather.Text, PhoneFather.Text, Benefit.Text,
                Order.Text, DateOrder.SelectedDate.Value, DateStart.SelectedDate.Value);

                MessageBox.Show("Студент добавлен", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                RefreshGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Заполните поля", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void RefreshGrid()
        {
            StudentGrid.ItemsSource = null;
            GridEvent.ItemsSource = null;
            GridAttendance.ItemsSource = null;
            using (Context context = new Context())
            {
                StudentGrid.ItemsSource = context.student.ToList();
                GridAttendance.ItemsSource = context.attendance.ToList();
                GridEvent.ItemsSource = context.@event.Include(x => x.TypeEvent).ToList();
            }
        }

        private void Download_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog fileDialog = new OpenFileDialog();
                if (fileDialog.ShowDialog() == true)
                {
                    fileName = fileDialog.FileName;
                    fileDialog.Filter = "EXCEL Files (*.xlsx)|*.xlsx|EXCEL Files 2003 (*.xls)|*.xls|All files (*.*)|*.*";

                    GridAttendance.Columns.Clear();
                    GridAttendance.ItemsSource = readFile(fileDialog.FileName);
                    GridAttendance.AutoGenerateColumns = true;

                }
            }
            catch { }
            
        }

        private DataView readFile(string fileNames)
        {

            var extension = fileNames.Substring(fileNames.LastIndexOf('.'));
            // поток для чтения.
            FileStream stream = File.Open(fileNames, FileMode.Open, FileAccess.Read);
            // Читатель для файлов с расширением *.xlsx.
            if (extension == ".xlsx")
                edr = ExcelReaderFactory.CreateOpenXmlReader(stream);
            // Читатель для файлов с расширением *.xls.
            else if (extension == ".xls")
                edr = ExcelReaderFactory.CreateBinaryReader(stream);

            var conf = new ExcelDataSetConfiguration
            {
                ConfigureDataTable = _ => new ExcelDataTableConfiguration
                {
                    UseHeaderRow = true
                }
            };
            // получаем DataView 
            DataSet dataSet = edr.AsDataSet(conf);
            DataView dtView = dataSet.Tables[0].AsDataView();
            // После завершения чтения освобождаем ресурсы.
            edr.Close();
            return dtView;
        }

        private void AddEvent_Click(object sender, RoutedEventArgs e)
        {
            Services userService = new Services();
            try
            {
                userService.AddEvent(NameEvent.Text, DateEvent.SelectedDate.Value, StatusChBox.IsChecked.Value, DescriptionTb.Text, Convert.ToInt32(StudTb.Text), Convert.ToInt32(TypeCBox.SelectedValue));
                MessageBox.Show("Мероприятие добавлено", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                RefreshGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Заполните поля", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void OpenWord_Click(object sender, RoutedEventArgs e)
        {
            Characteristika characteristika = new Characteristika();
            characteristika.ShowDialog();
        }

        private void AddToDb_Click(object sender, RoutedEventArgs e)
        {
            using (Context context = new Context())
            {
                using (var excel = new ExcelPackage(new FileInfo(fileName)))
                {
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    var sheet = excel.Workbook.Worksheets[0];

                    var rowCount = sheet.Dimension.Rows;
                    var columnCount = sheet.Dimension.Columns;
                    for (int i = 2; i <= rowCount; i++)
                    {
                        var entity = new Attendance();
                        for (int j = 1; j <= columnCount; j++)
                        {
                            var cell = sheet.Cells[i, j].Value;
                            switch (j)
                            {
                                case 1:
                                    entity.Month = cell.ToString();
                                    break;
                                case 2:
                                    entity.Student = cell.ToString(); break;
                                case 3:
                                    entity.InTotal = (int)(double)cell; break;
                                case 4:
                                    entity.Respectful = (int)(double)cell; break;
                                case 5:
                                    entity.NotRespectful = (int)(double)cell; break;
                                case 6:
                                    entity.Delay = (int)(double)cell; break;
                                case 7:
                                    entity.Description = (string?)cell; break;
                                default:
                                    MessageBox.Show("Проверьте таблицу на соответствие столбцов"); break;
                            }
                            
                            context.attendance.Add(entity);
                            
                        }
                        context.SaveChanges();
                        RefreshGrid();
                        //GridAttendance.AutoGenerateColumns = false;
                    }
                }
            }

        }

        private void EditStudetn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (Context context = new Context())
                {
                    Student student = new Student();
                    student = (Student)StudentGrid.SelectedItem;
                    context.Update(student);
                    context.SaveChanges();

                    MessageBox.Show("Успешно", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Выберите запись для изменения", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            RefreshGrid();
        }


        private void ValidationFio(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            string Symbol = e.Text;
            if (!Regex.Match(Symbol, @"[а-яА-Я]").Success)
            {
                e.Handled = true;
            }
        }
        private void ValidationCount(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            string Symbol = e.Text;
            if (!Regex.Match(Symbol, @"[0-9]").Success)
            {
                e.Handled = true;
            }
        }

        private void Phone_TextChanged(object sender, TextChangedEventArgs e)
        {
            string phoneNumber = Phone.Text;
            phoneNumber = new string(phoneNumber.Where(char.IsDigit).ToArray());

            if (phoneNumber.Length > 0)
            {
                phoneNumber = "(" + phoneNumber;
                if (phoneNumber.Length > 4)
                {
                    phoneNumber = phoneNumber.Insert(4, ")");
                }
                if (phoneNumber.Length > 8)
                {
                    phoneNumber = phoneNumber.Insert(8, "-");
                }
                if (phoneNumber.Length > 11)
                {
                    phoneNumber.Insert(11, "-");
                }
            }
            Phone.Text = phoneNumber;
            Phone.SelectionStart = Phone.Text.Length;
        }

        private void PhoneMother_TextChanged(object sender, TextChangedEventArgs e)
        {
            string phoneNumber = PhoneMother.Text;
            phoneNumber = new string(phoneNumber.Where(char.IsDigit).ToArray());

            if (phoneNumber.Length > 0)
            {
                phoneNumber = "(" + phoneNumber;
                if (phoneNumber.Length > 4)
                {
                    phoneNumber = phoneNumber.Insert(4, ")");
                }
                if (phoneNumber.Length > 8)
                {
                    phoneNumber = phoneNumber.Insert(8, "-");
                }
                if (phoneNumber.Length > 11)
                {
                    phoneNumber.Insert(11, "-");
                }
            }
            PhoneMother.Text = phoneNumber;
            PhoneMother.SelectionStart = PhoneMother.Text.Length;
        }

        private void PhoneFather_TextChanged(object sender, TextChangedEventArgs e)
        {
            string phoneNumber = PhoneFather.Text;
            phoneNumber = new string(phoneNumber.Where(char.IsDigit).ToArray());

            if (phoneNumber.Length > 0)
            {
                phoneNumber = "(" + phoneNumber;
                if (phoneNumber.Length > 4)
                {
                    phoneNumber = phoneNumber.Insert(4, ")");
                }
                if (phoneNumber.Length > 8)
                {
                    phoneNumber = phoneNumber.Insert(8, "-");
                }
                if (phoneNumber.Length > 11)
                {
                    phoneNumber.Insert(11, "-");
                }
            }
            PhoneFather.Text = phoneNumber;
            PhoneFather.SelectionStart = PhoneFather.Text.Length;
        }


        private void NotRespRb_Checked(object sender, RoutedEventArgs e)
        {

            foreach (Attendance drv in GridAttendance.ItemsSource)
            {
                var value = GridAttendance.ItemContainerGenerator.ContainerFromItem(drv) as DataGridRow;
                value.Background = System.Windows.Media.Brushes.White;
                if (drv.NotRespectful >= 3)
                {
                    
                    value.Background = System.Windows.Media.Brushes.Red;
                }
            }

        }

        private void InTotalRb_Checked(object sender, RoutedEventArgs e)
        {
            foreach (Attendance drv in GridAttendance.ItemsSource)
            {
                var value = GridAttendance.ItemContainerGenerator.ContainerFromItem(drv) as DataGridRow;
                value.Background = System.Windows.Media.Brushes.White;
                if (drv.InTotal > 5)
                {
                    
                    value.Background = System.Windows.Media.Brushes.Yellow;
                }
            }
        }

        private void ClearGrid_Click(object sender, RoutedEventArgs e)
        {
            foreach (Attendance drv in GridAttendance.ItemsSource)
            {
                var value = GridAttendance.ItemContainerGenerator.ContainerFromItem(drv) as DataGridRow;
                if (drv.InTotal > 2)
                {
                    value.Background = System.Windows.Media.Brushes.White;
                }
            }
        }

        private void DelStudetn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var dialogResult = MessageBox.Show("Вы действительно хотите удалить запись?", "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (MessageBoxResult.Yes == dialogResult)
                {
                    using (Context context = new Context())
                    {
                        Student student = new Student();
                        student = (Student)StudentGrid.SelectedItem;
                        context.Remove(student);
                        context.SaveChanges();

                        MessageBox.Show("Студент удален", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
            catch (ArgumentNullException ex)
            {
                MessageBox.Show("Выберите запись", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            RefreshGrid();
        }

        private void ClearDbStudent_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (Context context = new Context())
                {
                    context.student.RemoveRange(context.student);
                    context.SaveChanges();
                }
                MessageBox.Show("Успешно!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            RefreshGrid();
        }

        private void DownloadDbStudent_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog fileDialog = new OpenFileDialog();
                if (fileDialog.ShowDialog() == true)
                {
                    fileName = fileDialog.FileName;
                    fileDialog.Filter = "EXCEL Files (*.xlsx)|*.xlsx|EXCEL Files 2003 (*.xls)|*.xls|All files (*.*)|*.*";
                }
                using (Context context = new Context())
                {
                    using (var excel = new ExcelPackage(new FileInfo(fileName)))
                    {
                        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                        var sheet = excel.Workbook.Worksheets[0];

                        var rowCount = sheet.Dimension.Rows;
                        var columnCount = sheet.Dimension.Columns;
                        for (int i = 2; i <= rowCount; i++)
                        {
                            var entity = new Student();
                            for (int j = 1; j <= columnCount; j++)
                            {
                                var cell = sheet.Cells[i, j].Value;
                                if (cell != null)
                                {
                                    switch (j)
                                    {
                                        case 1:
                                            entity.Fio_student = cell.ToString();
                                            break;
                                        case 2:
                                            entity.Specialnost = cell.ToString(); break;
                                        case 3:
                                            entity.Birth = (DateTime)cell; break;
                                        case 4:
                                            entity.AdressRegistr = cell.ToString(); break;
                                        case 5:
                                            entity.Adress = cell.ToString(); break;
                                        case 6:
                                            entity.Phone = cell.ToString(); break;
                                        case 7:
                                            entity.Fio_mother = cell.ToString(); break;
                                        case 8:
                                            entity.Phone_mother = cell.ToString(); break;
                                        case 9:
                                            entity.Fio_father = cell.ToString(); break;
                                        case 10:
                                            entity.Phone_father = cell.ToString(); break;
                                        case 11:
                                            entity.Benefits = cell.ToString(); break;
                                        case 12:
                                            entity.Order_of_enrollment = cell.ToString(); break;
                                        case 13:
                                            entity.Date_enrollmant = (DateTime)cell; break;
                                        case 14:
                                            entity.Period = (DateTime)cell; break;
                                        default:
                                            MessageBox.Show("Проверьте таблицу на соответствие столбцов"); break;
                                    }
                                }
                                else
                                {
                                    return;
                                }

                                context.student.Add(entity);

                            }
                            context.SaveChanges();
                            StudentGrid.ItemsSource = context.student.ToList();
                            
                        }
                    }
                }
            }
            catch { }
            
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            //oWord.Quit();
            System.Windows.Application.Current.Shutdown();
        }

        private void DelEvent_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var dialogResult = MessageBox.Show("Вы действительно хотите удалить запись?", "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (MessageBoxResult.Yes == dialogResult)
                {
                    using (Context context = new Context())
                    {
                        Event report = new Event();
                        report = (Event)GridEvent.SelectedItem;
                        context.Remove(report);
                        context.SaveChanges();

                        MessageBox.Show("Запись удалена");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Выберите запись", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            RefreshGrid();
        }

        private void CreateReport_Click(object sender, RoutedEventArgs e)
        {
            //try
            //{
                //каталог старта программы
                var appDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                var relativePath = @"\Resource\report.docx";
                var fullPath = Path.Combine(appDir + relativePath);
                Microsoft.Office.Interop.Word._Application reportWord = new WordReport.Application();
                _Document oDoc = GetDoc(fullPath, reportWord);
                
                oDoc.Close();
                reportWord.Quit();
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show("Закройте Word и повторите попытку", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
            //}
        }

        public _Document GetDoc(string path, Microsoft.Office.Interop.Word._Application word)
        {
            _Document oDoc = word.Documents.Add(path);
            SetTemplate(oDoc);
            return oDoc;
        }

        public void SetTemplate(WordReport._Document oDoc)
        {
            using (Context context = new Context())
            {
                Event events = new Event();
                var type = context.typeEvent.Where(x => Convert.ToBoolean(x.Id_type)).ToList();
                var name = context.@event.Where(y => y.Name_event != null).ToList();
                string[] date;

                int typeEventCount = 1;
                int eventCount = 0;
                foreach (TypeEvent e in type)
                {
                    int index = 1;
                    WordReport.Row row = oDoc.Tables[1].Rows.Add(oDoc.Tables[1].Rows[2 + typeEventCount + eventCount].Cells[1]);               
                    oDoc.Tables[1].Rows[2 + typeEventCount + eventCount].Cells[1].Merge(oDoc.Tables[1].Rows[2 + typeEventCount + eventCount].Cells[4]);
                    row.Range.Text = e.Name_type.ToString();
                    foreach (var item in name.Where(x => x.Type_id == typeEventCount))
                    {
                        date = item.Date_event.ToString().Split(" ");
                        eventCount++;
                        oDoc.Tables[1].Rows.Add(oDoc.Tables[1].Rows[2 + typeEventCount + eventCount].Cells[4]);
                        oDoc.Tables[1].Rows[2 + typeEventCount + eventCount].Cells[1].Range.Text = index.ToString();
                        oDoc.Tables[1].Rows[2 + typeEventCount + eventCount].Cells[2].Range.Text = date[0];
                        oDoc.Tables[1].Rows[2 + typeEventCount + eventCount].Cells[3].Range.Text = item.Name_event.ToString();
                        oDoc.Tables[1].Rows[2 + typeEventCount + eventCount].Cells[4].Range.Text = item.Description;
                        index++;
                    }
                    typeEventCount++;
                }
            }
            
        }

        private void EditEvent_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (Context context = new Context())
                {
                    Event @event = new Event();
                    @event = (Event)GridEvent.SelectedItem;
                    context.Update(@event);
                    context.SaveChanges();

                    MessageBox.Show("Успешно", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Выберите запись для изменения", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            RefreshGrid();
        }

        private void SearchTb_TextChanged(object sender, TextChangedEventArgs e)
        {
            using(Context context = new Context())
            {
                if (SearchTb.Text == "")
                {
                    StudentGrid.ItemsSource = context.student.ToList();
                }
                else
                {
                    var filter = context.student.Where(x => x.Fio_student.StartsWith(SearchTb.Text));
                    StudentGrid.ItemsSource = filter.ToList();
                }
                
            }            
        }

        private void ClearDbAttendabce_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (Context context = new Context())
                {
                    context.attendance.RemoveRange(context.attendance);
                    context.SaveChanges();
                }
                MessageBox.Show("Успешно!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            RefreshGrid();
        }

        private void ClearDbEvent_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (Context context = new Context())
                {
                    context.@event.RemoveRange(context.@event);
                    context.SaveChanges();
                }
                MessageBox.Show("Успешно!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            RefreshGrid();
        }
    }
}



