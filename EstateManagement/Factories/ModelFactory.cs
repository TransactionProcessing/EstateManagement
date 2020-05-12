namespace EstateManagement.Factories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using DataTransferObjects.Responses;
    using Models.Estate;
    using Models.Merchant;

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

        #endregion
    }
}