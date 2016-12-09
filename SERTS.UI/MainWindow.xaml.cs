using SERTS.Core;
using SERTS.Core.DB;
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
using Xceed.Wpf.Toolkit;

namespace SERTS.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DataManager _manager;

        public MainWindow()
        {
            InitializeComponent();
            dateTimeBox.Text = DateTime.Now.ToString();
            DOBBox.Text = DateTime.Now.ToString();
            _manager = new DataManager();
            var events = _manager.GetEvents();
            eventsListBox.ItemsSource = events;
            var students = _manager.GetStudents();
            studentsListBox.ItemsSource = students;
        }

        private void addEventButton_Click(object sender, RoutedEventArgs e)
        {
            string name = eventNameBox.Text;
            string description = eventDescriptionBox.Text;
            string eventType = eventTypeIndividual.IsChecked.Value ? "Individualus" : "Komandinis";
            string sport = eventSportBox.Text;
            DateTime dateTime = DateTime.Parse(dateTimeBox.Text);
            short ageLimit = (short)ageLimitBox.Value.Value;
            short maxParticipant = (short)maxParticipantBox.Value.Value;
            bool guest = guestBox.IsChecked.Value;
            bool judge = judgeBox.IsChecked.Value;
            bool sponsor = sponsorBox.IsChecked.Value;

            var ev = new Event
            {
                Name = name,
                Description = description,
                EventType = eventType,
                Sport = sport,
                DateTime = dateTime,
                AgeLimit = ageLimit,
                ParticipantsAllowed = maxParticipant,
                Guest = guest,
                Judge = judge,
                Sponcor = sponsor,
                Latitude = 0,
                Longitude = 0
            };

            _manager.AddEvent(ev);
            System.Windows.MessageBox.Show("Renginys sėkmingai pridėtas!");



        }

        private void addStudentButton_Click(object sender, RoutedEventArgs e)
        {
            string name = studentNameBox.Text;
            string surname = studentSurnameBox.Text;
            string email = emailBox.Text;
            DateTime dob = DateTime.Parse(DOBBox.Text);
            string phone = phoneNumberBox.Text;
            string studentClass = classBox.Text;
            string school = schoolBox.Text;
            bool guest = studentGuestBox.IsChecked.Value;

            var st = new Student
            {
                Name = name,
                Surname = surname,
                Email = email,
                DOB = dob,
                PhoneNumber = phone,
                Class = studentClass,
                School = school,
                Guest = guest
            };

            var selectedStudent = studentsListBox.SelectedValue as Student;
            if (selectedStudent == null) // niekas nepasirinkta sarase, reiskia pridedinejam
            {
                _manager.AddStudent(st);
                System.Windows.MessageBox.Show("Mokinys sėkmingai pridėtas!");
            }
            else
            {
                _manager.UpdateStudent(selectedStudent.Id, st);
                System.Windows.MessageBox.Show("Mokinio informacija sėkmingai atnaujinta!");
            }
            studentsListBox.ItemsSource = _manager.GetStudents();
        }

        private void studentsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedStudent = studentsListBox.SelectedValue as Student;
            if (selectedStudent == null)
                return; // ?
            addStudentButton.Content = "Atnaujinti mokinį";
            studentNameBox.Text = selectedStudent.Name;
            studentSurnameBox.Text = selectedStudent.Surname;
            emailBox.Text = selectedStudent.Email;
            phoneNumberBox.Text = selectedStudent.PhoneNumber;
            DOBBox.Value = selectedStudent.DOB;
            classBox.Text = selectedStudent.Class;
            schoolBox.Text = selectedStudent.School;
            guestBox.IsChecked = selectedStudent.Guest;
        }

        private void deselectAllStudentts_Click(object sender, RoutedEventArgs e)
        {
            studentsListBox.UnselectAll();
            addStudentButton.Content = "Pridėti mokinį";
            studentNameBox.Text = "";
            emailBox.Text = "";
            studentSurnameBox.Text = "";
            phoneNumberBox.Text = "";
            DOBBox.Text = DateTime.Now.ToString();
            classBox.Text = "";
            schoolBox.Text = "";
            guestBox.IsChecked = false;
        }

        private void deleteStudentButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedStudent = studentsListBox.SelectedValue as Student;
            if (selectedStudent == null)
                return;
            _manager.DeleteStudent(selectedStudent.Id);
            System.Windows.MessageBox.Show("Mokinys sėkmingai ištrintas!");
            studentsListBox.ItemsSource = _manager.GetStudents();
        }
    }
}
