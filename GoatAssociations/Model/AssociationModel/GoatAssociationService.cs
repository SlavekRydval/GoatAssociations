namespace GoatAssociations.Model.AssociationModel
{
    using System;

    /// <summary>
    /// This class is responsible for reading and saving association from/to EA repository
    /// </summary>
    public class GoatAssociationService : IGoatAssociationService
    {
        private readonly EA.Connector connector;
        private readonly EA.Repository repository;

        public GoatAssociationService(EA.Connector Connector, EA.Repository Repository)
        {
            if (Connector == null || Repository == null)
                throw new ArgumentNullException("Neither Connector nor Repository can be null.");

            if (Connector.MetaType != "Association" && Connector.MetaType != "Aggregation")
                throw new ArgumentException($"Wrong MetaType ({Connector.MetaType}) of the Connector.");

            connector = Connector;
            repository = Repository;
        }


        #region Reading data
        public GoatAssociationModel Read()
        {
            GoatAssociationModel result = new GoatAssociationModel();

            SetLeftOrRight(result.Left, connector.ClientEnd, repository.GetElementByID(connector.ClientID));
            SetLeftOrRight(result.Right, connector.SupplierEnd, repository.GetElementByID(connector.SupplierID));
            result.Name = connector.Name;

            return result;
        }



        /// <summary>
        /// Sets the GoatAssociationEnd according to values in EA.ConnectorEnd
        /// </summary>
        /// <param name="GoatEnd">instance of GoatAssociationEnd that will be set</param>
        /// <param name="EAEnd">instance of EA.ConnectorEnd</param>
        /// <param name="MemberEnd">element that is at the connector end</param>
        private void SetLeftOrRight(GoatAssociationEndModel GoatEnd, EA.ConnectorEnd EAEnd, EA.Element MemberEnd)
        {
            GoatEnd.Multiplicity = EAEnd.Cardinality;
            GoatEnd.Aggregation = (AggregationType)EAEnd.Aggregation;
            GoatEnd.Derived = EAEnd.Derived;
            GoatEnd.Union = EAEnd.DerivedUnion;
            GoatEnd.IsOwnedByClassifier = EAEnd.OwnedByClassifier;
            GoatEnd.Role = EAEnd.Role;
            switch (EAEnd.Navigable)
            {
                case "Navigable": GoatEnd.Navigability = NavigabilityType.Navigable; break;
                case "Non-Navigable": GoatEnd.Navigability = NavigabilityType.NonNavigable; break;
                case "Unspecified": GoatEnd.Navigability = NavigabilityType.Unspecified; break;
            }
            GoatEnd.MemberEnd = MemberEnd.Name;
        }
        #endregion

        #region Saving data
        /// <summary>
        /// Updates values in connector ends and update them in repository.
        /// </summary>
        public void Save(GoatAssociationModel Association)
        {
            UpdateLeftOrRightConnector(Association.Left, connector.ClientEnd);
            UpdateLeftOrRightConnector(Association.Right, connector.SupplierEnd);

            //fixing design error in Sparx EA, you have to also change _connector.Direction
            if (Association.Left.Navigability == NavigabilityType.Navigable && Association.Right.Navigability == NavigabilityType.Navigable)
                connector.Direction = "Bi-Directional";
            else if (Association.Left.Navigability != NavigabilityType.Navigable && Association.Right.Navigability == NavigabilityType.Navigable)
                connector.Direction = "Source -> Destination";
            else if (Association.Left.Navigability == NavigabilityType.Navigable && Association.Right.Navigability != NavigabilityType.Navigable)
                connector.Direction = "Destination -> Source";
            else
                connector.Direction = "Unspecified";

            connector.Update(); //this is with high probability not neccessarry... could depend on EA version
        }

        /// <summary>
        /// Sets the EA.ConnectorEnd according to values in Model.GoatAssociationEnd
        /// </summary>
        /// <param name="GoatEnd">instance of GoatAssociationEnd as a source</param>
        /// <param name="EAEnd">instance of EA.ConnectorEnd as a target</param>
        private void UpdateLeftOrRightConnector(GoatAssociationEndModel GoatEnd, EA.ConnectorEnd EAEnd)
        {
            EAEnd.Cardinality = GoatEnd.Multiplicity;
            EAEnd.Aggregation = (int)GoatEnd.Aggregation;
            EAEnd.Derived = GoatEnd.Derived;
            EAEnd.DerivedUnion = GoatEnd.Union;
            EAEnd.OwnedByClassifier = GoatEnd.IsOwnedByClassifier;
            EAEnd.Role = GoatEnd.Role;
            switch (GoatEnd.Navigability)
            {
                case NavigabilityType.Navigable: EAEnd.Navigable = "Navigable"; break;
                case NavigabilityType.NonNavigable: EAEnd.Navigable = "Non-Navigable"; break;
                case NavigabilityType.Unspecified: EAEnd.Navigable = "Unspecified"; break;
            }
            EAEnd.Update();
        }
        #endregion

    }
}
