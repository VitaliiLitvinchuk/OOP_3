using static Task.TasksWorkers;

namespace Task.Homework
{
    public class Client
    {
        public void ClientCode(Skeleton skeleton)
        {
            Console.WriteLine(skeleton.DoAction());
        }
    }

    public class Homework12 : ITask
    {
        public void Start()
        {
            Client client = new();

            IHand hand1 = new Hand();
            IHand hand2 = new Exohand();

            Skeleton skeleton1 = new(hand1);
            Exoskeleton skeleton2 = new(hand2);

            client.ClientCode(skeleton1);
            client.ClientCode(skeleton2);
        }
    }

    public class Skeleton(IHand hand)
    {
        protected IHand _hand = hand;

        public virtual string DoAction()
        {
            return "Some bones are moving\n" + _hand.Move();
        }
    }

    public class Exoskeleton(IHand hand) : Skeleton(hand)
    {
        public override string DoAction()
        {
            return "Some exo bones is moving\n" + _hand.Move();
        }
    }

    public interface IHand
    {
        string Move();
    }

    public class Hand : IHand
    {
        public string Move() => "Hand is moving";
    }

    public class Exohand : IHand
    {
        public string Move() => "Exo hand is moving";
    }
}
