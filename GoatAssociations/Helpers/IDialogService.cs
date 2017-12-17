namespace GoatAssociations.Helpers
{
    interface IDialogService
    {
        void ShowMessage(string Message);

        /// <summary>
        /// Show association dialog
        /// </summary>
        /// <param name="DataContext">Data that will be updated</param>
        /// <returns>returns true if user click on OK button, false if user closes dialog in any other way</returns>
        bool ShowAssociationEditor(ViewModel.AssociationViewModel.GoatAssociationViewModel DataContext);

        void ShowError(string Message);
        void ShowWarning(string Message);
        void ShowAboutDialog(object DataContext);
    }
}
