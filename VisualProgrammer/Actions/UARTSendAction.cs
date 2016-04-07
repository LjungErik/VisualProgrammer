
namespace VisualProgrammer.Actions
{
    public class UARTSendAction : IRobotAction
    {
        public string Message { get; set; }

        public UARTSendAction(string message)
        {
            Message = message;
        }

        public string GetActionType()
        {
            return "UARTSend";
        }
    }
}
