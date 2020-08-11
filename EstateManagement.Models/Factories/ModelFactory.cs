namespace EstateManagement.Models.Factories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Contract;
    using EstateModel = Estate.Estate;
    using EstateEntity = EstateReporting.Database.Entities.Estate;
    using EstateOperatorEntity = EstateReporting.Database.Entities.EstateOperator;
    using EstateSecurityUserEntity = EstateReporting.Database.Entities.EstateSecurityUser;
    using EstateOperatorModel = Estate.Operator;
    using SecurityUserModel = SecurityUser;
    using MerchantModel = Merchant.Merchant;
    using MerchantAddressModel = Merchant.Address;
    using MerchantContactModel = Merchant.Contact;
    using MerchantOperatorModel = Merchant.Operator;
    using MerchantEntity = EstateReporting.Database.Entities.Merchant;
    using MerchantAddressEntity = EstateReporting.Database.Entities.MerchantAddress;
    using MerchantContactEntity = EstateReporting.Database.Entities.MerchantContact;
    using MerchantOperatorEntity = EstateReporting.Database.Entities.MerchantOperator;
    using MerchantDeviceEntity = EstateReporting.Database.Entities.MerchantDevice;
    using MerchantSecurityUserEntity = EstateReporting.Database.Entities.MerchantSecurityUser;
    using ContractModel = Contract.Contract;
    using ContractEntity = EstateReporting.Database.Entities.Contract;
    using ContractProductEntity = EstateReporting.Database.Entities.ContractProduct;
    using ContractProductTransactionFeeEntity = EstateReporting.Database.Entities.ContractProductTransactionFee;

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
        /// <param name="estate">The estate.</param>
        /// <param name="estateOperators">The estate operators.</param>
        /// <param name="estateSecurityUsers">The estate security users.</param>
        /// <returns></returns>
        public EstateModel ConvertFrom(EstateEntity estate,
                                       List<EstateOperatorEntity> estateOperators,
                                       List<EstateSecurityUserEntity> estateSecurityUsers)
        {
            EstateModel estateModel = new EstateModel();
            estateModel.EstateId = estate.EstateId;
            estateModel.Name = estate.Name;

            if (estateOperators != null && estateOperators.Any())
            {
                estateModel.Operators = new List<EstateOperatorModel>();
                estateOperators.ForEach(eo => estateModel.Operators.Add(new EstateOperatorModel
                                                                        {
                                                                            Name = eo.Name,
                                                                            RequireCustomMerchantNumber = eo.RequireCustomMerchantNumber,
                                                                            RequireCustomTerminalNumber = eo.RequireCustomTerminalNumber,
                                                                            OperatorId = eo.OperatorId
                                                                        }));
            }

            if (estateSecurityUsers != null && estateSecurityUsers.Any())
            {
                estateModel.SecurityUsers = new List<SecurityUserModel>();
                estateSecurityUsers.ForEach(esu => estateModel.SecurityUsers.Add(new SecurityUserModel
                                                                                 {
                                                                                     SecurityUserId = esu.SecurityUserId,
                                                                                     EmailAddress = esu.EmailAddress
                                                                                 }));
            }

            return estateModel;
        }

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
        public MerchantModel ConvertFrom(MerchantEntity merchant,
                                         List<MerchantAddressEntity> merchantAddresses,
                                         List<MerchantContactEntity> merchantContacts,
                                         List<MerchantOperatorEntity> merchantOperators,
                                         List<MerchantDeviceEntity> merchantDevices,
                                         List<MerchantSecurityUserEntity> merchantSecurityUsers)
        {
            MerchantModel merchantModel = new MerchantModel();
            merchantModel.EstateId = merchant.EstateId;
            merchantModel.MerchantId = merchant.MerchantId;
            merchantModel.MerchantName = merchant.Name;

            if (merchantAddresses != null && merchantAddresses.Any())
            {
                merchantModel.Addresses = new List<MerchantAddressModel>();
                merchantAddresses.ForEach(ma => merchantModel.Addresses.Add(new MerchantAddressModel
                                                                            {
                                                                                AddressId = ma.AddressId,
                                                                                AddressLine1 = ma.AddressLine1,
                                                                                AddressLine2 = ma.AddressLine2,
                                                                                AddressLine3 = ma.AddressLine3,
                                                                                AddressLine4 = ma.AddressLine4,
                                                                                Country = ma.Country,
                                                                                PostalCode = ma.PostalCode,
                                                                                Region = ma.Region,
                                                                                Town = ma.Town
                                                                            }));
            }

            if (merchantContacts != null && merchantContacts.Any())
            {
                merchantModel.Contacts = new List<MerchantContactModel>();
                merchantContacts.ForEach(mc => merchantModel.Contacts.Add(new MerchantContactModel
                                                                          {
                                                                              ContactEmailAddress = mc.EmailAddress,
                                                                              ContactId = mc.ContactId,
                                                                              ContactName = mc.Name,
                                                                              ContactPhoneNumber = mc.PhoneNumber
                                                                          }));
            }

            if (merchantOperators != null && merchantOperators.Any())
            {
                merchantModel.Operators = new List<MerchantOperatorModel>();
                merchantOperators.ForEach(mo => merchantModel.Operators.Add(new MerchantOperatorModel
                                                                            {
                                                                                Name = mo.Name,
                                                                                MerchantNumber = mo.MerchantNumber,
                                                                                OperatorId = mo.OperatorId,
                                                                                TerminalNumber = mo.TerminalNumber
                                                                            }));
            }

            if (merchantDevices != null && merchantDevices.Any())
            {
                merchantModel.Devices = new Dictionary<Guid, String>();
                merchantDevices.ForEach(md => merchantModel.Devices.Add(md.DeviceId, md.DeviceIdentifier));
            }

            if (merchantSecurityUsers != null && merchantSecurityUsers.Any())
            {
                merchantModel.SecurityUsers = new List<SecurityUserModel>();
                merchantSecurityUsers.ForEach(msu => merchantModel.SecurityUsers.Add(new SecurityUserModel
                                                                                     {
                                                                                         EmailAddress = msu.EmailAddress,
                                                                                         SecurityUserId = msu.SecurityUserId
                                                                                     }));
            }

            return merchantModel;
        }

        /// <summary>
        /// Converts from.
        /// </summary>
        /// <param name="contract">The contract.</param>
        /// <param name="contractProducts">The contract products.</param>
        /// <param name="productTransactionFees">The product transaction fees.</param>
        /// <returns></returns>
        public ContractModel ConvertFrom(ContractEntity contract,
                                         List<ContractProductEntity> contractProducts,
                                         List<ContractProductTransactionFeeEntity> productTransactionFees)
        {
            ContractModel contractModel = new ContractModel();
            contractModel.EstateId = contract.EstateId;
            contractModel.OperatorId = contract.OperatorId;
            contractModel.Description = contract.Description;
            contractModel.IsCreated = true; // Should this be stored at RM or is the fact its in RM mean true???
            contractModel.ContractId = contract.ContractId;

            if (contractProducts != null && contractProducts.Any())
            {
                contractModel.Products = new List<Product>();

                contractProducts.ForEach(p => contractModel.Products.Add(new Product
                                                                         {
                                                                             ProductId = p.ProductId,
                                                                             Value = p.Value,
                                                                             Name = p.ProductName,
                                                                             DisplayText = p.DisplayText
                                                                         }));
            }

            if (productTransactionFees != null && productTransactionFees.Any())
            {
                productTransactionFees.ForEach(f =>
                                               {
                                                   Product product = contractModel.Products.Single(p => p.ProductId == f.ProductId);

                                                   product.TransactionFees.Add(new TransactionFee
                                                                               {
                                                                                   TransactionFeeId = f.TransactionFeeId,
                                                                                   Value = f.Value,
                                                                                   Description = f.Description,
                                                                                   CalculationType = (CalculationType)f.CalculationType
                                                                               });
                                               });
            }

            return contractModel;
        }

        /// <summary>
        /// Converts from.
        /// </summary>
        /// <param name="productTransactionFees">The product transaction fees.</param>
        /// <returns></returns>
        public List<TransactionFee> ConvertFrom(List<ContractProductTransactionFeeEntity> productTransactionFees)
        {
            List<TransactionFee> productTransactionFeesModelList = new List<TransactionFee>();

            productTransactionFees.ForEach(f =>
                                           {
                                               productTransactionFeesModelList.Add(new TransactionFee
                                                                                   {
                                                                                       TransactionFeeId = f.TransactionFeeId,
                                                                                       Value = f.Value,
                                                                                       Description = f.Description,
                                                                                       CalculationType = (CalculationType)f.CalculationType});
                                           });

            return productTransactionFeesModelList;
        }

        #endregion
    }
}