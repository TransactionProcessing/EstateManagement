namespace EstateManagement.Models.Merchant;

using System;
using System.Diagnostics.CodeAnalysis;

[ExcludeFromCodeCoverage]
public class Device{
    public Guid DeviceId{ get; set; }
    public String DeviceIdentifier{ get; set; }
    public Boolean IsEnabled { get; set; }
}