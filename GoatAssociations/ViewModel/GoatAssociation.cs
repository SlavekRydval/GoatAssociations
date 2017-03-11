using System;

namespace GoatAssociations.ViewModel
{

    enum MultiplicityType { None, ZeroToOne, ZeroToMany, One, OneToMany, Many, Other }

    class GoatAssociation : Model.NotifyPropertyClass
    {
        private Model.GoatAssociation _goatAssociation;
        private EA.Connector _connector;

        public Model.GoatAssociationEnd Left { get { return _goatAssociation.Left; } }
        public Model.GoatAssociationEnd Right { get { return _goatAssociation.Right; } }

        /// <summary>
        /// Constructor sets up the GoatAssociation according to parameters
        /// </summary>
        /// <param name="GoatAssociation">instance of Model.GoatAssociation</param>
        /// <param name="Connector">Connector as a source of data</param>
        /// <param name="Repository">Repository where the connector is located</param>
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

            _goatAssociation.Right.PropertyChanged += Right_PropertyChanged;
            Right_PropertyChanged(_goatAssociation.Right, new System.ComponentModel.PropertyChangedEventArgs(nameof(Right.Multiplicity)));
        }

        /// <summary>
        /// Sets the GoatAssociationEnd according to values in EA.ConnectorEnd
        /// </summary>
        /// <param name="GoatEnd">instance of GoatAssociationEnd that will be set</param>
        /// <param name="EAEnd">instance of EA.ConnectorEnd</param>
        /// <param name="MemberEnd">element that is at the connector end</param>
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

        /// <summary>
        /// Sets the EA.ConnectorEnd according to values in Model.GoatAssociationEnd
        /// </summary>
        /// <param name="GoatEnd">instance of Model.GoatAssociationEnd as a source</param>
        /// <param name="EAEnd">instance of EA.ConnectorEnd as a target</param>
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

        /// TODO: Refactoring of Right_PropertyChanged and Left_PropertyChanged
        private void Right_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (!(sender is Model.GoatAssociationEnd))
                throw new ArgumentException($"Sender has to be of {nameof (Model)}.{nameof (Model.GoatAssociationEnd)} type.");

            if (e.PropertyName == nameof(Model.GoatAssociationEnd.Multiplicity))
            {
                switch (((Model.GoatAssociationEnd) sender).Multiplicity)
                {
                    case "": RightMultiplicityType = MultiplicityType.None; break;
                    case "1": RightMultiplicityType = MultiplicityType.One; break;
                    case "0..1": RightMultiplicityType = MultiplicityType.ZeroToOne; break;
                    case "1..*": RightMultiplicityType = MultiplicityType.OneToMany; break;
                    case "0..*": RightMultiplicityType = MultiplicityType.ZeroToMany; break;
                    case "*": RightMultiplicityType = MultiplicityType.Many; break;
                    default:
                        RightCustomMultiplicity = ((Model.GoatAssociationEnd)sender).Multiplicity;
                        RightMultiplicityType = MultiplicityType.Other;
                        break;
                }
            }
        }
        private void Left_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (!(sender is Model.GoatAssociationEnd))
                throw new ArgumentException($"Sender has to be of {nameof(Model)}.{nameof(Model.GoatAssociationEnd)} type.");
            
            if (e.PropertyName == nameof(Model.GoatAssociationEnd.Multiplicity))
            {
                switch (((Model.GoatAssociationEnd)sender).Multiplicity)
                {
                    case "": LeftMultiplicityType = MultiplicityType.None; break;
                    case "1": LeftMultiplicityType = MultiplicityType.One; break;
                    case "0..1": LeftMultiplicityType = MultiplicityType.ZeroToOne; break;
                    case "1..*": LeftMultiplicityType = MultiplicityType.OneToMany; break;
                    case "0..*": LeftMultiplicityType = MultiplicityType.ZeroToMany; break;
                    case "*": LeftMultiplicityType = MultiplicityType.Many; break;
                    default:
                        LeftCustomMultiplicity = ((Model.GoatAssociationEnd)sender).Multiplicity;
                        LeftMultiplicityType = MultiplicityType.Other;
                        break;
                }
            }
        }

        /// <summary>
        /// Updates values in connector ends and update them in repository.
        /// </summary>
        public void UpdateConnector()
        {
            UpdateLeftOrRightConnector(_goatAssociation.Left, _connector.ClientEnd);
            UpdateLeftOrRightConnector(_goatAssociation.Right, _connector.SupplierEnd);

            //fixing design error in Sparx EA, you have to also change _connector.Direction
            if (_goatAssociation.Left.Navigability == Model.NavigabilityType.Navigable && _goatAssociation.Right.Navigability == Model.NavigabilityType.Navigable)
                _connector.Direction = "Bi-Directional";
            else if (_goatAssociation.Left.Navigability != Model.NavigabilityType.Navigable && _goatAssociation.Right.Navigability == Model.NavigabilityType.Navigable)
                _connector.Direction = "Source -> Destination";
            else if (_goatAssociation.Left.Navigability == Model.NavigabilityType.Navigable && _goatAssociation.Right.Navigability != Model.NavigabilityType.Navigable)
                _connector.Direction = "Destination -> Source";
            else
                _connector.Direction = "Unspecified";

            _connector.Update(); //this is with high probability not neccessarry... could depend on EA version
        }


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
        private void SetLeftOrRightMultiplicityType(ref MultiplicityType LeftRightMultiplicityType, MultiplicityType NewValue, Model.GoatAssociationEnd AssociationEnd, string NameOfProperty)
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
                this.OnPropertyChanged(NameOfProperty);
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
                                                      Model.GoatAssociationEnd GoatAssociationEnd,
                                                      MultiplicityType MultiplicityType,
                                                      string ChangedPropertyName)
        {
            if (CustomMultiplicity != NewValue)
            {
                CustomMultiplicity = NewValue;
                GoatAssociationEnd.Multiplicity = NewValue;
                MultiplicityType = MultiplicityType.Other;
                this.OnPropertyChanged(ChangedPropertyName);

            }
            
        }


        #endregion



    }
}
