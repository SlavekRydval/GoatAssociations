namespace GoatAssociations.Design
{
    using GoatAssociations.Model.AssociationModel;
    using System;

    class DesignAssociationService : IGoatAssociationService
    {
        public GoatAssociationModel Read()
        {
            var result = new GoatAssociationModel()
            {
                Name = "Name given by design."
            };

            result.Left.Aggregation = AggregationType.Composite;
            result.Left.Derived = false;
            result.Left.IsOwnedByClassifier = false;
            result.Left.Multiplicity = "0..1";
            result.Left.Navigability = NavigabilityType.Unspecified;
            result.Left.Role = "role left";
            result.Left.Union = false;

            result.Right.Aggregation = AggregationType.None;
            result.Right.Derived = false;
            result.Right.IsOwnedByClassifier = true;
            result.Right.Multiplicity = "1..*";
            result.Right.Navigability = NavigabilityType.Navigable;
            result.Right.Role = "role right";
            result.Right.Union = true;

            return result; 
        }

        public void Save(GoatAssociationModel Association)
        {
            throw new NotImplementedException();
        }
    }
}
