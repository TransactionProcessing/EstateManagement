namespace EstateManagement.Client{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using DataTransferObjects.Requests.Contract;
    using DataTransferObjects.Requests.Estate;
    using DataTransferObjects.Requests.Merchant;
    using DataTransferObjects.Requests.Operator;
    using DataTransferObjects.Responses.Contract;
    using DataTransferObjects.Responses.Estate;
    using DataTransferObjects.Responses.Merchant;
    using DataTransferObjects.Responses.Operator;
    using DataTransferObjects.Responses.Settlement;

    /// <summary>
    /// 
    /// </summary>
    public interface IEstateClient{
        #region Methods

        Task AddContractToMerchant(String accessToken,
                                   Guid estateId,
                                   Guid merchantId,
                                   AddMerchantContractRequest request,
                                   CancellationToken cancellationToken);

        Task AddDeviceToMerchant(String accessToken,
                                 Guid estateId,
                                 Guid merchantId,
                                 AddMerchantDeviceRequest request,
                                 CancellationToken cancellationToken);

        Task<AddProductToContractResponse> AddProductToContract(String accessToken,
                                                                Guid estateId,
                                                                Guid contractId,
                                                                AddProductToContractRequest addProductToContractRequest,
                                                                CancellationToken cancellationToken);

        Task<AddTransactionFeeForProductToContractResponse> AddTransactionFeeForProductToContract(String accessToken,
                                                                                                  Guid estateId,
                                                                                                  Guid contractId,
                                                                                                  Guid productId,
                                                                                                  AddTransactionFeeForProductToContractRequest
                                                                                                      addTransactionFeeForProductToContractRequest,
                                                                                                  CancellationToken cancellationToken);

        Task AssignOperatorToMerchant(String accessToken,
                                                              Guid estateId,
                                                              Guid merchantId,
                                                              AssignOperatorRequest assignOperatorRequest,
                                                              CancellationToken cancellationToken);

        Task RemoveOperatorFromMerchant(String accessToken,
                                        Guid estateId,
                                        Guid merchantId,
                                        Guid operatorId,
                                        CancellationToken cancellationToken);

        Task<CreateContractResponse> CreateContract(String accessToken,
                                                    Guid estateId,
                                                    CreateContractRequest createContractRequest,
                                                    CancellationToken cancellationToken);

        Task<CreateEstateResponse> CreateEstate(String accessToken,
                                                CreateEstateRequest createEstateRequest,
                                                CancellationToken cancellationToken);

        Task<CreateEstateUserResponse> CreateEstateUser(String accessToken,
                                                        Guid estateId,
                                                        CreateEstateUserRequest createEstateUserRequest,
                                                        CancellationToken cancellationToken);

        Task<CreateMerchantResponse> CreateMerchant(String accessToken,
                                                    Guid estateId,
                                                    CreateMerchantRequest createMerchantRequest,
                                                    CancellationToken cancellationToken);

        Task<CreateMerchantUserResponse> CreateMerchantUser(String accessToken,
                                                            Guid estateId,
                                                            Guid merchantId,
                                                            CreateMerchantUserRequest createMerchantUserRequest,
                                                            CancellationToken cancellationToken);

        Task<CreateOperatorResponse> CreateOperator(String accessToken,
                                                    Guid estateId,
                                                    CreateOperatorRequest createOperatorRequest,
                                                    CancellationToken cancellationToken);

        Task DisableTransactionFeeForProduct(String accessToken,
                                             Guid estateId,
                                             Guid contractId,
                                             Guid productId,
                                             Guid transactionFeeId,
                                             CancellationToken cancellationToken);

        Task<ContractResponse> GetContract(String accessToken,
                                           Guid estateId,
                                           Guid contractId,
                                           CancellationToken cancellationToken);

        Task<List<ContractResponse>> GetContracts(String accessToken,
                                                  Guid estateId,
                                                  CancellationToken cancellationToken);

        Task<EstateResponse> GetEstate(String accessToken,
                                       Guid estateId,
                                       CancellationToken cancellationToken);

        Task<List<EstateResponse>> GetEstates(String accessToken,
                                              Guid estateId,
                                              CancellationToken cancellationToken);

        Task<MerchantResponse> GetMerchant(String accessToken,
                                           Guid estateId,
                                           Guid merchantId,
                                           CancellationToken cancellationToken);

        Task<List<ContractResponse>> GetMerchantContracts(String accessToken,
                                                          Guid estateId,
                                                          Guid merchantId,
                                                          CancellationToken cancellationToken);

        Task<List<MerchantResponse>> GetMerchants(String accessToken,
                                                  Guid estateId,
                                                  CancellationToken cancellationToken);

        Task<SettlementResponse> GetSettlement(String accessToken,
                                               Guid estateId,
                                               Guid? merchantId,
                                               Guid settlementId,
                                               CancellationToken cancellationToken);

        Task<List<SettlementResponse>> GetSettlements(String accessToken,
                                                      Guid estateId,
                                                      Guid? merchantId,
                                                      String startDate,
                                                      String endDate,
                                                      CancellationToken cancellationToken);

        Task<List<ContractProductTransactionFee>> GetTransactionFeesForProduct(String accessToken,
                                                                               Guid estateId,
                                                                               Guid merchantId,
                                                                               Guid contractId,
                                                                               Guid productId,
                                                                               CancellationToken cancellationToken);

        Task<MakeMerchantDepositResponse> MakeMerchantDeposit(String accessToken,
                                                              Guid estateId,
                                                              Guid merchantId,
                                                              MakeMerchantDepositRequest makeMerchantDepositRequest,
                                                              CancellationToken cancellationToken);

        Task<MakeMerchantWithdrawalResponse> MakeMerchantWithdrawal(String accessToken,
                                                                    Guid estateId,
                                                                    Guid merchantId,
                                                                    MakeMerchantWithdrawalRequest makeMerchantWithdrawalRequest,
                                                                    CancellationToken cancellationToken);

        Task SetMerchantSettlementSchedule(String accessToken,
                                           Guid estateId,
                                           Guid merchantId,
                                           SetSettlementScheduleRequest setSettlementScheduleRequest,
                                           CancellationToken cancellationToken);

        Task SwapDeviceForMerchant(String accessToken,
                                                               Guid estateId,
                                                               Guid merchantId,
                                                               SwapMerchantDeviceRequest request,
                                                               CancellationToken cancellationToken);

        Task UpdateMerchant(String accessToken,
                            Guid estateId,
                            Guid merchantId,
                            UpdateMerchantRequest request,
                            CancellationToken cancellationToken);

        Task AddMerchantAddress(String accessToken,
                                   Guid estateId,
                                   Guid merchantId,
                                   Address newAddressRequest,
                                   CancellationToken cancellationToken);

        Task UpdateMerchantAddress(String accessToken,
                                   Guid estateId,
                                   Guid merchantId,
                                   Guid addressId,
                                   Address updatedAddressRequest,
                                   CancellationToken cancellationToken);

        Task AddMerchantContact(String accessToken,
                                Guid estateId,
                                Guid merchantId,
                                Contact newContactRequest,
                                CancellationToken cancellationToken);

        Task UpdateMerchantContact(String accessToken,
                                   Guid estateId,
                                   Guid merchantId,
                                   Guid contactId,
                                   Contact updatedContactRequest,
                                   CancellationToken cancellationToken);

        #endregion
    }
}