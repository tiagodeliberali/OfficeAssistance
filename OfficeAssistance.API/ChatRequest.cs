namespace OfficeAssistance.API;

public class ChatRequest
{
    public List<ChatRequestItem> Conversation { get; set; }
}

public class ChatRequestItem
{
    public string Payload { get; set; }
    public string Sender { get; set; }
}
