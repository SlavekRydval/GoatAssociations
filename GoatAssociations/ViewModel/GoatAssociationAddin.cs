using GoatAssociations.Commands;

namespace GoatAssociations.ViewModel
{
    class GoatAssociationAddin: Model.NotifyPropertyClass
    {
        public Model.AddinInformation AddinInformation { get; } = new Model.AddinInformation();

        private ViewModel.GoatAssociation _goatAssociation; 
        public ViewModel.GoatAssociation Association
        {
            get
            {
                return _goatAssociation;
            }
            set
            {
                _goatAssociation = value;
                this.OnPropertyChanged(nameof (Association));
            }
        }


        private ViewModel.GoatAssociation goatAssociation = null;
        public ViewModel.GoatAssociation GoatAssociation
        {
            get { return goatAssociation;  }
        }


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

        private RelayCommand<Model.GoatAssociationEnd> _SetRoleNameCommand;
        public RelayCommand<Model.GoatAssociationEnd> SetRoleNameCommand
        {
            get
            {
                if (_SetRoleNameCommand == null)
                    _SetRoleNameCommand = new RelayCommand<Model.GoatAssociationEnd>(
                        (ae) => { AdjustRoleName(ae);  },
                        (ae) => { return true /*(o != null)*/; }
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
            goatAssociation = new ViewModel.GoatAssociation(new Model.GoatAssociation(), conn);
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

        private void AdjustRoleName(Model.GoatAssociationEnd GoatAssociationEnd)
        {
            GoatAssociationEnd.Role = "ahoj j8 jsem tonda a nic neum9m";
        }
        #endregion

    }
}
