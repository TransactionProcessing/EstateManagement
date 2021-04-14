namespace EstateManagement.Common.Examples
{
    using System.Collections.Generic;
    using DataTransferObjects.Requests;
    using Swashbuckle.AspNetCore.Filters;

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
                                                                                                             Value = ExampleData.ProductNullValue
                                                                                                         }
                                                                                            };

            SwaggerExample<AddProductToContractRequest> addProductToContractWithValue = new SwaggerExample<AddProductToContractRequest>
                                                                                        {
                                                                                            Name = "Product With Fixed Value",
                                                                                            Value = new AddProductToContractRequest
                                                                                                    {
                                                                                                        DisplayText = ExampleData.ProductDisplayText,
                                                                                                        ProductName = ExampleData.ProductName,
                                                                                                        Value = ExampleData.ProductValue
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