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

        /// <summary>
        /// Usualy is used by Designer (Visual Studio or Blend for Visual Studio). In code 
        /// is more useful using of constructor with parameters.
        /// </summary>
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

        /// <summary>
        /// Creates main ViewModel for Sparx EA addin GoatAssociation.
        /// </summary>
        /// <param name="AssociationService">Service responsible for reading and saving association properties</param>
        /// <param name="DialogService">Service responsible for dialog maintenance</param>
        public MainViewModel(IGoatAssociationService AssociationService,
                             IDialogService DialogService)
        {
            associationService = AssociationService;
            dialogService = DialogService;
        }

    }
}
