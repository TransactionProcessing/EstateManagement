namespace EstateManagement.Models.Factories
{
    using EstateAggregate;

    /// <summary>
    /// 
    /// </summary>
    public interface IModelFactory
    {
        #region Methods

        /// <summary>
        /// Converts from.
        /// </summary>
        /// <param name="estateAggregate">The estate aggregate.</param>
        /// <returns></returns>
        Estate ConvertFrom(EstateAggregate estateAggregate);

        #endregion
    }
}