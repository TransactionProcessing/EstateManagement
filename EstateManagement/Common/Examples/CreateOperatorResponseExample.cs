﻿namespace EstateManagement.Common.Examples
{
    using DataTransferObjects.Responses;
    using Swashbuckle.AspNetCore.Filters;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    public class CreateOperatorResponseExample : IExamplesProvider<CreateOperatorResponse>
    {
        public CreateOperatorResponse GetExamples()
        {
            return new CreateOperatorResponse
                   {
                       EstateId = ExampleData.EstateId,
                       OperatorId = ExampleData.OperatorId
                   };
        }
    }
}