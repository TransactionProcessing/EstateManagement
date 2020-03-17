﻿using System;
using System.Collections.Generic;
using System.Text;

namespace EstateManagement.Tests.Factories
{
    using System.Linq;
    using DataTransferObjects.Responses;
    using EstateAggregate;
    using EstateManagement.Factories;
    using Models;
    using Models.Merchant;
    using Shouldly;
    using Testing;
    using Xunit;

    public class ModelFactoryTests
    {
        [Fact]
        public void ModelFactory_EstateAggregate_WithNoOperatorsOrSecurityUsers_IsConverted()
        {
            Estate estateModel = TestData.EstateModel;

            ModelFactory modelFactory = new ModelFactory();

            EstateResponse estateResponse = modelFactory.ConvertFrom(estateModel);

            estateResponse.ShouldNotBeNull();
            estateResponse.EstateId.ShouldBe(estateModel.EstateId);
            estateResponse.EstateName.ShouldBe(estateModel.Name);
        }

        [Fact]
        public void ModelFactory_EstateAggregate_WithOperators_IsConverted()
        {
            Estate estateModel = TestData.EstateModel;

            ModelFactory modelFactory = new ModelFactory();

            EstateResponse estateResponse = modelFactory.ConvertFrom(estateModel);

            estateResponse.ShouldNotBeNull();
            estateResponse.EstateId.ShouldBe(estateModel.EstateId);
            estateResponse.EstateName.ShouldBe(estateModel.Name);
        }

        [Fact]
        public void ModelFactory_EstateAggregate_WithSecurityUsers_IsConverted()
        {
            Estate estateModel = TestData.EstateModel;

            ModelFactory modelFactory = new ModelFactory();

            EstateResponse estateResponse = modelFactory.ConvertFrom(estateModel);

            estateResponse.ShouldNotBeNull();
            estateResponse.EstateId.ShouldBe(estateModel.EstateId);
            estateResponse.EstateName.ShouldBe(estateModel.Name);
        }

        [Fact]
        public void ModelFactory_EstateAggregate_WithOperatorsAndSecurityUsers_IsConverted()
        {
            Estate estateModel = TestData.EstateModel;

            ModelFactory modelFactory = new ModelFactory();

            EstateResponse estateResponse = modelFactory.ConvertFrom(estateModel);

            estateResponse.ShouldNotBeNull();
            estateResponse.EstateId.ShouldBe(estateModel.EstateId);
            estateResponse.EstateName.ShouldBe(estateModel.Name);
        }

        [Fact]
        public void ModelFactory_EstateAggregate_NullInput_IsConverted()
        {
            Estate estateModel = null;

            ModelFactory modelFactory = new ModelFactory();

            EstateResponse estateResponse = modelFactory.ConvertFrom(estateModel);

            estateResponse.ShouldBeNull();
        }

        [Fact]
        public void ModelFactory_Merchant_IsConverted()
        {
            Merchant merchantModel = TestData.MerchantModelWithAddressesAndContacts;

            ModelFactory modelFactory = new ModelFactory();

            MerchantResponse merchantResponse = modelFactory.ConvertFrom(merchantModel);

            merchantResponse.ShouldNotBeNull();
            merchantResponse.MerchantId.ShouldBe(merchantModel.MerchantId);
            merchantResponse.MerchantName.ShouldBe(merchantModel.MerchantName);
            merchantResponse.EstateId.ShouldBe(merchantModel.EstateId);
            merchantResponse.Addresses.ShouldHaveSingleItem();
            
            AddressResponse addressResponse = merchantResponse.Addresses.Single();
            addressResponse.AddressId.ShouldBe(merchantModel.Addresses.Single().AddressId);
            addressResponse.AddressLine1.ShouldBe(merchantModel.Addresses.Single().AddressLine1);
            addressResponse.AddressLine2.ShouldBe(merchantModel.Addresses.Single().AddressLine2);
            addressResponse.AddressLine3.ShouldBe(merchantModel.Addresses.Single().AddressLine3);
            addressResponse.AddressLine4.ShouldBe(merchantModel.Addresses.Single().AddressLine4);
            addressResponse.Town.ShouldBe(merchantModel.Addresses.Single().Town);
            addressResponse.Region.ShouldBe(merchantModel.Addresses.Single().Region);
            addressResponse.Country.ShouldBe(merchantModel.Addresses.Single().Country);
            addressResponse.PostalCode.ShouldBe(merchantModel.Addresses.Single().PostalCode);

            merchantResponse.Contacts.ShouldHaveSingleItem();
            ContactResponse contactResponse = merchantResponse.Contacts.Single();
            contactResponse.ContactId.ShouldBe(merchantModel.Contacts.Single().ContactId);
            contactResponse.ContactEmailAddress.ShouldBe(merchantModel.Contacts.Single().ContactEmailAddress);
            contactResponse.ContactName.ShouldBe(merchantModel.Contacts.Single().ContactName);
            contactResponse.ContactPhoneNumber.ShouldBe(merchantModel.Contacts.Single().ContactPhoneNumber);
        }

        [Fact]
        public void ModelFactory_Merchant_NullInput_IsConverted()
        {
            Merchant merchantModel = null;

            ModelFactory modelFactory = new ModelFactory();

            MerchantResponse merchantResponse = modelFactory.ConvertFrom(merchantModel);

            merchantResponse.ShouldBeNull();
        }

        [Fact]
        public void ModelFactory_Merchant_NullAddresses_IsConverted()
        {
            Merchant merchantModel = TestData.MerchantModelWithNullAddressesAndWithContacts;

            ModelFactory modelFactory = new ModelFactory();

            MerchantResponse merchantResponse = modelFactory.ConvertFrom(merchantModel);

            merchantResponse.ShouldNotBeNull();
            merchantResponse.MerchantId.ShouldBe(merchantModel.MerchantId);
            merchantResponse.MerchantName.ShouldBe(merchantModel.MerchantName);
            merchantResponse.EstateId.ShouldBe(merchantModel.EstateId);
            merchantResponse.Addresses.ShouldBeNull();
            
            merchantResponse.Contacts.ShouldHaveSingleItem();
            ContactResponse contactResponse = merchantResponse.Contacts.Single();
            contactResponse.ContactId.ShouldBe(merchantModel.Contacts.Single().ContactId);
            contactResponse.ContactEmailAddress.ShouldBe(merchantModel.Contacts.Single().ContactEmailAddress);
            contactResponse.ContactName.ShouldBe(merchantModel.Contacts.Single().ContactName);
            contactResponse.ContactPhoneNumber.ShouldBe(merchantModel.Contacts.Single().ContactPhoneNumber);
        }

        [Fact]
        public void ModelFactory_Merchant_NullContacts_IsConverted()
        {
            Merchant merchantModel = TestData.MerchantModelWithAddressesAndNullContacts;

            ModelFactory modelFactory = new ModelFactory();

            MerchantResponse merchantResponse = modelFactory.ConvertFrom(merchantModel);

            merchantResponse.ShouldNotBeNull();
            merchantResponse.MerchantId.ShouldBe(merchantModel.MerchantId);
            merchantResponse.MerchantName.ShouldBe(merchantModel.MerchantName);
            merchantResponse.EstateId.ShouldBe(merchantModel.EstateId);
            merchantResponse.Addresses.ShouldHaveSingleItem();

            AddressResponse addressResponse = merchantResponse.Addresses.Single();
            addressResponse.AddressId.ShouldBe(merchantModel.Addresses.Single().AddressId);
            addressResponse.AddressLine1.ShouldBe(merchantModel.Addresses.Single().AddressLine1);
            addressResponse.AddressLine2.ShouldBe(merchantModel.Addresses.Single().AddressLine2);
            addressResponse.AddressLine3.ShouldBe(merchantModel.Addresses.Single().AddressLine3);
            addressResponse.AddressLine4.ShouldBe(merchantModel.Addresses.Single().AddressLine4);
            addressResponse.Town.ShouldBe(merchantModel.Addresses.Single().Town);
            addressResponse.Region.ShouldBe(merchantModel.Addresses.Single().Region);
            addressResponse.Country.ShouldBe(merchantModel.Addresses.Single().Country);
            addressResponse.PostalCode.ShouldBe(merchantModel.Addresses.Single().PostalCode);

            merchantResponse.Contacts.ShouldBeNull();
        }
    }
}
