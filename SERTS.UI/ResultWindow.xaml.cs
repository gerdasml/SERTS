using Aspose.OCR;
using Emgu.CV;
using Emgu.CV.UI;
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
using System.Windows.Threading;

namespace SERTS.UI
{
    /// <summary>
    /// Interaction logic for ResultWindow.xaml
    /// </summary>
    public partial class ResultWindow : Window
    {
        public event EventHandler<short> OnResultAdded;
        public ResultWindow(short currentResult)
        {
            InitializeComponent();
            resultBox.Value = currentResult;
        }

        private void changeResultOkButton_Click(object sender, RoutedEventArgs e)
        {
            var result = resultBox.Value;
            if (result == null)
                result = 0;
            OnResultAdded?.Invoke(this, result.Value);
            this.Close();
        }

        private void changeResultCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void importImage_Click(object sender, RoutedEventArgs e)
        {
            // Create OpenFileDialog 
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            // Set filter for file extension and default file extension 
            dlg.DefaultExt = ".png";
            dlg.Filter = "JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg|GIF Files (*.gif)|*.gif";
            // Display OpenFileDialog by calling ShowDialog method 
            bool? result = dlg.ShowDialog();
            // Get the selected file name and display in a TextBox 
            if (result == true)
            {
                // Open document 
                string filename = dlg.FileName;
                var eng = new OcrEngine();
                eng.Image = ImageStream.FromFile(filename);
                short number;
                if (eng.Process() && short.TryParse(eng.Text.ToString(), out number))
                {
                    //short number = short.Parse(eng.Text.ToString());
                    resultBox.Value = number;
                }
                else
                {
                    MessageBox.Show("Nepavyko nuskaityti", "", MessageBoxButton.OK, MessageBoxImage.Error);
                    //MessageBox.Show("Nepavyko nuskaityti");
                }

            }
        }

        private void camera_Click(object sender, RoutedEventArgs e)
        {
            ImageViewer viewer = new ImageViewer(); //create an image viewer
            Capture capture = new Capture(); //create a camera captue
            
            System.Windows.Forms.Application.Idle += new EventHandler(delegate (object s, EventArgs ee)
            {  //run this until application closed (close button click on image viewer)
                viewer.Image = capture.QueryFrame(); //draw the image obtained from camera

            });

            viewer.ShowDialog();
            return;
        }
    }
}
