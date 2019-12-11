namespace EstateManagement.IntegrationTests.Common
{
    using System;
    using System.Collections.Generic;
    using global::Shared.Logger;

    public class TestingContext
    {
        public TestingContext()
        {
            this.Estates = new Dictionary<String, Guid>();
            this.Merchants = new Dictionary<String, Guid>();
            this.Operators = new Dictionary<String, Guid>();
            this.EstateMerchants = new Dictionary<Guid, List<Guid>>();
        }

        public NlogLogger Logger { get; set; }

        public DockerHelper DockerHelper { get; set; }

        public Dictionary<String, Guid> Estates { get; set; }
        public Dictionary<String, Guid> Merchants { get; set; }

        public Dictionary<String, Guid> Operators { get; set; }

        public Dictionary<Guid, List<Guid>> EstateMerchants { get; set; }
    }
}