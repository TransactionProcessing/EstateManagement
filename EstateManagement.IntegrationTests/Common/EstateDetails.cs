using System;
using System.Collections.Generic;
using System.Text;

namespace EstateManagement.IntegrationTests.Common
{
    using System.Linq;
    using System.Xml.Serialization;

    public class EstateDetails
    {
        private EstateDetails(Guid estateId, String estateName)
        {
            this.EstateId = estateId;
            this.EstateName = estateName;
            this.Merchants= new Dictionary<String, Guid>();
            this.Operators=new Dictionary<String, Guid>();
            this.MerchantUsers = new Dictionary<String, Dictionary<String, String>>();
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

        public void AddMerchant(Guid merchantId,
                                String merchantName)
        {
            this.Merchants.Add(merchantName,merchantId);
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
    }
}
