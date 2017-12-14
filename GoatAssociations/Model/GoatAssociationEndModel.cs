namespace GoatAssociations.Model
{
    using GalaSoft.MvvmLight;

    /// <summary>
    /// Type of navigability of the association end
    /// </summary>
    public enum NavigabilityType { Unspecified, Navigable, NonNavigable }

    /// <summary>
    /// Aggregation type of the association end
    /// </summary>
    public enum AggregationType: int { None = 0, Shared = 1, Composite = 2}

    /// <summary>
    /// Association end in the Sparx EA
    /// </summary>
    public class GoatAssociationEndModel: ObservableObject
    {
        public const string MultiplicityWhenCompostite = "0..1";

        private NavigabilityType _navigability = NavigabilityType.Unspecified;
        /// <summary>
        /// Navigability of the association end
        /// </summary>
        public NavigabilityType Navigability
        {
            get => _navigability;
            set
            {
                Set(nameof(Navigability), ref _navigability, value);
                if (Navigability == NavigabilityType.NonNavigable && IsOwnedByClassifier)
                    IsOwnedByClassifier = false;
            }
        }

        private AggregationType _aggregation = AggregationType.None;
        /// <summary>
        /// Aggregation type of an association
        /// </summary>
        public AggregationType Aggregation
        {
            get => _aggregation;
            set {
                Set(nameof(Aggregation), ref _aggregation, value);
                if (Aggregation == AggregationType.Composite && Multiplicity != "0" && 
                    Multiplicity != "0..1" && Multiplicity != "1" && !string.IsNullOrWhiteSpace(Multiplicity))
                    Multiplicity = MultiplicityWhenCompostite;
            }
        }
    

        private string _multiplicity = "";
        public string Multiplicity
        {
            get => _multiplicity;
            set {
                Set(nameof(Multiplicity), ref _multiplicity, value);
                if (Aggregation == AggregationType.Composite && 
                    Multiplicity != "0" && Multiplicity != "0..1" && Multiplicity != "1" && !string.IsNullOrWhiteSpace(Multiplicity))
                    Aggregation = AggregationType.None;
            }
        }

        private bool _derived = false; 
        public bool Derived
        {
            get => _derived;
            set {
                Set(nameof(Derived), ref _derived, value);
                if (!Derived && Union)
                    Union = false; 
            }
        }

        private bool _union = false;
        public bool Union
        {
            get => _union;
            set {
                Set(nameof(Union), ref _union, value);
                if (Union)
                    Derived = true;
            }
        }

        private bool _isOwnedByClassifier = false;
        public bool IsOwnedByClassifier
        {
            get => _isOwnedByClassifier;
            set {
                Set(nameof(IsOwnedByClassifier), ref _isOwnedByClassifier, value);
                if (IsOwnedByClassifier && Navigability == NavigabilityType.NonNavigable)
                    Navigability = NavigabilityType.Navigable;
            }
        }

        private string _memberEnd = "";
        public string MemberEnd
        {
            get => _memberEnd;
            set => Set(nameof (MemberEnd), ref _memberEnd, value);
        }

        private string _role = "";
        public string Role
        {
            get => _role;
            set => Set(nameof(Role), ref _role, value);
        }
    }
}
