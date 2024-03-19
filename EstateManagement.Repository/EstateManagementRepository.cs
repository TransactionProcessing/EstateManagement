namespace EstateManagement.Repository{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Database.Contexts;
    using Database.Entities;
    using Microsoft.EntityFrameworkCore;
    using Models.Contract;
    using Models.Factories;
    using Shared.Exceptions;
    using Contract = Database.Entities.Contract;
    using TransactionFeeModel = Models.Contract.TransactionFee;
    using EstateModel = Models.Estate.Estate;
    using MerchantModel = Models.Merchant.Merchant;
    using ContractModel = Models.Contract.Contract;
    using StatementHeader = Models.MerchantStatement.StatementHeader;
    using StatementLine = Models.MerchantStatement.StatementLine;
    using EstateManagement.Models.Estate;
    using Models.File;
    using Estate = Database.Entities.Estate;
    using File = Models.File.File;
    using Transaction = Models.File.Transaction;

    [ExcludeFromCodeCoverage]
    public class EstateManagementRepository : IEstateManagementRepository{
        #region Fields

        private readonly Shared.EntityFramework.IDbContextFactory<EstateManagementGenericContext> ContextFactory;

        private readonly IModelFactory ModelFactory;

        #endregion

        #region Constructors

        public EstateManagementRepository(Shared.EntityFramework.IDbContextFactory<EstateManagementGenericContext> contextFactory,
                                          IModelFactory modelFactory){
            this.ContextFactory = contextFactory;
            this.ModelFactory = modelFactory;
        }

        #endregion

        #region Methods

        public async Task<ContractModel> GetContract(Guid estateId,
                                                     Guid contractId,
                                                     Boolean includeProducts,
                                                     Boolean includeProductsWithFees,
                                                     CancellationToken cancellationToken){
            EstateManagementGenericContext context = await this.ContextFactory.GetContext(estateId, EstateManagementRepository.ConnectionStringIdentifier, cancellationToken);

            Contract contract = await context.Contracts.SingleOrDefaultAsync(c => c.ContractId == contractId, cancellationToken);

            if (contract == null){
                throw new NotFoundException($"No contract found in read model with Id [{contractId}]");
            }
            
            List<ContractProduct> contractProducts = null;
            List<ContractProductTransactionFee> contractProductFees = null;

            if (includeProducts || includeProductsWithFees){
                contractProducts = await context.ContractProducts.Where(cp => cp.ContractReportingId == contract.ContractReportingId).ToListAsync(cancellationToken);
            }

            if (includeProductsWithFees){
                contractProductFees = await (from cptf in context.ContractProductTransactionFees
                                             join cp in context.ContractProducts on cptf.ContractProductReportingId equals cp.ContractProductReportingId
                                             where cp.ContractReportingId == contract.ContractReportingId
                                             select cptf).ToListAsync(cancellationToken);
            }

            return this.ModelFactory.ConvertFrom(estateId, contract, contractProducts, contractProductFees);
        }

        public async Task<List<ContractModel>> GetContracts(Guid estateId,
                                                            CancellationToken cancellationToken){
            EstateManagementGenericContext context = await this.ContextFactory.GetContext(estateId, EstateManagementRepository.ConnectionStringIdentifier, cancellationToken);

            var query = await (from c in context.Contracts
                               join cp in context.ContractProducts on c.ContractReportingId equals cp.ContractReportingId into cps
                               from contractprouduct in cps.DefaultIfEmpty()
                               join eo in context.EstateOperators on c.OperatorId equals eo.OperatorId
                               join e in context.Estates on eo.EstateReportingId equals e.EstateReportingId
                               select new{
                                             Estate = e,
                                             Contract = c,
                                             Product = contractprouduct,
                                             Operator = eo
                                         }).ToListAsync(cancellationToken);

            List<ContractModel> contracts = new List<ContractModel>();

            foreach (var contractData in query){
                // attempt to find the contract
                ContractModel contract = contracts.SingleOrDefault(c => c.ContractId == contractData.Contract.ContractId);

                if (contract == null){
                    // create the contract
                    contract = new ContractModel{
                                                    EstateReportingId = contractData.Estate.EstateReportingId,
                                                    EstateId = contractData.Estate.EstateId,
                                                    OperatorId = contractData.Contract.OperatorId,
                                                    OperatorName = contractData.Operator.Name,
                                                    Products = new List<Product>(),
                                                    Description = contractData.Contract.Description,
                                                    IsCreated = true,
                                                    ContractId = contractData.Contract.ContractId,
                                                    ContractReportingId = contractData.Contract.ContractReportingId
                                                };

                    contracts.Add(contract);
                }

                // Now add the product if not already added
                Boolean productFound = contract.Products.Any(p => p.ProductId == contractData.Product.ProductId);

                if (productFound == false){
                    if (contractData.Product != null){
                        // Not already there so need to add it
                        contract.Products.Add(new Product{
                                                             ProductId = contractData.Product.ProductId,
                                                             TransactionFees = null,
                                                             Value = contractData.Product.Value,
                                                             Name = contractData.Product.ProductName,
                                                             DisplayText = contractData.Product.DisplayText,
                                                             ContractProductReportingId = contractData.Product.ContractProductReportingId
                                                         });
                    }
                }
            }

            return contracts;
        }

        public async Task<EstateModel> GetEstate(Guid estateId,
                                                 CancellationToken cancellationToken){
            EstateManagementGenericContext context = await this.ContextFactory.GetContext(estateId, EstateManagementRepository.ConnectionStringIdentifier, cancellationToken);

            Estate estate = await context.Estates.SingleOrDefaultAsync(e => e.EstateId == estateId, cancellationToken);

            if (estate == null){
                throw new NotFoundException($"No estate found in read model with Id [{estateId}]");
            }

            List<EstateOperator> estateOperators = await context.EstateOperators.Where(eo => eo.EstateReportingId == estate.EstateReportingId).ToListAsync(cancellationToken);
            List<EstateSecurityUser> estateSecurityUsers = await context.EstateSecurityUsers.Where(esu => esu.EstateReportingId == estate.EstateReportingId).ToListAsync(cancellationToken);

            return this.ModelFactory.ConvertFrom(estate, estateOperators, estateSecurityUsers);
        }

        public async Task<List<ContractModel>> GetMerchantContracts(Guid estateId,
                                                                    Guid merchantId,
                                                                    CancellationToken cancellationToken){
            EstateManagementGenericContext context = await this.ContextFactory.GetContext(estateId, EstateManagementRepository.ConnectionStringIdentifier, cancellationToken);

            var x = await (from c in context.Contracts
                           join cp in context.ContractProducts on c.ContractReportingId equals cp.ContractReportingId
                           join eo in context.EstateOperators on c.OperatorId equals eo.OperatorId
                           join m in context.Merchants on c.EstateReportingId equals m.EstateReportingId
                           join e in context.Estates on c.EstateReportingId equals e.EstateReportingId
                           join mc in context.MerchantContracts on new {c.ContractReportingId, m.MerchantReportingId} equals new {mc.ContractReportingId, mc.MerchantReportingId}
                           where m.MerchantId == merchantId && e.EstateId == estateId
                           select new{
                                         Contract = c,
                                         Product = cp,
                                         Operator = eo
                                     }).ToListAsync(cancellationToken);

            List<ContractModel> contracts = new List<ContractModel>();

            foreach (var test in x){
                // attempt to find the contract
                ContractModel contract = contracts.SingleOrDefault(c => c.ContractId == test.Contract.ContractId);

                if (contract == null){
                    // create the contract
                    contract = new ContractModel{
                                                    OperatorId = test.Contract.OperatorId,
                                                    OperatorName = test.Operator.Name,
                                                    Products = new List<Product>(),
                                                    Description = test.Contract.Description,
                                                    IsCreated = true,
                                                    ContractId = test.Contract.ContractId,
                                                    ContractReportingId = test.Contract.ContractReportingId,
                                                    EstateReportingId = test.Operator.EstateReportingId,
                                                    EstateId = estateId
                                                };

                    contracts.Add(contract);
                }

                // Now add the product if not already added
                Boolean productFound = contract.Products.Any(p => p.ProductId == test.Product.ProductId);

                if (productFound == false){
                    // Not already there so need to add it
                    contract.Products.Add(new Product{
                                                         ProductId = test.Product.ProductId,
                                                         ContractProductReportingId = test.Product.ContractProductReportingId,
                                                         TransactionFees = null,
                                                         Value = test.Product.Value,
                                                         Name = test.Product.ProductName,
                                                         DisplayText = test.Product.DisplayText
                                                     });
                }
            }

            return contracts;
        }

        public async Task<MerchantModel> GetMerchant(Guid estateId, Guid merchantId, CancellationToken cancellationToken){
            EstateManagementGenericContext context = await this.ContextFactory.GetContext(estateId, EstateManagementRepository.ConnectionStringIdentifier, cancellationToken);

            Merchant merchant = await (from m in context.Merchants where m.MerchantId == merchantId select m).SingleOrDefaultAsync(cancellationToken);

            if (merchant == null){
                throw new NotFoundException($"Merchant with Id {merchantId} not found for Estate {estateId}");
            }

                List<MerchantAddress> merchantAddresses = await (from a in context.MerchantAddresses where a.MerchantReportingId == merchant.MerchantReportingId select a).ToListAsync(cancellationToken);
            List<MerchantContact> merchantContacts = await (from c in context.MerchantContacts where c.MerchantReportingId == merchant.MerchantReportingId select c).ToListAsync(cancellationToken);
            List<MerchantOperator> merchantOperators = await (from o in context.MerchantOperators where o.MerchantReportingId == merchant.MerchantReportingId select o).ToListAsync(cancellationToken);
            List<MerchantSecurityUser> merchantSecurityUsers = await (from u in context.MerchantSecurityUsers where u.MerchantReportingId == merchant.MerchantReportingId select u).ToListAsync(cancellationToken);
            List<MerchantDevice> merchantDevices = await (from d in context.MerchantDevices where d.MerchantReportingId == merchant.MerchantReportingId select d).ToListAsync(cancellationToken);

            return this.ModelFactory.ConvertFrom(estateId, merchant, merchantAddresses, merchantContacts, merchantOperators, merchantDevices, merchantSecurityUsers);
        }

        public async Task<MerchantModel> GetMerchantFromReference(Guid estateId,
                                                                  String reference,
                                                                  CancellationToken cancellationToken){
            EstateManagementGenericContext context = await this.ContextFactory.GetContext(estateId, EstateManagementRepository.ConnectionStringIdentifier, cancellationToken);

            Merchant merchant = await (from m in context.Merchants where m.Reference == reference select m).SingleOrDefaultAsync(cancellationToken);

            return this.ModelFactory.ConvertFrom(estateId,merchant, null, null, null, null, null);
        }

        public async Task<List<MerchantModel>> GetMerchants(Guid estateId,
                                                            CancellationToken cancellationToken){
            EstateManagementGenericContext context = await this.ContextFactory.GetContext(estateId, EstateManagementRepository.ConnectionStringIdentifier, cancellationToken);

            Estate estate = await context.Estates.SingleOrDefaultAsync(e => e.EstateId == estateId, cancellationToken:cancellationToken);
            List<Merchant> merchants = await (from m in context.Merchants where m.EstateReportingId == estate.EstateReportingId select m).ToListAsync(cancellationToken);
            List<MerchantAddress> merchantAddresses = await (from a in context.MerchantAddresses where merchants.Select(m => m.MerchantReportingId).Contains(a.MerchantReportingId) select a).ToListAsync(cancellationToken);
            List<MerchantContact> merchantContacts = await (from c in context.MerchantContacts where merchants.Select(m => m.MerchantReportingId).Contains(c.MerchantReportingId) select c).ToListAsync(cancellationToken);
            List<MerchantOperator> merchantOperators = await (from o in context.MerchantOperators where merchants.Select(m => m.MerchantReportingId).Contains(o.MerchantReportingId) select o).ToListAsync(cancellationToken);
            List<MerchantSecurityUser> merchantSecurityUsers = await (from u in context.MerchantSecurityUsers where merchants.Select(m => m.MerchantReportingId).Contains(u.MerchantReportingId) select u).ToListAsync(cancellationToken);
            List<MerchantDevice> merchantDevices = await (from d in context.MerchantDevices where merchants.Select(m => m.MerchantReportingId).Contains(d.MerchantReportingId) select d).ToListAsync(cancellationToken);

            if (merchants.Any() == false){
                return null;
            }

            List<MerchantModel> models = new List<MerchantModel>();

            foreach (Merchant m in merchants){
                List<MerchantAddress> a = merchantAddresses.Where(ma => ma.MerchantReportingId == m.MerchantReportingId).ToList();
                List<MerchantContact> c = merchantContacts.Where(mc => mc.MerchantReportingId == m.MerchantReportingId).ToList();
                List<MerchantOperator> o = merchantOperators.Where(mo => mo.MerchantReportingId == m.MerchantReportingId).ToList();
                List<MerchantSecurityUser> u = merchantSecurityUsers.Where(msu => msu.MerchantReportingId == m.MerchantReportingId).ToList();
                List<MerchantDevice> d = merchantDevices.Where(ma => ma.MerchantReportingId == m.MerchantReportingId).ToList();

                models.Add(this.ModelFactory.ConvertFrom(estateId, m, a, c, o, d, u));
            }

            return models;
        }

        public async Task<StatementHeader> GetStatement(Guid estateId,
                                                        Guid merchantStatementId,
                                                        CancellationToken cancellationToken){
            EstateManagementGenericContext context = await this.ContextFactory.GetContext(estateId, EstateManagementRepository.ConnectionStringIdentifier, cancellationToken);

            Database.Entities.StatementHeader statement = await context.StatementHeaders.Where(sl => sl.StatementId == merchantStatementId).SingleOrDefaultAsync(cancellationToken);

            List<Database.Entities.StatementLine> statementLines = await context.StatementLines.Where(sl => sl.StatementReportingId == statement.StatementReportingId).ToListAsync(cancellationToken);

            var lines = statementLines.GroupBy(g => new{
                                                           g.ActivityDateTime.Date,
                                                           g.ActivityDescription,
                                                           g.ActivityType
                                                       }).OrderBy(o => o.Key.Date).ThenBy(o => o.Key.ActivityDescription).ToList();

            Merchant merchant = await context.Merchants.Where(m => m.MerchantReportingId == statement.MerchantReportingId).SingleOrDefaultAsync(cancellationToken);
            MerchantAddress merchantAddress = await context.MerchantAddresses.Where(m => m.MerchantReportingId == statement.MerchantReportingId).FirstOrDefaultAsync(cancellationToken);
            MerchantContact merchantContact = await context.MerchantContacts.Where(m => m.MerchantReportingId == statement.MerchantReportingId).FirstOrDefaultAsync(cancellationToken);
            Estate estate = await context.Estates.Where(e => e.EstateId == estateId).SingleOrDefaultAsync(cancellationToken);

            StatementHeader header = new StatementHeader();
            header.EstateName = estate.Name;
            header.MerchantAddressLine1 = merchantAddress.AddressLine1;
            header.MerchantContactNumber = merchantContact.PhoneNumber;
            header.MerchantCountry = merchantAddress.Country;
            header.MerchantName = merchant.Name;
            header.MerchantPostcode = merchantAddress.PostalCode;
            header.MerchantRegion = merchantAddress.Region;
            header.MerchantTown = merchantAddress.Town;
            header.MerchantEmail = merchantContact.EmailAddress;
            header.StatementDate = statement.StatementGeneratedDate.ToString("dd-MM-yyyy");
            header.StatementId = "1111";
            header.StatementLines = new List<StatementLine>();

            Decimal statementTotal = 0;
            Decimal transactionsTotal = 0;
            Decimal feesTotal = 0;

            Int32 lineNumber = 1;
            foreach (var statementline in lines){
                header.StatementLines.Add(new StatementLine{
                                                               StatementLineDescription = statementline.Key.ActivityDescription,
                                                               StatementLineDate = statementline.Key.Date.ToString("dd-MM-yyyy"),
                                                               StatementLineAmount = statementline.Sum(s => (s.OutAmount * -1) + s.InAmount),
                                                               StatementLineAmountDisplay = $"{statementline.Sum(s => (s.OutAmount * -1) + s.InAmount)} KES",
                                                               StatementLineNumber = lineNumber.ToString()
                                                           });
                lineNumber++;
                statementTotal += statementline.Key.ActivityType == 1 ? statementline.Sum(s => s.OutAmount * -1) : statementline.Sum(s => s.InAmount);
                transactionsTotal += statementline.Key.ActivityType == 1 ? statementline.Sum(s => s.OutAmount * -1) : 0;
                feesTotal += statementline.Key.ActivityType != 1 ? statementline.Sum(s => s.InAmount) : 0;
            }

            header.StatementTotal = $"{statementTotal} KES";
            header.TransactionsValue = $"{transactionsTotal} KES";
            header.TransactionFeesValue = $"{feesTotal} KES";

            return header;
        }

        public async Task<File> GetFileDetails(Guid estateId, Guid fileId, CancellationToken cancellationToken){

            EstateManagementGenericContext context = await this.ContextFactory.GetContext(estateId, EstateManagementRepository.ConnectionStringIdentifier, cancellationToken);

            Database.Entities.File file = await context.Files.SingleOrDefaultAsync(f => f.FileId == fileId, cancellationToken);

            if (file == null){
                throw new NotFoundException($"File with Id [{fileId}] not found");
            }

            // Now get all the lines
            var fileLinesWithTransactions = await (from fileLine in context.FileLines
                                                   join txn in context.Transactions on fileLine.TransactionReportingId equals txn.TransactionReportingId into transactions
                                                   from t in transactions.DefaultIfEmpty()
                                                   where fileLine.FileReportingId == file.FileReportingId
                                                   select new{
                                                                 FileLine = fileLine,
                                                                 Transaction = t
                                                             }).ToListAsync(cancellationToken);

            Merchant m = await context.Merchants.SingleOrDefaultAsync(m => m.MerchantReportingId == file.MerchantReportingId, cancellationToken);

            File result = new File{
                                      FileId = file.FileId,
                                      Merchant = this.ModelFactory.ConvertFrom(estateId, m),
                                      FileReceivedDate = file.FileReceivedDate,
                                      FileReceivedDateTime = file.FileReceivedDateTime,
                                      FileLineDetails = new List<FileLineDetails>()
                                  };

            foreach (var fileLinesWithTransaction in fileLinesWithTransactions){
                FileLineDetails fileLineDetails = new FileLineDetails{
                                                                         FileLineData = fileLinesWithTransaction.FileLine.FileLineData,
                                                                         Status = fileLinesWithTransaction.FileLine.Status,
                                                                         FileLineNumber = fileLinesWithTransaction.FileLine.LineNumber
                                                                     };

                if(fileLinesWithTransaction.Transaction != null){
                    fileLineDetails.Transaction = new Transaction{
                                                                     AuthCode = fileLinesWithTransaction.Transaction.AuthorisationCode,
                                                                     IsAuthorised = fileLinesWithTransaction.Transaction.IsAuthorised,
                                                                     IsCompleted = fileLinesWithTransaction.Transaction.IsCompleted,
                                                                     ResponseCode = fileLinesWithTransaction.Transaction.ResponseCode,
                                                                     ResponseMessage = fileLinesWithTransaction.Transaction.ResponseMessage,
                                                                     TransactionId = fileLinesWithTransaction.Transaction.TransactionId,
                                                                     TransactionNumber = fileLinesWithTransaction.Transaction.TransactionNumber
                                                                 };
                }
                result.FileLineDetails.Add(fileLineDetails);
            }

            return result;
        }
        
        #endregion

        #region Others

        private const String ConnectionStringIdentifier = "EstateReportingReadModel";

        #endregion
    }
}