namespace EstateManagement.Models.Merchant;

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

[ExcludeFromCodeCoverage]
public class Contract{
    public Guid ContractId{ get; set; }

    public Boolean IsDeleted { get; set; }

    public List<Guid> ContractProducts{ get; set; }

    public Contract(){
        this.ContractProducts = new List<Guid>();
    }
}