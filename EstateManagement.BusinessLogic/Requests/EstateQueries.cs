using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstateManagement.BusinessLogic.Requests
{
    using MediatR;

    [ExcludeFromCodeCoverage]
    public class EstateQueries
    {
        public record GetEstateQuery(Guid EstateId) : IRequest<Models.Estate.Estate>;
        public record GetEstatesQuery(Guid EstateId) : IRequest<List<Models.Estate.Estate>>;
    }
}
