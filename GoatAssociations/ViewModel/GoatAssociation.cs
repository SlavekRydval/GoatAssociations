using System;

namespace GoatAssociations.ViewModel
{

    enum MultiplicityType { None, ZeroToOne, ZeroToMany, One, OneToMany, Many, Other }
    class GoatAssociation : Model.NotifyPropertyClass
    {
        Model.GoatAssociation _goatAssociation;
        EA.Connector _connector; 

        private void SetLeftOrRight(Model.GoatAssociationEnd GoatEnd, EA.ConnectorEnd EAEnd, EA.Element MemberEnd)
        {
            GoatEnd.Multiplicity = EAEnd.Cardinality;
            GoatEnd.Aggregation = (Model.AggregationType)EAEnd.Aggregation;
            GoatEnd.Derived = EAEnd.Derived;
            GoatEnd.Union = EAEnd.DerivedUnion;
            GoatEnd.IsOwnedByClassifier = EAEnd.OwnedByClassifier;
            GoatEnd.Role = EAEnd.Role;
            switch (EAEnd.Navigable)
            {
                case "Navigable": GoatEnd.Navigability = Model.NavigabilityType.Navigable; break;
                case "Non-Navigable": GoatEnd.Navigability = Model.NavigabilityType.NonNavigable; break;
                case "Unspecified": GoatEnd.Navigability = Model.NavigabilityType.Unspecified; break;
            }
            GoatEnd.MemberEnd = MemberEnd.Name;
        }

        private void UpdateLeftOrRightConnector(Model.GoatAssociationEnd GoatEnd, EA.ConnectorEnd EAEnd)
        {
            EAEnd.Cardinality = GoatEnd.Multiplicity;
            EAEnd.Aggregation = (int)GoatEnd.Aggregation;
            EAEnd.Derived = GoatEnd.Derived;
            EAEnd.DerivedUnion = GoatEnd.Union;
            EAEnd.OwnedByClassifier = GoatEnd.IsOwnedByClassifier;
            EAEnd.Role = GoatEnd.Role;
            switch (GoatEnd.Navigability)
            {
                case Model.NavigabilityType.Navigable: EAEnd.Navigable = "Navigable"; break;
                case Model.NavigabilityType.NonNavigable: EAEnd.Navigable = "Non-Navigable"; break;
                case Model.NavigabilityType.Unspecified: EAEnd.Navigable = "Unspecified"; break;
            }
            EAEnd.Update();
        }

        public GoatAssociation(Model.GoatAssociation GoatAssociation, EA.Connector Connector, EA.Repository Repository)
        {
            if (Connector.MetaType != "Association" && Connector.MetaType != "Aggregation")
                throw new ArgumentException($"Wrong MetaType ({Connector.MetaType}) of the Connector.");

            _goatAssociation = GoatAssociation;
            _connector = Connector;

            SetLeftOrRight(_goatAssociation.Left, _connector.ClientEnd, Repository.GetElementByID (_connector.ClientID));
            SetLeftOrRight(_goatAssociation.Right, _connector.SupplierEnd, Repository.GetElementByID(_connector.SupplierID));

            _goatAssociation.Left.PropertyChanged += Left_PropertyChanged;
            Left_PropertyChanged(_goatAssociation.Left, new System.ComponentModel.PropertyChangedEventArgs(nameof(Left.Multiplicity)));
        }

        private void Left_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            //if (!(sender is Model.GoatAssociationEnd))
            //    return; 

            if (e.PropertyName == nameof(Left.Multiplicity))
            {
                switch (Left.Multiplicity)
                {
                    case "": LeftMultiplicityType = MultiplicityType.None; break;
                    case "1": LeftMultiplicityType = MultiplicityType.One; break;
                    case "0..1": LeftMultiplicityType = MultiplicityType.ZeroToOne; break;
                    case "1..*": LeftMultiplicityType = MultiplicityType.OneToMany; break;
                    case "0..*": LeftMultiplicityType = MultiplicityType.ZeroToMany; break;
                    case "*": LeftMultiplicityType = MultiplicityType.Many; break;
                    default: LeftCustomMultiplicity = Left.Multiplicity; LeftMultiplicityType = MultiplicityType.Other; break;
                }

            }
        }

        public void UpdateConnector()
        {
            UpdateLeftOrRightConnector(_goatAssociation.Left, _connector.ClientEnd);
            UpdateLeftOrRightConnector(_goatAssociation.Right, _connector.SupplierEnd);
            _connector.Update();
        }

        public Model.GoatAssociationEnd Left { get { return _goatAssociation.Left; } }
        public Model.GoatAssociationEnd Right { get { return _goatAssociation.Right; } }

        private MultiplicityType _leftMultiplicityType = MultiplicityType.None;
        public MultiplicityType LeftMultiplicityType
        {
            get { return _leftMultiplicityType; }
            set
            {
                if (_leftMultiplicityType != value)
                {
                    _leftMultiplicityType = value;
                    switch (value)
                    {
                        case MultiplicityType.None:
                            Left.Multiplicity = ""; 
                            break;
                        case MultiplicityType.ZeroToOne:
                            Left.Multiplicity = "0..1";
                            break;
                        case MultiplicityType.ZeroToMany:
                            Left.Multiplicity = "0..*";
                            break;
                        case MultiplicityType.One:
                            Left.Multiplicity = "1";
                            break;
                        case MultiplicityType.OneToMany:
                            Left.Multiplicity = "1..*";
                            break;
                        case MultiplicityType.Many:
                            Left.Multiplicity = "*";
                            break;
                        case MultiplicityType.Other:
                            Left.Multiplicity = _leftCustomMultiplicity;
                            break;
                        default:
                            throw new NotImplementedException($"setter of {nameof(LeftMultiplicityType)}");
                    }
                    this.OnPropertyChanged(nameof(LeftMultiplicityType));
                }
            }
        }

        private string _leftCustomMultiplicity = "";
        public string LeftCustomMultiplicity
        {
            get { return _leftCustomMultiplicity; }
            set
            {
                if (_leftCustomMultiplicity != value)
                {
                    _leftCustomMultiplicity = value;
                    Left.Multiplicity = value;
                    LeftMultiplicityType = MultiplicityType.Other;
                    this.OnPropertyChanged(nameof (LeftMultiplicityType));
                }
            }
        }



    }
}
