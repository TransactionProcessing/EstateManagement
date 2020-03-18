using EstateModel = EstateManagement.Models.Estate;
using EstateEntity = EstateReporting.Database.Entities.Estate;
using EstateOperatorEntity = EstateReporting.Database.Entities.EstateOperator;
using EstateSecurityUserEntity = EstateReporting.Database.Entities.EstateSecurityUser;

namespace EstateManagement.Models.Factories
{
    using System.Collections.Generic;
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
        /// <param name="estateOperators">The estate operators.</param>
        /// <param name="estateSecurityUsers">The estate security users.</param>
        /// <returns></returns>
        EstateModel ConvertFrom(EstateEntity estate, List<EstateOperatorEntity> estateOperators, List<EstateSecurityUserEntity> estateSecurityUsers);
    }
}