using EstateModel = EstateManagement.Models.Estate;
using EstateEntity = EstateReporting.Database.Entities.Estate;

namespace EstateManagement.Models.Factories
{
    using EstateReporting.Database.Entities;

    /// <summary>
    /// 
    /// </summary>
    public interface IModelFactory
    {
        /// <summary>
        /// Converts from.
        /// </summary>
        /// <param name="estate">The estate.</param>
        /// <returns></returns>
        EstateModel ConvertFrom(EstateEntity estate);
    }
}