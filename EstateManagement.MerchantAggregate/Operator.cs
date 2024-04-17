namespace EstateManagement.MerchantAggregate
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// 
    /// </summary>
    internal record Operator(String Name, String MerchantNumber, String TerminalNumber, Boolean IsDeleted=false);
    
    internal class Contract{
        internal Guid ContractId{ get; private set; }
        internal List<Guid> ContractProducts { get; private set; }

        public Contract(Guid contractId){
            this.ContractId = contractId;
            this.ContractProducts = new List<Guid>();
        }

        public void AddContractProduct(Guid contractProductId){
            this.ContractProducts.Add(contractProductId);
        }
    }
}