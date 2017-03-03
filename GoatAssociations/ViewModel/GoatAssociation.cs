using System;

namespace GoatAssociations.ViewModel
{
    class GoatAssociation
    {

        private void SetLeftOrRight(Model.GoatAssociationEnd GoatEnd, EA.ConnectorEnd EAEnd)
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
        }


        public GoatAssociation(Model.GoatAssociation GoatAssociation, EA.Connector Connector)
        {
            if (Connector.MetaType != "Association" && Connector.MetaType != "Aggregation")
                throw new ArgumentException($"Wrong MetaType ({Connector.MetaType}) of the Connector.");

            SetLeftOrRight(GoatAssociation.Left, Connector.ClientEnd);
            SetLeftOrRight(GoatAssociation.Right, Connector.SupplierEnd);
        }


    }
}
