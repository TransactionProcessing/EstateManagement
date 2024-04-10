namespace EstateManagement.BusinessLogic.Services
{
    using System;
    using System.IO.Abstractions;
    using System.Reflection;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Models.MerchantStatement;

    public interface IStatementBuilder
    {
        Task<String> GetStatementHtml(StatementHeader statementHeader,
                                      CancellationToken cancellationToken);
    }

    public class StatementBuilder : IStatementBuilder
    {
        #region Fields

        /// <summary>
        /// The file system
        /// </summary>
        private readonly IFileSystem FileSystem;

        #endregion

        #region Constructors

        public StatementBuilder(IFileSystem fileSystem)
        {
            this.FileSystem = fileSystem;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the email receipt message.
        /// </summary>
        /// <param name="statementHeader">The statement header.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task<String> GetStatementHtml(StatementHeader statementHeader,
                                                   CancellationToken cancellationToken)
        {
            IDirectoryInfo path = this.FileSystem.Directory.GetParent(Assembly.GetExecutingAssembly().Location);
            

            String mainHtml = await this.FileSystem.File.ReadAllTextAsync($"{path}/Templates/Email/statement.html", cancellationToken);

            // Statement header class first
            PropertyInfo[] statementHeaderProperties = statementHeader.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            // Do the replaces for the transaction
            foreach (PropertyInfo propertyInfo in statementHeaderProperties)
            {
                mainHtml = mainHtml.Replace($"[{propertyInfo.Name}]", propertyInfo.GetValue(statementHeader)?.ToString());
            }

            StringBuilder lines = new StringBuilder();

            foreach (StatementLine statementLine in statementHeader.StatementLines)
            {
                String lineHtml = await this.FileSystem.File.ReadAllTextAsync($"{path}/Templates/Email/statementline.html", cancellationToken);

                PropertyInfo[] statementLineProperties = statementLine.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
                foreach (PropertyInfo propertyInfo in statementLineProperties)
                {
                    lineHtml = lineHtml.Replace($"[{propertyInfo.Name}]", propertyInfo.GetValue(statementLine)?.ToString());
                }

                lines.Append(lineHtml);
            }

            mainHtml = mainHtml.Replace("[StatementLinesData]", lines.ToString());

            return mainHtml;
        }

        #endregion
    }
}