using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GoatAssociations.Model;

namespace GoatAssociations.ViewModel
{
    public class GoatAssociationViewModel: ViewModelBase
    {
        GoatAssociationModel goatAssociationModel = null;

        public GoatAssociationViewModel(GoatAssociationModel goatAssociationModel)
        {
            this.goatAssociationModel = goatAssociationModel;
        }




        private RelayCommand<GoatAssociationEndModel> _SetRoleNameCommand;
        public RelayCommand<GoatAssociationEndModel> SetRoleNameCommand
        {
            get
            {
                if (_SetRoleNameCommand == null)
                    _SetRoleNameCommand = new RelayCommand<GoatAssociationEndModel>(
                        (ae) => { AdjustRoleName(ae); },
                        (ae) => { return true;  /*(ae != null);*/ } ///JTS, TODO: fix it.
                        );
                return _SetRoleNameCommand;
            }
        }


        private void AdjustRoleName(Model.GoatAssociationEndModel GoatAssociationEnd)
        {
            GoatAssociationEnd.Role = GoatAssociationEnd.MemberEnd;
        }




    }
}
