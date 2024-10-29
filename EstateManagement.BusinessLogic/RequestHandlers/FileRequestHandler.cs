using System.Threading;
using System.Threading.Tasks;
using EstateManagement.BusinessLogic.Manger;
using EstateManagement.BusinessLogic.Requests;
using EstateManagement.Models.File;
using MediatR;
using SimpleResults;

namespace EstateManagement.BusinessLogic.RequestHandlers;

public class FileRequestHandler : IRequestHandler<FileQueries.GetFileQuery, Result<File>> {
    private readonly IEstateManagementManager EstateManagementManager;

    public FileRequestHandler(IEstateManagementManager estateManagementManager) {
        this.EstateManagementManager = estateManagementManager;
    }
    public async Task<Result<File>> Handle(FileQueries.GetFileQuery query,
                                           CancellationToken cancellationToken) {
        return await this.EstateManagementManager.GetFileDetails(query.EstateId, query.FileId, cancellationToken);
    }
}