using System;
using Xunit;

namespace EstateManagement.MerchantStatementAggregate.Tests
{
    using Models.MerchantStatement;
    using Shouldly;
    using Testing;

    public class MerchantStatementAggregateTests
    {
        [Fact]
        public void MerchantStatementAggregate_CanBeCreated_IsCreated()
        {
            MerchantStatementAggregate merchantStatementAggregate = MerchantStatementAggregate.Create(TestData.MerchantStatementId);

            merchantStatementAggregate.ShouldNotBeNull();
            merchantStatementAggregate.AggregateId.ShouldBe(TestData.MerchantStatementId);
        }

        [Fact]
        public void MerchantStatementAggregate_CreateStatement_StatementIsCreated()
        {
            MerchantStatementAggregate merchantStatementAggregate = MerchantStatementAggregate.Create(TestData.MerchantStatementId);
            merchantStatementAggregate.CreateStatement(TestData.EstateId, TestData.MerchantId, TestData.StatementCreateDate);

            MerchantStatement merchantStatement = merchantStatementAggregate.GetStatement();
            merchantStatement.ShouldNotBeNull();
            merchantStatement.IsCreated.ShouldBeTrue();
        }

        [Fact]
        public void MerchantStatementAggregate_CreateStatement_AlreadyCreated_NoErrorThrown()
        {
            MerchantStatementAggregate merchantStatementAggregate = MerchantStatementAggregate.Create(TestData.MerchantStatementId);
            merchantStatementAggregate.CreateStatement(TestData.EstateId, TestData.MerchantId, TestData.StatementCreateDate);

            Should.NotThrow(() =>
                            {
                                merchantStatementAggregate.CreateStatement(TestData.EstateId, TestData.MerchantId, TestData.StatementCreateDate);
                            });
        }

        [Fact]
        public void MerchantStatementAggregate_CreateStatement_InvalidEstateId_ErrorThrown()
        {
            MerchantStatementAggregate merchantStatementAggregate = MerchantStatementAggregate.Create(TestData.MerchantStatementId);
            
            Should.Throw<ArgumentNullException>(() =>
                                                {
                                                    merchantStatementAggregate.CreateStatement(Guid.Empty, TestData.MerchantId, TestData.StatementCreateDate);
                                                });
        }

        [Fact]
        public void MerchantStatementAggregate_CreateStatement_InvalidMerchantId_ErrorThrown()
        {
            MerchantStatementAggregate merchantStatementAggregate = MerchantStatementAggregate.Create(TestData.MerchantStatementId);

            Should.Throw<ArgumentNullException>(() =>
                                                    {
                                                        merchantStatementAggregate.CreateStatement(TestData.EstateId, Guid.Empty, TestData.StatementCreateDate);
                                                    });
        }

        [Fact]
        public void MerchantStatementAggregate_AddTransactionToStatement_TransactionAddedToStatement()
        {
            MerchantStatementAggregate merchantStatementAggregate = MerchantStatementAggregate.Create(TestData.MerchantStatementId);
            merchantStatementAggregate.CreateStatement(TestData.EstateId, TestData.MerchantId, TestData.StatementCreateDate);
            merchantStatementAggregate.AddTransactionToStatement(TestData.Transaction1);

            MerchantStatement merchantStatement = merchantStatementAggregate.GetStatement(true);
            var statementLines = merchantStatement.GetStatementLines();
            statementLines.ShouldNotBeNull();
            statementLines.ShouldNotBeEmpty();
            statementLines.Count.ShouldBe(1);
        }

        [Fact]
        public void MerchantStatementAggregate_AddTransactionToStatement_TransactionAlreadyAdded_ErrorThrown()
        {
            MerchantStatementAggregate merchantStatementAggregate = MerchantStatementAggregate.Create(TestData.MerchantStatementId);
            merchantStatementAggregate.CreateStatement(TestData.EstateId, TestData.MerchantId, TestData.StatementCreateDate);
            merchantStatementAggregate.AddTransactionToStatement(TestData.Transaction1);

            Should.NotThrow(() =>
                                                    {
                                                        merchantStatementAggregate.AddTransactionToStatement(TestData.Transaction1);
                                                    });

            var statement = merchantStatementAggregate.GetStatement(true);
            var statementLines = statement.GetStatementLines();
            statementLines.Count.ShouldBe(1);
        }

        [Fact]
        public void MerchantStatementAggregate_AddTransactionToStatement_StatementNotCreated_ErrorThrown()
        {
            MerchantStatementAggregate merchantStatementAggregate = MerchantStatementAggregate.Create(TestData.MerchantStatementId);
            
            Should.Throw<InvalidOperationException>(() =>
                                                    {
                                                        merchantStatementAggregate.AddTransactionToStatement(TestData.Transaction1);
                                                    });
        }

        [Fact]
        public void MerchantStatementAggregate_AddTransactionToStatement_StatementAlreadyGenerated_ErrorThrown()
        {
            MerchantStatementAggregate merchantStatementAggregate = MerchantStatementAggregate.Create(TestData.MerchantStatementId);
            merchantStatementAggregate.CreateStatement(TestData.EstateId, TestData.MerchantId, TestData.StatementCreateDate);
            merchantStatementAggregate.AddTransactionToStatement(TestData.Transaction1);
            merchantStatementAggregate.AddSettledFeeToStatement(TestData.SettledFee1);
            merchantStatementAggregate.GenerateStatement(TestData.StatementGeneratedDate);

            Should.Throw<InvalidOperationException>(() =>
                                                    {
                                                        merchantStatementAggregate.AddTransactionToStatement(TestData.Transaction2); // Try add a second transaction
                                                    });
        }

        [Fact]
        public void MerchantStatementAggregate_AddSettledFeeToStatement_FeeAddedToStatement()
        {
            MerchantStatementAggregate merchantStatementAggregate = MerchantStatementAggregate.Create(TestData.MerchantStatementId);
            merchantStatementAggregate.CreateStatement(TestData.EstateId, TestData.MerchantId, TestData.StatementCreateDate);
            merchantStatementAggregate.AddSettledFeeToStatement(TestData.SettledFee1);

            MerchantStatement merchantStatement = merchantStatementAggregate.GetStatement(true);
            var statementLines = merchantStatement.GetStatementLines();
            statementLines.ShouldNotBeNull();
            statementLines.ShouldNotBeEmpty();
            statementLines.Count.ShouldBe(1);
        }

        [Fact]
        public void MerchantStatementAggregate_AddSettledFeeToStatement_SettledFeeAlreadyAdded_ErrorThrown()
        {
            MerchantStatementAggregate merchantStatementAggregate = MerchantStatementAggregate.Create(TestData.MerchantStatementId);
            merchantStatementAggregate.CreateStatement(TestData.EstateId, TestData.MerchantId, TestData.StatementCreateDate);
            merchantStatementAggregate.AddSettledFeeToStatement(TestData.SettledFee1);

            Should.NotThrow(() =>
            {
                merchantStatementAggregate.AddSettledFeeToStatement(TestData.SettledFee1);
            });

            var statement = merchantStatementAggregate.GetStatement(true);
            var statementLines = statement.GetStatementLines();
            statementLines.Count.ShouldBe(1);
        }
        
        [Fact]
        public void MerchantStatementAggregate_AddSettledFeeToStatement_StatementNotCreated_ErrorThrown()
        {
            MerchantStatementAggregate merchantStatementAggregate = MerchantStatementAggregate.Create(TestData.MerchantStatementId);

            Should.Throw<InvalidOperationException>(() =>
                                                    {
                                                        merchantStatementAggregate.AddSettledFeeToStatement(TestData.SettledFee1);
                                                    });
        }

        [Fact]
        public void MerchantStatementAggregate_AddSettledFeeToStatement_StatementAlreadyGenerated_ErrorThrown()
        {
            MerchantStatementAggregate merchantStatementAggregate = MerchantStatementAggregate.Create(TestData.MerchantStatementId);
            merchantStatementAggregate.CreateStatement(TestData.EstateId, TestData.MerchantId, TestData.StatementCreateDate);
            merchantStatementAggregate.AddTransactionToStatement(TestData.Transaction1);
            merchantStatementAggregate.AddTransactionToStatement(TestData.Transaction2);
            merchantStatementAggregate.AddSettledFeeToStatement(TestData.SettledFee1);
            merchantStatementAggregate.GenerateStatement(TestData.StatementGeneratedDate);

            Should.Throw<InvalidOperationException>(() =>
            {
                merchantStatementAggregate.AddSettledFeeToStatement(TestData.SettledFee2); ; // Try add a second fee
            });
        }

        [Fact]
        public void MerchantStatementAggregate_GenerateStatement_StatementIsGenerated()
        {
            MerchantStatementAggregate merchantStatementAggregate = MerchantStatementAggregate.Create(TestData.MerchantStatementId);
            merchantStatementAggregate.CreateStatement(TestData.EstateId, TestData.MerchantId, TestData.StatementCreateDate);
            merchantStatementAggregate.AddTransactionToStatement(TestData.Transaction1);
            merchantStatementAggregate.AddSettledFeeToStatement(TestData.SettledFee1);
            merchantStatementAggregate.GenerateStatement(TestData.StatementGeneratedDate);

            var merchantStatement = merchantStatementAggregate.GetStatement();
            merchantStatement.IsGenerated.ShouldBeTrue();
        }

        [Fact]
        public void MerchantStatementAggregate_GenerateStatement_StatementNotCreated_ErrorThrown()
        {
            MerchantStatementAggregate merchantStatementAggregate = MerchantStatementAggregate.Create(TestData.MerchantStatementId);

            Should.Throw<InvalidOperationException>(() =>
                                                    {
                                                        merchantStatementAggregate.GenerateStatement(TestData.StatementGeneratedDate);
                                                    });
        }

        [Fact]
        public void MerchantStatementAggregate_GenerateStatement_StatementAlreadyGenerated_ErrorThrown()
        {
            MerchantStatementAggregate merchantStatementAggregate = MerchantStatementAggregate.Create(TestData.MerchantStatementId);
            merchantStatementAggregate.CreateStatement(TestData.EstateId, TestData.MerchantId, TestData.StatementCreateDate);
            merchantStatementAggregate.AddTransactionToStatement(TestData.Transaction1);
            merchantStatementAggregate.AddSettledFeeToStatement(TestData.SettledFee1);
            merchantStatementAggregate.GenerateStatement(TestData.StatementGeneratedDate);

            Should.Throw<InvalidOperationException>(() =>
                                                    {
                                                        merchantStatementAggregate.GenerateStatement(TestData.StatementGeneratedDate);
                                                    });
        }

        [Fact]
        public void MerchantStatementAggregate_GenerateStatement_StatementHasNoTransaction_ErrorThrown()
        {
            MerchantStatementAggregate merchantStatementAggregate = MerchantStatementAggregate.Create(TestData.MerchantStatementId);
            merchantStatementAggregate.CreateStatement(TestData.EstateId, TestData.MerchantId, TestData.StatementCreateDate);

            Should.Throw<InvalidOperationException>(() =>
                                                    {
                                                        merchantStatementAggregate.GenerateStatement(TestData.StatementGeneratedDate);
                                                    });
        }

        [Fact]
        public void MerchantStatementAggregate_GenerateStatement_StatementHasNoSettledFees_ErrorThrown()
        {
            MerchantStatementAggregate merchantStatementAggregate = MerchantStatementAggregate.Create(TestData.MerchantStatementId);
            merchantStatementAggregate.CreateStatement(TestData.EstateId, TestData.MerchantId, TestData.StatementCreateDate);
            
            Should.Throw<InvalidOperationException>(() =>
                                                    {
                                                        merchantStatementAggregate.GenerateStatement(TestData.StatementGeneratedDate);
                                                    });
        }

        [Fact]
        public void MerchantStatementAggregate_EmailStatement_StatementHasBeenEmailed()
        {
            MerchantStatementAggregate merchantStatementAggregate = MerchantStatementAggregate.Create(TestData.MerchantStatementId);
            merchantStatementAggregate.CreateStatement(TestData.EstateId, TestData.MerchantId, TestData.StatementCreateDate);
            merchantStatementAggregate.AddTransactionToStatement(TestData.Transaction1);
            merchantStatementAggregate.AddSettledFeeToStatement(TestData.SettledFee1);
            merchantStatementAggregate.GenerateStatement(TestData.StatementGeneratedDate);

            merchantStatementAggregate.EmailStatement(TestData.StatementEmailedDate, TestData.MessageId);

            MerchantStatement statement = merchantStatementAggregate.GetStatement(false);
            statement.HasBeenEmailed.ShouldBeTrue();
        }

        [Fact]
        public void MerchantStatementAggregate_EmailStatement_NotCreated_ErrorThrown()
        {
            MerchantStatementAggregate merchantStatementAggregate = MerchantStatementAggregate.Create(TestData.MerchantStatementId);
            //merchantStatementAggregate.CreateStatement(TestData.EstateId, TestData.MerchantId, TestData.StatementCreateDate);
            //merchantStatementAggregate.AddTransactionToStatement(TestData.Transaction1);
            //merchantStatementAggregate.AddSettledFeeToStatement(TestData.SettledFee1);
            //merchantStatementAggregate.GenerateStatement(TestData.StatementGeneratedDate);

            Should.Throw<InvalidOperationException>(() =>
                                                    {
                                                        merchantStatementAggregate.EmailStatement(TestData.StatementEmailedDate,TestData.MessageId);
                                                    });
        }

        [Fact]
        public void MerchantStatementAggregate_EmailStatement_NotGenerated_ErrorThrown()
        {
            MerchantStatementAggregate merchantStatementAggregate = MerchantStatementAggregate.Create(TestData.MerchantStatementId);
            merchantStatementAggregate.CreateStatement(TestData.EstateId, TestData.MerchantId, TestData.StatementCreateDate);
            merchantStatementAggregate.AddTransactionToStatement(TestData.Transaction1);
            merchantStatementAggregate.AddSettledFeeToStatement(TestData.SettledFee1);

            Should.Throw<InvalidOperationException>(() =>
                                                    {
                                                        merchantStatementAggregate.EmailStatement(TestData.StatementEmailedDate, TestData.MessageId);
                                                    });
        }
    }
}
