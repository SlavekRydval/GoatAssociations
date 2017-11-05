using Microsoft.VisualStudio.TestTools.UnitTesting;
using GoatAssociations.Model;
using System;

namespace GoatAssociationUnitTests
{
    [TestClass]
    public class AssociationModelUnitTest
    {
        [TestMethod]
        public void AggregationTypesTest()
        {
            GoatAssociationModel Association = new GoatAssociationModel()
            {
                Name = nameof(Association)
            };

            Assert.IsTrue(Association.Name == nameof(Association) && 
                Association.Left.Aggregation == Association.Right.Aggregation && 
                Association.Left.Aggregation == AggregationType.None, "Part 1");

            Association.Left.Aggregation = AggregationType.Composite;
            Assert.IsTrue(Association.Name == nameof(Association) &&
                Association.Left.Aggregation == AggregationType.Composite &&
                Association.Right.Aggregation == AggregationType.None, "Part 2");


            Association.Right.Aggregation = AggregationType.Composite;
            Assert.IsTrue(Association.Name == nameof(Association) &&
                Association.Left.Aggregation == AggregationType.None &&
                Association.Right.Aggregation == AggregationType.Composite, "Part 3");

            Association.Left.Aggregation = AggregationType.Composite;
            Assert.IsTrue(Association.Name == nameof(Association) &&
                Association.Left.Aggregation == AggregationType.Composite &&
                Association.Right.Aggregation == AggregationType.None, "Part 4");

            Association.Left.Aggregation = AggregationType.None;
            Assert.IsTrue(Association.Name == nameof(Association) &&
                Association.Left.Aggregation == AggregationType.None &&
                Association.Right.Aggregation == AggregationType.None, "Part 5");
        }



        private void SetAssociation(GoatAssociationModel Association, 
            string LeftMultiplicity, string RightMultiplicity,
            AggregationType LeftAggregationType, AggregationType RightAggregationType,
            bool LeftIsOwned, bool RightIsOwned,
            NavigabilityType LeftNavigability, NavigabilityType RightNavigability,
            bool LeftDerived, bool RightDerived,
            bool LeftUnion, bool RightUnion)
        {
            Association.Left.Multiplicity = LeftMultiplicity;
            Association.Right.Multiplicity = RightMultiplicity;
            Association.Left.Aggregation = LeftAggregationType;
            Association.Right.Aggregation = RightAggregationType;
            Association.Left.IsOwnedByClassifier = LeftIsOwned;
            Association.Right.IsOwnedByClassifier = RightIsOwned;
            Association.Left.Navigability = LeftNavigability;
            Association.Right.Navigability = RightNavigability;
            Association.Left.Derived = LeftDerived;
            Association.Right.Derived = RightDerived;
            Association.Left.Union = LeftUnion;
            Association.Right.Union = RightUnion;
        }

        private void ClearAssociation(GoatAssociationModel Association)
        {
            SetAssociation(Association, "", "", AggregationType.None, AggregationType.None, false, false, 
                NavigabilityType.Unspecified, NavigabilityType.Unspecified, false, false, false, false);
        }

        private bool TestAssociation(GoatAssociationModel Association,
            string LeftMultiplicity, string RightMultiplicity,
            AggregationType LeftAggregationType, AggregationType RightAggregationType,
            bool LeftIsOwned, bool RightIsOwned,
            NavigabilityType LeftNavigability, NavigabilityType RightNavigability,
            bool LeftDerived, bool RightDerived,
            bool LeftUnion, bool RightUnion)
        {
            return Association.Left.Multiplicity == LeftMultiplicity &&
                   Association.Right.Multiplicity == RightMultiplicity &&
                   Association.Left.Aggregation == LeftAggregationType &&
                   Association.Right.Aggregation == RightAggregationType &&
                   Association.Left.IsOwnedByClassifier == LeftIsOwned &&
                   Association.Right.IsOwnedByClassifier == RightIsOwned &&
                   Association.Left.Navigability == LeftNavigability &&
                   Association.Right.Navigability == RightNavigability &&
                   Association.Left.Derived == LeftDerived &&
                   Association.Right.Derived == RightDerived &&
                   Association.Left.Union == LeftUnion &&
                   Association.Right.Union == RightUnion;
        }


        private void SetAndTest(GoatAssociationModel Association, int TestNum)
        {
            switch (TestNum)
            {
                case 1:
                    SetAssociation(Association, LeftMultiplicity: "1", RightMultiplicity: "1",
                                    LeftAggregationType: AggregationType.None, RightAggregationType: AggregationType.None,
                                    LeftIsOwned: true, RightIsOwned: true,
                                    LeftNavigability: NavigabilityType.Navigable, RightNavigability: NavigabilityType.Navigable,
                                    LeftDerived: false, RightDerived: false,
                                    LeftUnion: false, RightUnion: false);

                    Assert.IsTrue(TestAssociation(Association, LeftMultiplicity: "1", RightMultiplicity: "1",
                                    LeftAggregationType: AggregationType.None, RightAggregationType: AggregationType.None,
                                    LeftIsOwned: true, RightIsOwned: true,
                                    LeftNavigability: NavigabilityType.Navigable, RightNavigability: NavigabilityType.Navigable,
                                    LeftDerived: false, RightDerived: false,
                                    LeftUnion: false, RightUnion: false), $"SetAndTest case {TestNum}");
                    break;
                case 2:
                    SetAssociation(Association, LeftMultiplicity: "0..*", RightMultiplicity: "0..*",
                                    LeftAggregationType: AggregationType.None, RightAggregationType: AggregationType.None,
                                    LeftIsOwned: true, RightIsOwned: true,
                                    LeftNavigability: NavigabilityType.Navigable, RightNavigability: NavigabilityType.Navigable,
                                    LeftDerived: false, RightDerived: false,
                                    LeftUnion: false, RightUnion: false);

                    Assert.IsTrue(TestAssociation(Association, LeftMultiplicity: "0..*", RightMultiplicity: "0..*",
                                    LeftAggregationType: AggregationType.None, RightAggregationType: AggregationType.None,
                                    LeftIsOwned: true, RightIsOwned: true,
                                    LeftNavigability: NavigabilityType.Navigable, RightNavigability: NavigabilityType.Navigable,
                                    LeftDerived: false, RightDerived: false,
                                    LeftUnion: false, RightUnion: false), $"SetAndTest case {TestNum}");
                    break;
                case 3:
                    SetAssociation(Association, LeftMultiplicity: "0..1", RightMultiplicity: "*",
                                    LeftAggregationType: AggregationType.Composite, RightAggregationType: AggregationType.None,
                                    LeftIsOwned: false, RightIsOwned: true,
                                    LeftNavigability: NavigabilityType.NonNavigable, RightNavigability: NavigabilityType.Navigable,
                                    LeftDerived: false, RightDerived: false,
                                    LeftUnion: false, RightUnion: false);

                    Assert.IsTrue(TestAssociation(Association, LeftMultiplicity: "0..1", RightMultiplicity: "*",
                                    LeftAggregationType: AggregationType.Composite, RightAggregationType: AggregationType.None,
                                    LeftIsOwned: false, RightIsOwned: true,
                                    LeftNavigability: NavigabilityType.NonNavigable, RightNavigability: NavigabilityType.Navigable,
                                    LeftDerived: false, RightDerived: false,
                                    LeftUnion: false, RightUnion: false), $"SetAndTest case {TestNum}");
                    break;
                case 4:
                    SetAssociation(Association, LeftMultiplicity: "0..*", RightMultiplicity: "0..1",
                                    LeftAggregationType: AggregationType.None, RightAggregationType: AggregationType.Composite,
                                    LeftIsOwned: true, RightIsOwned: false,
                                    LeftNavigability: NavigabilityType.Navigable, RightNavigability: NavigabilityType.Unspecified,
                                    LeftDerived: false, RightDerived: false,
                                    LeftUnion: false, RightUnion: false);

                    Assert.IsTrue(TestAssociation(Association, LeftMultiplicity: "0..*", RightMultiplicity: "0..1",
                                    LeftAggregationType: AggregationType.None, RightAggregationType: AggregationType.Composite,
                                    LeftIsOwned: true, RightIsOwned: false,
                                    LeftNavigability: NavigabilityType.Navigable, RightNavigability: NavigabilityType.Unspecified,
                                    LeftDerived: false, RightDerived: false,
                                    LeftUnion: false, RightUnion: false), $"SetAndTest case {TestNum}");
                    break;
                case 5:
                    SetAssociation(Association, LeftMultiplicity: "0..*", RightMultiplicity: "0..1",
                                    LeftAggregationType: AggregationType.None, RightAggregationType: AggregationType.Composite,
                                    LeftIsOwned: true, RightIsOwned: false,
                                    LeftNavigability: NavigabilityType.Navigable, RightNavigability: NavigabilityType.Unspecified,
                                    LeftDerived: true, RightDerived: true,
                                    LeftUnion: true, RightUnion: true);

                    Assert.IsTrue(TestAssociation(Association, LeftMultiplicity: "0..*", RightMultiplicity: "0..1",
                                    LeftAggregationType: AggregationType.None, RightAggregationType: AggregationType.Composite,
                                    LeftIsOwned: true, RightIsOwned: false,
                                    LeftNavigability: NavigabilityType.Navigable, RightNavigability: NavigabilityType.Unspecified,
                                    LeftDerived: true, RightDerived: true,
                                    LeftUnion: true, RightUnion: true), $"SetAndTest case {TestNum}");
                    break;
                case 6:
                    SetAssociation(Association, LeftMultiplicity: "*", RightMultiplicity: "*",
                                    LeftAggregationType: AggregationType.Shared, RightAggregationType: AggregationType.None,
                                    LeftIsOwned: false, RightIsOwned: true,
                                    LeftNavigability: NavigabilityType.Unspecified, RightNavigability: NavigabilityType.Unspecified,
                                    LeftDerived: false, RightDerived: true,
                                    LeftUnion: false, RightUnion: false);

                    Assert.IsTrue(TestAssociation(Association, LeftMultiplicity: "*", RightMultiplicity: "*",
                                    LeftAggregationType: AggregationType.Shared, RightAggregationType: AggregationType.None,
                                    LeftIsOwned: false, RightIsOwned: true,
                                    LeftNavigability: NavigabilityType.Unspecified, RightNavigability: NavigabilityType.Unspecified,
                                    LeftDerived: false, RightDerived: true,
                                    LeftUnion: false, RightUnion: false), $"SetAndTest case {TestNum}");
                    break;
                case 7:
                    SetAssociation(Association, LeftMultiplicity: "5..*", RightMultiplicity: "14..*",
                                    LeftAggregationType: AggregationType.None, RightAggregationType: AggregationType.Shared,
                                    LeftIsOwned: false, RightIsOwned: true,
                                    LeftNavigability: NavigabilityType.Unspecified, RightNavigability: NavigabilityType.Unspecified,
                                    LeftDerived: true, RightDerived: true,
                                    LeftUnion: true, RightUnion: false);

                    Assert.IsTrue(TestAssociation(Association, LeftMultiplicity: "5..*", RightMultiplicity: "14..*",
                                    LeftAggregationType: AggregationType.None, RightAggregationType: AggregationType.Shared,
                                    LeftIsOwned: false, RightIsOwned: true,
                                    LeftNavigability: NavigabilityType.Unspecified, RightNavigability: NavigabilityType.Unspecified,
                                    LeftDerived: true, RightDerived: true,
                                    LeftUnion: true, RightUnion: false), $"SetAndTest case {TestNum}");
                    break;
                default:
                    throw new NotImplementedException();
            }



        }

        [TestMethod]
        public void AggregationWholeSettingsTest()
        {
            GoatAssociationModel Association = new GoatAssociationModel()
            {
                Name = nameof(Association)
            };

            int[] TestOrder1 = { 1, 2, 3, 4, 5, 6, 7};
            foreach (int num in TestOrder1)
                SetAndTest(Association, num);

            int[] TestOrder2 = { 5, 2, 1, 6, 1, 7, 3, 7, 4, 3, 1, 4, 3, 2, 1, 5, 2, 7, 2, 6, 3, 6, 7, 4, 5, 7, 5, 4, 2, 3, 2, 1, 5, 2, 1, 3, 7, 6, 1, 2, 7, 6, 6, 5 };
            foreach (int num in TestOrder2)
                SetAndTest(Association, num);

            Random rnd = new Random();
            for (int i = 0; i < 100000; i++)
                SetAndTest(Association, rnd.Next(1,6));
        }


    }
}
