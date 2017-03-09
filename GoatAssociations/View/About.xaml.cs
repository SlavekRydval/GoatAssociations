using System.Diagnostics;
using System.Windows;
using System.Windows.Documents;

namespace GoatAssociations.View
{
    /// <summary>
    /// Interaction logic for About.xaml
    /// </summary>
    public partial class About : Window
    {
        public About()
        {
            InitializeComponent();
        }

        //using click brokes MVVM rules as well as direct values of text in the 
        //about dialog. So far, it is acceptable for me
        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(((Hyperlink)sender).NavigateUri.ToString());
        }
    }
}
