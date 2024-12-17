using SimpleResults;

namespace EstateManagement.Factories{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using DataTransferObjects.Responses;
    using DataTransferObjects.Responses.Contract;
    using DataTransferObjects.Responses.Estate;
    using DataTransferObjects.Responses.File;
    using DataTransferObjects.Responses.Merchant;
    using DataTransferObjects.Responses.Operator;
    using DataTransferObjects.Responses.Settlement;
    using Models;
    using Models.Contract;
    using Models.Estate;
    using Models.File;
    using Models.Merchant;
    using AddressResponse = DataTransferObjects.Responses.Merchant.AddressResponse;
    using CalculationType = DataTransferObjects.Responses.Contract.CalculationType;
    using Contract = Models.Contract.Contract;
    using FeeType = DataTransferObjects.Responses.Contract.FeeType;
    using MerchantContractResponse = DataTransferObjects.Responses.Merchant.MerchantContractResponse;
    using MerchantOperatorResponse = DataTransferObjects.Responses.Merchant.MerchantOperatorResponse;
    using MerchantResponse = DataTransferObjects.Responses.Merchant.MerchantResponse;
    using Operator = Models.Operator.Operator;
    using SettlementSchedule = DataTransferObjects.Responses.Merchant.SettlementSchedule;
    using ProductType = DataTransferObjects.Responses.Contract.ProductType;
    using EstateManagement.Database.Entities;


    /// <summary>
    /// 
    /// </summary>

    public static class ModelFactory{
        #region Methods

        public static Result<List<ContractResponse>> ConvertFrom(List<Contract> contracts){
            List<Result<ContractResponse>> result = new();

            contracts.ForEach(c => result.Add(ModelFactory.ConvertFrom(c)));

            if (result.Any(c => c.IsFailed))
                return Result.Failure("Failed converting contracts");

            return Result.Success(result.Select(r => r.Data).ToList());
        }

        public static Result<ContractResponse> ConvertFrom(Contract contract){
            if (contract == null){
                return Result.Invalid("contract cannot be null");
            }

            ContractResponse contractResponse = new ContractResponse{
                                                                        EstateId = contract.EstateId,
                                                                        EstateReportingId = contract.EstateReportingId,
                                                                        ContractId = contract.ContractId,
                                                                        ContractReportingId = contract.ContractReportingId,
                                                                        OperatorId = contract.OperatorId,
                                                                        OperatorName = contract.OperatorName,
                                                                        Description = contract.Description
                                                                    };

            if (contract.Products != null && contract.Products.Any()){
                contractResponse.Products = new List<DataTransferObjects.Responses.Contract.ContractProduct>();

                contract.Products.ForEach(p => {
                                              DataTransferObjects.Responses.Contract.ContractProduct contractProduct = new DataTransferObjects.Responses.Contract.ContractProduct{
                                                                                                       ProductReportingId = p.ContractProductReportingId,
                                                                                                       ProductId = p.ContractProductId,
                                                                                                       Value = p.Value,
                                                                                                       DisplayText = p.DisplayText,
                                                                                                       Name = p.Name,
                                                                                                       ProductType = Enum.Parse<ProductType>(p.ProductType.ToString())
                                                                                                   };
                                              if (p.TransactionFees != null && p.TransactionFees.Any()){
                                                  contractProduct.TransactionFees = new List<DataTransferObjects.Responses.Contract.ContractProductTransactionFee>();
                                                  p.TransactionFees.ForEach(tf => {
                                                      DataTransferObjects.Responses.Contract.ContractProductTransactionFee transactionFee = new DataTransferObjects.Responses.Contract.ContractProductTransactionFee
                                                      {
                                                                                                                                                                    TransactionFeeId = tf.TransactionFeeId,
                                                                                                                                                                    Value = tf.Value,
                                                                                                                                                                    Description = tf.Description,
                                                                                                                                                                };
                                                                                transactionFee.CalculationType =
                                                                                    Enum.Parse<CalculationType>(tf.CalculationType.ToString());

                                                                                contractProduct.TransactionFees.Add(transactionFee);
                                                                            });
                                              }

                                              contractResponse.Products.Add(contractProduct);
                                          });
            }

            return Result.Success(contractResponse);
        }

        public static Result<List<EstateResponse>> ConvertFrom(List<Models.Estate.Estate> estates) {
            List<Result<EstateResponse>> result = new();

            estates.ForEach(c => result.Add(ModelFactory.ConvertFrom(c)));

            if (result.Any(c => c.IsFailed))
                return Result.Failure("Failed converting estates");

            return Result.Success(result.Select(r => r.Data).ToList());

        }

        public static Result<EstateResponse> ConvertFrom(Models.Estate.Estate estate){
            if (estate == null){
                return Result.Invalid("estate cannot be null");
            }

            EstateResponse estateResponse = new EstateResponse{
                                                                  EstateName = estate.Name,
                                                                  EstateId = estate.EstateId,
                                                                  EstateReportingId = estate.EstateReportingId,
                                                                  EstateReference = estate.Reference,
                                                                  Operators = new List<EstateOperatorResponse>(),
                                                                  SecurityUsers = new List<SecurityUserResponse>()
                                                              };

            if (estate.Operators != null && estate.Operators.Any()){
                estate.Operators.ForEach(o => estateResponse.Operators.Add(new EstateOperatorResponse{
                                                                                                         OperatorId = o.OperatorId,
                                                                                                         //RequireCustomTerminalNumber = o.RequireCustomTerminalNumber,
                                                                                                         //RequireCustomMerchantNumber = o.RequireCustomMerchantNumber,
                                                                                                         Name = o.Name,
                                                                                                         IsDeleted = o.IsDeleted
                                                                                                     }));
            }

            if (estate.SecurityUsers != null && estate.SecurityUsers.Any()){
                estate.SecurityUsers.ForEach(s => estateResponse.SecurityUsers.Add(new SecurityUserResponse{
                                                                                                               EmailAddress = s.EmailAddress,
                                                                                                               SecurityUserId = s.SecurityUserId
                                                                                                           }));
            }

            return Result.Success(estateResponse);
        }

        public static Result<MerchantResponse> ConvertFrom(Models.Merchant.Merchant merchant){
            if (merchant == null){
                return Result.Invalid("merchant cannot be null");
            }

            MerchantResponse merchantResponse = new MerchantResponse{
                                                                        EstateId = merchant.EstateId,
                                                                        EstateReportingId = merchant.EstateReportingId,
                                                                        MerchantId = merchant.MerchantId,
                                                                        MerchantReportingId = merchant.MerchantReportingId,
                                                                        MerchantName = merchant.MerchantName,
                                                                        SettlementSchedule = ModelFactory.ConvertFrom(merchant.SettlementSchedule),
                                                                        MerchantReference = merchant.Reference,
                                                                        NextStatementDate = merchant.NextStatementDate
                                                                    };

            if (merchant.Addresses != null && merchant.Addresses.Any()){
                merchantResponse.Addresses = new List<AddressResponse>();

                merchant.Addresses.ForEach(a => merchantResponse.Addresses.Add(new AddressResponse{
                                                                                                      AddressId = a.AddressId,
                                                                                                      Town = a.Town,
                                                                                                      Region = a.Region,
                                                                                                      PostalCode = a.PostalCode,
                                                                                                      Country = a.Country,
                                                                                                      AddressLine1 = a.AddressLine1,
                                                                                                      AddressLine2 = a.AddressLine2,
                                                                                                      AddressLine3 = a.AddressLine3,
                                                                                                      AddressLine4 = a.AddressLine4
                                                                                                  }));
            }

            if (merchant.Contacts != null && merchant.Contacts.Any()){
                merchantResponse.Contacts = new List<ContactResponse>();

                merchant.Contacts.ForEach(c => merchantResponse.Contacts.Add(new ContactResponse{
                                                                                                    ContactId = c.ContactId,
                                                                                                    ContactPhoneNumber = c.ContactPhoneNumber,
                                                                                                    ContactEmailAddress = c.ContactEmailAddress,
                                                                                                    ContactName = c.ContactName
                                                                                                }));
            }

            if (merchant.Devices != null && merchant.Devices.Any()){
                merchantResponse.Devices = new Dictionary<Guid, String>();

                foreach (Device device in merchant.Devices){
                    merchantResponse.Devices.Add(device.DeviceId, device.DeviceIdentifier);
                }
            }

            if (merchant.Operators != null && merchant.Operators.Any()){
                merchantResponse.Operators = new List<MerchantOperatorResponse>();

                merchant.Operators.ForEach(a => merchantResponse.Operators.Add(new MerchantOperatorResponse{
                                                                                                               Name = a.Name,
                                                                                                               MerchantNumber = a.MerchantNumber,
                                                                                                               OperatorId = a.OperatorId,
                                                                                                               TerminalNumber = a.TerminalNumber,
                                                                                                               IsDeleted = a.IsDeleted
                                                                                                           }));
            }

            if (merchant.Contracts != null && merchant.Contracts.Any()){
                merchantResponse.Contracts = new List<MerchantContractResponse>();
                merchant.Contracts.ForEach(mc => {
                                               merchantResponse.Contracts.Add(new MerchantContractResponse(){
                                                                                                                ContractId = mc.ContractId,
                                                                                                                ContractProducts = mc.ContractProducts,
                                                                                                                IsDeleted = mc.IsDeleted,
                                                                                                            });
                                           });
            }

            return merchantResponse;
        }

        private static SettlementSchedule ConvertFrom(Models.SettlementSchedule settlementSchedule){
            return settlementSchedule switch{
                Models.SettlementSchedule.Weekly => SettlementSchedule.Weekly,
                Models.SettlementSchedule.Monthly => SettlementSchedule.Monthly,
                Models.SettlementSchedule.Immediate => SettlementSchedule.Immediate,
                Models.SettlementSchedule.NotSet => SettlementSchedule.NotSet,
            };
        }

        public static Result<List<MerchantResponse>> ConvertFrom(List<Models.Merchant.Merchant> merchants){
            List<Result<MerchantResponse>> result = new();

            if (merchants == null)
                return Result.Success(new List<MerchantResponse>());

            merchants.ForEach(c => result.Add(ModelFactory.ConvertFrom(c)));

            if (result.Any(c => c.IsFailed))
                return Result.Failure("Failed converting merchants");

            return Result.Success(result.Select(r => r.Data).ToList());
        }

        public static Result<List<DataTransferObjects.Responses.Contract.ContractProductTransactionFee>> ConvertFrom(List<Models.Contract.ContractProductTransactionFee> transactionFees){
            List<DataTransferObjects.Responses.Contract.ContractProductTransactionFee> result = new ();
            transactionFees.ForEach(tf => {
                                        DataTransferObjects.Responses.Contract.ContractProductTransactionFee transactionFee = new DataTransferObjects.Responses.Contract.ContractProductTransactionFee
                                        {
                                            TransactionFeeId = tf.TransactionFeeId,
                                                                                                                            Value = tf.Value,
                                                                                                                            Description = tf.Description,
                                                                                                                        };
                                        transactionFee.CalculationType = Enum.Parse<CalculationType>(tf.CalculationType.ToString());
                                        transactionFee.FeeType = Enum.Parse<FeeType>(tf.FeeType.ToString());

                                        result.Add(transactionFee);
                                    });

            return Result.Success(result);
        }

        public static SettlementFeeResponse ConvertFrom(SettlementFeeModel model){
            if (model == null){
                return null;
            }

            SettlementFeeResponse response = new SettlementFeeResponse{
                                                                          SettlementDate = model.SettlementDate,
                                                                          CalculatedValue = model.CalculatedValue,
                                                                          SettlementId = model.SettlementId,
                                                                          MerchantId = model.MerchantId,
                                                                          MerchantName = model.MerchantName,
                                                                          FeeDescription = model.FeeDescription,
                                                                          TransactionId = model.TransactionId,
                                                                          IsSettled = model.IsSettled,
                                                                          OperatorIdentifier = model.OperatorIdentifier
                                                                      };

            return response;
        }

        //public static List<SettlementFeeResponse> ConvertFrom(List<SettlementFeeModel> model){
        //    if (model == null || model.Any() == false){
        //        return new List<SettlementFeeResponse>();
        //    }

        //    List<SettlementFeeResponse> response = new List<SettlementFeeResponse>();

        //    model.ForEach(m => response.Add(ModelFactory.ConvertFrom(m)));

        //    return response;
        //}

        public static Result<SettlementResponse> ConvertFrom(SettlementModel settlementModel){
            if (settlementModel == null){
                return Result.Invalid("settlementModel cannot be null");
            }

            SettlementResponse response = new SettlementResponse{
                                                                    SettlementDate = settlementModel.SettlementDate,
                                                                    IsCompleted = settlementModel.IsCompleted,
                                                                    NumberOfFeesSettled = settlementModel.NumberOfFeesSettled,
                                                                    SettlementId = settlementModel.SettlementId,
                                                                    ValueOfFeesSettled = settlementModel.ValueOfFeesSettled,
                                                                };

            settlementModel.SettlementFees.ForEach(f => response.SettlementFees.Add(ModelFactory.ConvertFrom(f)));

            return Result.Success(response);
        }

        public static Result<List<SettlementResponse>> ConvertFrom(List<SettlementModel> settlementModels){
            List<Result<SettlementResponse>> result = new();
            if (settlementModels == null) {
                return Result.Invalid("settlement models cannot be null");
            }
            settlementModels.ForEach(c => result.Add(ModelFactory.ConvertFrom(c)));

            if (result.Any(c => c.IsFailed))
                return Result.Failure("Failed converting settlementModels");

            return Result.Success(result.Select(r => r.Data).ToList());
        }

        public static Result<FileDetailsResponse> ConvertFrom(Models.File.File file){
            if (file == null){
                return Result.Invalid("file cannot be null");
            }

            FileDetailsResponse response = new() {
                Merchant = ModelFactory.ConvertFrom(file.Merchant), FileId = file.FileId, FileReceivedDate = file.FileReceivedDate,
                FileReceivedDateTime = file.FileReceivedDateTime,
                FileLineDetails = []
            };

            foreach (FileLineDetails modelFileLineDetail in file.FileLineDetails){
                FileLineDetailsResponse fileLineDetailsResponse = new();
                fileLineDetailsResponse.FileLineData = modelFileLineDetail.FileLineData;
                fileLineDetailsResponse.Status = modelFileLineDetail.Status;
                fileLineDetailsResponse.FileLineNumber = modelFileLineDetail.FileLineNumber;
                if (modelFileLineDetail.Transaction != null){
                    fileLineDetailsResponse.Transaction = new TransactionResponse{
                                                                             IsAuthorised = modelFileLineDetail.Transaction.IsAuthorised,
                                                                             IsCompleted = modelFileLineDetail.Transaction.IsCompleted,
                                                                             ResponseMessage = modelFileLineDetail.Transaction.ResponseMessage,
                                                                             AuthCode = modelFileLineDetail.Transaction.AuthCode,
                                                                             ResponseCode = modelFileLineDetail.Transaction.ResponseCode,
                                                                             TransactionId = modelFileLineDetail.Transaction.TransactionId,
                                                                             TransactionNumber = modelFileLineDetail.Transaction.TransactionNumber,
                                                                         };
                }

                response.FileLineDetails.Add(fileLineDetailsResponse);
            }

            return Result.Success(response);
        }

        #endregion

        public static Result<OperatorResponse> ConvertFrom(Operator @operator){
            if (@operator == null){
                return Result.Invalid("operator cannot be null");
            }

            OperatorResponse response = new();
            response.OperatorId = @operator.OperatorId;
            response.RequireCustomTerminalNumber = @operator.RequireCustomTerminalNumber;
            response.RequireCustomMerchantNumber = @operator.RequireCustomMerchantNumber;
            response.Name = @operator.Name;

            return Result.Success(response);
        }

        public static Result<List<OperatorResponse>> ConvertFrom(List<Operator> @operators)
        {
            if (@operators == null || @operators.Any() == false)
            {
                return Result.Success(new List<OperatorResponse>());
            }

            List<Result<OperatorResponse>> result = new();

            @operators.ForEach(c => result.Add(ModelFactory.ConvertFrom(c)));

            if (result.Any(c => c.IsFailed))
                return Result.Failure("Failed converting operators");

            return Result.Success(result.Select(r => r.Data).ToList());
        }
    }
}