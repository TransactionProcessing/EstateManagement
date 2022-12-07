namespace EstateManagement.BusinessLogic.Tests.Mediator;

using System;
using System.Threading;
using System.Threading.Tasks;
using BusinessLogic.Services;

public class DummyEstateDomainService : IEstateDomainService
{
    public async Task CreateEstate(Guid estateId,
                                   String estateName,
                                   CancellationToken cancellationToken) {
    }

    public async Task AddOperatorToEstate(Guid estateId,
                                          Guid operatorId,
                                          String operatorName,
                                          Boolean requireCustomMerchantNumber,
                                          Boolean requireCustomTerminalNumber,
                                          CancellationToken cancellationToken) {
    }

    public async Task<Guid> CreateEstateUser(Guid estateId,
                                             String emailAddress,
                                             String password,
                                             String givenName,
                                             String middleName,
                                             String familyName,
                                             CancellationToken cancellationToken) {
        return Guid.NewGuid();
    }
}