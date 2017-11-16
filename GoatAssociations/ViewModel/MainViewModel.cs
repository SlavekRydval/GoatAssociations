using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GoatAssociations.Helpers;
using GoatAssociations.Model;

namespace GoatAssociations.ViewModel
{
    class MainViewModel : ViewModelBase
    {
        private readonly GoatAssociationModel goatAssociation;
        private readonly IGoatAssociationService associationService;
        private readonly IDialogService dialogService;

        public MainViewModel(): 
            this (
                    (IsInDesignModeStatic ? (IGoatAssociationService)new Design.DesignAssociationService() : 
                                            new GoatAssociationService()), 
                    new DialogService ())
        {
#if DEBUG
            if (IsInDesignMode)
                goatAssociation = associationService.Read();
#endif
        }


        public MainViewModel(IGoatAssociationService AssociationService,
                             IDialogService DialogService)
        {
            associationService = AssociationService;
            dialogService = DialogService;

            AboutCommand = new RelayCommand(() => dialogService.ShowAboutDialog(new AddinInformationModel()));
        }


        /// <summary>
        /// Shows About Information
        /// </summary>
        public RelayCommand AboutCommand
        {
            get;
            private set;
        }


        public RelayCommand EditAssociationPropertiesCommand ;


    }
}
