﻿namespace EstateManagement.Models.Factories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Contract;
    using Merchant;
    using EstateModel = Estate.Estate;
    using EstateEntity = EstateManagement.Database.Entities.Estate;
    using EstateSecurityUserEntity = EstateManagement.Database.Entities.EstateSecurityUser;
    using EstateOperatorModel = Estate.Operator;
    using SecurityUserModel = SecurityUser;
    using MerchantModel = Merchant.Merchant;
    using MerchantAddressModel = Merchant.Address;
    using MerchantContactModel = Merchant.Contact;
    using MerchantOperatorModel = Merchant.Operator;
    using MerchantEntity = EstateManagement.Database.Entities.Merchant;
    using MerchantAddressEntity = EstateManagement.Database.Entities.MerchantAddress;
    using MerchantContactEntity = EstateManagement.Database.Entities.MerchantContact;
    using MerchantOperatorEntity = EstateManagement.Database.Entities.MerchantOperator;
    using MerchantDeviceEntity = EstateManagement.Database.Entities.MerchantDevice;
    using MerchantSecurityUserEntity = EstateManagement.Database.Entities.MerchantSecurityUser;
    using ContractModel = Contract.Contract;
    using ContractEntity = EstateManagement.Database.Entities.Contract;
    using ContractProductEntity = EstateManagement.Database.Entities.ContractProduct;
    using ContractProductTransactionFeeEntity = EstateManagement.Database.Entities.ContractProductTransactionFee;
    using ContractProductTransactionFeeTransactionFeeModel = Contract.ContractProductTransactionFee;
    using OperatorEntity = EstateManagement.Database.Entities.Operator;

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
                                       List<EstateSecurityUserEntity> estateSecurityUsers,
                                       List<OperatorEntity> operators)
        {
            EstateModel estateModel = new EstateModel();
            estateModel.EstateId = estate.EstateId;
            estateModel.EstateReportingId = estate.EstateReportingId;
            estateModel.Name = estate.Name;
            estateModel.Reference = estate.Reference;

            if (operators != null && operators.Any())
            {
                estateModel.Operators = new List<EstateOperatorModel>();
                foreach (var @operator in operators){
                estateModel.Operators.Add(new EstateOperatorModel{
                                                                         OperatorId = @operator.OperatorId,
                                                                         Name = @operator.Name
                                                                     });
                }
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

        public MerchantModel ConvertFrom(Guid estateId, MerchantEntity merchant){
            MerchantModel merchantModel = new MerchantModel();
            merchantModel.EstateId = estateId;
            merchantModel.MerchantReportingId = merchant.MerchantReportingId;
            merchantModel.MerchantId = merchant.MerchantId;
            merchantModel.MerchantName = merchant.Name;
            merchantModel.Reference = merchant.Reference;
            merchantModel.SettlementSchedule = (SettlementSchedule)merchant.SettlementSchedule;

            return merchantModel;
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
        public MerchantModel ConvertFrom(Guid estateId, 
                                         MerchantEntity merchant,
                                         List<MerchantAddressEntity> merchantAddresses,
                                         List<MerchantContactEntity> merchantContacts,
                                         List<MerchantOperatorEntity> merchantOperators,
                                         List<MerchantDeviceEntity> merchantDevices,
                                         List<MerchantSecurityUserEntity> merchantSecurityUsers)
        {
            MerchantModel merchantModel = this.ConvertFrom(estateId, merchant);

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
                merchantModel.Devices = new List<Device>();
                merchantDevices.ForEach(md => merchantModel.Devices.Add(new Device{
                                                                                      DeviceIdentifier = md.DeviceIdentifier,
                                                                                      DeviceId = md.DeviceId,
                                                                                  }));
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
        public ContractModel ConvertFrom(Guid estateId,
                                         ContractEntity contract,
                                         List<ContractProductEntity> contractProducts,
                                         List<ContractProductTransactionFeeEntity> productTransactionFees)
        {
            ContractModel contractModel = new ContractModel();
            contractModel.EstateId = estateId;
            contractModel.OperatorId = contract.OperatorId;
            contractModel.Description = contract.Description;
            contractModel.IsCreated = true; // Should this be stored at RM or is the fact its in RM mean true???
            contractModel.ContractId = contract.ContractId;
            contractModel.ContractReportingId=contract.ContractReportingId;
            
            if (contractProducts != null && contractProducts.Any())
            {
                contractModel.Products = new List<Product>();

                contractProducts.ForEach(p => contractModel.Products.Add(new Product
                                                                         {
                                                                             ContractProductReportingId = p.ContractProductReportingId,
                                                                             ContractProductId = p.ContractProductId,
                                                                             Value = p.Value,
                                                                             Name = p.ProductName,
                                                                             DisplayText = p.DisplayText,
                                                                             ProductType = (ProductType)p.ProductType
                                                                         }));
            }

            if (productTransactionFees != null && productTransactionFees.Any())
            {
                productTransactionFees.ForEach(f =>
                                               {
                                                   Product product = contractModel.Products.Single(p => p.ContractProductId == f.ContractProductId);

                                                   product.TransactionFees.Add(new ContractProductTransactionFee()
                                                                               {
                                                                                   ContractProductTransactionFeeReportingId = f.ContractProductTransactionFeeReportingId,
                                                                                   TransactionFeeId = f.ContractProductTransactionFeeId,
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
        public List<ContractProductTransactionFeeTransactionFeeModel> ConvertFrom(List<ContractProductTransactionFeeEntity> productTransactionFees)
        {
            List<ContractProductTransactionFeeTransactionFeeModel> productTransactionFeesModelList = new List<ContractProductTransactionFeeTransactionFeeModel>();

            productTransactionFees.ForEach(f =>
                                           {
                                               productTransactionFeesModelList.Add(new ContractProductTransactionFeeTransactionFeeModel
                                               {
                                                                                       TransactionFeeId = f.ContractProductTransactionFeeId,
                                                                                       ContractProductTransactionFeeReportingId = f.ContractProductTransactionFeeReportingId,
                                                                                       Value = f.Value,
                                                                                       Description = f.Description,
                                                                                       CalculationType = (CalculationType)f.CalculationType,
                                                                                       FeeType = (FeeType)f.FeeType,
                                                                                       IsEnabled = f.IsEnabled
                                                                                           }
                                                                                   );
                                           });

            return productTransactionFeesModelList;
        }
        
        #endregion
    }
}