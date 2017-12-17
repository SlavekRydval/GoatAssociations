using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GoatAssociations.Commands;
using GoatAssociations.Helpers;

namespace GoatAssociations.ViewModel
{
    /// <summary>
    /// Entry gate to the addin
    /// </summary>
    class AddinMainViewModel: ViewModelBase
    {
        //attributes
        private IDialogService DialogService;

        //constructors
        public AddinMainViewModel(EA.Repository Repository, IDialogService DialogService)
        {
            this.Repository = Repository;
            this.DialogService = DialogService;

            AboutCommand = new GalaSoft.MvvmLight.Command.RelayCommand(() => DialogService.ShowAboutDialog(new Model.MetadataModel.GoatAddinInformationModel()));

            EditAssociationCommand = new RelayCommandWithResult<EA.Connector, bool>(
                (connector) => { EditAssociation(connector, EditAssociationCommand); },
                (connector) => { return (connector.MetaType == "Association" || connector.MetaType == "Aggregation"); }
            );
        }

        //properties
        public EA.Repository Repository { get; set; } = null;

        #region Commands definition
        /// <summary>
        /// Shows About Information
        /// </summary>
        public GalaSoft.MvvmLight.Command.RelayCommand AboutCommand
        {
            get;
        }

        /// <summary>
        /// Lets edit a connection
        /// </summary>
        public RelayCommandWithResult<EA.Connector,bool> EditAssociationCommand
        {
            get;
        }
        #endregion



        //--------up to here is refactoring done-----

        #region commands definition
        //////START OBSOLETE
        private RelayCommand<Model.AssociationModel.GoatAssociationEndModel> _SetRoleNameCommand;
        public RelayCommand<Model.AssociationModel.GoatAssociationEndModel> SetRoleNameCommand
        {
            get
            {
                if (_SetRoleNameCommand == null)
                    _SetRoleNameCommand = new RelayCommand<Model.AssociationModel.GoatAssociationEndModel>(
                        (ae) => { AdjustRoleName(ae); },
                        (ae) => { return true;  /*(ae != null);*/ } ///JTS, TODO: fix it.
                        );
                return _SetRoleNameCommand;
            }
        }
        ////////END OBSOLETE
        #endregion

        #region commands execution
        private void EditAssociation(EA.Connector EAConnector, RelayCommandWithResult<EA.Connector, bool> command)
        {
            //MVVM code
            //step 1: create new viewmodel
            //step 2: prepare data
            //step 3: call a command for editing an association

            var x = new AssociationViewModel.GoatAssociationViewModel(new Model.AssociationModel.GoatAssociationService(EAConnector, Repository), DialogService);
            x.EditAssociationCommand.Execute(this);
            command.Result = x.EditAssociationCommand.Result;
            return;
        }


        //////START OBSOLETE
        private void AdjustRoleName(Model.AssociationModel.GoatAssociationEndModel GoatAssociationEnd)
        {
            GoatAssociationEnd.Role = GoatAssociationEnd.MemberEnd;
        }
        ////////END OBSOLETE
        #endregion

    }
}
