namespace EstateManagement.IntegrationTesting.Helpers;

using DataTransferObjects;
using DataTransferObjects.Requests;
using Newtonsoft.Json;
using Shared.Extensions;
using Shared.General;
using Shared.IntegrationTesting;
using Shouldly;
using TechTalk.SpecFlow;

public static class SpecflowExtensions
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
    public static T GetEnumValue<T>(TableRow row,
                                    String key) where T : struct
    {
        String field = SpecflowTableHelper.GetStringRowValue(row, key);

        return Enum.Parse<T>(field, true);
    }

    public static Guid CalculateSettlementAggregateId(DateTime settlementDate,
                                                      Guid merchantId,
                                                      Guid estateId)
    {
        Guid aggregateId = GuidCalculator.Combine(estateId, merchantId, settlementDate.ToGuid());
        return aggregateId;
    }

    public static List<SettlementFeeDetails> ToSettlementFeeDetails(this TableRows tableRows, string estateName,
                                                                    string merchantName,
                                                                    String settlementDateString,
                                                                    List<EstateDetails> estateDetailsList){
        List<SettlementFeeDetails> settlementFeeDetailsList = new List<SettlementFeeDetails>();
        EstateDetails estateDetails = estateDetailsList.SingleOrDefault(e => e.EstateName == estateName);
        estateDetails.ShouldNotBeNull();

        foreach (TableRow tableRow in tableRows){
            SettlementFeeDetails settlementFeeDetails = new SettlementFeeDetails();
            DateTime settlementDate = SpecflowTableHelper.GetDateForDateString(settlementDateString, DateTime.UtcNow.Date);
            Guid settlementId = SpecflowExtensions.CalculateSettlementAggregateId(settlementDate, estateDetails.GetMerchant(merchantName).MerchantId, estateDetails.EstateId);
            settlementFeeDetails.SettlementId = settlementId;
            settlementFeeDetails.EstateId = estateDetails.EstateId;
            settlementFeeDetails.MerchantId = estateDetails.GetMerchant(merchantName).MerchantId;
            settlementFeeDetails.FeeDescription = SpecflowTableHelper.GetStringRowValue(tableRow, "FeeDescription");
            settlementFeeDetails.IsSettled = SpecflowTableHelper.GetBooleanValue(tableRow, "IsSettled");
            settlementFeeDetails.Operator = SpecflowTableHelper.GetStringRowValue(tableRow, "Operator");
            settlementFeeDetails.CalculatedValue = SpecflowTableHelper.GetDecimalValue(tableRow, "CalculatedValue");
        }
        return settlementFeeDetailsList;
    }

    public static SettlementDetails ToSettlementDetails(this TableRows tableRows, string estateName, List<EstateDetails> estateDetailsList)
    {
        SettlementDetails result = new SettlementDetails();

        EstateDetails estateDetails = estateDetailsList.SingleOrDefault(e => e.EstateName == estateName);
        estateDetails.ShouldNotBeNull();
        result.EstateId = estateDetails.EstateId;
            
        foreach (TableRow tableRow in tableRows)
        {
            result.SettlementDate = SpecflowTableHelper.GetDateForDateString(SpecflowTableHelper.GetStringRowValue(tableRow, "SettlementDate"), DateTime.UtcNow.Date);
            result.NumberOfFeesSettled = SpecflowTableHelper.GetIntValue(tableRow, "NumberOfFeesSettled");
            result.ValueOfFeesSettled = SpecflowTableHelper.GetDecimalValue(tableRow, "ValueOfFeesSettled");
            result.IsCompleted = SpecflowTableHelper.GetBooleanValue(tableRow, "IsCompleted");
        }

        return result;
    }

    public static SettlementDetails ToSettlementDetails(this TableRows tableRows, string estateName,
                                                        string merchantName, List<EstateDetails> estateDetailsList)
    {
        SettlementDetails result = new SettlementDetails();

        EstateDetails estateDetails = estateDetailsList.SingleOrDefault(e => e.EstateName == estateName);
        estateDetails.ShouldNotBeNull();
        result.EstateId = estateDetails.EstateId;
        // Lookup the merchant id
        result.MerchantId = estateDetails.GetMerchant(merchantName).MerchantId;
             
        foreach (TableRow tableRow in tableRows){
            result.SettlementDate = SpecflowTableHelper.GetDateForDateString(SpecflowTableHelper.GetStringRowValue(tableRow, "SettlementDate"), DateTime.UtcNow.Date);
            result.NumberOfFeesSettled = SpecflowTableHelper.GetIntValue(tableRow, "NumberOfFeesSettled");
            result.ValueOfFeesSettled = SpecflowTableHelper.GetDecimalValue(tableRow, "ValueOfFeesSettled");
            result.IsCompleted = SpecflowTableHelper.GetBooleanValue(tableRow, "IsCompleted");
        }

        return result;
    }

    public static List<(EstateDetails, Guid, SetSettlementScheduleRequest)> ToSetSettlementScheduleRequests(this TableRows tableRows, List<EstateDetails> estateDetailsList)
    {

        List<(EstateDetails, Guid, SetSettlementScheduleRequest)> requests = new List<(EstateDetails, Guid, SetSettlementScheduleRequest)>();
        foreach (TableRow tableRow in tableRows)
        {
            String estateName = SpecflowTableHelper.GetStringRowValue(tableRow, "EstateName");
            EstateDetails estateDetails = estateDetailsList.SingleOrDefault(e => e.EstateName == estateName);
            estateDetails.ShouldNotBeNull();

            // Lookup the merchant id
            String merchantName = SpecflowTableHelper.GetStringRowValue(tableRow, "MerchantName");
            Guid merchantId = estateDetails.GetMerchant(merchantName).MerchantId;

            SettlementSchedule schedule = Enum.Parse<SettlementSchedule>(SpecflowTableHelper.GetStringRowValue(tableRow, "SettlementSchedule"));

            SetSettlementScheduleRequest setSettlementScheduleRequest = new SetSettlementScheduleRequest
                                                                        {
                                                                            SettlementSchedule = schedule
                                                                        };
            requests.Add((estateDetails, merchantId, setSettlementScheduleRequest));
        }

        return requests;
    }

    public static List<(EstateDetails, Guid, SwapMerchantDeviceRequest)> ToSwapMerchantDeviceRequests(this TableRows tableRows, List<EstateDetails> estateDetailsList){

        List<(EstateDetails, Guid, SwapMerchantDeviceRequest)> requests = new List<(EstateDetails, Guid, SwapMerchantDeviceRequest)>();
        foreach (TableRow tableRow in tableRows){
            String estateName = SpecflowTableHelper.GetStringRowValue(tableRow, "EstateName");
            EstateDetails estateDetails = estateDetailsList.SingleOrDefault(e => e.EstateName == estateName);
            estateDetails.ShouldNotBeNull();

            // Lookup the merchant id
            String merchantName = SpecflowTableHelper.GetStringRowValue(tableRow, "MerchantName");
            Guid merchantId = estateDetails.GetMerchant(merchantName).MerchantId;

            String originalDeviceIdentifier = SpecflowTableHelper.GetStringRowValue(tableRow, "OriginalDeviceIdentifier");
            String newDeviceIdentifier = SpecflowTableHelper.GetStringRowValue(tableRow, "NewDeviceIdentifier");

            SwapMerchantDeviceRequest swapMerchantDeviceRequest = new SwapMerchantDeviceRequest{
                                                                                                   OriginalDeviceIdentifier = originalDeviceIdentifier,
                                                                                                   NewDeviceIdentifier = newDeviceIdentifier
                                                                                               };
            requests.Add((estateDetails, merchantId, swapMerchantDeviceRequest));
        }

        return requests;
    }

    public static List<String> ToAutomaticDepositRequests(this TableRows tableRows, List<EstateDetails> estateDetailsList, String testBankSortCode, String testBankAccountNumber){
        List<String> requests = new List<String>();
        foreach (TableRow tableRow in tableRows){
            Decimal amount = SpecflowTableHelper.GetDecimalValue(tableRow, "Amount");
            DateTime depositDateTime = SpecflowTableHelper.GetDateForDateString(SpecflowTableHelper.GetStringRowValue(tableRow, "DateTime"), DateTime.UtcNow);
            String merchantName = SpecflowTableHelper.GetStringRowValue(tableRow, "MerchantName");
            String estateName = SpecflowTableHelper.GetStringRowValue(tableRow, "EstateName");

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
    public static List<(CalculationType, String, Decimal?, FeeType)> ToContractTransactionFeeDetails(this TableRows tableRows){
        var transactionFees = new List<(CalculationType, String, Decimal?, FeeType)>();
        foreach (TableRow tableRow in tableRows)
        {
            CalculationType calculationType = SpecflowExtensions.GetEnumValue<CalculationType>(tableRow, "CalculationType");
            FeeType feeType = SpecflowExtensions.GetEnumValue<FeeType>(tableRow, "FeeType");
            String feeDescription = SpecflowTableHelper.GetStringRowValue(tableRow, "FeeDescription");
            Decimal feeValue = SpecflowTableHelper.GetDecimalValue(tableRow, "Value");
        }

        return transactionFees;
    }

    public static List<(String,String)> ToContractDetails(this TableRows tableRows){
        List<(String, String)> contracts = new List<(String, String)>();
        foreach (TableRow tableRow in tableRows){
            String contractDescription = SpecflowTableHelper.GetStringRowValue(tableRow, "ContractDescription");
            String productName = SpecflowTableHelper.GetStringRowValue(tableRow, "ProductName");
            contracts.Add((contractDescription,productName));
        }

        return contracts;
    }

    public static List<String> ToSecurityUsersDetails(this TableRows tableRows)
    {
        List<String> results = new List<String>();
        foreach (TableRow tableRow in tableRows)
        {
            String emailAddress = SpecflowTableHelper.GetStringRowValue(tableRow, "EmailAddress");
            results.Add(emailAddress);
        }

        return results;
    }


    public static List<String> ToOperatorDetails(this TableRows tableRows)
    {
        List<String> results = new List<String>();
        foreach (TableRow tableRow in tableRows)
        {
            String operatorName = SpecflowTableHelper.GetStringRowValue(tableRow, "OperatorName");
            results.Add(operatorName);
        }

        return results;
    }

    public static List<String> ToEstateDetails(this TableRows tableRows)
    {
        List<String> results = new List<String>();
        foreach (TableRow tableRow in tableRows){
            String estateName = SpecflowTableHelper.GetStringRowValue(tableRow, "EstateName");
            results.Add(estateName);
        }

        return results;
    }

    public static List<CreateNewUserRequest> ToCreateNewUserRequests(this TableRows tableRows, List<EstateDetails> estateDetailsList)
    {
        List<CreateNewUserRequest> createUserRequests = new List<CreateNewUserRequest>();
        foreach (TableRow tableRow in tableRows){
            Int32 userType = tableRow.ContainsKey("MerchantName") switch{
                true => 2,
                _ => 1
            };

            String estateName = SpecflowTableHelper.GetStringRowValue(tableRow, "EstateName");
            EstateDetails estateDetails = estateDetailsList.SingleOrDefault(e => e.EstateName == estateName);
            estateDetails.ShouldNotBeNull();
            String merchantName = null;
            Guid? merchantId = null;
            if (userType == 2){
                merchantName = SpecflowTableHelper.GetStringRowValue(tableRow, "MerchantName");
                merchantId = estateDetails.GetMerchant(merchantName).MerchantId;
            }

            CreateNewUserRequest createUserRequest = new CreateNewUserRequest
                                                     {
                                                         EmailAddress =
                                                             SpecflowTableHelper.GetStringRowValue(tableRow, "EmailAddress"),
                                                         FamilyName =
                                                             SpecflowTableHelper.GetStringRowValue(tableRow, "FamilyName"),
                                                         GivenName =
                                                             SpecflowTableHelper.GetStringRowValue(tableRow, "GivenName"),
                                                         MiddleName =
                                                             SpecflowTableHelper.GetStringRowValue(tableRow, "MiddleName"),
                                                         Password =
                                                             SpecflowTableHelper.GetStringRowValue(tableRow, "Password"),
                                                         UserType = userType,
                                                         EstateId = estateDetails.EstateId,
                                                         MerchantId = merchantId,
                                                         MerchantName = merchantName
                                                     };
            createUserRequests.Add(createUserRequest);
        }
        return createUserRequests;
    }

    public static List<CreateEstateRequest> ToCreateEstateRequests(this TableRows tableRows)
    {
        List<CreateEstateRequest> requests = new List<CreateEstateRequest>();
        foreach (TableRow tableRow in tableRows)
        {
            CreateEstateRequest createEstateRequest = new CreateEstateRequest
                                                      {
                                                          EstateId = Guid.NewGuid(),
                                                          EstateName = SpecflowTableHelper.GetStringRowValue(tableRow, "EstateName")
                                                      };
            requests.Add(createEstateRequest);
        }

        return requests;
    }

    public static List<(EstateDetails estate, CreateOperatorRequest request)> ToCreateOperatorRequests(this TableRows tableRows, List<EstateDetails> estateDetailsList)
    {
        List<(EstateDetails estate, CreateOperatorRequest request)> requests = new();

        foreach (TableRow tableRow in tableRows)
        {
            String estateName = SpecflowTableHelper.GetStringRowValue(tableRow, "EstateName");

            EstateDetails estateDetails = estateDetailsList.SingleOrDefault(e => e.EstateName == estateName);
            estateDetails.ShouldNotBeNull();

            String operatorName = SpecflowTableHelper.GetStringRowValue(tableRow, "OperatorName");
            Boolean requireCustomMerchantNumber = SpecflowTableHelper.GetBooleanValue(tableRow, "RequireCustomMerchantNumber");
            Boolean requireCustomTerminalNumber = SpecflowTableHelper.GetBooleanValue(tableRow, "RequireCustomTerminalNumber");

            CreateOperatorRequest createOperatorRequest = new CreateOperatorRequest
                                                          {
                                                              Name = operatorName,
                                                              RequireCustomMerchantNumber = requireCustomMerchantNumber,
                                                              RequireCustomTerminalNumber = requireCustomTerminalNumber
                                                          };
            requests.Add((estateDetails, createOperatorRequest));
        }

        return requests;
    }

    public static List<(EstateDetails estate, CreateMerchantRequest)> ToCreateMerchantRequests(this TableRows tableRows, List<EstateDetails> estateDetailsList)
    {
        List<(EstateDetails estate, CreateMerchantRequest)> requests = new();
        foreach (TableRow tableRow in tableRows)
        {
            String estateName = SpecflowTableHelper.GetStringRowValue(tableRow, "EstateName");
            EstateDetails estateDetails = estateDetailsList.SingleOrDefault(e => e.EstateName == estateName);
            estateDetails.ShouldNotBeNull();
            String settlementSchedule = SpecflowTableHelper.GetStringRowValue(tableRow, "SettlementSchedule");

            SettlementSchedule schedule = SettlementSchedule.Immediate;
            if (String.IsNullOrEmpty(settlementSchedule) == false)
            {
                schedule = Enum.Parse<SettlementSchedule>(settlementSchedule);
            }

            CreateMerchantRequest createMerchantRequest = new CreateMerchantRequest
                                                          {
                                                              Name = SpecflowTableHelper.GetStringRowValue(tableRow, "MerchantName"),
                                                              Contact = new Contact
                                                                        {
                                                                            ContactName =
                                                                                SpecflowTableHelper.GetStringRowValue(tableRow,
                                                                                                                      "ContactName"),
                                                                            EmailAddress =
                                                                                SpecflowTableHelper.GetStringRowValue(tableRow,
                                                                                                                      "EmailAddress")
                                                                        },
                                                              Address = new Address
                                                                        {
                                                                            AddressLine1 =
                                                                                SpecflowTableHelper.GetStringRowValue(tableRow,
                                                                                                                      "AddressLine1"),
                                                                            Town =
                                                                                SpecflowTableHelper.GetStringRowValue(tableRow, "Town"),
                                                                            Region =
                                                                                SpecflowTableHelper.GetStringRowValue(tableRow,
                                                                                                                      "Region"),
                                                                            Country =
                                                                                SpecflowTableHelper.GetStringRowValue(tableRow,
                                                                                                                      "Country")
                                                                        },
                                                              SettlementSchedule = schedule
                                                          };
            requests.Add((estateDetails, createMerchantRequest));
        }

        return requests;
    }

    public static List<(EstateDetails, Guid, AssignOperatorRequest)> ToAssignOperatorRequests(this TableRows tableRows, List<EstateDetails> estateDetailsList)
    {
        List<(EstateDetails, Guid, AssignOperatorRequest)> requests = new();

        foreach (TableRow tableRow in tableRows)
        {
            String estateName = SpecflowTableHelper.GetStringRowValue(tableRow, "EstateName");
            EstateDetails estateDetails = estateDetailsList.SingleOrDefault(e => e.EstateName == estateName);
            estateDetails.ShouldNotBeNull();

            // Lookup the merchant id
            String merchantName = SpecflowTableHelper.GetStringRowValue(tableRow, "MerchantName");
            Guid merchantId = estateDetails.GetMerchantId(merchantName);

            String operatorName = SpecflowTableHelper.GetStringRowValue(tableRow, "OperatorName");
            Guid operatorId = estateDetails.GetOperatorId(operatorName);
            AssignOperatorRequest assignOperatorRequest = new AssignOperatorRequest
                                                          {
                                                              OperatorId = operatorId,
                                                              MerchantNumber =
                                                                  SpecflowTableHelper.GetStringRowValue(tableRow, "MerchantNumber"),
                                                              TerminalNumber =
                                                                  SpecflowTableHelper.GetStringRowValue(tableRow, "TerminalNumber"),
                                                          };

            requests.Add((estateDetails, merchantId, assignOperatorRequest));
        }

        return requests;
    }

    public static List<(EstateDetails, Guid, AddMerchantDeviceRequest)> ToAddMerchantDeviceRequests(this TableRows tableRows, List<EstateDetails> estateDetailsList)
    {
        List<(EstateDetails, Guid, AddMerchantDeviceRequest)> result = new List<(EstateDetails, Guid, AddMerchantDeviceRequest)>();

        foreach (TableRow tableRow in tableRows)
        {
            String estateName = SpecflowTableHelper.GetStringRowValue(tableRow, "EstateName");
            EstateDetails estateDetails = estateDetailsList.SingleOrDefault(e => e.EstateName == estateName);
            estateDetails.ShouldNotBeNull();

            String merchantName = SpecflowTableHelper.GetStringRowValue(tableRow, "MerchantName");
            Guid merchantId = estateDetails.GetMerchantId(merchantName);

            String deviceIdentifier = SpecflowTableHelper.GetStringRowValue(tableRow, "DeviceIdentifier");

            AddMerchantDeviceRequest addMerchantDeviceRequest = new AddMerchantDeviceRequest
                                                                {
                                                                    DeviceIdentifier = deviceIdentifier
                                                                };

            result.Add((estateDetails, merchantId, addMerchantDeviceRequest));
        }

        return result;
    }

    public static List<(EstateDetails, CreateContractRequest)> ToCreateContractRequests(this TableRows tableRows, List<EstateDetails> estateDetailsList)
    {
        List<(EstateDetails, CreateContractRequest)> result = new();

        foreach (TableRow tableRow in tableRows)
        {
            String estateName = SpecflowTableHelper.GetStringRowValue(tableRow, "EstateName");
            EstateDetails estateDetails = estateDetailsList.SingleOrDefault(e => e.EstateName == estateName);
            estateDetails.ShouldNotBeNull();

            String operatorName = SpecflowTableHelper.GetStringRowValue(tableRow, "OperatorName");
            Guid operatorId = estateDetails.GetOperatorId(operatorName);

            CreateContractRequest createContractRequest = new CreateContractRequest
                                                          {
                                                              OperatorId = operatorId,
                                                              Description = SpecflowTableHelper.GetStringRowValue(tableRow, "ContractDescription")
                                                          };
            result.Add((estateDetails, createContractRequest));
        }

        return result;
    }

    public static List<(EstateDetails, Contract, AddProductToContractRequest)> ToAddProductToContractRequest(this TableRows tableRows, List<EstateDetails> estateDetailsList)
    {
        List<(EstateDetails, Contract, AddProductToContractRequest)> result = new List<(EstateDetails, Contract, AddProductToContractRequest)>();
        foreach (TableRow tableRow in tableRows)
        {
            String estateName = SpecflowTableHelper.GetStringRowValue(tableRow, "EstateName");
            EstateDetails estateDetails = estateDetailsList.SingleOrDefault(e => e.EstateName == estateName);
            estateDetails.ShouldNotBeNull();

            String contractName = SpecflowTableHelper.GetStringRowValue(tableRow, "ContractDescription");
            Contract contract = estateDetails.GetContract(contractName);

            String productValue = SpecflowTableHelper.GetStringRowValue(tableRow, "Value");

            var productTypeString = SpecflowTableHelper.GetStringRowValue(tableRow, "ProductType");
            var productType = Enum.Parse<ProductType>(productTypeString, true);
            AddProductToContractRequest addProductToContractRequest = new AddProductToContractRequest
                                                                      {
                                                                          ProductName =
                                                                              SpecflowTableHelper.GetStringRowValue(tableRow,
                                                                                                                    "ProductName"),
                                                                          DisplayText =
                                                                              SpecflowTableHelper.GetStringRowValue(tableRow,
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

    public static List<(EstateDetails, Contract, Product, AddTransactionFeeForProductToContractRequest)> ToAddTransactionFeeForProductToContractRequests(this TableRows tableRows, List<EstateDetails> estateDetailsList)
    {
        List<(EstateDetails, Contract, Product, AddTransactionFeeForProductToContractRequest)> result = new();
        foreach (TableRow tableRow in tableRows)
        {
            String estateName = SpecflowTableHelper.GetStringRowValue(tableRow, "EstateName");
            EstateDetails estateDetails = estateDetailsList.SingleOrDefault(e => e.EstateName == estateName);
            estateDetails.ShouldNotBeNull();

            String contractName = SpecflowTableHelper.GetStringRowValue(tableRow, "ContractDescription");
            Contract contract = estateDetails.GetContract(contractName);

            String productName = SpecflowTableHelper.GetStringRowValue(tableRow, "ProductName");
            Product product = contract.GetProduct(productName);

            var calculationTypeString = SpecflowTableHelper.GetStringRowValue(tableRow, "CalculationType");
            var calculationType = Enum.Parse<CalculationType>(calculationTypeString, true);
            AddTransactionFeeForProductToContractRequest addTransactionFeeForProductToContractRequest = new AddTransactionFeeForProductToContractRequest
                                                                                                        {
                                                                                                            Value =
                                                                                                                SpecflowTableHelper.GetDecimalValue(tableRow,
                                                                                                                                                    "Value"),
                                                                                                            Description =
                                                                                                                SpecflowTableHelper.GetStringRowValue(tableRow,
                                                                                                                                                      "FeeDescription"),
                                                                                                            CalculationType = calculationType
                                                                                                        };
            result.Add((estateDetails, contract, product, addTransactionFeeForProductToContractRequest));
        }

        return result;
    }

    public static List<(EstateDetails, Guid, MakeMerchantDepositRequest)> ToMakeMerchantDepositRequest(this TableRows tableRows, List<EstateDetails> estateDetailsList)
    {
        List<(EstateDetails, Guid, MakeMerchantDepositRequest)> result = new List<(EstateDetails, Guid, MakeMerchantDepositRequest)>();

        foreach (TableRow tableRow in tableRows)
        {
            String estateName = SpecflowTableHelper.GetStringRowValue(tableRow, "EstateName");
            EstateDetails estateDetails = estateDetailsList.SingleOrDefault(e => e.EstateName == estateName);
            estateDetails.ShouldNotBeNull();

            String merchantName = SpecflowTableHelper.GetStringRowValue(tableRow, "MerchantName");
            Guid merchantId = estateDetails.GetMerchantId(merchantName);

            MakeMerchantDepositRequest makeMerchantDepositRequest = new MakeMerchantDepositRequest
                                                                    {
                                                                        DepositDateTime = SpecflowTableHelper.GetDateForDateString(SpecflowTableHelper.GetStringRowValue(tableRow, "DateTime"), DateTime.UtcNow),
                                                                        Reference = SpecflowTableHelper.GetStringRowValue(tableRow, "Reference"),
                                                                        Amount = SpecflowTableHelper.GetDecimalValue(tableRow, "Amount")
                                                                    };

            result.Add((estateDetails, merchantId, makeMerchantDepositRequest));
        }

        return result;
    }

    public static List<(EstateDetails, Guid, MakeMerchantWithdrawalRequest)> ToMakeMerchantWithdrawalRequest(this TableRows tableRows, List<EstateDetails> estateDetailsList)
    {
        List<(EstateDetails, Guid, MakeMerchantWithdrawalRequest)> result = new List<(EstateDetails, Guid, MakeMerchantWithdrawalRequest)>();

        foreach (TableRow tableRow in tableRows)
        {
            String estateName = SpecflowTableHelper.GetStringRowValue(tableRow, "EstateName");
            EstateDetails estateDetails = estateDetailsList.SingleOrDefault(e => e.EstateName == estateName);
            estateDetails.ShouldNotBeNull();

            String merchantName = SpecflowTableHelper.GetStringRowValue(tableRow, "MerchantName");
            Guid merchantId = estateDetails.GetMerchantId(merchantName);

            MakeMerchantWithdrawalRequest makeMerchantWithdrawalRequest = new MakeMerchantWithdrawalRequest
                                                                          {
                                                                              WithdrawalDateTime =
                                                                                  SpecflowExtensions.GetDateTimeForDateString(SpecflowTableHelper
                                                                                                                                  .GetStringRowValue(tableRow,
                                                                                                                                                     "DateTime"),
                                                                                                                              DateTime.Now),
                                                                              Amount =
                                                                                  SpecflowTableHelper.GetDecimalValue(tableRow,
                                                                                                                      "Amount")
                                                                          };

            result.Add((estateDetails, merchantId, makeMerchantWithdrawalRequest));
        }

        return result;
    }

    public static DateTime GetDateTimeForDateString(String dateString,
                                                    DateTime today)
    {
        switch (dateString.ToUpper())
        {
            case "TODAY":
                return today;
            case "YESTERDAY":
                return today.AddDays(-1);
            case "LASTWEEK":
                return today.AddDays(-7);
            case "LASTMONTH":
                return today.AddMonths(-1);
            case "LASTYEAR":
                return today.AddYears(-1);
            case "TOMORROW":
                return today.AddDays(1);
            default:
                return DateTime.Parse(dateString);
        }
    }
}