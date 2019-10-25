namespace EstateManagement.Models.Factories
{
    using EstateAggregate;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="EstateManagement.Models.Factories.IModelFactory" />
    public class ModelFactory : IModelFactory
    {
        #region Methods

        /// <summary>
        /// Converts from.
        /// </summary>
        /// <param name="estateAggregate">The estate aggregate.</param>
        /// <returns></returns>
        public Estate ConvertFrom(EstateAggregate estateAggregate)
        {
            if (estateAggregate == null)
            {
                return null;
            }

            return new Estate
                   {
                       EstateId = estateAggregate.AggregateId,
                       Name = estateAggregate.EstateName
                   };
        }

        #endregion
    }
}