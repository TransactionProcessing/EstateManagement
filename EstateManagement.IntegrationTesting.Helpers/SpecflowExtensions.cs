namespace EstateManagement.IntegrationTesting.Helpers;

using DataTransferObjects;
using Newtonsoft.Json;
using Shared.Extensions;
using Shared.General;
using Shared.IntegrationTesting;
using Shouldly;
using System.Collections.Generic;
using DataTransferObjects.Requests.Contract;
using DataTransferObjects.Requests.Estate;
using DataTransferObjects.Requests.Merchant;
using DataTransferObjects.Requests.Operator;
using DataTransferObjects.Responses.Contract;
using DataTransferObjects.Responses.Merchant;
using DataTransferObjects.Responses.Operator;
using Ductus.FluentDocker.Model.Compose;
using Newtonsoft.Json.Bson;
using Reqnroll;
using AssignOperatorToMerchantRequest = DataTransferObjects.Requests.Merchant.AssignOperatorRequest;
using AssignOperatorToEstateRequest = DataTransferObjects.Requests.Estate.AssignOperatorRequest;

public static class ReqnrollExtensions
{
    public class SettlementDetails
    {
        public Guid EstateId { get; set; }
        public Guid MerchantId { get; set; }
        public DateTime SettlementDate { get; set; }
        public Int32 NumberOfFeesSettled { get; set; }
        public Decimal ValueOfFeesSettled { get; set; }
        public Boolean IsCompleted { get; set; }
    }
    
    public class SettlementFeeDetails
    {
        public Guid EstateId { get; set; }
        public Guid MerchantId { get; set; }
        public Guid SettlementId { get; set; }
        public String FeeDescription { get; set; }
        public Boolean IsSettled { get; set; }
        public String Operator { get; set; }
        public Decimal CalculatedValue { get; set; }
    }
    public static T GetEnumValue<T>(DataTableRow row,
                                    String key) where T : struct
    {
        String field = ReqnrollTableHelper.GetStringRowValue(row, key);

        return Enum.Parse<T>(field, true);
    }

    public static Guid CalculateSettlementAggregateId(DateTime settlementDate,
                                                      Guid merchantId,
                                                      Guid estateId)
    {
        Guid aggregateId = GuidCalculator.Combine(estateId, merchantId, settlementDate.ToGuid());
        return aggregateId;
    }
    
    public static List<SettlementFeeDetails> ToSettlementFeeDetails(this DataTableRows tableRows, string estateName,
                                                                    string merchantName,
                                                                    String settlementDateString,
                                                                    List<EstateDetails> estateDetailsList){
        List<SettlementFeeDetails> settlementFeeDetailsList = new List<SettlementFeeDetails>();
        EstateDetails estateDetails = estateDetailsList.SingleOrDefault(e => e.EstateName == estateName);
        estateDetails.ShouldNotBeNull();

        foreach (DataTableRow tableRow in tableRows){
            SettlementFeeDetails settlementFeeDetails = new SettlementFeeDetails();
            DateTime settlementDate = ReqnrollTableHelper.GetDateForDateString(settlementDateString, DateTime.UtcNow.Date);
            Guid settlementId = ReqnrollExtensions.CalculateSettlementAggregateId(settlementDate, estateDetails.GetMerchant(merchantName).MerchantId, estateDetails.EstateId);
            settlementFeeDetails.SettlementId = settlementId;
            settlementFeeDetails.EstateId = estateDetails.EstateId;
            settlementFeeDetails.MerchantId = estateDetails.GetMerchant(merchantName).MerchantId;
            settlementFeeDetails.FeeDescription = ReqnrollTableHelper.GetStringRowValue(tableRow, "FeeDescription");
            settlementFeeDetails.IsSettled = ReqnrollTableHelper.GetBooleanValue(tableRow, "IsSettled");
            settlementFeeDetails.Operator = ReqnrollTableHelper.GetStringRowValue(tableRow, "Operator");
            settlementFeeDetails.CalculatedValue = ReqnrollTableHelper.GetDecimalValue(tableRow, "CalculatedValue");
        }
        return settlementFeeDetailsList;
    }

    public static SettlementDetails ToSettlementDetails(this DataTableRows tableRows, string estateName, List<EstateDetails> estateDetailsList)
    {
        SettlementDetails result = new SettlementDetails();

        EstateDetails estateDetails = estateDetailsList.SingleOrDefault(e => e.EstateName == estateName);
        estateDetails.ShouldNotBeNull();
        result.EstateId = estateDetails.EstateId;
            
        foreach (DataTableRow tableRow in tableRows)
        {
            result.SettlementDate = ReqnrollTableHelper.GetDateForDateString(ReqnrollTableHelper.GetStringRowValue(tableRow, "SettlementDate"), DateTime.UtcNow.Date);
            result.NumberOfFeesSettled = ReqnrollTableHelper.GetIntValue(tableRow, "NumberOfFeesSettled");
            result.ValueOfFeesSettled = ReqnrollTableHelper.GetDecimalValue(tableRow, "ValueOfFeesSettled");
            result.IsCompleted = ReqnrollTableHelper.GetBooleanValue(tableRow, "IsCompleted");
        }

        return result;
    }

    public static SettlementDetails ToSettlementDetails(this DataTableRows tableRows, string estateName,
                                                        string merchantName, List<EstateDetails> estateDetailsList)
    {
        SettlementDetails result = new SettlementDetails();

        EstateDetails estateDetails = estateDetailsList.SingleOrDefault(e => e.EstateName == estateName);
        estateDetails.ShouldNotBeNull();
        result.EstateId = estateDetails.EstateId;
        // Lookup the merchant id
        result.MerchantId = estateDetails.GetMerchant(merchantName).MerchantId;
             
        foreach (DataTableRow tableRow in tableRows){
            result.SettlementDate = ReqnrollTableHelper.GetDateForDateString(ReqnrollTableHelper.GetStringRowValue(tableRow, "SettlementDate"), DateTime.UtcNow.Date);
            result.NumberOfFeesSettled = ReqnrollTableHelper.GetIntValue(tableRow, "NumberOfFeesSettled");
            result.ValueOfFeesSettled = ReqnrollTableHelper.GetDecimalValue(tableRow, "ValueOfFeesSettled");
            result.IsCompleted = ReqnrollTableHelper.GetBooleanValue(tableRow, "IsCompleted");
        }

        return result;
    }

    public static List<(EstateDetails, MerchantResponse, Guid, Address)> ToAddressUpdates(this DataTableRows tableRows, List<EstateDetails> estateDetailsList){

        List<(EstateDetails, MerchantResponse, Guid, Address)> result = new ();

        foreach (DataTableRow tableRow in tableRows)
        {
            String estateName = ReqnrollTableHelper.GetStringRowValue(tableRow, "EstateName");
            EstateDetails estateDetails = estateDetailsList.SingleOrDefault(e => e.EstateName == estateName);
            estateDetails.ShouldNotBeNull();
            
            foreach (DataTableRow dataTableRow in tableRows){

                String merchantName = ReqnrollTableHelper.GetStringRowValue(tableRow, "MerchantName");
                MerchantResponse merchant = estateDetails.GetMerchant(merchantName);

                Guid addressId = merchant.Addresses.First().AddressId;

                Address addressUpdateRequest = new Address(){
                                                                AddressLine1 = ReqnrollTableHelper.GetStringRowValue(tableRow, "AddressLine1"),
                                                                AddressLine2 = ReqnrollTableHelper.GetStringRowValue(tableRow, "AddressLine2"),
                                                                AddressLine3 = ReqnrollTableHelper.GetStringRowValue(tableRow, "AddressLine3"),
                                                                AddressLine4 = ReqnrollTableHelper.GetStringRowValue(tableRow, "AddressLine4"),
                                                                Town = ReqnrollTableHelper.GetStringRowValue(tableRow, "Town"),
                                                                Region = ReqnrollTableHelper.GetStringRowValue(tableRow, "Region"),
                                                                Country = ReqnrollTableHelper.GetStringRowValue(tableRow, "Country"),
                                                                PostalCode = ReqnrollTableHelper.GetStringRowValue(tableRow, "PostalCode")
                                                            };
                result.Add((estateDetails, merchant, addressId,addressUpdateRequest));
            }
            
        }

        return result;
    }

    public static List<(EstateDetails, MerchantResponse, Guid, Contact)> ToContactUpdates(this DataTableRows tableRows, List<EstateDetails> estateDetailsList)
    {

        List<(EstateDetails, MerchantResponse, Guid, Contact)> result = new();

        foreach (DataTableRow tableRow in tableRows)
        {
            String estateName = ReqnrollTableHelper.GetStringRowValue(tableRow, "EstateName");
            EstateDetails estateDetails = estateDetailsList.SingleOrDefault(e => e.EstateName == estateName);
            estateDetails.ShouldNotBeNull();

            foreach (DataTableRow dataTableRow in tableRows)
            {

                String merchantName = ReqnrollTableHelper.GetStringRowValue(tableRow, "MerchantName");
                MerchantResponse merchant = estateDetails.GetMerchant(merchantName);

                Guid contactId = merchant.Contacts.First().ContactId;

                Contact contactUpdateRequest = new Contact()
                {
                    ContactName = ReqnrollTableHelper.GetStringRowValue(tableRow, "ContactName"),
                    EmailAddress = ReqnrollTableHelper.GetStringRowValue(tableRow, "EmailAddress"),
                    PhoneNumber = ReqnrollTableHelper.GetStringRowValue(tableRow, "PhoneNumber"),
                };
                result.Add((estateDetails, merchant, contactId, contactUpdateRequest));
            }
        }

        return result;
    }

    public static List<(EstateDetails, Guid, SetSettlementScheduleRequest)> ToSetSettlementScheduleRequests(this DataTableRows tableRows, List<EstateDetails> estateDetailsList)
    {

        List<(EstateDetails, Guid, SetSettlementScheduleRequest)> requests = new List<(EstateDetails, Guid, SetSettlementScheduleRequest)>();
        foreach (DataTableRow tableRow in tableRows)
        {
            String estateName = ReqnrollTableHelper.GetStringRowValue(tableRow, "EstateName");
            EstateDetails estateDetails = estateDetailsList.SingleOrDefault(e => e.EstateName == estateName);
            estateDetails.ShouldNotBeNull();

            // Lookup the merchant id
            String merchantName = ReqnrollTableHelper.GetStringRowValue(tableRow, "MerchantName");
            Guid merchantId = estateDetails.GetMerchant(merchantName).MerchantId;

            SettlementSchedule schedule = Enum.Parse<SettlementSchedule>(ReqnrollTableHelper.GetStringRowValue(tableRow, "SettlementSchedule"));

            SetSettlementScheduleRequest setSettlementScheduleRequest = new SetSettlementScheduleRequest
                                                                        {
                                                                            SettlementSchedule = schedule
                                                                        };
            requests.Add((estateDetails, merchantId, setSettlementScheduleRequest));
        }

        return requests;
    }

    public static List<(EstateDetails, Guid,String, SwapMerchantDeviceRequest)> ToSwapMerchantDeviceRequests(this DataTableRows tableRows, List<EstateDetails> estateDetailsList){

        List<(EstateDetails, Guid, String, SwapMerchantDeviceRequest)> requests = new List<(EstateDetails, Guid,String, SwapMerchantDeviceRequest)>();
        foreach (DataTableRow tableRow in tableRows){
            String estateName = ReqnrollTableHelper.GetStringRowValue(tableRow, "EstateName");
            EstateDetails estateDetails = estateDetailsList.SingleOrDefault(e => e.EstateName == estateName);
            estateDetails.ShouldNotBeNull();

            // Lookup the merchant id
            String merchantName = ReqnrollTableHelper.GetStringRowValue(tableRow, "MerchantName");
            Guid merchantId = estateDetails.GetMerchant(merchantName).MerchantId;

            String originalDeviceIdentifier = ReqnrollTableHelper.GetStringRowValue(tableRow, "OriginalDeviceIdentifier");
            String newDeviceIdentifier = ReqnrollTableHelper.GetStringRowValue(tableRow, "NewDeviceIdentifier");

            SwapMerchantDeviceRequest swapMerchantDeviceRequest = new SwapMerchantDeviceRequest{
                                                                                                   NewDeviceIdentifier = newDeviceIdentifier
                                                                                               };
            requests.Add((estateDetails, merchantId, originalDeviceIdentifier, swapMerchantDeviceRequest));
        }

        return requests;
    }

    public static List<String> ToAutomaticDepositRequests(this DataTableRows tableRows, List<EstateDetails> estateDetailsList, String testBankSortCode, String testBankAccountNumber){
        List<String> requests = new List<String>();
        foreach (DataTableRow tableRow in tableRows){
            Decimal amount = ReqnrollTableHelper.GetDecimalValue(tableRow, "Amount");
            DateTime depositDateTime = ReqnrollTableHelper.GetDateForDateString(ReqnrollTableHelper.GetStringRowValue(tableRow, "DateTime"), DateTime.UtcNow);
            String merchantName = ReqnrollTableHelper.GetStringRowValue(tableRow, "MerchantName");
            String estateName = ReqnrollTableHelper.GetStringRowValue(tableRow, "EstateName");

            Guid estateId = Guid.NewGuid();
            EstateDetails estateDetails = estateDetailsList.SingleOrDefault(e => e.EstateName == estateName);
            estateDetails.ShouldNotBeNull();

            var merchant = estateDetails.GetMerchant(merchantName);

            var depositReference = $"{estateDetails.EstateReference}-{merchant.MerchantReference}";

            // This will send a request to the Test Host (test bank)
            var makeDepositRequest = new{
                                            date_time = depositDateTime,
                                            from_sort_code = "665544",
                                            from_account_number = "12312312",
                                            to_sort_code = testBankSortCode,
                                            to_account_number = testBankAccountNumber,
                                            deposit_reference = depositReference,
                                            amount = amount
                                        };
                
            requests.Add(JsonConvert.SerializeObject(makeDepositRequest));
        }

        return requests;
    }
    public static List<(CalculationType, String, Decimal?, FeeType)> ToContractTransactionFeeDetails(this DataTableRows tableRows){
        var transactionFees = new List<(CalculationType, String, Decimal?, FeeType)>();
        foreach (DataTableRow tableRow in tableRows)
        {
            CalculationType calculationType = ReqnrollTableHelper.GetEnumValue<CalculationType>(tableRow, "CalculationType");
            FeeType feeType = ReqnrollTableHelper.GetEnumValue<FeeType>(tableRow, "FeeType");
            String feeDescription = ReqnrollTableHelper.GetStringRowValue(tableRow, "FeeDescription");
            Decimal feeValue = ReqnrollTableHelper.GetDecimalValue(tableRow, "Value");
        }

        return transactionFees;
    }

    public static List<(String,String)> ToContractDetails(this DataTableRows tableRows){
        List<(String, String)> contracts = new List<(String, String)>();
        foreach (DataTableRow tableRow in tableRows){
            String contractDescription = ReqnrollTableHelper.GetStringRowValue(tableRow, "ContractDescription");
            String productName = ReqnrollTableHelper.GetStringRowValue(tableRow, "ProductName");
            contracts.Add((contractDescription,productName));
        }

        return contracts;
    }

    public static List<String> ToSecurityUsersDetails(this DataTableRows tableRows)
    {
        List<String> results = new List<String>();
        foreach (DataTableRow tableRow in tableRows)
        {
            String emailAddress = ReqnrollTableHelper.GetStringRowValue(tableRow, "EmailAddress");
            results.Add(emailAddress);
        }

        return results;
    }


    public static List<String> ToOperatorDetails(this DataTableRows tableRows)
    {
        List<String> results = new List<String>();
        foreach (DataTableRow tableRow in tableRows)
        {
            String operatorName = ReqnrollTableHelper.GetStringRowValue(tableRow, "OperatorName");
            results.Add(operatorName);
        }

        return results;
    }

    public static List<String> ToEstateDetails(this DataTableRows tableRows)
    {
        List<String> results = new List<String>();
        foreach (DataTableRow tableRow in tableRows){
            String estateName = ReqnrollTableHelper.GetStringRowValue(tableRow, "EstateName");
            results.Add(estateName);
        }

        return results;
    }

    public static List<CreateNewUserRequest> ToCreateNewUserRequests(this DataTableRows tableRows, List<EstateDetails> estateDetailsList)
    {
        List<CreateNewUserRequest> createUserRequests = new List<CreateNewUserRequest>();
        foreach (DataTableRow tableRow in tableRows){
            Int32 userType = tableRow.ContainsKey("MerchantName") switch{
                true => 2,
                _ => 1
            };

            String estateName = ReqnrollTableHelper.GetStringRowValue(tableRow, "EstateName");
            EstateDetails estateDetails = estateDetailsList.SingleOrDefault(e => e.EstateName == estateName);
            estateDetails.ShouldNotBeNull();
            String merchantName = null;
            Guid? merchantId = null;
            if (userType == 2){
                merchantName = ReqnrollTableHelper.GetStringRowValue(tableRow, "MerchantName");
                merchantId = estateDetails.GetMerchant(merchantName).MerchantId;
            }

            CreateNewUserRequest createUserRequest = new CreateNewUserRequest
                                                     {
                                                         EmailAddress =
                                                             ReqnrollTableHelper.GetStringRowValue(tableRow, "EmailAddress"),
                                                         FamilyName =
                                                             ReqnrollTableHelper.GetStringRowValue(tableRow, "FamilyName"),
                                                         GivenName =
                                                             ReqnrollTableHelper.GetStringRowValue(tableRow, "GivenName"),
                                                         MiddleName =
                                                             ReqnrollTableHelper.GetStringRowValue(tableRow, "MiddleName"),
                                                         Password =
                                                             ReqnrollTableHelper.GetStringRowValue(tableRow, "Password"),
                                                         UserType = userType,
                                                         EstateId = estateDetails.EstateId,
                                                         MerchantId = merchantId,
                                                         MerchantName = merchantName
                                                     };
            createUserRequests.Add(createUserRequest);
        }
        return createUserRequests;
    }

    public static List<CreateEstateRequest> ToCreateEstateRequests(this DataTableRows tableRows)
    {
        List<CreateEstateRequest> requests = new List<CreateEstateRequest>();
        foreach (DataTableRow tableRow in tableRows)
        {
            CreateEstateRequest createEstateRequest = new CreateEstateRequest
                                                      {
                                                          EstateId = Guid.NewGuid(),
                                                          EstateName = ReqnrollTableHelper.GetStringRowValue(tableRow, "EstateName")
                                                      };
            requests.Add(createEstateRequest);
        }

        return requests;
    }

    public static List<(EstateDetails estate, CreateOperatorRequest request)> ToCreateOperatorRequests(this DataTableRows tableRows, List<EstateDetails> estateDetailsList)
    {
        List<(EstateDetails estate, CreateOperatorRequest request)> requests = new();

        foreach (DataTableRow tableRow in tableRows)
        {
            String estateName = ReqnrollTableHelper.GetStringRowValue(tableRow, "EstateName");

            EstateDetails estateDetails = estateDetailsList.SingleOrDefault(e => e.EstateName == estateName);
            estateDetails.ShouldNotBeNull();

            Guid operatorId = Guid.NewGuid();
            String operatorName = ReqnrollTableHelper.GetStringRowValue(tableRow, "OperatorName");
            Boolean requireCustomMerchantNumber = ReqnrollTableHelper.GetBooleanValue(tableRow, "RequireCustomMerchantNumber");
            Boolean requireCustomTerminalNumber = ReqnrollTableHelper.GetBooleanValue(tableRow, "RequireCustomTerminalNumber");

            CreateOperatorRequest createOperatorRequest = new CreateOperatorRequest
                                                          {
                                                              OperatorId = operatorId,
                                                              Name = operatorName,
                                                              RequireCustomMerchantNumber = requireCustomMerchantNumber,
                                                              RequireCustomTerminalNumber = requireCustomTerminalNumber
                                                          };
            requests.Add((estateDetails, createOperatorRequest));
        }

        return requests;
    }

    public static List<(EstateDetails estate, AssignOperatorToEstateRequest request)> ToAssignOperatorToEstateRequests(this DataTableRows tableRows, List<EstateDetails> estateDetailsList)
    {
        List<(EstateDetails estate, AssignOperatorToEstateRequest request)> requests = new();

        foreach (DataTableRow tableRow in tableRows)
        {
            String estateName = ReqnrollTableHelper.GetStringRowValue(tableRow, "EstateName");

            EstateDetails estateDetails = estateDetailsList.SingleOrDefault(e => e.EstateName == estateName);
            estateDetails.ShouldNotBeNull();


            String operatorName = ReqnrollTableHelper.GetStringRowValue(tableRow, "OperatorName");
            Guid operatorId = estateDetails.GetOperatorId(operatorName);
            
            AssignOperatorToEstateRequest assignOperatorRequest = new AssignOperatorToEstateRequest()
                                                          {
                                                              OperatorId = operatorId
            };
            requests.Add((estateDetails, assignOperatorRequest));
        }

        return requests;
    }

    public static List<(EstateDetails estate, CreateMerchantRequest)> ToCreateMerchantRequests(this DataTableRows tableRows, List<EstateDetails> estateDetailsList)
    {
        List<(EstateDetails estate, CreateMerchantRequest)> requests = new();
        foreach (DataTableRow tableRow in tableRows)
        {
            String estateName = ReqnrollTableHelper.GetStringRowValue(tableRow, "EstateName");
            EstateDetails estateDetails = estateDetailsList.SingleOrDefault(e => e.EstateName == estateName);
            estateDetails.ShouldNotBeNull();
            String settlementSchedule = ReqnrollTableHelper.GetStringRowValue(tableRow, "SettlementSchedule");

            SettlementSchedule schedule = SettlementSchedule.Immediate;
            if (String.IsNullOrEmpty(settlementSchedule) == false)
            {
                schedule = Enum.Parse<SettlementSchedule>(settlementSchedule);
            }

            CreateMerchantRequest createMerchantRequest = new CreateMerchantRequest
                                                          {
                                                              Name = ReqnrollTableHelper.GetStringRowValue(tableRow, "MerchantName"),
                                                              Contact = new Contact
                                                                        {
                                                                            ContactName =
                                                                                ReqnrollTableHelper.GetStringRowValue(tableRow,
                                                                                                                      "ContactName"),
                                                                            EmailAddress =
                                                                                ReqnrollTableHelper.GetStringRowValue(tableRow,
                                                                                                                      "EmailAddress")
                                                                        },
                                                              Address = new Address
                                                                        {
                                                                            AddressLine1 =
                                                                                ReqnrollTableHelper.GetStringRowValue(tableRow,
                                                                                                                      "AddressLine1"),
                                                                            Town =
                                                                                ReqnrollTableHelper.GetStringRowValue(tableRow, "Town"),
                                                                            Region =
                                                                                ReqnrollTableHelper.GetStringRowValue(tableRow,
                                                                                                                      "Region"),
                                                                            Country =
                                                                                ReqnrollTableHelper.GetStringRowValue(tableRow,
                                                                                                                      "Country")
                                                                        },
                                                              SettlementSchedule = schedule,
                                                              MerchantId = Guid.NewGuid()
                                                          };
            requests.Add((estateDetails, createMerchantRequest));
        }

        return requests;
    }

    public static List<(EstateDetails, Guid, AssignOperatorToMerchantRequest)> ToAssignOperatorRequests(this DataTableRows tableRows, List<EstateDetails> estateDetailsList)
    {
        List<(EstateDetails, Guid, AssignOperatorToMerchantRequest)> requests = new();

        foreach (DataTableRow tableRow in tableRows)
        {
            String estateName = ReqnrollTableHelper.GetStringRowValue(tableRow, "EstateName");
            EstateDetails estateDetails = estateDetailsList.SingleOrDefault(e => e.EstateName == estateName);
            estateDetails.ShouldNotBeNull();

            // Lookup the merchant id
            String merchantName = ReqnrollTableHelper.GetStringRowValue(tableRow, "MerchantName");
            Guid merchantId = estateDetails.GetMerchantId(merchantName);

            String operatorName = ReqnrollTableHelper.GetStringRowValue(tableRow, "OperatorName");
            Guid operatorId = estateDetails.GetOperatorId(operatorName);
            AssignOperatorToMerchantRequest assignOperatorRequest = new AssignOperatorToMerchantRequest
            {
                                                                        OperatorId = operatorId,
                                                                        MerchantNumber =
                                                                            ReqnrollTableHelper.GetStringRowValue(tableRow, "MerchantNumber"),
                                                                        TerminalNumber =
                                                                            ReqnrollTableHelper.GetStringRowValue(tableRow, "TerminalNumber"),
                                                                    };

            requests.Add((estateDetails, merchantId, assignOperatorRequest));
        }

        return requests;
    }

    public static List<(EstateDetails, Guid, UpdateMerchantRequest)> ToUpdateMerchantRequests(this DataTableRows tableRows, List<EstateDetails> estateDetailsList){
        List<(EstateDetails, Guid, UpdateMerchantRequest)> result = new List<(EstateDetails, Guid, UpdateMerchantRequest)>();

        foreach (DataTableRow tableRow in tableRows){
            String estateName = ReqnrollTableHelper.GetStringRowValue(tableRow, "EstateName");
            EstateDetails estateDetails = estateDetailsList.SingleOrDefault(e => e.EstateName == estateName);
            estateDetails.ShouldNotBeNull();

            String merchantName = ReqnrollTableHelper.GetStringRowValue(tableRow, "MerchantName");
            Guid merchantId = estateDetails.GetMerchantId(merchantName);

            String updateMerchantName = ReqnrollTableHelper.GetStringRowValue(tableRow, "UpdateMerchantName");

            String settlementSchedule = ReqnrollTableHelper.GetStringRowValue(tableRow, "SettlementSchedule");

            SettlementSchedule schedule = SettlementSchedule.Immediate;
            if (String.IsNullOrEmpty(settlementSchedule) == false)
            {
                schedule = Enum.Parse<SettlementSchedule>(settlementSchedule);
            }

            UpdateMerchantRequest updateMerchantRequest = new UpdateMerchantRequest{
                                                                                       Name = updateMerchantName,
                                                                                       SettlementSchedule = schedule
                                                                                   };

            result.Add((estateDetails, merchantId,updateMerchantRequest));
        }

        return result;
    }

    public static List<(EstateDetails, Guid, OperatorResponse)> ToOperatorResponses(this DataTableRows tableRows, List<EstateDetails> estateDetailsList){
        List<(EstateDetails, Guid, OperatorResponse)> result = new();

        foreach (DataTableRow tableRow in tableRows)
        {
            String estateName = ReqnrollTableHelper.GetStringRowValue(tableRow, "EstateName");
            EstateDetails estateDetails = estateDetailsList.SingleOrDefault(e => e.EstateName == estateName);
            estateDetails.ShouldNotBeNull();

            String operatorName = ReqnrollTableHelper.GetStringRowValue(tableRow, "OperatorName");
            Boolean requireCustomMerchantNumber = ReqnrollTableHelper.GetBooleanValue(tableRow, "RequireCustomMerchantNumber");
            Boolean requireCustomTerminalNumber = ReqnrollTableHelper.GetBooleanValue(tableRow, "RequireCustomTerminalNumber");
            Guid operatorId = estateDetails.GetOperatorId(operatorName);


            OperatorResponse operatorResponse = new(){
                                                         RequireCustomTerminalNumber = requireCustomTerminalNumber,
                                                         RequireCustomMerchantNumber = requireCustomMerchantNumber,
                                                         Name = operatorName,
                                                         OperatorId = operatorId
                                                     };

            result.Add((estateDetails, operatorId, operatorResponse));
        }

        return result;
    }

    public static List<(EstateDetails, Guid, UpdateOperatorRequest)> ToUpdateOperatorRequests(this DataTableRows tableRows, List<EstateDetails> estateDetailsList)
    {
        List<(EstateDetails, Guid, UpdateOperatorRequest)> result = new();

        foreach (DataTableRow tableRow in tableRows)
        {
            String estateName = ReqnrollTableHelper.GetStringRowValue(tableRow, "EstateName");
            EstateDetails estateDetails = estateDetailsList.SingleOrDefault(e => e.EstateName == estateName);
            estateDetails.ShouldNotBeNull();

            String merchantName = ReqnrollTableHelper.GetStringRowValue(tableRow, "OperatorName");
            Guid operatorId = estateDetails.GetOperatorId(merchantName);

            String updateOperatorName = ReqnrollTableHelper.GetStringRowValue(tableRow, "UpdateOperatorName");
            Boolean requireCustomMerchantNumber = ReqnrollTableHelper.GetBooleanValue(tableRow, "RequireCustomMerchantNumber");
            Boolean requireCustomTerminalNumber = ReqnrollTableHelper.GetBooleanValue(tableRow, "RequireCustomTerminalNumber");


            UpdateOperatorRequest updateOperatorRequest = new UpdateOperatorRequest{
                                                                                       RequireCustomTerminalNumber = requireCustomTerminalNumber,
                                                                                       RequireCustomMerchantNumber = requireCustomMerchantNumber,
                                                                                       Name = updateOperatorName
                                                                                   };

            result.Add((estateDetails, operatorId, updateOperatorRequest));
        }

        return result;
    }

    public static List<(EstateDetails, Guid, AddMerchantDeviceRequest)> ToAddMerchantDeviceRequests(this DataTableRows tableRows, List<EstateDetails> estateDetailsList)
    {
        List<(EstateDetails, Guid, AddMerchantDeviceRequest)> result = new List<(EstateDetails, Guid, AddMerchantDeviceRequest)>();

        foreach (DataTableRow tableRow in tableRows)
        {
            String estateName = ReqnrollTableHelper.GetStringRowValue(tableRow, "EstateName");
            EstateDetails estateDetails = estateDetailsList.SingleOrDefault(e => e.EstateName == estateName);
            estateDetails.ShouldNotBeNull();

            String merchantName = ReqnrollTableHelper.GetStringRowValue(tableRow, "MerchantName");
            Guid merchantId = estateDetails.GetMerchantId(merchantName);

            String deviceIdentifier = ReqnrollTableHelper.GetStringRowValue(tableRow, "DeviceIdentifier");

            AddMerchantDeviceRequest addMerchantDeviceRequest = new AddMerchantDeviceRequest
                                                                {
                                                                    DeviceIdentifier = deviceIdentifier
                                                                };

            result.Add((estateDetails, merchantId, addMerchantDeviceRequest));
        }

        return result;
    }

    public static List<(EstateDetails, CreateContractRequest)> ToCreateContractRequests(this DataTableRows tableRows, List<EstateDetails> estateDetailsList)
    {
        List<(EstateDetails, CreateContractRequest)> result = new();

        foreach (DataTableRow tableRow in tableRows)
        {
            String estateName = ReqnrollTableHelper.GetStringRowValue(tableRow, "EstateName");
            EstateDetails estateDetails = estateDetailsList.SingleOrDefault(e => e.EstateName == estateName);
            estateDetails.ShouldNotBeNull();

            String operatorName = ReqnrollTableHelper.GetStringRowValue(tableRow, "OperatorName");
            Guid operatorId = estateDetails.GetOperatorId(operatorName);

            CreateContractRequest createContractRequest = new CreateContractRequest
                                                          {
                                                              OperatorId = operatorId,
                                                              Description = ReqnrollTableHelper.GetStringRowValue(tableRow, "ContractDescription")
                                                          };
            result.Add((estateDetails, createContractRequest));
        }

        return result;
    }

    public static List<(EstateDetails, Contract, AddProductToContractRequest)> ToAddProductToContractRequest(this DataTableRows tableRows, List<EstateDetails> estateDetailsList)
    {
        List<(EstateDetails, Contract, AddProductToContractRequest)> result = new List<(EstateDetails, Contract, AddProductToContractRequest)>();
        foreach (DataTableRow tableRow in tableRows)
        {
            String estateName = ReqnrollTableHelper.GetStringRowValue(tableRow, "EstateName");
            EstateDetails estateDetails = estateDetailsList.SingleOrDefault(e => e.EstateName == estateName);
            estateDetails.ShouldNotBeNull();

            String contractName = ReqnrollTableHelper.GetStringRowValue(tableRow, "ContractDescription");
            Contract contract = estateDetails.GetContract(contractName);

            String productValue = ReqnrollTableHelper.GetStringRowValue(tableRow, "Value");

            var productTypeString = ReqnrollTableHelper.GetStringRowValue(tableRow, "ProductType");
            var productType = Enum.Parse<ProductType>(productTypeString, true);
            AddProductToContractRequest addProductToContractRequest = new AddProductToContractRequest
                                                                      {
                                                                          ProductName =
                                                                              ReqnrollTableHelper.GetStringRowValue(tableRow,
                                                                                                                    "ProductName"),
                                                                          DisplayText =
                                                                              ReqnrollTableHelper.GetStringRowValue(tableRow,
                                                                                                                    "DisplayText"),
                                                                          Value = null,
                                                                          ProductType = productType
                                                                      };

            if (String.IsNullOrEmpty(productValue) == false)
            {
                addProductToContractRequest.Value = Decimal.Parse(productValue);
            }

            result.Add((estateDetails, contract, addProductToContractRequest));
        }

        return result;
    }

    public static List<(EstateDetails, Guid, Guid)> ToAddContractToMerchantRequests(this DataTableRows tableRows, List<EstateDetails> estateDetailsList){
        List<(EstateDetails, Guid, Guid)> result = new List<(EstateDetails, Guid, Guid)>();

        foreach (DataTableRow tableRow in tableRows){
            String estateName = ReqnrollTableHelper.GetStringRowValue(tableRow, "EstateName");
            EstateDetails estateDetails = estateDetailsList.SingleOrDefault(e => e.EstateName == estateName);
            estateDetails.ShouldNotBeNull();

            String? merchantName = ReqnrollTableHelper.GetStringRowValue(tableRow, "MerchantName");
            Guid merchantId = estateDetails.GetMerchantId(merchantName);

            String contractName = ReqnrollTableHelper.GetStringRowValue(tableRow, "ContractDescription");
            Contract contract = estateDetails.GetContract(contractName);
            result.Add((estateDetails, merchantId, contract.ContractId));
        }

        return result;
    }

    public static List<(EstateDetails, Contract, Product, AddTransactionFeeForProductToContractRequest)> ToAddTransactionFeeForProductToContractRequests(this DataTableRows tableRows, List<EstateDetails> estateDetailsList)
    {
        List<(EstateDetails, Contract, Product, AddTransactionFeeForProductToContractRequest)> result = new();
        foreach (DataTableRow tableRow in tableRows)
        {
            String estateName = ReqnrollTableHelper.GetStringRowValue(tableRow, "EstateName");
            EstateDetails estateDetails = estateDetailsList.SingleOrDefault(e => e.EstateName == estateName);
            estateDetails.ShouldNotBeNull();

            String contractName = ReqnrollTableHelper.GetStringRowValue(tableRow, "ContractDescription");
            Contract contract = estateDetails.GetContract(contractName);

            String productName = ReqnrollTableHelper.GetStringRowValue(tableRow, "ProductName");
            Product product = contract.GetProduct(productName);

            var calculationTypeString = ReqnrollTableHelper.GetStringRowValue(tableRow, "CalculationType");
            var calculationType = Enum.Parse<CalculationType>(calculationTypeString, true);
            AddTransactionFeeForProductToContractRequest addTransactionFeeForProductToContractRequest = new AddTransactionFeeForProductToContractRequest
                                                                                                        {
                                                                                                            Value =
                                                                                                                ReqnrollTableHelper.GetDecimalValue(tableRow,
                                                                                                                                                    "Value"),
                                                                                                            Description =
                                                                                                                ReqnrollTableHelper.GetStringRowValue(tableRow,
                                                                                                                                                      "FeeDescription"),
                                                                                                            CalculationType = calculationType
                                                                                                        };
            result.Add((estateDetails, contract, product, addTransactionFeeForProductToContractRequest));
        }

        return result;
    }

    public static List<(EstateDetails, Guid, MakeMerchantDepositRequest)> ToMakeMerchantDepositRequest(this DataTableRows tableRows, List<EstateDetails> estateDetailsList)
    {
        List<(EstateDetails, Guid, MakeMerchantDepositRequest)> result = new List<(EstateDetails, Guid, MakeMerchantDepositRequest)>();

        foreach (DataTableRow tableRow in tableRows)
        {
            String estateName = ReqnrollTableHelper.GetStringRowValue(tableRow, "EstateName");
            EstateDetails estateDetails = estateDetailsList.SingleOrDefault(e => e.EstateName == estateName);
            estateDetails.ShouldNotBeNull();

            String merchantName = ReqnrollTableHelper.GetStringRowValue(tableRow, "MerchantName");
            Guid merchantId = estateDetails.GetMerchantId(merchantName);

            MakeMerchantDepositRequest makeMerchantDepositRequest = new MakeMerchantDepositRequest
                                                                    {
                                                                        DepositDateTime = ReqnrollTableHelper.GetDateForDateString(ReqnrollTableHelper.GetStringRowValue(tableRow, "DateTime"), DateTime.UtcNow),
                                                                        Reference = ReqnrollTableHelper.GetStringRowValue(tableRow, "Reference"),
                                                                        Amount = ReqnrollTableHelper.GetDecimalValue(tableRow, "Amount")
                                                                    };

            result.Add((estateDetails, merchantId, makeMerchantDepositRequest));
        }

        return result;
    }

    public static List<(EstateDetails, Guid, MakeMerchantWithdrawalRequest)> ToMakeMerchantWithdrawalRequest(this DataTableRows tableRows, List<EstateDetails> estateDetailsList)
    {
        List<(EstateDetails, Guid, MakeMerchantWithdrawalRequest)> result = new List<(EstateDetails, Guid, MakeMerchantWithdrawalRequest)>();

        foreach (DataTableRow tableRow in tableRows)
        {
            String estateName = ReqnrollTableHelper.GetStringRowValue(tableRow, "EstateName");
            EstateDetails estateDetails = estateDetailsList.SingleOrDefault(e => e.EstateName == estateName);
            estateDetails.ShouldNotBeNull();

            String merchantName = ReqnrollTableHelper.GetStringRowValue(tableRow, "MerchantName");
            Guid merchantId = estateDetails.GetMerchantId(merchantName);

            MakeMerchantWithdrawalRequest makeMerchantWithdrawalRequest = new MakeMerchantWithdrawalRequest
                                                                          {
                                                                              WithdrawalDateTime =
                                                                                  ReqnrollTableHelper.GetDateForDateString(ReqnrollTableHelper
                                                                                                                                   .GetStringRowValue(tableRow,
                                                                                                                                                      "DateTime"),
                                                                                                                               DateTime.Now),
                                                                              Amount =
                                                                                  ReqnrollTableHelper.GetDecimalValue(tableRow,
                                                                                                                      "Amount")
                                                                          };

            result.Add((estateDetails, merchantId, makeMerchantWithdrawalRequest));
        }

        return result;
    }
}