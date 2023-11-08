namespace LocalObservationSender;

public class ChargeSession
{
    public int Id { get; set; } = 1;
    public string Start { get; set; }
    public string Stop { get; set; }
    public string Auth { get; set; } = "";
    public int AuthReason { get; set; } = 0;
    public decimal EnergyKWh { get; set; } = 0;
    public decimal MeterValueStart { get; set; } = 0.24m;
    public decimal MeterValueStop { get; set; } = 0.24m;
    public string Unit { get; set; } = "kWh";
    public string Sn { get; set; } = "test test";
}

public class ChargeSessionWithSignature
{
    public ChargeSession? ChargeSession { get; set; }
    public string Signature { get; set; }
}