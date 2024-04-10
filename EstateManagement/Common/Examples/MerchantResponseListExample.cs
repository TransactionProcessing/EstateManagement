namespace EstateManagement.Common.Examples
{
    using System;
    using System.Collections.Generic;
    using DataTransferObjects.Responses;
    using Swashbuckle.AspNetCore.Filters;
    using System.Diagnostics.CodeAnalysis;
    using DataTransferObjects.Responses.Contract;
    using DataTransferObjects.Responses.Merchant;

    [ExcludeFromCodeCoverage]
    public class MerchantResponseListExample : IExamplesProvider<List<MerchantResponse>>
    {
        public List<MerchantResponse> GetExamples()
        {
            return new List<MerchantResponse>
                   {
                       new MerchantResponse
                       {
                           EstateId = ExampleData.EstateId,
                           MerchantId = ExampleData.MerchantId,
                           Operators = new List<MerchantOperatorResponse>
                                       {
                                           new MerchantOperatorResponse
                                           {
                                               OperatorId = ExampleData.OperatorId,
                                               MerchantNumber = ExampleData.OperatorMerchantNumber,
                                               Name = ExampleData.OperatorName,
                                               TerminalNumber = ExampleData.OperatorTerminalNumber
                                           }
                                       },
                           Addresses = new List<AddressResponse>
                                       {
                                           new AddressResponse
                                           {
                                               AddressLine1 = ExampleData.AddressLine1,
                                               AddressLine2 = ExampleData.AddressLine2,
                                               AddressLine3 = ExampleData.AddressLine3,
                                               AddressLine4 = ExampleData.AddressLine4,
                                               Country = ExampleData.Country,
                                               PostalCode = ExampleData.PostalCode,
                                               Region = ExampleData.Region,
                                               Town = ExampleData.Town,
                                               AddressId = ExampleData.AddressId
                                           }
                                       },
                           Contacts = new List<ContactResponse>
                                      {
                                          new ContactResponse
                                          {
                                              ContactName = ExampleData.ContactName,
                                              ContactPhoneNumber = ExampleData.ContactPhoneNumber,
                                              ContactEmailAddress = ExampleData.ContactEmailAddress,
                                              ContactId = ExampleData.ContactId
                                          }
                                      },
                           Devices = new Dictionary<Guid, String>()
                                     {
                                         {ExampleData.DeviceId, ExampleData.DeviceIdentifier}
                                     },
                           MerchantName = ExampleData.MerchantName

                       }
                   };

        }
    }
}