namespace GoatAssociations.Helpers
{
    public interface IDialogService
    {
        void ShowMessage(string Message);
        void ShowError(string Message);
        void ShowWarning(string Message);
        void ShowAboutDialog();

    }
}
