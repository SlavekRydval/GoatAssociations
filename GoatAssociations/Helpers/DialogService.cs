using System;

namespace GoatAssociations.Helpers
{
    class DialogService : IDialogService
    {
        public void ShowAboutDialog(object DataContext)
        {
            var About = new View.About
            {
                DataContext = this
            };
            About.ShowDialog();
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
