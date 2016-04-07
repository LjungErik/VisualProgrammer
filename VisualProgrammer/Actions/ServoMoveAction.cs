
namespace VisualProgrammer.Actions
{
    public class ServoMoveAction : IRobotAction
    {
        public int Servo { get; set; }

        public int Degrees { get; set; }

        public ServoMoveAction(int servo, int degrees)
        {
            this.Servo = servo;
            this.Degrees = degrees;
        }

        public string GetActionType()
        {
            return "ServoMove";
        }
    }
}
