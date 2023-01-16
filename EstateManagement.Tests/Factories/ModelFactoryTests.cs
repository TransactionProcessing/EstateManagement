using System;
using System.Collections.Generic;
using System.Text;

namespace EstateManagement.Tests.Factories
{
    using System.Linq;
    using DataTransferObjects.Responses;
    using EstateAggregate;
    using EstateManagement.Factories;
    using Models;
    using Models.Contract;
    using Models.Estate;
    using Models.Merchant;
    using Shouldly;
    using Testing;
    using Xunit;

    public class ModelFactoryTests
    {
        [Fact]
        public void ModelFactory_Estate_WithNoOperatorsOrSecurityUsers_IsConverted()
        {
            Estate estateModel = TestData.EstateModel;

            ModelFactory modelFactory = new ModelFactory();

            EstateResponse estateResponse = modelFactory.ConvertFrom(estateModel);

            estateResponse.ShouldNotBeNull();
            estateResponse.EstateId.ShouldBe(estateModel.EstateId);
            estateResponse.EstateName.ShouldBe(estateModel.Name);
        }

        [Fact]
        public void ModelFactory_Estate_WithOperators_IsConverted()
        {
            Estate estateModel = TestData.EstateModelWithOperators;

            ModelFactory modelFactory = new ModelFactory();

            EstateResponse estateResponse = modelFactory.ConvertFrom(estateModel);

            estateResponse.ShouldNotBeNull();
            estateResponse.EstateId.ShouldBe(estateModel.EstateId);
            estateResponse.EstateName.ShouldBe(estateModel.Name);
            estateResponse.Operators.ShouldNotBeNull();
            estateResponse.Operators.Count.ShouldBe(estateModel.Operators.Count);
            estateResponse.SecurityUsers.ShouldBeEmpty();
        }

        [Fact]
        public void ModelFactory_Estate_WithSecurityUsers_IsConverted()
        {
            Estate estateModel = TestData.EstateModelWithSecurityUsers;

            ModelFactory modelFactory = new ModelFactory();

            EstateResponse estateResponse = modelFactory.ConvertFrom(estateModel);

            estateResponse.ShouldNotBeNull();
            estateResponse.EstateId.ShouldBe(estateModel.EstateId);
            estateResponse.EstateName.ShouldBe(estateModel.Name);
            estateResponse.Operators.ShouldBeEmpty();
            estateResponse.SecurityUsers.ShouldNotBeNull();
            estateResponse.SecurityUsers.Count.ShouldBe(estateModel.SecurityUsers.Count);
        }

        [Fact]
        public void ModelFactory_Estate_WithOperatorsAndSecurityUsers_IsConverted()
        {
            Estate estateModel = TestData.EstateModelWithOperatorsAndSecurityUsers;

            ModelFactory modelFactory = new ModelFactory();

            EstateResponse estateResponse = modelFactory.ConvertFrom(estateModel);

            estateResponse.ShouldNotBeNull();
            estateResponse.EstateId.ShouldBe(estateModel.EstateId);
            estateResponse.EstateName.ShouldBe(estateModel.Name);
            estateResponse.Operators.ShouldNotBeNull();
            estateResponse.Operators.Count.ShouldBe(estateModel.Operators.Count);
            estateResponse.SecurityUsers.ShouldNotBeNull();
            estateResponse.SecurityUsers.Count.ShouldBe(estateModel.SecurityUsers.Count);
        }

        [Fact]
        public void ModelFactory_Estate_NullInput_IsConverted()
        {
            Estate estateModel = null;

            ModelFactory modelFactory = new ModelFactory();

            EstateResponse estateResponse = modelFactory.ConvertFrom(estateModel);

            estateResponse.ShouldBeNull();
        }

        [Theory]
        [InlineData(SettlementSchedule.NotSet)]
        [InlineData(SettlementSchedule.Immediate)]
        [InlineData(SettlementSchedule.Weekly)]
        [InlineData(SettlementSchedule.Monthly)]
        public void ModelFactory_Merchant_IsConverted(SettlementSchedule settlementSchedule)
        {
            Merchant merchantModel = TestData.MerchantModelWithAddressesContactsDevicesAndOperators(settlementSchedule);

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
            Merchant merchantModel = TestData.MerchantModelWithNullAddresses;

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

            merchantResponse.Devices.ShouldHaveSingleItem();
            KeyValuePair<Guid, String> device = merchantResponse.Devices.Single();
            device.Key.ShouldBe(merchantModel.Devices.Single().Key);
            device.Value.ShouldBe(merchantModel.Devices.Single().Value);

            merchantResponse.Operators.ShouldHaveSingleItem();
            MerchantOperatorResponse operatorDetails = merchantResponse.Operators.Single();
            operatorDetails.Name.ShouldBe(merchantModel.Operators.Single().Name);
            operatorDetails.OperatorId.ShouldBe(merchantModel.Operators.Single().OperatorId);
            operatorDetails.MerchantNumber.ShouldBe(merchantModel.Operators.Single().MerchantNumber);
            operatorDetails.TerminalNumber.ShouldBe(merchantModel.Operators.Single().TerminalNumber);
        }

        [Fact]
        public void ModelFactory_Merchant_NullContacts_IsConverted()
        {
            Merchant merchantModel = TestData.MerchantModelWithNullContacts;

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

            merchantResponse.Devices.ShouldHaveSingleItem();
            KeyValuePair<Guid, String> device = merchantResponse.Devices.Single();
            device.Key.ShouldBe(merchantModel.Devices.Single().Key);
            device.Value.ShouldBe(merchantModel.Devices.Single().Value);

            merchantResponse.Operators.ShouldHaveSingleItem();
            MerchantOperatorResponse operatorDetails = merchantResponse.Operators.Single();
            operatorDetails.Name.ShouldBe(merchantModel.Operators.Single().Name);
            operatorDetails.OperatorId.ShouldBe(merchantModel.Operators.Single().OperatorId);
            operatorDetails.MerchantNumber.ShouldBe(merchantModel.Operators.Single().MerchantNumber);
            operatorDetails.TerminalNumber.ShouldBe(merchantModel.Operators.Single().TerminalNumber);
        }

        [Fact]
        public void ModelFactory_Merchant_NullDevices_IsConverted()
        {
            Merchant merchantModel = TestData.MerchantModelWithNullDevices;

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

            merchantResponse.Devices.ShouldBeNull();

            merchantResponse.Operators.ShouldHaveSingleItem();
            MerchantOperatorResponse operatorDetails = merchantResponse.Operators.Single();
            operatorDetails.Name.ShouldBe(merchantModel.Operators.Single().Name);
            operatorDetails.OperatorId.ShouldBe(merchantModel.Operators.Single().OperatorId);
            operatorDetails.MerchantNumber.ShouldBe(merchantModel.Operators.Single().MerchantNumber);
            operatorDetails.TerminalNumber.ShouldBe(merchantModel.Operators.Single().TerminalNumber);
        }

        [Fact]
        public void ModelFactory_Merchant_NullOperators_IsConverted()
        {
            Merchant merchantModel = TestData.MerchantModelWithNullOperators;

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

            merchantResponse.Devices.ShouldHaveSingleItem();
            var device = merchantResponse.Devices.Single();
            device.Key.ShouldBe(merchantModel.Devices.Single().Key);
            device.Value.ShouldBe(merchantModel.Devices.Single().Value);

            merchantResponse.Operators.ShouldBeNull();
        }

        [Fact]
        public void ModelFactory_MerchantList_IsConverted()
        {
            List<Merchant> merchantModelList = new List<Merchant>
                                               {
                                                   TestData.MerchantModelWithAddressesContactsDevicesAndOperators()
                                               };

            ModelFactory modelFactory = new ModelFactory();

            List<MerchantResponse> merchantResponseList = modelFactory.ConvertFrom(merchantModelList);

            merchantResponseList.ShouldNotBeNull();
            merchantResponseList.ShouldNotBeEmpty();
            merchantResponseList.Count.ShouldBe(merchantModelList.Count);
        }
        
        [Fact]
        public void ModelFactory_Contract_ContractOnly_IsConverted()
        {
            Contract contractModel = TestData.ContractModel;

            ModelFactory modelFactory = new ModelFactory();

            ContractResponse contractResponse = modelFactory.ConvertFrom(contractModel);

            contractResponse.ShouldNotBeNull();
            contractResponse.EstateId.ShouldBe(contractModel.EstateId);
            contractResponse.OperatorId.ShouldBe(contractModel.OperatorId);
            contractResponse.OperatorName.ShouldBe(contractModel.OperatorName);
            contractResponse.ContractId.ShouldBe(contractModel.ContractId);
            contractResponse.Description.ShouldBe(contractModel.Description);
            contractResponse.Products.ShouldBeNull();
        }

        [Fact]
        public void ModelFactory_Contract_ContractWithProducts_IsConverted()
        {
            Contract contractModel = TestData.ContractModelWithProducts;

            ModelFactory modelFactory = new ModelFactory();

            ContractResponse contractResponse = modelFactory.ConvertFrom(contractModel);

            contractResponse.ShouldNotBeNull();
            contractResponse.EstateId.ShouldBe(contractModel.EstateId);
            contractResponse.OperatorId.ShouldBe(contractModel.OperatorId);
            contractResponse.ContractId.ShouldBe(contractModel.ContractId);
            contractResponse.Description.ShouldBe(contractModel.Description);
            contractResponse.Products.ShouldNotBeNull();
            contractResponse.Products.ShouldHaveSingleItem();

            ContractProduct contractProduct = contractResponse.Products.Single();
            Product expectedContractProduct = contractModel.Products.Single();

            contractProduct.ProductId.ShouldBe(expectedContractProduct.ProductId);
            contractProduct.Value.ShouldBe(expectedContractProduct.Value);
            contractProduct.DisplayText.ShouldBe(expectedContractProduct.DisplayText);
            contractProduct.Name.ShouldBe(expectedContractProduct.Name);
            contractProduct.ProductType.ShouldBe(Enum.Parse<DataTransferObjects.ProductType>(expectedContractProduct.ProductType.ToString()));
            contractProduct.TransactionFees.ShouldBeNull();
        }

        [Fact]
        public void ModelFactory_Contract_ContractWithProductsAndFees_IsConverted()
        {
            Contract contractModel = TestData.ContractModelWithProductsAndTransactionFees;

            ModelFactory modelFactory = new ModelFactory();

            ContractResponse contractResponse = modelFactory.ConvertFrom(contractModel);

            contractResponse.ShouldNotBeNull();
            contractResponse.EstateId.ShouldBe(contractModel.EstateId);
            contractResponse.OperatorId.ShouldBe(contractModel.OperatorId);
            contractResponse.ContractId.ShouldBe(contractModel.ContractId);
            contractResponse.Description.ShouldBe(contractModel.Description);
            contractResponse.Products.ShouldNotBeNull();
            contractResponse.Products.ShouldHaveSingleItem();
            
            ContractProduct contractProduct = contractResponse.Products.Single();
            Product expectedContractProduct = contractModel.Products.Single();

            contractProduct.ProductId.ShouldBe(expectedContractProduct.ProductId);
            contractProduct.Value.ShouldBe(expectedContractProduct.Value);
            contractProduct.DisplayText.ShouldBe(expectedContractProduct.DisplayText);
            contractProduct.Name.ShouldBe(expectedContractProduct.Name);
            contractProduct.ProductType.ShouldBe(Enum.Parse<DataTransferObjects.ProductType>(expectedContractProduct.ProductType.ToString()));
            contractProduct.TransactionFees.ShouldNotBeNull();
            contractProduct.TransactionFees.ShouldHaveSingleItem();

            ContractProductTransactionFee productTransactionFee = contractProduct.TransactionFees.Single();
            ContractProductTransactionFee expectedProductTransactionFee = contractProduct.TransactionFees.Single();

            productTransactionFee.TransactionFeeId.ShouldBe(expectedProductTransactionFee.TransactionFeeId);
            productTransactionFee.Value.ShouldBe(expectedProductTransactionFee.Value);
            productTransactionFee.CalculationType.ShouldBe(expectedProductTransactionFee.CalculationType);
            productTransactionFee.Description.ShouldBe(expectedProductTransactionFee.Description);
        }

        [Fact]
        public void ModelFactory_Contract_NullContract_IsConverted()
        {
            Contract contractModel = null;

            ModelFactory modelFactory = new ModelFactory();

            ContractResponse contractResponse = modelFactory.ConvertFrom(contractModel);

            contractResponse.ShouldBeNull();
        }


        [Fact]
        public void ModelFactory_TransactionFeeList_IsConverted()
        {
            List<TransactionFee> transactionFeeModelList = TestData.ProductTransactionFees;

            ModelFactory modelFactory = new ModelFactory();

            List<ContractProductTransactionFee> transactionFeeResponseList = modelFactory.ConvertFrom(transactionFeeModelList);

            transactionFeeResponseList.ShouldNotBeNull();
            transactionFeeResponseList.ShouldNotBeEmpty();
            transactionFeeResponseList.Count.ShouldBe(transactionFeeModelList.Count);
        }

        [Fact]
        public void ModelFactory_ContractList_IsConverted()
        {
            List<Contract> contractModel = new List<Contract>
                                           {
                                               TestData.ContractModel
                                           };

            ModelFactory modelFactory = new ModelFactory();

            var contractResponses = modelFactory.ConvertFrom(contractModel);

            contractResponses.ShouldNotBeNull();
            contractResponses.ShouldHaveSingleItem();
            contractResponses.Single().EstateId.ShouldBe(contractModel.Single().EstateId);
            contractResponses.Single().OperatorId.ShouldBe(contractModel.Single().OperatorId);
            contractResponses.Single().ContractId.ShouldBe(contractModel.Single().ContractId);
            contractResponses.Single().Description.ShouldBe(contractModel.Single().Description);
            contractResponses.Single().Products.ShouldBeNull();
        }

        [Fact]
        public void ModelFactory_ConvertFrom_SettlementFeeModel_ModelConverted()
        {
            ModelFactory factory = new ModelFactory();
            SettlementFeeResponse response = factory.ConvertFrom(TestData.SettlementFeeModel);

            response.ShouldSatisfyAllConditions(r => r.SettlementDate.ShouldBe(TestData.SettlementFeeModel.SettlementDate),
                                                r => r.SettlementId.ShouldBe(TestData.SettlementFeeModel.SettlementId),
                                                r => r.CalculatedValue.ShouldBe(TestData.SettlementFeeModel.CalculatedValue),
                                                r => r.FeeDescription.ShouldBe(TestData.SettlementFeeModel.FeeDescription),
                                                r => r.IsSettled.ShouldBe(TestData.SettlementFeeModel.IsSettled),
                                                r => r.MerchantId.ShouldBe(TestData.SettlementFeeModel.MerchantId),
                                                r => r.MerchantName.ShouldBe(TestData.SettlementFeeModel.MerchantName),
                                                r => r.TransactionId.ShouldBe(TestData.SettlementFeeModel.TransactionId));
        }

        [Fact]
        public void ModelFactory_ConvertFrom_SettlementFeeModel_ModelIsNull_ModelConverted()
        {
            ModelFactory factory = new ModelFactory();
            SettlementFeeModel model = null;
            SettlementFeeResponse response = factory.ConvertFrom(model);

            response.ShouldBeNull();
        }

        [Fact]
        public void ModelFactory_ConvertFrom_SettlementFeeModelList_ModelConverted()
        {
            ModelFactory factory = new ModelFactory();
            var response = factory.ConvertFrom(TestData.SettlementFeeModels);

            response.ShouldNotBeNull();
            response.ShouldNotBeEmpty();
            response.Count.ShouldBe(TestData.SettlementFeeModels.Count);
        }

        [Fact]
        public void ModelFactory_ConvertFrom_SettlementFeeModelList_ListIsNull_ModelConverted()
        {
            ModelFactory factory = new ModelFactory();
            List<SettlementFeeModel> modelList = null;
            List<SettlementFeeResponse> response = factory.ConvertFrom(modelList);

            response.ShouldNotBeNull();
            response.ShouldBeEmpty();
        }

        [Fact]
        public void ModelFactory_ConvertFrom_SettlementFeeModelList_ListIsEmpty_ModelConverted()
        {
            ModelFactory factory = new ModelFactory();
            List<SettlementFeeModel> modelList = new List<SettlementFeeModel>();
            List<SettlementFeeResponse> response = factory.ConvertFrom(modelList);

            response.ShouldNotBeNull();
            response.ShouldBeEmpty();
        }

        [Fact]
        public void ModelFactory_ConvertFrom_SettlementModel_ModelConverted()
        {
            ModelFactory factory = new ModelFactory();
            SettlementResponse response = factory.ConvertFrom(TestData.SettlementModel);

            response.ShouldSatisfyAllConditions(r => r.SettlementDate.ShouldBe(TestData.SettlementModel.SettlementDate),
                                                r => r.SettlementId.ShouldBe(TestData.SettlementModel.SettlementId),
                                                r => r.NumberOfFeesSettled.ShouldBe(TestData.SettlementModel.NumberOfFeesSettled),
                                                r => r.ValueOfFeesSettled.ShouldBe(TestData.SettlementModel.ValueOfFeesSettled),
                                                r => r.IsCompleted.ShouldBe(TestData.SettlementModel.IsCompleted),
                                                r => r.SettlementFees.Count.ShouldBe(TestData.SettlementModel.SettlementFees.Count));
        }

        [Fact]
        public void ModelFactory_ConvertFrom_SettlementModel_ModelIsNull_ModelConverted()
        {
            ModelFactory factory = new ModelFactory();
            SettlementModel model = null;
            SettlementResponse response = factory.ConvertFrom(model);

            response.ShouldBeNull();
        }

        [Fact]
        public void ModelFactory_ConvertFrom_SettlementModelList_ModelConverted()
        {
            ModelFactory factory = new ModelFactory();
            List<SettlementResponse> response = factory.ConvertFrom(TestData.SettlementModels);

            response.ShouldNotBeNull();
            response.ShouldNotBeEmpty();
            response.Count.ShouldBe(TestData.SettlementModels.Count);
        }

        [Fact]
        public void ModelFactory_ConvertFrom_SettlementModelList_ListIsNull_ModelConverted()
        {
            ModelFactory factory = new ModelFactory();
            List<SettlementModel> settlementModeList = null;
            List<SettlementResponse> response = factory.ConvertFrom(settlementModeList);

            response.ShouldNotBeNull();
            response.ShouldBeEmpty();
        }

        [Fact]
        public void ModelFactory_ConvertFrom_SettlementModelList_ListIsEmpty_ModelConverted()
        {
            ModelFactory factory = new ModelFactory();
            List<SettlementModel> settlementModeList = new List<SettlementModel>();
            List<SettlementResponse> response = factory.ConvertFrom(settlementModeList);

            response.ShouldNotBeNull();
            response.ShouldBeEmpty();
        }
    }
}
