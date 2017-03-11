using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoatAssociations.Model
{
    enum NavigabilityType { Unspecified, Navigable, NonNavigable }
    enum AggregationType: int { None = 0, Shared = 1, Composite = 2}

    class GoatAssociationEnd : NotifyPropertyClass
    {
        public NavigabilityType Navigability { get; set; } = NavigabilityType.Unspecified;

        private AggregationType _aggregation = AggregationType.None;
        public AggregationType Aggregation
        {
            get
            {
                return _aggregation;
            }
            set
            {
                if (_aggregation != value)
                {
                    _aggregation = value;
                    this.OnPropertyChanged(nameof(Aggregation));
                }
            }
        }

        private string _multiplicity = "";
        public string Multiplicity
        {
            get
            {
                return _multiplicity;
            }
            set
            {
                if (_multiplicity != value)
                {
                    _multiplicity = value;
                    this.OnPropertyChanged(nameof(Multiplicity));
                }
            }
        }
        public bool Derived { get; set; } = false;
        public bool Union { get; set; } = false;
        public bool IsOwnedByClassifier { get; set; } = false;

        public string MemberEnd { get; set; } = "";

        private string _role = "";
        public string Role
        {
            get
            {
                return _role;
            }
            set
            {
                if (_role != value)
                {
                    _role = value;
                    this.OnPropertyChanged(nameof(Role));
                }
            }
        }
    }
}
