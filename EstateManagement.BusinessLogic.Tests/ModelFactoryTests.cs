using System;
using System.Collections.Generic;
using System.Text;
using EstateModel = EstateManagement.Models.Estate.Estate;
using EstateEntity = EstateManagement.Database.Entities.Estate;
using EstateOperatorEntity = EstateManagement.Database.Entities.EstateOperator;
using EstateSecurityUserEntity = EstateManagement.Database.Entities.EstateSecurityUser;

using ContractModel = EstateManagement.Models.Contract.Contract;
using ContractEntity = EstateManagement.Database.Entities.Contract;
using ContractProductEntity = EstateManagement.Database.Entities.ContractProduct;
using ContractProductTransactionFeeEntity = EstateManagement.Database.Entities.ContractProductTransactionFee;
using ContractProductTransactionFeeModel = EstateManagement.Models.Contract.TransactionFee;

using MerchantEntity = EstateManagement.Database.Entities.Merchant;
using MerchantAddressEntity = EstateManagement.Database.Entities.MerchantAddress;
using MerchantContactEntity = EstateManagement.Database.Entities.MerchantContact;
using MerchantOperatorEntity = EstateManagement.Database.Entities.MerchantOperator;
using MerchantDeviceEntity = EstateManagement.Database.Entities.MerchantDevice;
using MerchantSecurityUserEntity = EstateManagement.Database.Entities.MerchantSecurityUser;

namespace EstateManagement.BusinessLogic.Tests
{
    using System.Linq;
    using System.Runtime.InteropServices;
    using EstateAggregate;
    using EstateManagement.Database.Entities;
    using Models;
    using Models.Contract;
    using Models.Factories;
    using Shouldly;
    using Testing;
    using Xunit;
    using Merchant = Models.Merchant.Merchant;

    public class ModelFactoryTests
    {
        [Fact]
        public void EstateEntities_ConvertFrom_EstateConverted()
        {
            EstateEntity estate = TestData.EstateEntity;
            List<EstateOperatorEntity> estateOperators = new List<EstateOperatorEntity>
                                                         {
                                                             TestData.EstateOperatorEntity
                                                         };
            List<EstateSecurityUserEntity> estateSecurityUsers = new List<EstateSecurityUserEntity>
                                                                 {
                                                                     TestData.EstateSecurityUserEntity
                                                                 };

            ModelFactory modelFactory = new ModelFactory();

            EstateModel estateModel = modelFactory.ConvertFrom(estate, estateOperators, estateSecurityUsers);

            estateModel.ShouldNotBeNull();
            estateModel.EstateId.ShouldBe(estate.EstateId);
            estateModel.Name.ShouldBe(estate.Name);
            estateModel.Operators.ShouldNotBeNull();
            estateModel.Operators.Count.ShouldBe(estateOperators.Count);
            estateModel.SecurityUsers.ShouldNotBeNull();
            estateModel.SecurityUsers.Count.ShouldBe(estateSecurityUsers.Count);
        }

        [Fact]
        public void EstateEntities_ConvertFrom_NoOperators_EstateConverted()
        {
            EstateEntity estate = TestData.EstateEntity;
            List<EstateOperatorEntity> estateOperators = new List<EstateOperatorEntity>();
            List<EstateSecurityUserEntity> estateSecurityUsers = new List<EstateSecurityUserEntity>
                                                                 {
                                                                     TestData.EstateSecurityUserEntity
                                                                 };

            ModelFactory modelFactory = new ModelFactory();

            EstateModel estateModel = modelFactory.ConvertFrom(estate, estateOperators, estateSecurityUsers);

            estateModel.ShouldNotBeNull();
            estateModel.EstateId.ShouldBe(estate.EstateId);
            estateModel.Name.ShouldBe(estate.Name);
            estateModel.Operators.ShouldBeNull();
            estateModel.SecurityUsers.ShouldNotBeNull();
            estateModel.SecurityUsers.Count.ShouldBe(estateSecurityUsers.Count);
        }

        [Fact]
        public void EstateEntities_ConvertFrom_NullOperators_EstateConverted()
        {
            EstateEntity estate = TestData.EstateEntity;
            List<EstateOperatorEntity> estateOperators = null;
            List<EstateSecurityUserEntity> estateSecurityUsers = new List<EstateSecurityUserEntity>
                                                                 {
                                                                     TestData.EstateSecurityUserEntity
                                                                 };

            ModelFactory modelFactory = new ModelFactory();

            EstateModel estateModel = modelFactory.ConvertFrom(estate, estateOperators, estateSecurityUsers);

            estateModel.ShouldNotBeNull();
            estateModel.EstateId.ShouldBe(estate.EstateId);
            estateModel.Name.ShouldBe(estate.Name);
            estateModel.Operators.ShouldBeNull();
            estateModel.SecurityUsers.ShouldNotBeNull();
            estateModel.SecurityUsers.Count.ShouldBe(estateSecurityUsers.Count);
        }

        [Fact]
        public void EstateEntities_ConvertFrom_NoSecurityUsers_EstateConverted()
        {
            EstateEntity estate = TestData.EstateEntity;
            List<EstateOperatorEntity> estateOperators = new List<EstateOperatorEntity>
                                                         {
                                                             TestData.EstateOperatorEntity
                                                         };
            List<EstateSecurityUserEntity> estateSecurityUsers = null;

            ModelFactory modelFactory = new ModelFactory();

            EstateModel estateModel = modelFactory.ConvertFrom(estate, estateOperators, estateSecurityUsers);

            estateModel.ShouldNotBeNull();
            estateModel.EstateId.ShouldBe(estate.EstateId);
            estateModel.Name.ShouldBe(estate.Name);
            estateModel.Operators.ShouldNotBeNull();
            estateModel.Operators.Count.ShouldBe(estateOperators.Count);
            estateModel.SecurityUsers.ShouldBeNull();
        }

        [Fact]
        public void EstateEntities_ConvertFrom_NullSecurityUsers_EstateConverted()
        {
            EstateEntity estate = TestData.EstateEntity;
            List<EstateOperatorEntity> estateOperators = new List<EstateOperatorEntity>
                                                         {
                                                             TestData.EstateOperatorEntity
                                                         };
            List<EstateSecurityUserEntity> estateSecurityUsers = null;

            ModelFactory modelFactory = new ModelFactory();

            EstateModel estateModel = modelFactory.ConvertFrom(estate, estateOperators, estateSecurityUsers);

            estateModel.ShouldNotBeNull();
            estateModel.EstateId.ShouldBe(estate.EstateId);
            estateModel.Name.ShouldBe(estate.Name);
            estateModel.Operators.ShouldNotBeNull();
            estateModel.Operators.Count.ShouldBe(estateOperators.Count);
            estateModel.SecurityUsers.ShouldBeNull();
        }

        [Fact]
        public void MerchantEntities_ConvertFrom_MerchantConverted()
        {
            MerchantEntity merchant = TestData.MerchantEntity;
            List<MerchantAddress> merchantAddresses = new List<MerchantAddressEntity>
                                                      {
                                                          TestData.MerchantAddressEntity
                                                      };
            List<MerchantContact> merchantContacts = new List<MerchantContactEntity>
                                                     {
                                                         TestData.MerchantContactEntity
                                                     };
            List<MerchantOperator> merchantOperators = new List<MerchantOperatorEntity>
                                                       {
                                                           TestData.MerchantOperatorEntity
                                                       };
            List<MerchantDevice> merchantDevices = new List<MerchantDeviceEntity>
                                                   {
                                                       TestData.MerchantDeviceEntity
                                                   };
            List<MerchantSecurityUser> merchantSecurityUsers = new List<MerchantSecurityUserEntity>
                                                               {
                                                                   TestData.MerchantSecurityUserEntity
                                                               };
            ModelFactory modelFactory = new ModelFactory();

            Merchant merchantModel = modelFactory.ConvertFrom(merchant, merchantAddresses, merchantContacts, merchantOperators, merchantDevices, merchantSecurityUsers);

            merchantModel.ShouldNotBeNull();
            merchantModel.MerchantId.ShouldBe(merchant.MerchantId);
            merchantModel.MerchantName.ShouldBe(merchant.Name);
            merchantModel.Addresses.ShouldNotBeNull();
            merchantModel.Addresses.Count.ShouldBe(merchantAddresses.Count);
            merchantModel.Contacts.ShouldNotBeNull();
            merchantModel.Contacts.Count.ShouldBe(merchantContacts.Count);
            merchantModel.Operators.ShouldNotBeNull();
            merchantModel.Operators.Count.ShouldBe(merchantOperators.Count);
            merchantModel.Devices.ShouldNotBeNull();
            merchantModel.Devices.Count.ShouldBe(merchantDevices.Count);
            merchantModel.SecurityUsers.ShouldNotBeNull();
            merchantModel.SecurityUsers.Count.ShouldBe(merchantSecurityUsers.Count);
        }

        [Fact]
        public void MerchantEntities_ConvertFrom_NoAddresses_MerchantConverted()
        {
            MerchantEntity merchant = TestData.MerchantEntity;
            List<MerchantAddress> merchantAddresses = new List<MerchantAddressEntity>();
            List<MerchantContact> merchantContacts = new List<MerchantContactEntity>
                                                     {
                                                         TestData.MerchantContactEntity
                                                     };
            List<MerchantOperator> merchantOperators = new List<MerchantOperatorEntity>
                                                       {
                                                           TestData.MerchantOperatorEntity
                                                       };
            List<MerchantDevice> merchantDevices = new List<MerchantDeviceEntity>
                                                   {
                                                       TestData.MerchantDeviceEntity
                                                   };
            List<MerchantSecurityUser> merchantSecurityUsers = new List<MerchantSecurityUserEntity>
                                                               {
                                                                   TestData.MerchantSecurityUserEntity
                                                               };
            ModelFactory modelFactory = new ModelFactory();

            Merchant merchantModel = modelFactory.ConvertFrom(merchant, merchantAddresses, merchantContacts, merchantOperators, merchantDevices, merchantSecurityUsers);

            merchantModel.ShouldNotBeNull();
            merchantModel.MerchantId.ShouldBe(merchant.MerchantId);
            merchantModel.MerchantName.ShouldBe(merchant.Name);
            merchantModel.Addresses.ShouldBeNull();
            merchantModel.Contacts.ShouldNotBeNull();
            merchantModel.Contacts.Count.ShouldBe(merchantContacts.Count);
            merchantModel.Operators.ShouldNotBeNull();
            merchantModel.Operators.Count.ShouldBe(merchantOperators.Count);
            merchantModel.Devices.ShouldNotBeNull();
            merchantModel.Devices.Count.ShouldBe(merchantDevices.Count);
            merchantModel.SecurityUsers.ShouldNotBeNull();
            merchantModel.SecurityUsers.Count.ShouldBe(merchantSecurityUsers.Count);
        }

        [Fact]
        public void MerchantEntities_ConvertFrom_NullAddresses_MerchantConverted()
        {
            MerchantEntity merchant = TestData.MerchantEntity;
            List<MerchantAddress> merchantAddresses = null;
            List<MerchantContact> merchantContacts = new List<MerchantContactEntity>
                                                     {
                                                         TestData.MerchantContactEntity
                                                     };
            List<MerchantOperator> merchantOperators = new List<MerchantOperatorEntity>
                                                       {
                                                           TestData.MerchantOperatorEntity
                                                       };
            List<MerchantDevice> merchantDevices = new List<MerchantDeviceEntity>
                                                   {
                                                       TestData.MerchantDeviceEntity
                                                   };
            List<MerchantSecurityUser> merchantSecurityUsers = new List<MerchantSecurityUserEntity>
                                                               {
                                                                   TestData.MerchantSecurityUserEntity
                                                               };
            ModelFactory modelFactory = new ModelFactory();

            Merchant merchantModel = modelFactory.ConvertFrom(merchant, merchantAddresses, merchantContacts, merchantOperators, merchantDevices, merchantSecurityUsers);

            merchantModel.ShouldNotBeNull();
            merchantModel.MerchantId.ShouldBe(merchant.MerchantId);
            merchantModel.MerchantName.ShouldBe(merchant.Name);
            merchantModel.Addresses.ShouldBeNull();
            merchantModel.Contacts.ShouldNotBeNull();
            merchantModel.Contacts.Count.ShouldBe(merchantContacts.Count);
            merchantModel.Operators.ShouldNotBeNull();
            merchantModel.Operators.Count.ShouldBe(merchantOperators.Count);
            merchantModel.Devices.ShouldNotBeNull();
            merchantModel.Devices.Count.ShouldBe(merchantDevices.Count);
            merchantModel.SecurityUsers.ShouldNotBeNull();
            merchantModel.SecurityUsers.Count.ShouldBe(merchantSecurityUsers.Count);
        }

        [Fact]
        public void MerchantEntities_ConvertFrom_NoContacts_MerchantConverted()
        {
            MerchantEntity merchant = TestData.MerchantEntity;
            List<MerchantAddress> merchantAddresses = new List<MerchantAddressEntity>
                                                      {
                                                          TestData.MerchantAddressEntity
                                                      };
            List<MerchantContact> merchantContacts = new List<MerchantContactEntity>();
            List<MerchantOperator> merchantOperators = new List<MerchantOperatorEntity>
                                                       {
                                                           TestData.MerchantOperatorEntity
                                                       };
            List<MerchantDevice> merchantDevices = new List<MerchantDeviceEntity>
                                                   {
                                                       TestData.MerchantDeviceEntity
                                                   };
            List<MerchantSecurityUser> merchantSecurityUsers = new List<MerchantSecurityUserEntity>
                                                               {
                                                                   TestData.MerchantSecurityUserEntity
                                                               };
            ModelFactory modelFactory = new ModelFactory();

            Merchant merchantModel = modelFactory.ConvertFrom(merchant, merchantAddresses, merchantContacts, merchantOperators, merchantDevices, merchantSecurityUsers);

            merchantModel.ShouldNotBeNull();
            merchantModel.MerchantId.ShouldBe(merchant.MerchantId);
            merchantModel.MerchantName.ShouldBe(merchant.Name);
            merchantModel.Addresses.ShouldNotBeNull();
            merchantModel.Addresses.Count.ShouldBe(merchantAddresses.Count);
            merchantModel.Contacts.ShouldBeNull();
            merchantModel.Operators.ShouldNotBeNull();
            merchantModel.Operators.Count.ShouldBe(merchantOperators.Count);
            merchantModel.Devices.ShouldNotBeNull();
            merchantModel.Devices.Count.ShouldBe(merchantDevices.Count);
            merchantModel.SecurityUsers.ShouldNotBeNull();
            merchantModel.SecurityUsers.Count.ShouldBe(merchantSecurityUsers.Count);
        }

        [Fact]
        public void MerchantEntities_ConvertFrom_NullContacts_MerchantConverted()
        {
            MerchantEntity merchant = TestData.MerchantEntity;
            List<MerchantAddress> merchantAddresses = new List<MerchantAddressEntity>
                                                      {
                                                          TestData.MerchantAddressEntity
                                                      };
            List<MerchantContact> merchantContacts = null;
            List<MerchantOperator> merchantOperators = new List<MerchantOperatorEntity>
                                                       {
                                                           TestData.MerchantOperatorEntity
                                                       };
            List<MerchantDevice> merchantDevices = new List<MerchantDeviceEntity>
                                                   {
                                                       TestData.MerchantDeviceEntity
                                                   };
            List<MerchantSecurityUser> merchantSecurityUsers = new List<MerchantSecurityUserEntity>
                                                               {
                                                                   TestData.MerchantSecurityUserEntity
                                                               };
            ModelFactory modelFactory = new ModelFactory();

            Merchant merchantModel = modelFactory.ConvertFrom(merchant, merchantAddresses, merchantContacts, merchantOperators, merchantDevices, merchantSecurityUsers);

            merchantModel.ShouldNotBeNull();
            merchantModel.MerchantId.ShouldBe(merchant.MerchantId);
            merchantModel.MerchantName.ShouldBe(merchant.Name);
            merchantModel.Addresses.ShouldNotBeNull();
            merchantModel.Addresses.Count.ShouldBe(merchantAddresses.Count);
            merchantModel.Contacts.ShouldBeNull();
            merchantModel.Operators.ShouldNotBeNull();
            merchantModel.Operators.Count.ShouldBe(merchantOperators.Count);
            merchantModel.Devices.ShouldNotBeNull();
            merchantModel.Devices.Count.ShouldBe(merchantDevices.Count);
            merchantModel.SecurityUsers.ShouldNotBeNull();
            merchantModel.SecurityUsers.Count.ShouldBe(merchantSecurityUsers.Count);
        }

        [Fact]
        public void MerchantEntities_ConvertFrom_NoOperators_MerchantConverted()
        {
            MerchantEntity merchant = TestData.MerchantEntity;
            List<MerchantAddress> merchantAddresses = new List<MerchantAddressEntity>
                                                      {
                                                          TestData.MerchantAddressEntity
                                                      };
            List<MerchantContact> merchantContacts = new List<MerchantContactEntity>
                                                     {
                                                         TestData.MerchantContactEntity
                                                     };
            List<MerchantOperator> merchantOperators = new List<MerchantOperatorEntity>();
            List<MerchantDevice> merchantDevices = new List<MerchantDeviceEntity>
                                                   {
                                                       TestData.MerchantDeviceEntity
                                                   };
            List<MerchantSecurityUser> merchantSecurityUsers = new List<MerchantSecurityUserEntity>
                                                               {
                                                                   TestData.MerchantSecurityUserEntity
                                                               };
            ModelFactory modelFactory = new ModelFactory();

            Merchant merchantModel = modelFactory.ConvertFrom(merchant, merchantAddresses, merchantContacts, merchantOperators, merchantDevices, merchantSecurityUsers);

            merchantModel.ShouldNotBeNull();
            merchantModel.MerchantId.ShouldBe(merchant.MerchantId);
            merchantModel.MerchantName.ShouldBe(merchant.Name);
            merchantModel.Addresses.ShouldNotBeNull();
            merchantModel.Addresses.Count.ShouldBe(merchantAddresses.Count);
            merchantModel.Contacts.ShouldNotBeNull();
            merchantModel.Contacts.Count.ShouldBe(merchantContacts.Count);
            merchantModel.Operators.ShouldBeNull();
            merchantModel.Devices.ShouldNotBeNull();
            merchantModel.Devices.Count.ShouldBe(merchantDevices.Count);
            merchantModel.SecurityUsers.ShouldNotBeNull();
            merchantModel.SecurityUsers.Count.ShouldBe(merchantSecurityUsers.Count);
        }

        [Fact]
        public void MerchantEntities_ConvertFrom_NullOperators_MerchantConverted()
        {
            MerchantEntity merchant = TestData.MerchantEntity;
            List<MerchantAddress> merchantAddresses = new List<MerchantAddressEntity>
                                                      {
                                                          TestData.MerchantAddressEntity
                                                      };
            List<MerchantContact> merchantContacts = new List<MerchantContactEntity>
                                                     {
                                                         TestData.MerchantContactEntity
                                                     };
            List<MerchantOperator> merchantOperators = null;
            List<MerchantDevice> merchantDevices = new List<MerchantDeviceEntity>
                                                   {
                                                       TestData.MerchantDeviceEntity
                                                   };
            List<MerchantSecurityUser> merchantSecurityUsers = new List<MerchantSecurityUserEntity>
                                                               {
                                                                   TestData.MerchantSecurityUserEntity
                                                               };
            ModelFactory modelFactory = new ModelFactory();

            Merchant merchantModel = modelFactory.ConvertFrom(merchant, merchantAddresses, merchantContacts, merchantOperators, merchantDevices, merchantSecurityUsers);

            merchantModel.ShouldNotBeNull();
            merchantModel.MerchantId.ShouldBe(merchant.MerchantId);
            merchantModel.MerchantName.ShouldBe(merchant.Name);
            merchantModel.Addresses.ShouldNotBeNull();
            merchantModel.Addresses.Count.ShouldBe(merchantAddresses.Count);
            merchantModel.Contacts.ShouldNotBeNull();
            merchantModel.Contacts.Count.ShouldBe(merchantContacts.Count);
            merchantModel.Operators.ShouldBeNull();
            merchantModel.Devices.ShouldNotBeNull();
            merchantModel.Devices.Count.ShouldBe(merchantDevices.Count);
            merchantModel.SecurityUsers.ShouldNotBeNull();
            merchantModel.SecurityUsers.Count.ShouldBe(merchantSecurityUsers.Count);
        }

        [Fact]
        public void MerchantEntities_ConvertFrom_NoDevices_MerchantConverted()
        {
            MerchantEntity merchant = TestData.MerchantEntity;
            List<MerchantAddress> merchantAddresses = new List<MerchantAddressEntity>
                                                      {
                                                          TestData.MerchantAddressEntity
                                                      };
            List<MerchantContact> merchantContacts = new List<MerchantContactEntity>
                                                     {
                                                         TestData.MerchantContactEntity
                                                     };
            List<MerchantOperator> merchantOperators = new List<MerchantOperatorEntity>
                                                       {
                                                           TestData.MerchantOperatorEntity
                                                       };
            List<MerchantDevice> merchantDevices = new List<MerchantDeviceEntity>();
            List<MerchantSecurityUser> merchantSecurityUsers = new List<MerchantSecurityUserEntity>
                                                               {
                                                                   TestData.MerchantSecurityUserEntity
                                                               };
            ModelFactory modelFactory = new ModelFactory();

            Merchant merchantModel = modelFactory.ConvertFrom(merchant, merchantAddresses, merchantContacts, merchantOperators, merchantDevices, merchantSecurityUsers);

            merchantModel.ShouldNotBeNull();
            merchantModel.MerchantId.ShouldBe(merchant.MerchantId);
            merchantModel.MerchantName.ShouldBe(merchant.Name);
            merchantModel.Addresses.ShouldNotBeNull();
            merchantModel.Addresses.Count.ShouldBe(merchantAddresses.Count);
            merchantModel.Contacts.ShouldNotBeNull();
            merchantModel.Contacts.Count.ShouldBe(merchantContacts.Count);
            merchantModel.Operators.ShouldNotBeNull();
            merchantModel.Operators.Count.ShouldBe(merchantOperators.Count);
            merchantModel.Devices.ShouldBeNull();
            merchantModel.SecurityUsers.ShouldNotBeNull();
            merchantModel.SecurityUsers.Count.ShouldBe(merchantSecurityUsers.Count);
        }

        [Fact]
        public void MerchantEntities_ConvertFrom_NullDevices_MerchantConverted()
        {
            MerchantEntity merchant = TestData.MerchantEntity;
            List<MerchantAddress> merchantAddresses = new List<MerchantAddressEntity>
                                                      {
                                                          TestData.MerchantAddressEntity
                                                      };
            List<MerchantContact> merchantContacts = new List<MerchantContactEntity>
                                                     {
                                                         TestData.MerchantContactEntity
                                                     };
            List<MerchantOperator> merchantOperators = new List<MerchantOperatorEntity>
                                                       {
                                                           TestData.MerchantOperatorEntity
                                                       };
            List<MerchantDevice> merchantDevices = null;
            List<MerchantSecurityUser> merchantSecurityUsers = new List<MerchantSecurityUserEntity>
                                                               {
                                                                   TestData.MerchantSecurityUserEntity
                                                               };
            ModelFactory modelFactory = new ModelFactory();

            Merchant merchantModel = modelFactory.ConvertFrom(merchant, merchantAddresses, merchantContacts, merchantOperators, merchantDevices, merchantSecurityUsers);

            merchantModel.ShouldNotBeNull();
            merchantModel.MerchantId.ShouldBe(merchant.MerchantId);
            merchantModel.MerchantName.ShouldBe(merchant.Name);
            merchantModel.Addresses.ShouldNotBeNull();
            merchantModel.Addresses.Count.ShouldBe(merchantAddresses.Count);
            merchantModel.Contacts.ShouldNotBeNull();
            merchantModel.Contacts.Count.ShouldBe(merchantContacts.Count);
            merchantModel.Operators.ShouldNotBeNull();
            merchantModel.Operators.Count.ShouldBe(merchantOperators.Count);
            merchantModel.Devices.ShouldBeNull();
            merchantModel.SecurityUsers.ShouldNotBeNull();
            merchantModel.SecurityUsers.Count.ShouldBe(merchantSecurityUsers.Count);
        }

        [Fact]
        public void MerchantEntities_ConvertFrom_NoSecurityUsers_MerchantConverted()
        {
            MerchantEntity merchant = TestData.MerchantEntity;
            List<MerchantAddress> merchantAddresses = new List<MerchantAddressEntity>
                                                      {
                                                          TestData.MerchantAddressEntity
                                                      };
            List<MerchantContact> merchantContacts = new List<MerchantContactEntity>
                                                     {
                                                         TestData.MerchantContactEntity
                                                     };
            List<MerchantOperator> merchantOperators = new List<MerchantOperatorEntity>
                                                       {
                                                           TestData.MerchantOperatorEntity
                                                       };
            List<MerchantDevice> merchantDevices = new List<MerchantDeviceEntity>
                                                   {
                                                       TestData.MerchantDeviceEntity
                                                   };
            List<MerchantSecurityUser> merchantSecurityUsers = new List<MerchantSecurityUserEntity>();

            ModelFactory modelFactory = new ModelFactory();

            Merchant merchantModel = modelFactory.ConvertFrom(merchant, merchantAddresses, merchantContacts, merchantOperators, merchantDevices, merchantSecurityUsers);

            merchantModel.ShouldNotBeNull();
            merchantModel.MerchantId.ShouldBe(merchant.MerchantId);
            merchantModel.MerchantName.ShouldBe(merchant.Name);
            merchantModel.Addresses.ShouldNotBeNull();
            merchantModel.Addresses.Count.ShouldBe(merchantAddresses.Count);
            merchantModel.Contacts.ShouldNotBeNull();
            merchantModel.Contacts.Count.ShouldBe(merchantContacts.Count);
            merchantModel.Operators.ShouldNotBeNull();
            merchantModel.Operators.Count.ShouldBe(merchantOperators.Count);
            merchantModel.Devices.ShouldNotBeNull();
            merchantModel.Devices.Count.ShouldBe(merchantDevices.Count);
            merchantModel.SecurityUsers.ShouldBeNull();
        }

        [Fact]
        public void MerchantEntities_ConvertFrom_NullSecurityUsers_MerchantConverted()
        {
            MerchantEntity merchant = TestData.MerchantEntity;
            List<MerchantAddress> merchantAddresses = new List<MerchantAddressEntity>
                                                      {
                                                          TestData.MerchantAddressEntity
                                                      };
            List<MerchantContact> merchantContacts = new List<MerchantContactEntity>
                                                     {
                                                         TestData.MerchantContactEntity
                                                     };
            List<MerchantOperator> merchantOperators = new List<MerchantOperatorEntity>
                                                       {
                                                           TestData.MerchantOperatorEntity
                                                       };
            List<MerchantDevice> merchantDevices = new List<MerchantDeviceEntity>
                                                   {
                                                       TestData.MerchantDeviceEntity
                                                   };
            List<MerchantSecurityUser> merchantSecurityUsers = null;

            ModelFactory modelFactory = new ModelFactory();

            Merchant merchantModel = modelFactory.ConvertFrom(merchant, merchantAddresses, merchantContacts, merchantOperators, merchantDevices, merchantSecurityUsers);

            merchantModel.ShouldNotBeNull();
            merchantModel.MerchantId.ShouldBe(merchant.MerchantId);
            merchantModel.MerchantName.ShouldBe(merchant.Name);
            merchantModel.Addresses.ShouldNotBeNull();
            merchantModel.Addresses.Count.ShouldBe(merchantAddresses.Count);
            merchantModel.Contacts.ShouldNotBeNull();
            merchantModel.Contacts.Count.ShouldBe(merchantContacts.Count);
            merchantModel.Operators.ShouldNotBeNull();
            merchantModel.Operators.Count.ShouldBe(merchantOperators.Count);
            merchantModel.Devices.ShouldNotBeNull();
            merchantModel.Devices.Count.ShouldBe(merchantDevices.Count);
            merchantModel.SecurityUsers.ShouldBeNull();
        }

        [Fact]
        public void ContractEntities_ContractProductTransactionFee_ConvertFrom_TransactionFeeConverted()
        {
            List<ContractProductTransactionFeeEntity> contractProductsTransactionFees = new List<ContractProductTransactionFeeEntity>
                                                                                        {
                                                                                            TestData.ContractProductTransactionFeeEntity
                                                                                        };

            ModelFactory modelFactory = new ModelFactory();
            List<ContractProductTransactionFeeModel> transactionFeesModelList = modelFactory.ConvertFrom(contractProductsTransactionFees);

            transactionFeesModelList.ShouldNotBeNull();
            transactionFeesModelList.ShouldHaveSingleItem();

            ContractProductTransactionFeeModel contractProductTransactionFee = transactionFeesModelList.Single();
            ContractProductTransactionFee expectedContractProductTransactionFee = contractProductsTransactionFees.Single();
            contractProductTransactionFee.TransactionFeeId.ShouldBe(expectedContractProductTransactionFee.TransactionFeeId);
            contractProductTransactionFee.Description.ShouldBe(expectedContractProductTransactionFee.Description);
            contractProductTransactionFee.Value.ShouldBe(expectedContractProductTransactionFee.Value);
            contractProductTransactionFee.CalculationType.ShouldBe((CalculationType)expectedContractProductTransactionFee.CalculationType);
        }

        [Fact]
        public void ContractEntities_ConvertFrom_ContractConverted()
        {
            ContractEntity contract = TestData.ContractEntity;
            List<ContractProductEntity> contractProducts = new List<ContractProductEntity>
                                                      {
                                                          TestData.ContractProductEntity
                                                      };
            List<ContractProductTransactionFeeEntity> contractProductsTransactionFees= new List<ContractProductTransactionFeeEntity>
                                                                         {
                                                                             TestData.ContractProductTransactionFeeEntity
                                                                         };

            ModelFactory modelFactory = new ModelFactory();

            ContractModel contractModel = modelFactory.ConvertFrom(contract, contractProducts, contractProductsTransactionFees);

            contractModel.ShouldNotBeNull();
            contractModel.ContractId.ShouldBe(contract.ContractId);
            contractModel.OperatorId.ShouldBe(contract.OperatorId);
            contractModel.Description.ShouldBe(contract.Description);
            contractModel.Products.ShouldNotBeNull();
            contractModel.Products.ShouldHaveSingleItem();

            Product contractProduct = contractModel.Products.Single();
            ContractProduct expectedContractProduct = contractProducts.Single();
            contractProduct.ProductId.ShouldBe(expectedContractProduct.ProductId);
            contractProduct.DisplayText.ShouldBe(expectedContractProduct.DisplayText);
            contractProduct.Name.ShouldBe(expectedContractProduct.ProductName);
            contractProduct.Value.ShouldBe(expectedContractProduct.Value);
            contractProduct.ProductType.ShouldBe((ProductType)expectedContractProduct.ProductType);
            contractProduct.TransactionFees.ShouldNotBeNull();
            contractProduct.TransactionFees.ShouldHaveSingleItem();

            ContractProductTransactionFeeModel contractProductTransactionFee = contractProduct.TransactionFees.Single();
            ContractProductTransactionFee expectedContractProductTransactionFee = contractProductsTransactionFees.Single();
            contractProductTransactionFee.TransactionFeeId.ShouldBe(expectedContractProductTransactionFee.TransactionFeeId);
            contractProductTransactionFee.Description.ShouldBe(expectedContractProductTransactionFee.Description);
            contractProductTransactionFee.Value.ShouldBe(expectedContractProductTransactionFee.Value);
            contractProductTransactionFee.CalculationType.ShouldBe((CalculationType)expectedContractProductTransactionFee.CalculationType);
        }

        [Fact]
        public void ContractEntities_ConvertFrom_NullProducts_ContractConverted()
        {
            ContractEntity contract = TestData.ContractEntity;
            List<ContractProductEntity> contractProducts = null;
            List<ContractProductTransactionFeeEntity> contractProductsTransactionFees = null;

            ModelFactory modelFactory = new ModelFactory();

            ContractModel contractModel = modelFactory.ConvertFrom(contract, contractProducts, contractProductsTransactionFees);

            contractModel.ShouldNotBeNull();
            contractModel.ContractId.ShouldBe(contract.ContractId);
            contractModel.OperatorId.ShouldBe(contract.OperatorId);
            contractModel.Description.ShouldBe(contract.Description);
            contractModel.Products.ShouldBeNull();
        }

        [Fact]
        public void ContractEntities_ConvertFrom_EmptyProducts_ContractConverted()
        {
            ContractEntity contract = TestData.ContractEntity;
            List<ContractProductEntity> contractProducts = new List<ContractProduct>();
            List<ContractProductTransactionFeeEntity> contractProductsTransactionFees = null;

            ModelFactory modelFactory = new ModelFactory();

            ContractModel contractModel = modelFactory.ConvertFrom(contract, contractProducts, contractProductsTransactionFees);

            contractModel.ShouldNotBeNull();
            contractModel.ContractId.ShouldBe(contract.ContractId);
            contractModel.OperatorId.ShouldBe(contract.OperatorId);
            contractModel.Description.ShouldBe(contract.Description);
            contractModel.Products.ShouldBeNull();
        }

        [Fact]
        public void ContractEntities_ConvertFrom_NullProductTransactionFees_ContractConverted()
        {
            ContractEntity contract = TestData.ContractEntity;
            List<ContractProductEntity> contractProducts = new List<ContractProductEntity>
                                                      {
                                                          TestData.ContractProductEntity
                                                      };
            List<ContractProductTransactionFeeEntity> contractProductsTransactionFees = null;

            ModelFactory modelFactory = new ModelFactory();

            ContractModel contractModel = modelFactory.ConvertFrom(contract, contractProducts, contractProductsTransactionFees);

            contractModel.ShouldNotBeNull();
            contractModel.ContractId.ShouldBe(contract.ContractId);
            contractModel.OperatorId.ShouldBe(contract.OperatorId);
            contractModel.Description.ShouldBe(contract.Description);
            contractModel.Products.ShouldNotBeNull();
            contractModel.Products.ShouldHaveSingleItem();

            Product contractProduct = contractModel.Products.Single();
            ContractProduct expectedContractProduct = contractProducts.Single();
            contractProduct.ProductId.ShouldBe(expectedContractProduct.ProductId);
            contractProduct.DisplayText.ShouldBe(expectedContractProduct.DisplayText);
            contractProduct.Name.ShouldBe(expectedContractProduct.ProductName);
            contractProduct.Value.ShouldBe(expectedContractProduct.Value);
            contractProduct.TransactionFees.ShouldBeEmpty();
        }

        [Fact]
        public void ContractEntities_ConvertFrom_EmptyProductTransactionFees_ContractConverted()
        {
            ContractEntity contract = TestData.ContractEntity;
            List<ContractProductEntity> contractProducts = new List<ContractProductEntity>
                                                      {
                                                          TestData.ContractProductEntity
                                                      };
            List<ContractProductTransactionFeeEntity> contractProductsTransactionFees = new List<ContractProductTransactionFeeEntity>();

            ModelFactory modelFactory = new ModelFactory();

            ContractModel contractModel = modelFactory.ConvertFrom(contract, contractProducts, contractProductsTransactionFees);

            contractModel.ShouldNotBeNull();
            contractModel.ContractId.ShouldBe(contract.ContractId);
            contractModel.OperatorId.ShouldBe(contract.OperatorId);
            contractModel.Description.ShouldBe(contract.Description);
            contractModel.Products.ShouldNotBeNull();
            contractModel.Products.ShouldHaveSingleItem();

            Product contractProduct = contractModel.Products.Single();
            ContractProduct expectedContractProduct = contractProducts.Single();
            contractProduct.ProductId.ShouldBe(expectedContractProduct.ProductId);
            contractProduct.DisplayText.ShouldBe(expectedContractProduct.DisplayText);
            contractProduct.Name.ShouldBe(expectedContractProduct.ProductName);
            contractProduct.Value.ShouldBe(expectedContractProduct.Value);
            contractProduct.TransactionFees.ShouldBeEmpty();
        }

        
    }
}
