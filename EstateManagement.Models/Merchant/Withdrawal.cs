namespace EstateManagement.Models.Merchant;

using System;
using System.Diagnostics.CodeAnalysis;

[ExcludeFromCodeCoverage]
public class Withdrawal
{
    #region Properties
        
    public Decimal Amount { get; set; }
        
    public DateTime WithdrawalDateTime { get; set; }
        
    public Guid WithdrawalId { get; set; }

    #endregion
}