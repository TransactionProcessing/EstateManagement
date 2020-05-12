using System;
using System.Collections.Generic;
using System.Text;
using EstateModel = EstateManagement.Models.Estate.Estate;
using EstateEntity = EstateReporting.Database.Entities.Estate;
using EstateOperatorEntity = EstateReporting.Database.Entities.EstateOperator;
using EstateSecurityUserEntity = EstateReporting.Database.Entities.EstateSecurityUser;

using MerchantEntity = EstateReporting.Database.Entities.Merchant;
using MerchantAddressEntity = EstateReporting.Database.Entities.MerchantAddress;
using MerchantContactEntity = EstateReporting.Database.Entities.MerchantContact;
using MerchantOperatorEntity = EstateReporting.Database.Entities.MerchantOperator;
using MerchantDeviceEntity = EstateReporting.Database.Entities.MerchantDevice;
using MerchantSecurityUserEntity = EstateReporting.Database.Entities.MerchantSecurityUser;

namespace EstateManagement.BusinessLogic.Tests
{
    using EstateAggregate;
    using EstateReporting.Database.Entities;
    using Models;
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
            merchantModel.EstateId.ShouldBe(merchant.EstateId);
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
            merchantModel.EstateId.ShouldBe(merchant.EstateId);
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
            merchantModel.EstateId.ShouldBe(merchant.EstateId);
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
            merchantModel.EstateId.ShouldBe(merchant.EstateId);
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
            merchantModel.EstateId.ShouldBe(merchant.EstateId);
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
            merchantModel.EstateId.ShouldBe(merchant.EstateId);
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
            merchantModel.EstateId.ShouldBe(merchant.EstateId);
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
            merchantModel.EstateId.ShouldBe(merchant.EstateId);
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
            merchantModel.EstateId.ShouldBe(merchant.EstateId);
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
            merchantModel.EstateId.ShouldBe(merchant.EstateId);
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
            merchantModel.EstateId.ShouldBe(merchant.EstateId);
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
    }
}
