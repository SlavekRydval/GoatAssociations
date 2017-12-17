using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GoatAssociations.Commands;
using GoatAssociations.Helpers;
using GoatAssociations.Model.AssociationModel;
using System;

namespace GoatAssociations.ViewModel.AssociationViewModel
{
    enum MultiplicityType { None, ZeroToOne, ZeroToMany, One, OneToMany, Many, Other }

    class GoatAssociationViewModel : ViewModelBase
    {
        private readonly IGoatAssociationService associationService;
        private readonly IDialogService dialogService;

        public GoatAssociationModel GoatAssociation { get; private set; }
        public GoatAssociationEndModel Left { get { return GoatAssociation.Left; } }
        public GoatAssociationEndModel Right { get { return GoatAssociation.Right; } }

        /// <summary>
        /// Usualy is used by Designer (Visual Studio or Blend for Visual Studio). In code 
        /// is more useful using of constructor with parameters.
        /// </summary>
        public GoatAssociationViewModel() :
            this(
                    AssociationService: (IsInDesignModeStatic ? (IGoatAssociationService)new Design.DesignAssociationService() :
                                            new GoatAssociationService(null, null)),
                    DialogService: new DialogService())
        {
            #if DEBUG
            if (IsInDesignMode)
            {
                GoatAssociation = associationService.Read();
            }
            #endif
        }

        /// <summary>
        /// Creates main ViewModel for Sparx EA addin GoatAssociation.
        /// </summary>
        /// <param name="AssociationService">Service responsible for reading and saving association properties</param>
        /// <param name="DialogService">Service responsible for dialog maintenance</param>
        public GoatAssociationViewModel(IGoatAssociationService AssociationService,
                             IDialogService DialogService)
        {
            associationService = AssociationService;
            dialogService = DialogService;
        }



        #region Commands definition
        /// <summary>
        /// Edit an association
        /// </summary>
        private RelayCommandWithResult<bool> _EditAssociationCommand;
        public RelayCommandWithResult<bool> EditAssociationCommand
        {
            get
            {
                if (_EditAssociationCommand == null)
                    _EditAssociationCommand = new RelayCommandWithResult<bool>(() => 
                    {
                        GoatAssociation = associationService.Read();

                        GoatAssociation.Left.PropertyChanged += RightLeft_PropertyChanged;
                        RightLeft_PropertyChanged(GoatAssociation.Left, new System.ComponentModel.PropertyChangedEventArgs(nameof(Left.Multiplicity)));

                        GoatAssociation.Right.PropertyChanged += RightLeft_PropertyChanged;
                        RightLeft_PropertyChanged(GoatAssociation.Right, new System.ComponentModel.PropertyChangedEventArgs(nameof(Right.Multiplicity)));

                        EditAssociationCommand.Result = dialogService.ShowAssociationEditor(this);

                        if (EditAssociationCommand.Result)
                            associationService.Save (GoatAssociation);

                        
                    });
                return _EditAssociationCommand;
            }
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


        private void AdjustRoleName(GoatAssociationEndModel GoatAssociationEnd)
        {
            GoatAssociationEnd.Role = GoatAssociationEnd.MemberEnd;
        }
        #endregion        



        #region Left and Right MultiplicityType
        private MultiplicityType _leftMultiplicityType = MultiplicityType.None;
        public MultiplicityType LeftMultiplicityType
        {
            get { return _leftMultiplicityType; }
            set { SetLeftOrRightMultiplicityType(ref _leftMultiplicityType, value, Left, nameof(LeftMultiplicityType)); }
        }

        private MultiplicityType _rightMultiplicityType = MultiplicityType.None;
        public MultiplicityType RightMultiplicityType
        {
            get { return _rightMultiplicityType; }
            set { SetLeftOrRightMultiplicityType(ref _rightMultiplicityType, value, Right, nameof(RightMultiplicityType)); }
        }

        /// <summary>
        /// Setter for LeftMultiplicityType and RightMultiplicityType
        /// </summary>
        /// <param name="LeftRightMultiplicityType">internal attribute for storing value of the Propertyu</param>
        /// <param name="NewValue">New value for the property/attribute</param>
        /// <param name="AssociationEnd">instance of AssociationEnd whose Multiplicity will be changed</param>
        /// <param name="NameOfProperty">name of the property that is going to be changed</param>
        private void SetLeftOrRightMultiplicityType(ref MultiplicityType LeftRightMultiplicityType, MultiplicityType NewValue, GoatAssociationEndModel AssociationEnd, string NameOfProperty)
        {
            if (LeftRightMultiplicityType != NewValue)
            {
                LeftRightMultiplicityType = NewValue;

                switch (NewValue)
                {
                    case MultiplicityType.None:
                        AssociationEnd.Multiplicity = "";
                        break;
                    case MultiplicityType.ZeroToOne:
                        AssociationEnd.Multiplicity = "0..1";
                        break;
                    case MultiplicityType.ZeroToMany:
                        AssociationEnd.Multiplicity = "0..*";
                        break;
                    case MultiplicityType.One:
                        AssociationEnd.Multiplicity = "1";
                        break;
                    case MultiplicityType.OneToMany:
                        AssociationEnd.Multiplicity = "1..*";
                        break;
                    case MultiplicityType.Many:
                        AssociationEnd.Multiplicity = "*";
                        break;
                    case MultiplicityType.Other:
                        AssociationEnd.Multiplicity = _rightCustomMultiplicity;
                        break;
                    default:
                        throw new NotImplementedException($"setter of {NameOfProperty}");
                }
                RaisePropertyChanged(NameOfProperty);
            }
        }
        #endregion

        #region Left and Right Custom Multiplicity
        private string _leftCustomMultiplicity = "";
        public string LeftCustomMultiplicity
        {
            get { return _leftCustomMultiplicity; }
            set { SetRightOrLeftCustomMultiplicity(ref _leftCustomMultiplicity, value, Left, LeftMultiplicityType, nameof(LeftMultiplicityType)); }
        }

        private string _rightCustomMultiplicity = "";
        public string RightCustomMultiplicity
        {
            get { return _rightCustomMultiplicity; }
            set { SetRightOrLeftCustomMultiplicity(ref _rightCustomMultiplicity, value, Right, RightMultiplicityType, nameof(RightMultiplicityType)); }
        }

        /// <summary>
        /// Setter for Right and Left CustomMultiplicity
        /// </summary>
        /// <param name="CustomMultiplicity">target for a new value</param>
        /// <param name="NewValue">new value that will be assigned</param>
        /// <param name="GoatAssociationEnd">an instance of GoatAssociationEnd whose multiplicity will be changed</param>
        /// <param name="MultiplicityType">Right or Left multiplicity type that will be changed</param>
        /// <param name="ChangedPropertyName">Name of property that is changed</param>
        private void SetRightOrLeftCustomMultiplicity(ref string CustomMultiplicity,
                                                      string NewValue,
                                                      GoatAssociationEndModel GoatAssociationEnd,
                                                      MultiplicityType MultiplicityType,
                                                      string ChangedPropertyName)
        {
            if (CustomMultiplicity != NewValue)
            {
                CustomMultiplicity = NewValue;
                GoatAssociationEnd.Multiplicity = NewValue;
                MultiplicityType = MultiplicityType.Other;
                RaisePropertyChanged(ChangedPropertyName);

            }

        }


        #endregion

        private void RightLeft_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (!(sender is GoatAssociationEndModel))
                throw new ArgumentException($"Sender has to be of {nameof(GoatAssociationEndModel)} type.");

            MultiplicityType mt;

            if (sender == Left)
                mt = LeftMultiplicityType;
            else if (sender == Right)
                mt = RightMultiplicityType;
            else
                throw new ArgumentException("Sender if not end of myself.");

            if (e.PropertyName == nameof(GoatAssociationEndModel.Multiplicity))
            {
                switch (((GoatAssociationEndModel)sender).Multiplicity)
                {
                    case "": mt = MultiplicityType.None; break;
                    case "1": mt = MultiplicityType.One; break;
                    case "0..1": mt = MultiplicityType.ZeroToOne; break;
                    case "1..*": mt = MultiplicityType.OneToMany; break;
                    case "0..*": mt = MultiplicityType.ZeroToMany; break;
                    case "*": mt = MultiplicityType.Many; break;
                    default:
                        if (sender == Left)
                            LeftCustomMultiplicity = ((GoatAssociationEndModel)sender).Multiplicity;
                        else
                            RightCustomMultiplicity = ((GoatAssociationEndModel)sender).Multiplicity;
                        mt = MultiplicityType.Other;
                        break;
                }

                if (sender == Left)
                    LeftMultiplicityType = mt;
                else
                    RightMultiplicityType = mt;
            }
        }


    }
}
