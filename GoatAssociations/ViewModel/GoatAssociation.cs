using System;

namespace GoatAssociations.ViewModel
{
    class GoatAssociation
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
        }

        public void UpdateConnector()
        {
            UpdateLeftOrRightConnector(_goatAssociation.Left, _connector.ClientEnd);
            UpdateLeftOrRightConnector(_goatAssociation.Right, _connector.SupplierEnd);
            _connector.Update();
        }

        public Model.GoatAssociationEnd Left { get { return _goatAssociation.Left; } }
        public Model.GoatAssociationEnd Right { get { return _goatAssociation.Right; } }

    }
}
