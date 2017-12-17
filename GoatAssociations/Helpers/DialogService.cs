using System;

namespace GoatAssociations.Helpers
{
    class DialogService : IDialogService
    {
        public void ShowAboutDialog(object DataContext)
        {
            var About = new View.About
            {
                DataContext = DataContext
            };
            About.ShowDialog();
        }
       
        public bool ShowAssociationEditor(ViewModel.AssociationViewModel.GoatAssociationViewModel DataContext)
        {
            var Editor = new View.GoatAssociation
            {
                DataContext = DataContext
            };
            return (Editor.ShowDialog() == true);
        }

        public void ShowError(string Message)
        {
            throw new NotImplementedException();
        }

        public void ShowMessage(string Message)
        {
            throw new NotImplementedException();
        }

        public void ShowWarning(string Message)
        {
            throw new NotImplementedException();
        }
    }
}
