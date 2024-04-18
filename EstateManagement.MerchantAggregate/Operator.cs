namespace EstateManagement.MerchantAggregate
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// 
    /// </summary>
    internal record Operator(String Name, String MerchantNumber, String TerminalNumber, Boolean IsDeleted=false);

    internal record Contract{
        public Contract(Boolean IsDeleted = false){
            this.IsDeleted = IsDeleted;
            this.ContractProducts = new List<Guid>();
        }

        public List<Guid> ContractProducts { get; init; }
        public Boolean IsDeleted{ get; init; }
    }
}