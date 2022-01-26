namespace EstateManagement.Bootstrapper
{
    using System.Diagnostics.CodeAnalysis;
    using System.IO.Abstractions;
    using BusinessLogic.Manger;
    using BusinessLogic.Services;
    using Lamar;
    using Microsoft.Extensions.DependencyInjection;
    using Models.Factories;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Lamar.ServiceRegistry" />
    [ExcludeFromCodeCoverage]
    public class MiscRegistry : ServiceRegistry
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MiscRegistry"/> class.
        /// </summary>
        public MiscRegistry()
        {
            this.AddSingleton<IEstateManagementManager, EstateManagementManager>();
            this.AddSingleton<IModelFactory, ModelFactory>();
            this.AddSingleton<Factories.IModelFactory, Factories.ModelFactory>();
            this.AddSingleton<IStatementBuilder, StatementBuilder>();
            this.AddSingleton<IFileSystem, FileSystem>();
            this.AddSingleton<IPDFGenerator, PDFGenerator>();
        }

        #endregion
    }
}