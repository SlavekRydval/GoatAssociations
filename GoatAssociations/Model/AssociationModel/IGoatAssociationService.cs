namespace GoatAssociations.Model.AssociationModel
{
    /// <summary>
    /// Interface providing methods for reading and saving association
    /// </summary>
    public interface IGoatAssociationService
    {
        /// <summary>
        /// Create an instance of GoatAssociationModel class and fill it with data
        /// </summary>
        /// <returns></returns>
        GoatAssociationModel Read();

        /// <summary>
        /// Saves the data given in the parameters
        /// </summary>
        /// <param name="Association">Data that should be saved.</param>
        void Save(GoatAssociationModel Association);
    }
}
