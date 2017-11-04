using GalaSoft.MvvmLight;
using GoatAssociations.Commands;

namespace GoatAssociations.ViewModel
{
    class GoatAssociationAddin: ViewModelBase
    {
        public Model.AddinInformation AddinInformation { get; } = new Model.AddinInformation();

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
        private RelayCommand _AboutCommand;
        public RelayCommand AboutCommand
        {
            get
            {
                if (_AboutCommand == null)
                    _AboutCommand = new RelayCommand(
                        () => { About(); }, //Execute
                        () => { return true; } //CanExecute
                    );

                return _AboutCommand;
            }
        }

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
        #endregion

        #region commands execution

        private void About()
        {
            var About = new View.About();
            About.DataContext = this;
            About.ShowDialog();
        }

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

        private void AdjustRoleName(Model.GoatAssociationEndModel GoatAssociationEnd)
        {
            GoatAssociationEnd.Role = GoatAssociationEnd.MemberEnd;
        }
        #endregion

    }
}
