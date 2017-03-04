using System.Windows;

namespace GoatAssociations.View
{
    /// <summary>
    /// Interaction logic for GoatAssociation.xaml
    /// </summary>
    public partial class GoatAssociation : Window
    {
        public GoatAssociation()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //it is against the MVVM rules!!!
            ///TODO: FIX IT.
            DialogResult = true;
        }
    }
}
