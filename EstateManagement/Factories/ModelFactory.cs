namespace EstateManagement.Factories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using DataTransferObjects.Responses;
    using Microsoft.Extensions.DependencyModel.Resolution;
    using Models;
    using Models.Contract;
    using Models.Estate;
    using Models.Merchant;
    using CalculationType = DataTransferObjects.CalculationType;
    using FeeType = DataTransferObjects.FeeType;
    using SettlementSchedule = DataTransferObjects.SettlementSchedule;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="EstateManagement.Factories.IModelFactory" />
    public class ModelFactory : IModelFactory
    {
        #region Methods

        /// <summary>
        /// Converts from.
        /// </summary>
        /// <param name="contracts">The contracts.</param>
        /// <returns></returns>
        public List<ContractResponse> ConvertFrom(List<Contract> contracts)
        {
            List<ContractResponse> result = new List<ContractResponse>();

            contracts.ForEach(c => result.Add(this.ConvertFrom(c)));

            return result;
        }

        public ContractResponse ConvertFrom(Contract contract)
        {
            if (contract == null)
            {
                return null;
            }

            ContractResponse contractResponse = new ContractResponse
                                                {
                                                    ContractId = contract.ContractId,
                                                    EstateId = contract.EstateId,
                                                    OperatorId = contract.OperatorId,
                                                    OperatorName = contract.OperatorName,
                                                    Description = contract.Description
                                                };

            if (contract.Products != null && contract.Products.Any())
            {
                contractResponse.Products = new List<ContractProduct>();

                contract.Products.ForEach(p =>
                                          {
                                              ContractProduct contractProduct = new ContractProduct
                                                                                {
                                                                                    ProductId = p.ProductId,
                                                                                    Value = p.Value,
                                                                                    DisplayText = p.DisplayText,
                                                                                    Name = p.Name
                                                                                };
                                              if (p.TransactionFees != null && p.TransactionFees.Any())
                                              {
                                                  contractProduct.TransactionFees = new List<ContractProductTransactionFee>();
                                                  p.TransactionFees.ForEach(tf =>
                                                                            {
                                                                                ContractProductTransactionFee transactionFee = new ContractProductTransactionFee
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

        /// <summary>
        /// Converts from.
        /// </summary>
        /// <param name="estate">The estate.</param>
        /// <returns></returns>
        public EstateResponse ConvertFrom(Estate estate)
        {
            if (estate == null)
            {
                return null;
            }

            EstateResponse estateResponse = new EstateResponse
                                            {
                                                EstateName = estate.Name,
                                                EstateId = estate.EstateId,
                                                EstateReference = estate.Reference,
                                                Operators = new List<EstateOperatorResponse>(),
                                                SecurityUsers = new List<SecurityUserResponse>()
                                            };

            if (estate.Operators != null && estate.Operators.Any())
            {
                estate.Operators.ForEach(o => estateResponse.Operators.Add(new EstateOperatorResponse
                                                                           {
                                                                               Name = o.Name,
                                                                               OperatorId = o.OperatorId,
                                                                               RequireCustomMerchantNumber = o.RequireCustomMerchantNumber,
                                                                               RequireCustomTerminalNumber = o.RequireCustomTerminalNumber
                                                                           }));
            }

            if (estate.SecurityUsers != null && estate.SecurityUsers.Any())
            {
                estate.SecurityUsers.ForEach(s => estateResponse.SecurityUsers.Add(new SecurityUserResponse
                                                                                   {
                                                                                       EmailAddress = s.EmailAddress,
                                                                                       SecurityUserId = s.SecurityUserId
                                                                                   }));
            }

            return estateResponse;
        }

        /// <summary>
        /// Converts from.
        /// </summary>
        /// <param name="merchant">The merchant.</param>
        /// <returns></returns>
        public MerchantResponse ConvertFrom(Merchant merchant)
        {
            if (merchant == null)
            {
                return null;
            }

            MerchantResponse merchantResponse = new MerchantResponse
                                                {
                                                    EstateId = merchant.EstateId,
                                                    MerchantId = merchant.MerchantId,
                                                    MerchantName = merchant.MerchantName,
                                                    SettlementSchedule = this.ConvertFrom(merchant.SettlementSchedule),
                                                    MerchantReference = merchant.Reference,
                                                    NextStatementDate = merchant.NextStatementDate
                                                };

            if (merchant.Addresses != null && merchant.Addresses.Any())
            {
                merchantResponse.Addresses = new List<AddressResponse>();

                merchant.Addresses.ForEach(a => merchantResponse.Addresses.Add(new AddressResponse
                                                                               {
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

            if (merchant.Contacts != null && merchant.Contacts.Any())
            {
                merchantResponse.Contacts = new List<ContactResponse>();

                merchant.Contacts.ForEach(c => merchantResponse.Contacts.Add(new ContactResponse
                                                                             {
                                                                                 ContactId = c.ContactId,
                                                                                 ContactPhoneNumber = c.ContactPhoneNumber,
                                                                                 ContactEmailAddress = c.ContactEmailAddress,
                                                                                 ContactName = c.ContactName
                                                                             }));
            }

            if (merchant.Devices != null && merchant.Devices.Any())
            {
                merchantResponse.Devices = new Dictionary<Guid, String>();

                foreach ((Guid key, String value) in merchant.Devices)
                {
                    merchantResponse.Devices.Add(key, value);
                }
            }

            if (merchant.Operators != null && merchant.Operators.Any())
            {
                merchantResponse.Operators = new List<MerchantOperatorResponse>();

                merchant.Operators.ForEach(a => merchantResponse.Operators.Add(new MerchantOperatorResponse
                                                                               {
                                                                                   Name = a.Name,
                                                                                   MerchantNumber = a.MerchantNumber,
                                                                                   OperatorId = a.OperatorId,
                                                                                   TerminalNumber = a.TerminalNumber
                                                                               }));
            }

            return merchantResponse;
        }

        private SettlementSchedule ConvertFrom(Models.SettlementSchedule settlementSchedule)
        {
            return settlementSchedule switch
            {
                Models.SettlementSchedule.Weekly => DataTransferObjects.SettlementSchedule.Weekly,
                Models.SettlementSchedule.Monthly => DataTransferObjects.SettlementSchedule.Monthly,
                Models.SettlementSchedule.Immediate => DataTransferObjects.SettlementSchedule.Immediate,
                Models.SettlementSchedule.NotSet => DataTransferObjects.SettlementSchedule.NotSet,
            };
        }
        
        /// <summary>
        /// Converts from.
        /// </summary>
        /// <param name="merchants">The merchants.</param>
        /// <returns></returns>
        public List<MerchantResponse> ConvertFrom(List<Merchant> merchants)
        {
            List<MerchantResponse> result = new List<MerchantResponse>();

            merchants.ForEach(m => result.Add(this.ConvertFrom(m)));

            return result;
        }

        /// <summary>
        /// Converts from.
        /// </summary>
        /// <param name="transactionFees">The transaction fees.</param>
        /// <returns></returns>
        public List<ContractProductTransactionFee> ConvertFrom(List<TransactionFee> transactionFees)
        {
            List<ContractProductTransactionFee> result = new List<ContractProductTransactionFee>();
            transactionFees.ForEach(tf =>
                                    {
                                        ContractProductTransactionFee transactionFee = new ContractProductTransactionFee
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

        public SettlementFeeResponse ConvertFrom(SettlementFeeModel model)
        {
            if (model == null)
            {
                return null;
            }

            SettlementFeeResponse response = new SettlementFeeResponse
            {
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

        public List<SettlementFeeResponse> ConvertFrom(List<SettlementFeeModel> model)
        {
            if (model == null || model.Any() == false)
            {
                return new List<SettlementFeeResponse>();
            }

            List<SettlementFeeResponse> response = new List<SettlementFeeResponse>();

            model.ForEach(m => response.Add(this.ConvertFrom(m)));

            return response;
        }

        public SettlementResponse ConvertFrom(SettlementModel model)
        {
            if (model == null)
            {
                return null;
            }

            SettlementResponse response = new SettlementResponse
            {
                SettlementDate = model.SettlementDate,
                IsCompleted = model.IsCompleted,
                NumberOfFeesSettled = model.NumberOfFeesSettled,
                SettlementId = model.SettlementId,
                ValueOfFeesSettled = model.ValueOfFeesSettled,
            };

            model.SettlementFees.ForEach(f => response.SettlementFees.Add(this.ConvertFrom(f)));

            return response;
        }

        public List<SettlementResponse> ConvertFrom(List<SettlementModel> model)
        {
            if (model == null || model.Any() == false)
            {
                return new List<SettlementResponse>();
            }

            List<SettlementResponse> response = new List<SettlementResponse>();

            model.ForEach(m => response.Add(this.ConvertFrom(m)));

            return response;
        }

        #endregion
    }
}