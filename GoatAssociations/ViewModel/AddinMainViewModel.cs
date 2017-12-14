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

            AboutCommand = new GalaSoft.MvvmLight.Command.RelayCommand(() => DialogService.ShowAboutDialog(new Model.GoatAddinInformationModel()));

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


        private ViewModel.GoatAssociation _goatAssociation; 
        public ViewModel.GoatAssociation Association
        {
            get => _goatAssociation;
            set => Set(nameof(Association), ref _goatAssociation, value);
        }


        private ViewModel.GoatAssociation goatAssociation = null;
        public ViewModel.GoatAssociation GoatAssociation
        {
            get => goatAssociation;
        }


        #region commands definition
        //////START OBSOLETE
        private RelayCommand<Model.GoatAssociationEndModel> _SetRoleNameCommand;
        public RelayCommand<Model.GoatAssociationEndModel> SetRoleNameCommand
        {
            get
            {
                if (_SetRoleNameCommand == null)
                    _SetRoleNameCommand = new RelayCommand<Model.GoatAssociationEndModel>(
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
            goatAssociation = new ViewModel.GoatAssociation(new Model.GoatAssociationModel(), EAConnector, Repository);
            try
            {
                View.GoatAssociation dlg = new View.GoatAssociation();
                dlg.DataContext = this;
                command.Result = (dlg.ShowDialog() == true);
                if (command.Result)
                    goatAssociation.UpdateConnector();
            }
            finally
            {
                goatAssociation = null;
            }
        }


        //////START OBSOLETE
        private void AdjustRoleName(Model.GoatAssociationEndModel GoatAssociationEnd)
        {
            GoatAssociationEnd.Role = GoatAssociationEnd.MemberEnd;
        }
        ////////END OBSOLETE
        #endregion

    }
}
