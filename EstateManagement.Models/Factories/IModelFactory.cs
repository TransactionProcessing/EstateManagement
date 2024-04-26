using EstateModel = EstateManagement.Models.Estate.Estate;
using EstateEntity = EstateManagement.Database.Entities.Estate;
using EstateOperatorEntity = EstateManagement.Database.Entities.EstateOperator;
using EstateSecurityUserEntity = EstateManagement.Database.Entities.EstateSecurityUser;
using MerchantModel = EstateManagement.Models.Merchant.Merchant;
using MerchantEntity = EstateManagement.Database.Entities.Merchant;
using MerchantAddressEntity = EstateManagement.Database.Entities.MerchantAddress;
using MerchantContactEntity = EstateManagement.Database.Entities.MerchantContact;
using MerchantOperatorEntity = EstateManagement.Database.Entities.MerchantOperator;
using MerchantDeviceEntity = EstateManagement.Database.Entities.MerchantDevice;
using MerchantSecurityUserEntity = EstateManagement.Database.Entities.MerchantSecurityUser;
using ContractModel = EstateManagement.Models.Contract.Contract;
using TransactionFeeModel = EstateManagement.Models.Contract.TransactionFee;
using ContractEntity = EstateManagement.Database.Entities.Contract;
using ContractProductEntity = EstateManagement.Database.Entities.ContractProduct;
using ContractProductTransactionFeeEntity = EstateManagement.Database.Entities.ContractProductTransactionFee;
using StatementLineEntity = EstateManagement.Database.Entities.StatementLine;
using StatementLineModel = EstateManagement.Models.MerchantStatement.StatementLine;
using OperatorEntity = EstateManagement.Database.Entities.Operator;

namespace EstateManagement.Models.Factories
{
    using System;
    using System.Collections.Generic;
    using Operator;

    /// <summary>
    /// 
    /// </summary>
    public interface IModelFactory
    {
        #region Methods

        /// <summary>
        /// Converts from.
        /// </summary>
        /// <param name="estate">The estate.</param>
        /// <param name="estateOperators">The estate operators.</param>
        /// <param name="estateSecurityUsers">The estate security users.</param>
        /// <returns></returns>
        EstateModel ConvertFrom(EstateEntity estate,
                                List<EstateOperatorEntity> estateOperators,
                                List<EstateSecurityUserEntity> estateSecurityUsers,
                                List<OperatorEntity> operators);
        
        MerchantModel ConvertFrom(Guid estateId, MerchantEntity merchant);

        MerchantModel ConvertFrom(Guid estateId, 
                                  MerchantEntity merchant,
                                  List<MerchantAddressEntity> merchantAddresses,
                                  List<MerchantContactEntity> merchantContacts,
                                  List<MerchantOperatorEntity> merchantOperators,
                                  List<MerchantDeviceEntity> merchantDevices,
                                  List<MerchantSecurityUserEntity> merchantSecurityUsers);

        /// <summary>
        /// Converts from.
        /// </summary>
        /// <param name="contract">The contract.</param>
        /// <param name="contractProducts">The contract products.</param>
        /// <param name="productTransactionFees">The product transaction fees.</param>
        /// <returns></returns>
        ContractModel ConvertFrom(Guid estateId, 
                                  ContractEntity contract,
                                  List<ContractProductEntity> contractProducts,
                                  List<ContractProductTransactionFeeEntity> productTransactionFees);

        /// <summary>
        /// Converts from.
        /// </summary>
        /// <param name="productTransactionFees">The product transaction fees.</param>
        /// <returns></returns>
        List<TransactionFeeModel> ConvertFrom(List<ContractProductTransactionFeeEntity> productTransactionFees);

        #endregion
    }
}