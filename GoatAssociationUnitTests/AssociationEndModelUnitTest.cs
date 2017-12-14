using Microsoft.VisualStudio.TestTools.UnitTesting;
using GoatAssociations.Model;


namespace GoatAssociationUnitTests
{
    [TestClass]
    public class AssociationEndModelUnitTest
    {
        [TestMethod]
        public void UnionMeansDerived()
        {
            GoatAssociationEndModel AssociationEnd = new GoatAssociationEndModel
            {
                Union = true
            };
            Assert.IsTrue(AssociationEnd.Union, "Union is not true.");
            Assert.IsTrue(AssociationEnd.Derived, "Although Union is true, Derived is false.");
        }

        [TestMethod]
        public void ChangeDerivedToFalseMeansNoUnion()
        {
            GoatAssociationEndModel AssociationEnd = new GoatAssociationEndModel
            {
                Union = true,
                Derived = false

            };
            Assert.IsFalse(AssociationEnd.Derived, "Derived is not true.");
            Assert.IsFalse(AssociationEnd.Union, "Although Derived is false, Derived is false.");
        }

        [TestMethod]
        public void CompositeMeansStrictMultiplicity()
        {
            GoatAssociationEndModel AssociationEnd = new GoatAssociationEndModel
            {
                Multiplicity = "any random string.",
                Aggregation = AggregationType.Composite

            };
            Assert.IsTrue(AssociationEnd.Aggregation == AggregationType.Composite, "Aggregation is not Composite!");
            Assert.IsTrue(AssociationEnd.Multiplicity == GoatAssociationEndModel.MultiplicityWhenCompostite, "Multiplicity wasn't changed.");

            AssociationEnd.Aggregation = AggregationType.None;
            AssociationEnd.Multiplicity = "";
            AssociationEnd.Aggregation = AggregationType.Composite;
            Assert.IsTrue(AssociationEnd.Multiplicity == "", "Multiplicity has been changed.");
        }

        [TestMethod]
        public void MultiplicityChangeMeansCompositePossibleChange()
        {
            string s = "any random string.";
            GoatAssociationEndModel AssociationEnd = new GoatAssociationEndModel
            {
                Aggregation = AggregationType.Composite,
                Multiplicity = s

            };
            Assert.IsTrue(AssociationEnd.Multiplicity == s, "Multiplicity wasn't changed.");
            Assert.IsFalse(AssociationEnd.Aggregation == AggregationType.Composite, "Aggregation is Composite!");
        }

        [TestMethod]
        public void IsOwnedByClassifierDoesntMeanNonNavigable()
        {
            //it is not a UML rule but is reasonable
            GoatAssociationEndModel AssociationEnd = new GoatAssociationEndModel
            {
                Navigability = NavigabilityType.NonNavigable,
                IsOwnedByClassifier = true

            };
            Assert.IsTrue(AssociationEnd.IsOwnedByClassifier, "IsOwnedByClassifier is not true.");
            Assert.IsTrue(AssociationEnd.Navigability != NavigabilityType.NonNavigable, "Navigability failure.");

        }

        [TestMethod]
        public void NavigabilityChangeMeansPossibleOwnershipChange()
        {
            //it is not a UML rule but is reasonable
            GoatAssociationEndModel AssociationEnd = new GoatAssociationEndModel
            {
                IsOwnedByClassifier = true,
                Navigability = NavigabilityType.NonNavigable

            };
            Assert.IsTrue(AssociationEnd.Navigability == NavigabilityType.NonNavigable, "AssociationEnd.Navigability != NavigabilityType.NonNavigable.");
            Assert.IsFalse(AssociationEnd.IsOwnedByClassifier, "Ownership failure.");

        }


        [TestMethod]
        public void MoreComplexTest()
        {
            GoatAssociationEndModel AssociationEnd = new GoatAssociationEndModel
            {
                Role = nameof (AssociationEnd),
                Aggregation = AggregationType.None,
                Multiplicity = "10",
                Union = false,
                Derived = false
            };

            Assert.IsTrue(AssociationEnd.Role == nameof(AssociationEnd) && AssociationEnd.Aggregation == AggregationType.None &&
                AssociationEnd.Multiplicity == "10" && !AssociationEnd.Union && !AssociationEnd.Derived, "Test 1");

            AssociationEnd.Union = true;
            Assert.IsTrue(AssociationEnd.Role == nameof(AssociationEnd) && AssociationEnd.Aggregation == AggregationType.None &&
                AssociationEnd.Multiplicity == "10" && AssociationEnd.Union && AssociationEnd.Derived, "Test 2");

            AssociationEnd.Aggregation = AggregationType.Composite;
            Assert.IsTrue(AssociationEnd.Role == nameof(AssociationEnd) && AssociationEnd.Aggregation == AggregationType.Composite &&
                AssociationEnd.Multiplicity == GoatAssociationEndModel.MultiplicityWhenCompostite && 
                AssociationEnd.Union && AssociationEnd.Derived, "Test 3");

            AssociationEnd.Multiplicity = "10..15";
            Assert.IsTrue(AssociationEnd.Role == nameof(AssociationEnd) && AssociationEnd.Aggregation == AggregationType.None &&
                AssociationEnd.Multiplicity == "10..15" && AssociationEnd.Union && AssociationEnd.Derived, "Test 4");
        }

    }

}
