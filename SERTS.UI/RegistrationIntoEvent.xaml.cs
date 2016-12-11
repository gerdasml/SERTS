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
using System.Windows.Shapes;

namespace SERTS.UI
{
    /// <summary>
    /// Interaction logic for RegistrationIntoEvent.xaml
    /// </summary>
    public partial class RegistrationIntoEvent : Window
    {
        public event EventHandler<Student> OnStudentAdded;

        public RegistrationIntoEvent(int eventId, IDataManager manager)
        {
            InitializeComponent();
            var registeredStudents = manager.GetParticipants(eventId);
            var notRegisteredStudents = manager.GetStudents().Where(x => registeredStudents.Where(y => y.Id == x.Id).Count() == 0);
            studentsListBox.ItemsSource = notRegisteredStudents;
        }

        private void cancelAddParticipant_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void addParticipant_Click(object sender, RoutedEventArgs e)
        {
            var student = studentsListBox.SelectedItem as Student;
            if (student == null)
                return;
            OnStudentAdded?.Invoke(this, student);
            this.Close();
        }
    }
}
