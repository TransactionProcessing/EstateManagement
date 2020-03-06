namespace EstateManagement.Models.Factories
{
    using EstateModel = EstateManagement.Models.Estate;
    using EstateEntity = EstateReporting.Database.Entities.Estate;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="EstateManagement.Models.Factories.IModelFactory" />
    public class ModelFactory : IModelFactory
    {
        /// <summary>
        /// Converts from.
        /// </summary>
        /// <param name="estate">The estate.</param>
        /// <returns></returns>
        public EstateModel ConvertFrom(EstateEntity estate)
        {
            return new EstateModel
                   {
                       EstateId = estate.EstateId,
                       Name = estate.Name
                   };
        }
    }
}