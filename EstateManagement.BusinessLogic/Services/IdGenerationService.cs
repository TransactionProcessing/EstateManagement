﻿using System;
using System.Linq;
using System.Text;

namespace EstateManagement.BusinessLogic.Services
{
    using System.Security.Cryptography;
    using Newtonsoft.Json;
    using Shared.Serialisation;

    public class IdGenerationService
    {
        internal delegate Guid GenerateUniqueIdFromObject(Object payload);

        internal delegate Guid GenerateUniqueIdFromString(String payload);


        private static readonly JsonSerialiser JsonSerialiser = new(() => new JsonSerializerSettings
                                                                          {
                                                                              Formatting = Formatting.None
                                                                          });

        private static readonly GenerateUniqueIdFromObject GenerateUniqueId =
            data => IdGenerationService.GenerateGuidFromString(IdGenerationService.JsonSerialiser.Serialise(data));

        private static readonly GenerateUniqueIdFromString GenerateGuidFromString = uniqueKey => {
                                                                                        using SHA256 sha256Hash = SHA256.Create();
                                                                                        //Generate hash from the key
                                                                                        Byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(uniqueKey));

                                                                                        Byte[] j = bytes.Skip(Math.Max(0, bytes.Count() - 16)).ToArray(); //Take last 16

                                                                                        //Create our Guid.
                                                                                        return new Guid(j);
                                                                                    };

        public static Guid GenerateEventId(Object o) => IdGenerationService.GenerateUniqueId(o);

        public static Guid GenerateAddressId((String addressLine1,
                                             String town,
                                             String region,
                                             String postalCode,
                                             String country) address) => IdGenerationService.GenerateUniqueId(address);
    }
}
