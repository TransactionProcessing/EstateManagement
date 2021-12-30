using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstateManagement.BusinessLogic.Tests.Services
{
    using System.IO;
    using System.IO.Abstractions;
    using System.IO.Abstractions.TestingHelpers;
    using System.Reflection;
    using System.Threading;
    using BusinessLogic.Services;
    using Models.MerchantStatement;
    using Moq;
    using Shouldly;
    using Xunit;

    public class StatementBuilderTests
    {
        [Fact]
        public async Task StatementBuilder_GetStatementHtml_HtmlReturned()
        {
            var path = Directory.GetParent(Assembly.GetExecutingAssembly().Location);

            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
                                                {
                                                    { $"{path}/Templates/Email/statement.html", new MockFileData("Statement Id: [StatementId] [StatementLinesData]") },
                                                    { $"{path}/Templates/Email/statementline.html", new MockFileData("Statement Line: [StatementLineDescription]") }
                                                });

            IStatementBuilder statementBuilder = new StatementBuilder(fileSystem);

            StatementHeader header = new StatementHeader();
            header.StatementId = "111";
            header.StatementLines = new List<StatementLine>();
            header.StatementLines.Add(new StatementLine
                                      {
                                          StatementLineAmount = 100,
                                          StatementLineAmountDisplay = "100 KES",
                                          StatementLineDate = "01-01-2021",
                                          StatementLineDescription = "Test Statement Line",
                                          StatementLineNumber = "1"
                                      });

            String html = await statementBuilder.GetStatementHtml(header, CancellationToken.None);

            html.ShouldNotBeNullOrEmpty();
            html.ShouldContain($"Statement Id: {header.StatementId}");
            html.ShouldContain($"Statement Line: {header.StatementLines.First().StatementLineDescription}");
        }
    }
}
