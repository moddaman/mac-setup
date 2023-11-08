namespace LocalCommandSender;

public record CommandRequestPayload(
    int Id,
    DateTime Timestamp,
    DateTime ExpiresAt,
    List<Argument> Arguments);
    
public record Argument(int Id, string Value);