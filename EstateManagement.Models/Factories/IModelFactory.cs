using EstateModel = EstateManagement.Models.Estate.Estate;
using EstateEntity = EstateReporting.Database.Entities.Estate;
using EstateOperatorEntity = EstateReporting.Database.Entities.EstateOperator;
using EstateSecurityUserEntity = EstateReporting.Database.Entities.EstateSecurityUser;
using MerchantModel = EstateManagement.Models.Merchant.Merchant;
using MerchantEntity = EstateReporting.Database.Entities.Merchant;
using MerchantAddressEntity = EstateReporting.Database.Entities.MerchantAddress;
using MerchantContactEntity = EstateReporting.Database.Entities.MerchantContact;
using MerchantOperatorEntity = EstateReporting.Database.Entities.MerchantOperator;
using MerchantDeviceEntity = EstateReporting.Database.Entities.MerchantDevice;
using MerchantSecurityUserEntity = EstateReporting.Database.Entities.MerchantSecurityUser;

namespace EstateManagement.Models.Factories
{
    using System.Collections.Generic;
    

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

        /// <summary>
        /// Converts from.
        /// </summary>
        /// <param name="merchant">The merchant.</param>
        /// <param name="merchantAddresses">The merchant addresses.</param>
        /// <param name="merchantContacts">The merchant contacts.</param>
        /// <param name="merchantOperators">The merchant operators.</param>
        /// <param name="merchantDevices">The merchant devices.</param>
        /// <param name="merchantSecurityUsers">The merchant security users.</param>
        /// <returns></returns>
        MerchantModel ConvertFrom(MerchantEntity merchant, List<MerchantAddressEntity> merchantAddresses, List<MerchantContactEntity> merchantContacts,
                                  List<MerchantOperatorEntity> merchantOperators, List<MerchantDeviceEntity> merchantDevices,
                                  List<MerchantSecurityUserEntity> merchantSecurityUsers);
    }
}