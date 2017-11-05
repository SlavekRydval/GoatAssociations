using GalaSoft.MvvmLight;
using GoatAssociations.Helpers;
using GoatAssociations.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        }

    }
}
