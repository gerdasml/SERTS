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
            string eventType =  "Individualus";
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

            var selectedEvent = eventsListBox.SelectedValue as Event;
            if (selectedEvent == null) // niekas nepasirinkta sarase, reiskia pridedinejam
            {
                _manager.AddEvent(ev);
                System.Windows.MessageBox.Show("Renginys sėkmingai pridėtas!");
            }
            else
            {
                _manager.UpdateEvent(selectedEvent.Number, ev);
                System.Windows.MessageBox.Show("Renginio informacija sėkmingai atnaujinta!");
            }
            eventsListBox.ItemsSource = _manager.GetEvents();


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

        private void eventsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedEvent = eventsListBox.SelectedValue as Event;
            if (selectedEvent == null)
                return; // ?
            addEventButton.Content = "Atnaujinti renginį";
            eventNameBox.Text = selectedEvent.Name;
            eventDescriptionBox.Text = selectedEvent.Description;
            eventSportBox.Text = selectedEvent.Sport;
            dateTimeBox.Value = selectedEvent.DateTime;
            ageLimitBox.Value = selectedEvent.AgeLimit;
            maxParticipantBox.Value = selectedEvent.ParticipantsAllowed;
            judgeBox.IsChecked = selectedEvent.Judge;
            sponsorBox.IsChecked = selectedEvent.Sponcor;
            guestBox.IsChecked = selectedEvent.Guest;

            registeredStudents.ItemsSource = _manager.GetParticipants(selectedEvent.Number);
        }

        private void deselectEventsButton_Click(object sender, RoutedEventArgs e)
        {
            eventsListBox.UnselectAll();
            registeredStudents.ItemsSource = null;
            addEventButton.Content = "Pridėti renginį";
            eventNameBox.Text = "";
            eventDescriptionBox.Text = "";
            eventSportBox.Text = "";
            dateTimeBox.Text = DateTime.Now.ToString();
            ageLimitBox.Value = 6;
            maxParticipantBox.Value = 0;
            judgeBox.IsChecked = false;
            sponsorBox.IsChecked = false;
            guestBox.IsChecked = false;
        }

        private void deleteEventButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedEvent = eventsListBox.SelectedValue as Event;
            if (selectedEvent == null)
                return;
            _manager.DeleteEvent(selectedEvent.Number);
            System.Windows.MessageBox.Show("Renginys sėkmingai ištrintas!");
            eventsListBox.ItemsSource = _manager.GetEvents();
        }

        private void unregisterStudentButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedEvent = eventsListBox.SelectedValue as Event;
            var selectedStudent = registeredStudents.SelectedValue as Student;
            if (selectedEvent == null || selectedStudent == null)
                return;
            _manager.DeleteParticipant(selectedStudent.Id, selectedEvent.Number);
            registeredStudents.ItemsSource = _manager.GetParticipants(selectedEvent.Number);
        }

        private void registerStudentButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedEvent = eventsListBox.SelectedValue as Event;
            if (selectedEvent == null)
                return;
            var w = new RegistrationIntoEvent(selectedEvent.Number, _manager);
            w.ShowDialog();
        }
    }
}
