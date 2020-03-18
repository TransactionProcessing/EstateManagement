namespace EstateManagement.Models.Factories
{
    using System.Collections.Generic;
    using Microsoft.EntityFrameworkCore.Internal;
    using EstateModel = EstateManagement.Models.Estate;
    using EstateEntity = EstateReporting.Database.Entities.Estate;
    using EstateOperatorEntity = EstateReporting.Database.Entities.EstateOperator;
    using EstateSecurityUserEntity = EstateReporting.Database.Entities.EstateSecurityUser;

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
        /// <param name="estateOperators">The estate operators.</param>
        /// <param name="estateSecurityUsers">The estate security users.</param>
        /// <returns></returns>
        public EstateModel ConvertFrom(EstateEntity estate, List<EstateOperatorEntity> estateOperators, List<EstateSecurityUserEntity> estateSecurityUsers)
        {
            EstateModel estateModel= new EstateModel();
            estateModel.EstateId = estate.EstateId;
            estateModel.Name = estate.Name;

            if (estateOperators != null && estateOperators.Any())
            {
                estateModel.Operators = new List<Operator>();
                estateOperators.ForEach(eo => estateModel.Operators.Add(new Operator
                                                                        {
                                                                            Name = eo.Name,
                                                                            RequireCustomMerchantNumber = eo.RequireCustomMerchantNumber,
                                                                            RequireCustomTerminalNumber = eo.RequireCustomTerminalNumber,
                                                                            OperatorId = eo.OperatorId
                                                                        }));
            }

            if (estateSecurityUsers != null && estateSecurityUsers.Any())
            {
                estateModel.SecurityUsers = new List<SecurityUser>();
                estateSecurityUsers.ForEach(esu => estateModel.SecurityUsers.Add(new SecurityUser
                                                                                 {
                                                                                     SecurityUserId = esu.SecurityUserId,
                                                                                     EmailAddress = esu.EmailAddress
                                                                                 }));
            }

            return estateModel;
        }
    }
}