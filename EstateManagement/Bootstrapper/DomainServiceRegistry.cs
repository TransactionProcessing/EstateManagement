namespace EstateManagement.Bootstrapper
{
    using BusinessLogic.Services;
    using Lamar;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Lamar.ServiceRegistry" />
    public class DomainServiceRegistry : ServiceRegistry
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DomainServiceRegistry"/> class.
        /// </summary>
        public DomainServiceRegistry()
        {
            this.AddSingleton<IEstateDomainService, EstateDomainService>();
            this.AddSingleton<IMerchantDomainService, MerchantDomainService>();
            this.AddSingleton<IContractDomainService, ContractDomainService>();
            this.AddSingleton<IMerchantStatementDomainService, MerchantStatementDomainService>();
        }

        #endregion
    }
}