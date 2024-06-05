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


    /// <summary>
    /// 
    /// </summary>

    public static class ModelFactory{
        #region Methods

        public static List<ContractResponse> ConvertFrom(List<Contract> contracts){
            List<ContractResponse> result = new List<ContractResponse>();

            contracts.ForEach(c => result.Add(ModelFactory.ConvertFrom(c)));

            return result;
        }

        public static ContractResponse ConvertFrom(Contract contract){
            if (contract == null){
                return null;
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
                contractResponse.Products = new List<ContractProduct>();

                contract.Products.ForEach(p => {
                                              ContractProduct contractProduct = new ContractProduct{
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

            return contractResponse;
        }

        public static EstateResponse ConvertFrom(Estate estate){
            if (estate == null){
                return null;
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

            return estateResponse;
        }

        public static MerchantResponse ConvertFrom(Merchant merchant){
            if (merchant == null){
                return null;
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

        public static List<MerchantResponse> ConvertFrom(List<Merchant> merchants){
            List<MerchantResponse> result = new List<MerchantResponse>();

            merchants.ForEach(m => result.Add(ModelFactory.ConvertFrom(m)));

            return result;
        }

        public static List<DataTransferObjects.Responses.Contract.ContractProductTransactionFee> ConvertFrom(List<Models.Contract.ContractProductTransactionFee> transactionFees){
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

            return result;
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

        public static List<SettlementFeeResponse> ConvertFrom(List<SettlementFeeModel> model){
            if (model == null || model.Any() == false){
                return new List<SettlementFeeResponse>();
            }

            List<SettlementFeeResponse> response = new List<SettlementFeeResponse>();

            model.ForEach(m => response.Add(ModelFactory.ConvertFrom(m)));

            return response;
        }

        public static SettlementResponse ConvertFrom(SettlementModel model){
            if (model == null){
                return null;
            }

            SettlementResponse response = new SettlementResponse{
                                                                    SettlementDate = model.SettlementDate,
                                                                    IsCompleted = model.IsCompleted,
                                                                    NumberOfFeesSettled = model.NumberOfFeesSettled,
                                                                    SettlementId = model.SettlementId,
                                                                    ValueOfFeesSettled = model.ValueOfFeesSettled,
                                                                };

            model.SettlementFees.ForEach(f => response.SettlementFees.Add(ModelFactory.ConvertFrom(f)));

            return response;
        }

        public static List<SettlementResponse> ConvertFrom(List<SettlementModel> model){
            if (model == null || model.Any() == false){
                return new List<SettlementResponse>();
            }

            List<SettlementResponse> response = new List<SettlementResponse>();

            model.ForEach(m => response.Add(ModelFactory.ConvertFrom(m)));

            return response;
        }

        public static FileDetailsResponse ConvertFrom(File model){
            if (model == null){
                return null;
            }

            FileDetailsResponse response = new FileDetailsResponse();

            response.Merchant = ModelFactory.ConvertFrom(model.Merchant);
            response.FileId = model.FileId;
            response.FileReceivedDate = model.FileReceivedDate;
            response.FileReceivedDateTime = model.FileReceivedDateTime;
            response.FileLineDetails = new List<FileLineDetailsResponse>();

            foreach (FileLineDetails modelFileLineDetail in model.FileLineDetails){
                FileLineDetailsResponse fileLineDetailsResponse = new FileLineDetailsResponse();
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

            return response;
        }

        #endregion

        public static OperatorResponse ConvertFrom(Operator model){
            if (model == null){
                return null;
            }

            OperatorResponse response = new();
            response.OperatorId = model.OperatorId;
            response.RequireCustomTerminalNumber = model.RequireCustomTerminalNumber;
            response.RequireCustomMerchantNumber = model.RequireCustomMerchantNumber;
            response.Name = model.Name;

            return response;
        }

        public static List<OperatorResponse> ConvertFrom(List<Operator> models)
        {
            if (models == null || models.Any() == false)
            {
                return new List<OperatorResponse>();
            }

            List<OperatorResponse> response = new();

            models.ForEach(o => response.Add(ModelFactory.ConvertFrom(o)));

            return response;
        }
    }
}