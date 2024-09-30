using Task.Homework.h_t_26_09_2024;
using static Task.TasksWorkers;

namespace Task.Homework
{
    public class Homework7 : ITask
    {
        public void Start()
        {
            CustomerList<double> list = new();
            list.AddFirst(12.5);
            list.AddLast(6.7);
            list.AddFirst(3.14);
            list.AddFirst(2.71);

            foreach (var item in list)
                Console.WriteLine(item);

            list.Remove(2.71);

            foreach (var item in list)
                Console.WriteLine(item);
        }
    }
}

namespace Task.Homework.h_t_26_09_2024
{
    public class CustomerList<T>
    {
        private record Node(T Data)
        {
            public Node? Next = default;
        }
        private Node? _head;
        private Node? _tail;
        private int _size = 0;
        public void AddFirst(T item)
        {
            Node newNode = new(item)
            {
                Next = _head
            };
            _head = newNode;

            if (_size == 0)
                _tail = _head;

            _size++;
        }

        public void AddLast(T item)
        {
            Node newNode = new(item);

            if (_size == 0)
            {
                _head = newNode;
                _tail = newNode;
            }
            else
            {
                _tail!.Next = newNode;
                _tail = newNode;
            }

            _size++;
        }

        public void Clear()
        {
            _head = default;
            _tail = default;
            _size = 0;
        }

        public bool Contains(T item)
            => IndexOf(item) >= 0;

        public T? Find(Predicate<T> match)
        {
            Node? current = _head;
            while (current != default)
            {
                if (match(current.Data))
                    return current.Data;
                current = current.Next;
            }
            return default;
        }

        public T? FindLast(Predicate<T> match)
        {
            Node? current = _head;
            T? result = default;

            while (current != default)
            {
                if (match(current.Data))
                    result = current.Data;
                current = current.Next;
            }

            return result;
        }

        public void RemoveFirst()
        {
            if (_size == 0)
                throw new InvalidOperationException("List is empty");

            _head = _head!.Next;
            _size--;

            if (_size == 0)
                _tail = default;
        }

        public void RemoveLast()
        {
            if (_size == 0)
                throw new InvalidOperationException("List is empty");

            if (_size == 1)
            {
                _head = default;
                _tail = default;
            }
            else
            {
                Node? current = _head;

                while (current!.Next != _tail)
                    current = current.Next;

                current.Next = default;
                _tail = current;
            }

            _size--;
        }

        public void Remove(T item)
        {
            int index = IndexOf(item);
            if (index == -1)
                return;

            if (index == 0)
                RemoveFirst();
            else
            {
                Node? current = _head;
                for (int i = 0; i < index - 1; i++)
                    current = current!.Next;

                current!.Next = current.Next!.Next;
                if (current.Next == default)
                    _tail = current;

                _size--;
            }
        }

        private int IndexOf(T item)
        {
            Node? current = _head;
            int index = 0;

            while (current != default)
            {
                if (Equals(current.Data, item))
                    return index;

                current = current.Next;
                index++;
            }

            return -1;
        }

        public IEnumerator<T> GetEnumerator()
        {
            Node? current = _head;
            while (current != default)
            {
                yield return current.Data;
                current = current.Next;
            }
        }
    }
}