namespace EstateManagement.Common.Examples;

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using DataTransferObjects.Responses;
using Swashbuckle.AspNetCore.Filters;

[ExcludeFromCodeCoverage]
public class EstatesResponseExample : IExamplesProvider<List<EstateResponse>>
{
    public List<EstateResponse> GetExamples(){
        return new List<EstateResponse>{



                                           new EstateResponse{
                                                                 EstateId = ExampleData.EstateId,
                                                                 EstateName = ExampleData.EstateName,
                                                                 Operators = new List<EstateOperatorResponse>{
                                                                                                                 new EstateOperatorResponse{
                                                                                                                                               OperatorId = ExampleData.OperatorId,
                                                                                                                                               Name = ExampleData.OperatorName,
                                                                                                                                               RequireCustomMerchantNumber = true,
                                                                                                                                               RequireCustomTerminalNumber = true
                                                                                                                                           }
                                                                                                             },
                                                                 SecurityUsers = new List<SecurityUserResponse>{
                                                                                                                   new SecurityUserResponse{
                                                                                                                                               EmailAddress = ExampleData.EstateUserEmailAddress,
                                                                                                                                               SecurityUserId = ExampleData.UserId
                                                                                                                                           }
                                                                                                               }
                                                             }
                                       };
    }
}