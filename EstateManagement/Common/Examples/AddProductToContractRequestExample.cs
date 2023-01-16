namespace EstateManagement.Common.Examples
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using DataTransferObjects;
    using DataTransferObjects.Requests;
    using Swashbuckle.AspNetCore.Filters;

    [ExcludeFromCodeCoverage]
    public class AddProductToContractRequestExample : IMultipleExamplesProvider<AddProductToContractRequest>
    {
        public IEnumerable<SwaggerExample<AddProductToContractRequest>> GetExamples()
        {
            SwaggerExample<AddProductToContractRequest> addProductToContractWithNullValue = new SwaggerExample<AddProductToContractRequest>
                                                                                            {
                                                                                                Name = "Product With Variable Value",
                                                                                                Value =  new AddProductToContractRequest{
                                                                                                             DisplayText = ExampleData.ProductDisplayText,
                                                                                                             ProductName = ExampleData.ProductName,
                                                                                                             Value = ExampleData.ProductNullValue,
                                                                                                             ProductType = ProductType.MobileTopup
                                                                                                         }
                                                                                            };

            SwaggerExample<AddProductToContractRequest> addProductToContractWithValue = new SwaggerExample<AddProductToContractRequest>
                                                                                        {
                                                                                            Name = "Product With Fixed Value",
                                                                                            Value = new AddProductToContractRequest
                                                                                                    {
                                                                                                        DisplayText = ExampleData.ProductDisplayText,
                                                                                                        ProductName = ExampleData.ProductName,
                                                                                                        Value = ExampleData.ProductValue,
                                                                                                        ProductType = ProductType.MobileTopup
                                                                                            }
                                                                                        };

            return new List<SwaggerExample<AddProductToContractRequest>>
                   {
                       addProductToContractWithValue,
                       addProductToContractWithNullValue
                   };
        }
    }
}