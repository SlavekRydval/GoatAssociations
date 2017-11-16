using GalaSoft.MvvmLight;
using GoatAssociations.Commands;

namespace GoatAssociations.ViewModel
{
    class GoatAssociationAddin: ViewModelBase
    {

        private ViewModel.GoatAssociation _goatAssociation; 
        public ViewModel.GoatAssociation Association
        {
            get => _goatAssociation;
            set
            {
                Set(nameof (Association), ref _goatAssociation, value);
                //_goatAssociation = value;
                //this.OnPropertyChanged(nameof (Association));
            }
        }


        private ViewModel.GoatAssociation goatAssociation = null;
        public ViewModel.GoatAssociation GoatAssociation
        {
            get => goatAssociation;
        }

        public EA.Repository Repository { get; set; } = null;



        #region commands definition

        private RelayCommandWithResult<EA.Connector, bool> _EditAssociationCommand;
        public RelayCommandWithResult<EA.Connector, bool> EditAssociationCommand
        {
            get
            {
                if (_EditAssociationCommand == null)
                    _EditAssociationCommand = new RelayCommandWithResult<EA.Connector, bool>(
                        (conn) => { EditAssociation(conn, EditAssociationCommand); }, //Execute
                        (conn) => { return (conn.MetaType == "Association" || conn.MetaType == "Aggregation"); } //CanExecute
                    );

                return _EditAssociationCommand;
            }
        }

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


        private void EditAssociation(EA.Connector conn, RelayCommandWithResult<EA.Connector, bool> command)
        {
            goatAssociation = new ViewModel.GoatAssociation(new Model.GoatAssociationModel(), conn, Repository);
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
