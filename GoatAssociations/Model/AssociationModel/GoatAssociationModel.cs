namespace GoatAssociations.Model.AssociationModel
{
    using GalaSoft.MvvmLight;

    public class GoatAssociationModel: ObservableObject
    {
        public GoatAssociationEndModel Left { get; set; } = new GoatAssociationEndModel();
        public GoatAssociationEndModel Right { get; set; } = new GoatAssociationEndModel();

        private string _name = ""; 
        /// <summary>
        /// Name of the association, can be set by a user
        /// </summary>
        public string Name
        {
            get => _name;
            set => Set (nameof (Name), ref _name, value);
        }


        public GoatAssociationModel()
        {
            Left.PropertyChanged += LeftRight_PropertyChanged;
            Right.PropertyChanged += LeftRight_PropertyChanged;
        }

        //MVVM question: Is this really Model stuff or should it be moved to ViewModel?
        private void LeftRight_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(GoatAssociationEndModel.Aggregation))
            {
                if (sender == Left)
                {
                    if (Left.Aggregation != AggregationType.None)
                        Right.Aggregation = AggregationType.None;
                }
                else
                if (sender == Right)
                {
                    if (Right.Aggregation != AggregationType.None)
                        Left.Aggregation = AggregationType.None;
                }

            }
        }
    }
}
