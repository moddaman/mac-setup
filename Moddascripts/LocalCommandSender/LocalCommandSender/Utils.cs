namespace LocalCommandSender;

public static class Utils
{
    public static List<Argument> GetBufferOverloadArgument()
    {
        var list = new List<Argument>();
        for (int i = 0; i < 200; i++)
        {
            list.Add(new Argument(1, "80"));
        }
        return list;
    }
    
    public static (int commandId, List<Argument> arguments) GetSetDynamicCurrent()
    {
        var list = new List<Argument>();
        for (int i = 0; i < 1; i++)
        {
            list.Add(new Argument(1, "2"));
        }
        return (48, list);
    }
    
    
    public static (int commandId, List<Argument> arguments) GetSetDeviceCli(string cliCommand)
    {
        var list = new List<Argument>();
        for (int i = 0; i < 1; i++)
        {
            list.Add(new Argument(1, cliCommand));
        }
        return (93, list);
    }
}