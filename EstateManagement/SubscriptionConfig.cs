namespace EstateManagement;

using System;

public class SubscriptionConfig
{
    public Boolean IsEnabled { get; set; }
    public String Filter { get; set; }
    public String Ignore { get; set; }
    public String StreamName { get; set; }
}