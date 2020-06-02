using ModelsApp.Employee;
using ModelsApp.Result;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfClientEmployee
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            FiltrEmployee filterEmpl = new FiltrEmployee();
            filterEmpl.Surname = txtFilterSurname.Text;
            filterEmpl.Name = txtFilterName.Text;
            filterEmpl.Middle_name = txtFilterMiddleName.Text;

            Logic.HttpClientHR httpClientHR = new Logic.HttpClientHR();
            List<Employee> result = await httpClientHR.getListEmployeeAsync(filterEmpl);
            dgEmployee.ItemsSource = result;
        }

        private async void btnModifyEmployee_Click(object sender, RoutedEventArgs e)
        {
            Logic.HttpClientHR httpClientHR = new Logic.HttpClientHR();
            ResultApi resultCreatedEmployee = new ResultApi();
            string txtEmplyeeId = string.Empty;

            Employee newEmpl = new Employee();
            txtEmplyeeId = txtemployee_id.Text;
            if (txtEmplyeeId == "") txtEmplyeeId = "0";
            newEmpl.employee_id = Convert.ToInt32(txtEmplyeeId);
            newEmpl.name = txtName.Text;
            newEmpl.surname = txtSurname.Text;
            newEmpl.middle_name = txtMiddleName.Text;
            newEmpl.birthday = Convert.ToDateTime(txtDateBidthDay.SelectedDate);

            if (newEmpl.employee_id > 0)
            {
                resultCreatedEmployee = await httpClientHR.changedEmployeeAsync(newEmpl);

                if (resultCreatedEmployee.code == 0)
                {
                    MessageBox.Show("Сохранено");
                    tabListEmployee.IsSelected = true;
                    tabEmployee.Visibility = Visibility.Hidden;
                }
                else
                {
                    MessageBox.Show("Ошибка при сохранении изменений. \n" + resultCreatedEmployee.info);
                }
            }
            else
            {                
                resultCreatedEmployee = await httpClientHR.createNewEmployeeAsunc(newEmpl);

                if (resultCreatedEmployee.code == 0 && resultCreatedEmployee.employee.employee_id > 0)
                {
                    txtemployee_id.Text = resultCreatedEmployee.employee.employee_id.ToString();
                    MessageBox.Show("Сохранено. ");
                    tabListEmployee.IsSelected = true;
                    tabEmployee.Visibility = Visibility.Hidden;                    
                }
                else 
                {                    
                    MessageBox.Show("Ошибка создания сотрудника. \n" + resultCreatedEmployee.info);
                }
            }
        }

        private void btnCancelModifyEmployee_Click(object sender, RoutedEventArgs e)
        {            
            tabListEmployee.IsSelected = true;
            tabEmployee.Visibility = Visibility.Hidden;
        }

        private void btnAddNewEmployee_Click(object sender, RoutedEventArgs e)
        {
            tabEmployee.Visibility = Visibility.Visible;
            tabEmployee.IsSelected = true;

            txtemployee_id.Text = "";
            txtSurname.Text = "";
            txtName.Text = "";
            txtMiddleName.Text = "";
            txtDateBidthDay.SelectedDate = Convert.ToDateTime("01/01/1900");
                
        }

        private void btnModifyEmployee_Click_1(object sender, RoutedEventArgs e)
        {
            int indexRow = dgEmployee.SelectedIndex;
            int employee_id;

            if (indexRow >= 0 && indexRow < dgEmployee.Items.Count - 1)
            {                
                var empl = (Employee)dgEmployee.SelectedItem;
                employee_id = empl.employee_id;
                this.showDetailEmployee(employee_id);
                tabEmployee.Visibility = Visibility.Visible;
            }
            else
            {
                MessageBox.Show("Не выбрана строка сотрудника.");
            }
            
        }

        private async void showDetailEmployee(int employee_id)
        {
            Logic.HttpClientHR httpClient = new Logic.HttpClientHR();
            tabEmployee.IsSelected = true;
            var empl = await httpClient.getEmployeeAsync(employee_id);

            txtemployee_id.Text = empl.employee_id.ToString();
            txtSurname.Text = empl.surname;
            txtName.Text = empl.name;
            txtMiddleName.Text = empl.middle_name;
            txtDateBidthDay.SelectedDate = empl.birthday;
        }

        private async void btnDeleteEmployee_Click(object sender, RoutedEventArgs e)
        {
            int indexRow = dgEmployee.SelectedIndex;
            Logic.HttpClientHR httpClient = new Logic.HttpClientHR();
            int employee_id;

            if (indexRow >= 0 && indexRow < dgEmployee.Items.Count - 1)
            {
                var empl = (Employee)dgEmployee.SelectedItem;
                employee_id = empl.employee_id;
                Result result = await httpClient.deleteEmployeeAsync(employee_id);

                if (result.code == 0) MessageBox.Show("Удалено");
                else MessageBox.Show("ОШибка удаления. \n" + result.info);
                
            }
            else
            {
                MessageBox.Show("Не выбрана строка сотрудника для удаления.");
            }
        }
    }
}
