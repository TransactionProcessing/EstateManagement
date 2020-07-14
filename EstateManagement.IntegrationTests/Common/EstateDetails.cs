using System;
using System.Collections.Generic;
using System.Text;

namespace EstateManagement.IntegrationTests.Common
{
    using System.Linq;
    using System.Xml.Serialization;
    using DataTransferObjects;

    public class EstateDetails
    {
        private EstateDetails(Guid estateId, String estateName)
        {
            this.EstateId = estateId;
            this.EstateName = estateName;
            this.Merchants= new Dictionary<String, Guid>();
            this.Operators=new Dictionary<String, Guid>();
            this.MerchantUsers = new Dictionary<String, Dictionary<String, String>>();
            this.Contracts = new List<Contract>();
        }

        public String EstateUser { get; private set; }
        public String EstatePassword { get; private set; }

        public String AccessToken { get; private set; }

        public static EstateDetails Create(Guid estateId,
                                           String estateName)
        {
            return new EstateDetails(estateId,estateName);
        }

        public void AddOperator(Guid operatorId,
                                String operatorName)
        {
            this.Operators.Add(operatorName,operatorId);
        }

        public void AddContract(Guid contractId,
                                String contractName,
                                Guid operatorId)
        {
            this.Contracts.Add(new Contract
                               {
                                   ContractId = contractId,
                                   Description = contractName,
                                   OperatorId = operatorId,
                               });
        }

        public void AddMerchant(Guid merchantId,
                                String merchantName)
        {
            this.Merchants.Add(merchantName,merchantId);
        }

        public Contract GetContract(String contractName)
        {
            return this.Contracts.Single(c => c.Description == contractName);
        }
        public Contract GetContract(Guid contractId)
        {
            return this.Contracts.Single(c => c.ContractId == contractId);
        }

        public Guid GetMerchantId(String merchantName)
        {
            return this.Merchants.Single(m => m.Key == merchantName).Value;
        }

        public Guid GetOperatorId(String operatorName)
        {
            return this.Operators.Single(o => o.Key == operatorName).Value;
        }

        public void SetEstateUser(String userName,
                                  String password)
        {
            this.EstateUser = userName;
            this.EstatePassword = password;
        }
        
        public void AddMerchantUser(String merchantName,
                                    String userName,
                                    String password)
        {
            if (this.MerchantUsers.ContainsKey(merchantName))
            {
                Dictionary<String, String> merchantUsersList = this.MerchantUsers[merchantName];
                if (merchantUsersList.ContainsKey(userName) == false)
                {
                    merchantUsersList.Add(userName,password);
                }
            }
            else
            {
                Dictionary<String,String> merchantUsersList = new Dictionary<String, String>();
                merchantUsersList.Add(userName,password);
                this.MerchantUsers.Add(merchantName,merchantUsersList);
            }
        }

        public void AddMerchantUserToken(String merchantName,
                                    String userName,
                                    String token)
        {
            if (this.MerchantUsersTokens.ContainsKey(merchantName))
            {
                Dictionary<String, String> merchantUsersList = this.MerchantUsersTokens[merchantName];
                if (merchantUsersList.ContainsKey(userName) == false)
                {
                    merchantUsersList.Add(userName, token);
                }
            }
            else
            {
                Dictionary<String, String> merchantUsersList = new Dictionary<String, String>();
                merchantUsersList.Add(userName, token);
                this.MerchantUsersTokens.Add(merchantName, merchantUsersList);
            }
        }

        public void SetEstateUserToken(String accessToken)
        {
            this.AccessToken = accessToken;
        }
        
        public Guid EstateId { get; private set; }
        public String EstateName { get; private set; }

        private Dictionary<String, Guid> Operators;

        private Dictionary<String, Guid> Merchants;
        
        private Dictionary<String, Dictionary<String,String>> MerchantUsers;
        private Dictionary<String, Dictionary<String, String>> MerchantUsersTokens;

        private List<Contract> Contracts;
    }

    public class Contract
    {
        public Guid ContractId { get; set; }

        public Guid OperatorId { get; set; }

        public String Description { get; set; }

        public List<Product> Products { get; set; }

        public void AddProduct(Guid productId,
                               String name,
                               String displayText,
                               Decimal? value = null)
        {
            Product product = new Product
                              {
                                  ProductId = productId,
                                  DisplayText = displayText,
                                  Name = name,
                                  Value = value
                              };

            if (this.Products == null)
            {
                this.Products = new List<Product>();
            }
            this.Products.Add(product);
        }

        public Product GetProduct(Guid productId)
        {
            return this.Products.SingleOrDefault(p => p.ProductId == productId);
        }

        public Product GetProduct(String name)
        {
            return this.Products.SingleOrDefault(p => p.Name == name);
        }
    }

    public class Product
    {
        public Guid ProductId { get; set; }

        public String Name { get; set; }
        public String DisplayText { get; set; }

        public Decimal? Value { get; set; }

        public List<TransactionFee> TransactionFees { get; set; }

        public void AddTransactionFee(Guid transactionFeeId,
                                      CalculationType calculationType,
                                      String description,
                                      Decimal value)
        {
            TransactionFee transactionFee = new TransactionFee
                              {
                                  TransactionFeeId = transactionFeeId,
                                  CalculationType = calculationType,
                                  Description = description,
                                  Value = value
                              };

            if (this.TransactionFees == null)
            {
                this.TransactionFees = new List<TransactionFee>();
            }
            this.TransactionFees.Add(transactionFee);
        }

        public TransactionFee GetTransactionFee(Guid transactionFeeId)
        {
            return this.TransactionFees.SingleOrDefault(t => t.TransactionFeeId== transactionFeeId);
        }

        public TransactionFee GetTransactionFee(String description)
        {
            return this.TransactionFees.SingleOrDefault(t => t.Description == description);
        }
    }

    public class TransactionFee
    {
        public Guid TransactionFeeId { get; set; }

        public CalculationType CalculationType { get; set; }

        public String Description { get; set; }

        public Decimal  Value { get; set; }
    }
}
