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
    }
}
