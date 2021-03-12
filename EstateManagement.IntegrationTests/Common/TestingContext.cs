namespace EstateManagement.IntegrationTests.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using global::Shared.Logger;
    using Shouldly;
    using TechTalk.SpecFlow;

    public class TestingContext
    {
        public TestingContext()
        {
            this.Estates = new List<EstateDetails>();
            this.Clients = new List<ClientDetails>();
            this.IdentityResources = new List<String>();
        }
        
        public NlogLogger Logger { get; set; }

        public List<String> IdentityResources;

        public DockerHelper DockerHelper { get; set; }

        private List<ClientDetails> Clients;

        private List<EstateDetails> Estates;

        public String AccessToken { get; set; }

        public EstateDetails GetEstateDetails(TableRow tableRow)
        {
            String estateName = SpecflowTableHelper.GetStringRowValue(tableRow, "EstateName");

            EstateDetails estateDetails = this.GetEstateDetails(estateName);

            return estateDetails;
        }

        public List<Guid> GetAllEstateIds()
        {
            return this.Estates.Select(e => e.EstateId).ToList();
        }

        public EstateDetails GetEstateDetails(String estateName)
        {
            EstateDetails estateDetails = this.Estates.SingleOrDefault(e => e.EstateName == estateName);

            return estateDetails;
        }

        public void AddEstateDetails(Guid estateId, String estateName)
        {
            this.Estates.Add(EstateDetails.Create(estateId,estateName));
        }

        public void AddClientDetails(String clientId,
                                     String clientSecret,
                                     String grantType)
        {
            this.Clients.Add(ClientDetails.Create(clientId,clientSecret,grantType));
        }

        public ClientDetails GetClientDetails(String clientId)
        {
            ClientDetails clientDetails = this.Clients.SingleOrDefault(c => c.ClientId == clientId);

            clientDetails.ShouldNotBeNull();

            return clientDetails;
        }
    }

    public class ClientDetails
    {
        public String ClientId { get; private set; }
        public String ClientSecret { get; private set; }
        public String GrantType { get; private set; }

        private ClientDetails(String clientId,
                              String clientSecret,
                              String grantType)
        {
            this.ClientId = clientId;
            this.ClientSecret = clientSecret;
            this.GrantType = grantType;
        }

        public static ClientDetails Create(String clientId,
                                           String clientSecret,
                                           String grantType)
        {
            return new ClientDetails(clientId,clientSecret,grantType);
        }
    }
}