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
    using EstateModel = Models.Estate.Estate;
    using MerchantModel = Models.Merchant.Merchant;
    using ContractModel = Models.Contract.Contract;
    using StatementHeader = Models.MerchantStatement.StatementHeader;
    using StatementLine = Models.MerchantStatement.StatementLine;
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
            List<Database.Entities.ContractProductTransactionFee> contractProductFees = null;

            if (includeProducts || includeProductsWithFees){
                contractProducts = await context.ContractProducts.Where(cp => cp.ContractId == contract.ContractId).ToListAsync(cancellationToken);
            }

            if (includeProductsWithFees){
                contractProductFees = await (from cptf in context.ContractProductTransactionFees
                                             join cp in context.ContractProducts on cptf.ContractProductId equals cp.ContractProductId
                                             where cp.ContractId == contract.ContractId
                                             select cptf).ToListAsync(cancellationToken);
            }

            return this.ModelFactory.ConvertFrom(estateId, contract, contractProducts, contractProductFees);
        }

        public async Task<List<ContractModel>> GetContracts(Guid estateId,
                                                            CancellationToken cancellationToken){
            EstateManagementGenericContext context = await this.ContextFactory.GetContext(estateId, EstateManagementRepository.ConnectionStringIdentifier, cancellationToken);

            var query = await (from c in context.Contracts
                               join cp in context.ContractProducts on c.ContractId equals cp.ContractId into cps
                               from contractprouduct in cps.DefaultIfEmpty()
                               join eo in context.Operators on c.OperatorId equals eo.OperatorId
                               join e in context.Estates on eo.EstateId equals e.EstateId
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
                Boolean productFound = contract.Products.Any(p => p.ContractProductId == contractData.Product.ContractProductId);

                if (productFound == false){
                    if (contractData.Product != null){
                        // Not already there so need to add it
                        contract.Products.Add(new Product{
                                                             ContractProductId = contractData.Product.ContractProductId,
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

            List<EstateSecurityUser> estateSecurityUsers = await context.EstateSecurityUsers.Where(esu => esu.EstateId == estate.EstateId).ToListAsync(cancellationToken);
            List<Operator> operators = await context.Operators.Where(eo => eo.EstateId == estate.EstateId).ToListAsync(cancellationToken);

            return this.ModelFactory.ConvertFrom(estate, estateSecurityUsers, operators);
        }

        public async Task<List<ContractModel>> GetMerchantContracts(Guid estateId,
                                                                    Guid merchantId,
                                                                    CancellationToken cancellationToken){
            EstateManagementGenericContext context = await this.ContextFactory.GetContext(estateId, EstateManagementRepository.ConnectionStringIdentifier, cancellationToken);

            var x = await (from c in context.Contracts
                           join cp in context.ContractProducts on c.ContractId equals cp.ContractId
                           join eo in context.Operators on c.OperatorId equals eo.OperatorId
                           join m in context.Merchants on c.EstateId equals m.EstateId
                           join e in context.Estates on c.EstateId equals e.EstateId
                           join mc in context.MerchantContracts on new {c.ContractId, m.MerchantId} equals new {mc.ContractId, mc.MerchantId}
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
                                                    EstateId = estateId
                                                };

                    contracts.Add(contract);
                }

                // Now add the product if not already added
                Boolean productFound = contract.Products.Any(p => p.ContractProductId == test.Product.ContractProductId);

                if (productFound == false){
                    // Not already there so need to add it
                    contract.Products.Add(new Product{
                                                         ContractProductId = test.Product.ContractProductId,
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

            List<MerchantAddress> merchantAddresses = await (from a in context.MerchantAddresses where a.MerchantId == merchantId select a).ToListAsync(cancellationToken);
            List<MerchantContact> merchantContacts = await (from c in context.MerchantContacts where c.MerchantId == merchantId select c).ToListAsync(cancellationToken);
            List<MerchantOperator> merchantOperators = await (from o in context.MerchantOperators where o.MerchantId == merchantId select o).ToListAsync(cancellationToken);
            List<MerchantSecurityUser> merchantSecurityUsers = await (from u in context.MerchantSecurityUsers where u.MerchantId == merchantId select u).ToListAsync(cancellationToken);
            List<MerchantDevice> merchantDevices = await (from d in context.MerchantDevices where d.MerchantId == merchantId select d).ToListAsync(cancellationToken);

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
            List<Merchant> merchants = await (from m in context.Merchants where m.EstateId == estateId select m).ToListAsync(cancellationToken);
            List<MerchantAddress> merchantAddresses = await (from a in context.MerchantAddresses where merchants.Select(m => m.MerchantId).Contains(a.MerchantId) select a).ToListAsync(cancellationToken);
            List<MerchantContact> merchantContacts = await (from c in context.MerchantContacts where merchants.Select(m => m.MerchantId).Contains(c.MerchantId) select c).ToListAsync(cancellationToken);
            List<MerchantOperator> merchantOperators = await (from o in context.MerchantOperators where merchants.Select(m => m.MerchantId).Contains(o.MerchantId) select o).ToListAsync(cancellationToken);
            List<MerchantSecurityUser> merchantSecurityUsers = await (from u in context.MerchantSecurityUsers where merchants.Select(m => m.MerchantId).Contains(u.MerchantId) select u).ToListAsync(cancellationToken);
            List<MerchantDevice> merchantDevices = await (from d in context.MerchantDevices where merchants.Select(m => m.MerchantId).Contains(d.MerchantId) select d).ToListAsync(cancellationToken);

            if (merchants.Any() == false){
                return null;
            }

            List<MerchantModel> models = new List<MerchantModel>();

            foreach (Merchant m in merchants){
                List<MerchantAddress> a = merchantAddresses.Where(ma => ma.MerchantId == m.MerchantId).ToList();
                List<MerchantContact> c = merchantContacts.Where(mc => mc.MerchantId == m.MerchantId).ToList();
                List<MerchantOperator> o = merchantOperators.Where(mo => mo.MerchantId == m.MerchantId).ToList();
                List<MerchantSecurityUser> u = merchantSecurityUsers.Where(msu => msu.MerchantId == m.MerchantId).ToList();
                List<MerchantDevice> d = merchantDevices.Where(ma => ma.MerchantId == m.MerchantId).ToList();

                models.Add(this.ModelFactory.ConvertFrom(estateId, m, a, c, o, d, u));
            }

            return models;
        }

        public async Task<StatementHeader> GetStatement(Guid estateId,
                                                        Guid merchantStatementId,
                                                        CancellationToken cancellationToken){
            EstateManagementGenericContext context = await this.ContextFactory.GetContext(estateId, EstateManagementRepository.ConnectionStringIdentifier, cancellationToken);

            Database.Entities.StatementHeader statement = await context.StatementHeaders.Where(sl => sl.StatementId == merchantStatementId).SingleOrDefaultAsync(cancellationToken);

            List<Database.Entities.StatementLine> statementLines = await context.StatementLines.Where(sl => sl.StatementId == statement.StatementId).ToListAsync(cancellationToken);

            var lines = statementLines.GroupBy(g => new{
                                                           g.ActivityDateTime.Date,
                                                           g.ActivityDescription,
                                                           g.ActivityType
                                                       }).OrderBy(o => o.Key.Date).ThenBy(o => o.Key.ActivityDescription).ToList();

            Merchant merchant = await context.Merchants.Where(m => m.MerchantId == statement.MerchantId).SingleOrDefaultAsync(cancellationToken);
            MerchantAddress merchantAddress = await context.MerchantAddresses.Where(m => m.MerchantId == statement.MerchantId).FirstOrDefaultAsync(cancellationToken);
            MerchantContact merchantContact = await context.MerchantContacts.Where(m => m.MerchantId == statement.MerchantId).FirstOrDefaultAsync(cancellationToken);
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
                                                   join txn in context.Transactions on fileLine.TransactionId equals txn.TransactionId into transactions
                                                   from t in transactions.DefaultIfEmpty()
                                                   where fileLine.FileId == file.FileId
                                                   select new{
                                                                 FileLine = fileLine,
                                                                 Transaction = t
                                                             }).ToListAsync(cancellationToken);

            Merchant m = await context.Merchants.SingleOrDefaultAsync(m => m.MerchantId == file.MerchantId, cancellationToken);

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

        public async Task<List<Models.Operator.Operator>> GetOperators(Guid estateId, CancellationToken cancellationToken){
            EstateManagementGenericContext context = await this.ContextFactory.GetContext(estateId, EstateManagementRepository.ConnectionStringIdentifier, cancellationToken);

            Estate estate = await context.Estates.SingleOrDefaultAsync(e => e.EstateId == estateId, cancellationToken: cancellationToken);
            List<Operator> operators = await (from o in context.Operators where o.EstateId == estate.EstateId select o).ToListAsync(cancellationToken);

            List<Models.Operator.Operator> models = new();

            foreach (Operator @operator in operators){
                models.Add(new Models.Operator.Operator{
                                                           OperatorId = @operator.OperatorId,
                                                           RequireCustomTerminalNumber = @operator.RequireCustomTerminalNumber,
                                                           RequireCustomMerchantNumber = @operator.RequireCustomMerchantNumber,
                                                           Name = @operator.Name,
                                                       });
            }

            return models;
        }

        #endregion

        #region Others

        private const String ConnectionStringIdentifier = "EstateReportingReadModel";

        #endregion
    }
}