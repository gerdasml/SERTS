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

            schoolBox.ItemsSource = _manager.GetStudents().Select(x => x.School).Distinct();
            FillResults();
        }

        private void FillResults()
        {
            int index = 0;
            var studentCol = new DataGridTextColumn()
            {
                Header = "Mokinys",
                Binding = new Binding(string.Format("[{0}]", index++))
            };
            resultsDataGrid.Columns.Clear();
            resultsDataGrid.Items.Clear();

            resultsDataGrid.Columns.Add(studentCol);
            var allEvents = _manager.GetEvents();
            foreach(var e in allEvents)
            {
                var eventCol = new DataGridTextColumn()
                {
                    Header = e.Name,
                    Binding = new Binding(string.Format("[{0}]", index++))
                };
                resultsDataGrid.Columns.Add(eventCol);
            }
            var totalCol = new DataGridTextColumn()
            {
                Header = "Iš viso",
                Binding = new Binding(string.Format("[{0}]", index++))
            };
            resultsDataGrid.Columns.Add(totalCol);

            var allResults = _manager.GetAllResults();
            var allStudents = _manager.GetStudents().ToList();
            foreach (var studentId in allResults.Keys)
            {
                var item = new List<object>();
                item.Add(allStudents.Where(x=>x.Id == studentId).Select(x=> x.Name + " " +x.Surname).FirstOrDefault());
                foreach(var e in allEvents)
                {
                    item.Add(allResults[studentId][e.Number]);
                }
                item.Add(_manager.GetTotalScore(studentId));
                resultsDataGrid.Items.Add(item);
            }
        }

        private void addEventButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                string name = eventNameBox.Text;
                string description = eventDescriptionBox.Text;
                string eventType =  "Individualus";
                string sport = eventSportBox.SelectedValue.ToString();
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
            catch
            {
                System.Windows.MessageBox.Show("Neteisingai ivesti duomenys", "", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void addStudentButton_Click(object sender, RoutedEventArgs e)
        {
            try
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


                schoolBox.ItemsSource = _manager.GetStudents().Select(x => x.School).Distinct();
            }
            catch
            {
                System.Windows.MessageBox.Show("Neteisingai ivesti duomenys", "", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void studentsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedStudent = studentsListBox.SelectedValue as Student;
            if (selectedStudent == null)
            {
               // System.Windows.MessageBox.Show("Nepasirinktas mokinys", "", MessageBoxButton.OK, MessageBoxImage.Error);
                return; // ?
            }
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
            {
                System.Windows.MessageBox.Show("Nepasirinktas mokinys", "", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            _manager.DeleteStudent(selectedStudent.Id);
            System.Windows.MessageBox.Show("Mokinys sėkmingai ištrintas!");
            studentsListBox.ItemsSource = _manager.GetStudents();


            schoolBox.ItemsSource = _manager.GetStudents().Select(x => x.School).Distinct();
        }

        private void eventsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedEvent = eventsListBox.SelectedValue as Event;
            if (selectedEvent == null)
            {
                System.Windows.MessageBox.Show("Nepasirinktas renginys", "", MessageBoxButton.OK, MessageBoxImage.Error);
                return; // ?
            }
            addEventButton.Content = "Atnaujinti renginį";
            eventNameBox.Text = selectedEvent.Name;
            eventDescriptionBox.Text = selectedEvent.Description;
            eventSportBox.SelectedValue = selectedEvent.Sport;
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
            {
                System.Windows.MessageBox.Show("Nepasirinktas renginys", "", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            _manager.DeleteEvent(selectedEvent.Number);
            System.Windows.MessageBox.Show("Renginys sėkmingai ištrintas!");
            eventsListBox.ItemsSource = _manager.GetEvents();
        }

        private void unregisterStudentButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedEvent = eventsListBox.SelectedValue as Event;
            var selectedStudent = registeredStudents.SelectedValue as Student;
            if (selectedEvent == null || selectedStudent == null)
            {
                System.Windows.MessageBox.Show("Nepasirinktas renginys arba mokinys", "", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            _manager.DeleteParticipant(selectedStudent.Id, selectedEvent.Number);
            registeredStudents.ItemsSource = _manager.GetParticipants(selectedEvent.Number);
        }

        private void registerStudentButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedEvent = eventsListBox.SelectedValue as Event;
            if (selectedEvent == null)
            {
                System.Windows.MessageBox.Show("Nepasirinktas renginys", "", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            var w = new RegistrationIntoEvent(selectedEvent.Number, _manager);
            w.OnStudentAdded += (s, selectedStudent) =>
             {
                 _manager.RegisterPerson(new Participant { StudentId = selectedStudent.Id, EventNumber = selectedEvent.Number });
                 registeredStudents.ItemsSource = _manager.GetParticipants(selectedEvent.Number);
             };
            w.ShowDialog();
        }

        private void pointsButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedEvent = eventsListBox.SelectedValue as Event;
            var selectedStudent = registeredStudents.SelectedValue as Student;
            if (selectedEvent == null || selectedStudent == null)
            {
                System.Windows.MessageBox.Show("Nepasirinktas mokinys arba renginys", "", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            var currentResult = _manager.GetResult(selectedEvent.Number, selectedStudent.Id);
            var rw = new ResultWindow(currentResult);
            rw.OnResultAdded += (s, newResult) =>
            {
                if (currentResult == 0 && newResult == 0)
                    return;
                if (currentResult == 0 && newResult > 0)
                    _manager.AddResult(new Result { StudentId = selectedStudent.Id, EventNumber = selectedEvent.Number, Points = newResult });
                else if (currentResult > 0 && newResult == 0)
                    _manager.DeleteResult(selectedStudent.Id, selectedEvent.Number);
                else
                    _manager.UpdateResult(selectedStudent.Id, selectedEvent.Number, newResult);
                FillResults();
            };
            rw.ShowDialog();

        }
    }
}
