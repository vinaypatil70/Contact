using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
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
using Microsoft.Win32;

namespace Contact
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<Models.Contact> lstContacts = new List<Models.Contact>();
        string filePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Contacts.json";

        public MainWindow()
        {
            InitializeComponent();
            LoadCountries();
        }   

        private async void LoadCountries()
        {
            List<Models.Country> countries = new List<Models.Country>();
            HttpClient objClient = new HttpClient();
            HttpResponseMessage response = await objClient.GetAsync("https://restcountries.eu/rest/v2/all");
            if(response.StatusCode == HttpStatusCode.OK)
            {
                string result = await response.Content.ReadAsStringAsync();
                cmbCountryCode.ItemsSource = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Models.Country>>(result);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Select a picture";
            op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
              "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
              "Portable Network Graphic (*.png)|*.png";
            if (op.ShowDialog() == true)
            {
                imgPhoto.Source = new BitmapImage(new Uri(op.FileName));
            }
        }

        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            int age = DateTime.Now.Year - dateDOB.SelectedDate.Value.Year;
            if (DateTime.Now.DayOfYear < dateDOB.SelectedDate.Value.DayOfYear)
                age = age - 1;

            txtAge.Content = age.ToString();
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            txtPermanantAddress.Text = txtCurrentAddress.Text;
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            txtPermanantAddress.Text = string.Empty;
        }

        private void BtnSubmit_Click(object sender, RoutedEventArgs e)
        {
            Models.Contact objContact = new Models.Contact();
            objContact.FirstName = txtFirstName.Text;
            objContact.MiddleName = txtMiddleName.Text;
            objContact.LastName = txtLastName.Text;
            objContact.Gender = rdbMale.IsChecked == true ? "Male" : "Female";
            objContact.PhoneNumber = txtPhoneNumber.Text;
            objContact.MaritalStatus = ((ContentControl)cmbMaritalStatus.SelectedItem).Content.ToString();
            objContact.CurrentAddress = txtCurrentAddress.Text;
            objContact.PermanantAddress = txtPermanantAddress.Text;
            objContact.DOB = dateDOB.SelectedDate.Value;
            objContact.PhotoUri = imgPhoto.Source.ToString();
            objContact.Height = Convert.ToDouble(String.Format("{0:0.00}", sldHeight.Value));
            lstContacts.Add(objContact);

            string json = Newtonsoft.Json.JsonConvert.SerializeObject(lstContacts);
            System.IO.File.WriteAllText(filePath, json);
            MessageBoxResult result = MessageBox.Show("Details Saved Sucessfully.", "Save", MessageBoxButton.OK, MessageBoxImage.Information);
            if (result == MessageBoxResult.OK)
            {
                ShowContactDetails();
                reset();
            }
        }

        private void ShowContactDetails()
        {
            tabAddContact.IsSelected = false;
            tabMyContacts.IsSelected = true;
        }

        private void BtnReset_Click(object sender, RoutedEventArgs e)
        {
            reset();
        }

        private void reset()
        {
            txtFirstName.Text = string.Empty;
            txtLastName.Text = string.Empty;
            txtMiddleName.Text = string.Empty;
            txtCurrentAddress.Text = string.Empty;
            txtPermanantAddress.Text = string.Empty;
            imgPhoto.Source = null;
            rdbMale.IsChecked = true;
            dateDOB.SelectedDate = new DateTime();
        }

        private void TabContacts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (System.IO.File.Exists(filePath))
            {
                string json = System.IO.File.ReadAllText(filePath);
                listContacts.ItemsSource = grdContacts.ItemsSource = lstContacts = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Models.Contact>>(json);
            }
        }
    }
}
