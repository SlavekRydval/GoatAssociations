using Microsoft.VisualStudio.TestTools.UnitTesting;
using GoatAssociations.Model;
using System;

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
        public void NavigabilityAndOwnership()
        {
            ///TODO: two tests just like above
            throw new NotImplementedException();
        }

        [TestMethod]
        public void ComplexTest()
        {
            ///TODO: two tests just like above
            throw new NotImplementedException();
        }

    }

}
