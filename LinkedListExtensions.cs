using System.Collections.Generic;

namespace Simulacao_T1
{
    public static class LinkedListExtensions
    {
        //iterates the List nodes in reverse way
        private static IEnumerable<LinkedListNode<T>> Reverse<T>(this LinkedList<T> list)
        {
            LinkedListNode<T>? el = list.Last;
            while (el != null)
            {
                yield return el;
                el = el.Previous;
            }
        }

        public static void SortedInsertion(this LinkedList<Event> list, Event value)
        {
            foreach (LinkedListNode<Event> node in list.Reverse())
            {
                //if inserted value is bigger than current node value, inserts after it
                if (value.CompareTo(node.Value) >= 0)
                {
                    list.AddAfter(node, value);
                    return;
                }
            }

            list.AddFirst(value);
        }
    }
}
