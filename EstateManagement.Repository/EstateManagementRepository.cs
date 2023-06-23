namespace EstateManagement.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Database.Contexts;
    using EstateManagement.Database.Entities;
    using Microsoft.EntityFrameworkCore;
    using Models.Contract;
    using Models.Factories;
    using Shared.Exceptions;
    using Contract = EstateManagement.Database.Entities.Contract;
    using TransactionFeeModel = Models.Contract.TransactionFee;
    using EstateModel = Models.Estate.Estate;
    using MerchantModel = Models.Merchant.Merchant;
    using ContractModel = Models.Contract.Contract;
    using StatementHeader = Models.MerchantStatement.StatementHeader;
    using StatementLine = Models.MerchantStatement.StatementLine;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="EstateManagement.Repository.IEstateManagementRepository" />
    public class EstateManagementRepository : IEstateManagementRepository
    {
        #region Fields

        /// <summary>
        /// The context factory
        /// </summary>
        private readonly Shared.EntityFramework.IDbContextFactory<EstateManagementGenericContext> ContextFactory;

        /// <summary>
        /// The model factory
        /// </summary>
        private readonly IModelFactory ModelFactory;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EstateManagementRepository"/> class.
        /// </summary>
        /// <param name="contextFactory">The context factory.</param>
        /// <param name="modelFactory">The model factory.</param>
        public EstateManagementRepository(Shared.EntityFramework.IDbContextFactory<EstateManagementGenericContext> contextFactory,
                                          IModelFactory modelFactory)
        {
            this.ContextFactory = contextFactory;
            this.ModelFactory = modelFactory;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the contract.
        /// </summary>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="contractId">The contract identifier.</param>
        /// <param name="includeProducts">if set to <c>true</c> [include products].</param>
        /// <param name="includeProductsWithFees">if set to <c>true</c> [include products with fees].</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        /// <exception cref="NotFoundException">No contract found in read model with Id [{contractId}]</exception>
        public async Task<ContractModel> GetContract(Guid estateId,
                                                     Guid contractId,
                                                     Boolean includeProducts,
                                                     Boolean includeProductsWithFees,
                                                     CancellationToken cancellationToken)
        {
            EstateManagementGenericContext context = await this.ContextFactory.GetContext(estateId, ConnectionStringIdentifier, cancellationToken);

            Contract contract = await context.Contracts.SingleOrDefaultAsync(c => c.ContractId == contractId, cancellationToken);

            if (contract == null)
            {
                throw new NotFoundException($"No contract found in read model with Id [{contractId}]");
            }

            List<ContractProduct> contractProducts = null;
            List<ContractProductTransactionFee> contractProductFees = null;

            if (includeProducts || includeProductsWithFees)
            {
                contractProducts = await context.ContractProducts.Where(cp => cp.ContractId == contractId).ToListAsync(cancellationToken);
            }

            if (includeProductsWithFees)
            {
                contractProductFees = await context.ContractProductTransactionFees.Where(f => f.ContractId == contractId)
                                                   .ToListAsync(cancellationToken);
            }

            return this.ModelFactory.ConvertFrom(contract, contractProducts, contractProductFees);
        }

        private const String ConnectionStringIdentifier = "EstateReportingReadModel";

        public async Task<MerchantModel> GetMerchantFromReference(Guid estateId, 
                                                                  String reference,
                                                                  CancellationToken cancellationToken)
        {
            EstateManagementGenericContext context = await this.ContextFactory.GetContext(estateId, EstateManagementRepository.ConnectionStringIdentifier, cancellationToken);

            Merchant merchant = await (from m in context.Merchants where m.Reference == reference select m).SingleOrDefaultAsync(cancellationToken);

            return this.ModelFactory.ConvertFrom(merchant, null, null, null, null, null);
        }
        public async Task<List<ContractModel>> GetContracts(Guid estateId,
                                                                         CancellationToken cancellationToken)
        {
            EstateManagementGenericContext context = await this.ContextFactory.GetContext(estateId, EstateManagementRepository.ConnectionStringIdentifier, cancellationToken);

            var query = await (from c in context.Contracts
                           join cp in context.ContractProducts on c.ContractId equals cp.ContractId into cps
                           from contractprouduct in cps.DefaultIfEmpty() 
                           join eo in context.EstateOperators on c.OperatorId equals eo.OperatorId
                           select new
                           {
                               Contract = c,
                               Product = contractprouduct,
                               Operator = eo
                           }).ToListAsync(cancellationToken);

            List<ContractModel> contracts = new List<ContractModel>();

            foreach (var contractData in query)
            {
                // attempt to find the contract
                ContractModel contract = contracts.SingleOrDefault(c => c.ContractId == contractData.Contract.ContractId);

                if (contract == null)
                {
                    // create the contract
                    contract = new ContractModel
                    {
                        OperatorId = contractData.Contract.OperatorId,
                        OperatorName = contractData.Operator.Name,
                        Products = new List<Product>(),
                        Description = contractData.Contract.Description,
                        IsCreated = true,
                        ContractId = contractData.Contract.ContractId
                    };

                    contracts.Add(contract);
                }

                // Now add the product if not already added
                Boolean productFound = contract.Products.Any(p => p.ProductId == contractData.Product.ProductId);

                if (productFound == false)
                {
                    if (contractData.Product != null)
                    {
                        // Not already there so need to add it
                        contract.Products.Add(new Product
                                              {
                                                  ProductId = contractData.Product.ProductId,
                                                  TransactionFees = null,
                                                  Value = contractData.Product.Value,
                                                  Name = contractData.Product.ProductName,
                                                  DisplayText = contractData.Product.DisplayText
                                              });
                    }
                }
            }

            return contracts;
        }

        /// <summary>
        /// Gets the estate.
        /// </summary>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        /// <exception cref="NotFoundException">No estate found with Id [{estateId}]</exception>
        public async Task<EstateModel> GetEstate(Guid estateId,
                                                 CancellationToken cancellationToken)
        {
            EstateManagementGenericContext context = await this.ContextFactory.GetContext(estateId, EstateManagementRepository.ConnectionStringIdentifier, cancellationToken);

            Estate estate = await context.Estates.SingleOrDefaultAsync(e => e.EstateId == estateId, cancellationToken);

            if (estate == null)
            {
                throw new NotFoundException($"No estate found in read model with Id [{estateId}]");
            }

            List<EstateOperator> estateOperators = await context.EstateOperators.Where(eo => eo.EstateReportingId == estate.EstateReportingId).ToListAsync(cancellationToken);
            List<EstateSecurityUser> estateSecurityUsers = await context.EstateSecurityUsers.Where(esu => esu.EstateReportingId == estate.EstateReportingId).ToListAsync(cancellationToken);

            return this.ModelFactory.ConvertFrom(estate, estateOperators, estateSecurityUsers);
        }

        /// <summary>
        /// Gets the merchant contracts.
        /// </summary>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="merchantId">The merchant identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task<List<ContractModel>> GetMerchantContracts(Guid estateId,
                                                                    Guid merchantId,
                                                                    CancellationToken cancellationToken)
        {
            EstateManagementGenericContext context = await this.ContextFactory.GetContext(estateId, EstateManagementRepository.ConnectionStringIdentifier, cancellationToken);

            var x = await (from c in context.Contracts
                    join cp in context.ContractProducts on c.ContractId equals cp.ContractId
                    join eo in context.EstateOperators on c.OperatorId equals eo.OperatorId
                    join m in context.Merchants on c.EstateReportingId equals m.EstateReportingId
                    join e in context.Estates on c.EstateReportingId equals e.EstateReportingId
                           where m.MerchantId == merchantId && e.EstateId == estateId
                    select new
                           {
                               Contract = c,
                               Product = cp,
                               Operator = eo
                           }).ToListAsync(cancellationToken);
            
            List<ContractModel> contracts = new List<ContractModel>();

            foreach (var test in x)
            {
                // attempt to find the contract
                ContractModel contract = contracts.SingleOrDefault(c => c.ContractId == test.Contract.ContractId);

                if (contract == null)
                {
                    // create the contract
                    contract = new ContractModel
                               {
                                   OperatorId = test.Contract.OperatorId,
                                   OperatorName = test.Operator.Name,
                                   Products = new List<Product>(),
                                   Description = test.Contract.Description,
                                   IsCreated = true,
                                   ContractId = test.Contract.ContractId
                               };
                    
                    contracts.Add(contract);
                }

                // Now add the product if not already added
                Boolean productFound = contract.Products.Any(p => p.ProductId == test.Product.ProductId);

                if (productFound == false)
                {
                    // Not already there so need to add it
                    contract.Products.Add(new Product
                                          {
                                              ProductId = test.Product.ProductId,
                                              TransactionFees = null,
                                              Value = test.Product.Value,
                                              Name = test.Product.ProductName,
                                              DisplayText = test.Product.DisplayText
                                          });
                }
            }

            return contracts;
        }

        /// <summary>
        /// Gets the merchants.
        /// </summary>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task<List<MerchantModel>> GetMerchants(Guid estateId,
                                                            CancellationToken cancellationToken){

            // TODO: Rework this flow
            return null;
            //EstateManagementGenericContext context = await this.ContextFactory.GetContext(estateId, EstateManagementRepository.ConnectionStringIdentifier, cancellationToken);

            //var estate = await context.Estates.SingleOrDefaultAsync(e => e.EstateId == estateId);
            //List<Merchant> merchants = await (from m in context.Merchants where m.EstateReportingId == estate.EstateReportingId select m).ToListAsync(cancellationToken);

            //List<MerchantAddress> merchantAddresses = await (from a in context.MerchantAddresses where a.EstateId == estateId select a).ToListAsync(cancellationToken);
            //List<MerchantContact> merchantContacts = await (from c in context.MerchantContacts where c.EstateId == estateId select c).ToListAsync(cancellationToken);
            //List<MerchantDevice> merchantDevices = await (from d in context.MerchantDevices where d.EstateId == estateId select d).ToListAsync(cancellationToken);
            //List<MerchantSecurityUser> merchantSecurityUsers =
            //    await (from u in context.MerchantSecurityUsers where u.EstateId == estateId select u).ToListAsync(cancellationToken);
            //List<MerchantOperator> merchantOperators = await (from o in context.MerchantOperators where o.EstateId == estateId select o).ToListAsync(cancellationToken);

            //if (merchants.Any() == false)
            //{
            //    return null;
            //}

            //List<MerchantModel> models = new List<MerchantModel>();

            //foreach (Merchant merchant in merchants)
            //{
            //    List<MerchantAddress> addresses = merchantAddresses.Where(a => a.MerchantId == merchant.MerchantId).ToList();
            //    List<MerchantContact> contacts = merchantContacts.Where(c => c.MerchantId == merchant.MerchantId).ToList();
            //    List<MerchantDevice> devices = merchantDevices.Where(d => d.MerchantId == merchant.MerchantId).ToList();
            //    List<MerchantSecurityUser> securityUsers = merchantSecurityUsers.Where(s => s.MerchantId == merchant.MerchantId).ToList();
            //    List<MerchantOperator> operators = merchantOperators.Where(o => o.MerchantId == merchant.MerchantId).ToList();

            //    models.Add(this.ModelFactory.ConvertFrom(merchant, addresses, contacts, operators, devices, securityUsers));
            //}

            //return models;
        }
        
        /// <summary>
        /// Gets the transaction fees for product.
        /// </summary>
        /// <param name="estateId">The estate identifier.</param>
        /// <param name="merchantId">The merchant identifier.</param>
        /// <param name="contractId">The contract identifier.</param>
        /// <param name="productId">The product identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task<List<TransactionFeeModel>> GetTransactionFeesForProduct(Guid estateId,
                                                                                  Guid merchantId,
                                                                                  Guid contractId,
                                                                                  Guid productId,
                                                                                  CancellationToken cancellationToken)
        {
            EstateManagementGenericContext context = await this.ContextFactory.GetContext(estateId, EstateManagementRepository.ConnectionStringIdentifier, cancellationToken);

            List<ContractProductTransactionFee> transactionFees = await context
                                                                        .ContractProductTransactionFees
                                                                        .Where(c => c.ContractId == contractId && c.ProductId == productId
                                                                               && c.IsEnabled == true)
                                                                        .ToListAsync(cancellationToken);

            List<TransactionFeeModel> transactionFeeModels = this.ModelFactory.ConvertFrom(transactionFees);

            return transactionFeeModels;
        }

        public async Task<StatementHeader> GetStatement(Guid estateId,
                                                                               Guid merchantStatementId,
                                                                               CancellationToken cancellationToken)
        {
            EstateManagementGenericContext context = await this.ContextFactory.GetContext(estateId, EstateManagementRepository.ConnectionStringIdentifier, cancellationToken);

            var statement = await context.StatementHeaders.Where(sl => sl.StatementId == merchantStatementId).SingleOrDefaultAsync(cancellationToken);

            var statementLines = await context.StatementLines.Where(sl => sl.StatementId == merchantStatementId).ToListAsync(cancellationToken);

            var lines = statementLines.GroupBy(g => new
                                                    {
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
            foreach (var statementline in lines)
            {
                header.StatementLines.Add(new StatementLine
                                          {
                                              StatementLineDescription = statementline.Key.ActivityDescription,
                                              StatementLineDate = statementline.Key.Date.ToString("dd-MM-yyyy"),
                                              StatementLineAmount = statementline.Sum(s => (s.OutAmount * -1)+ s.InAmount),
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
        
        #endregion
    }
}