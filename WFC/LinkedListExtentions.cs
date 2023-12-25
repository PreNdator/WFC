using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WFC
{
    public static class LinkedListExtensions
    {
        public static void RemoveElementsByCondition<T>(this LinkedList<T> list, Predicate<T> condition)
        {
            var currentNode = list.First;

            while (currentNode != null)
            {
                var nextNode = currentNode.Next;

                if (condition(currentNode.Value))
                {
                    list.Remove(currentNode);
                }

                currentNode = nextNode;
            }
        }

        public static T GetRandomElement<T>(this LinkedList<T> list)
        {
            if (list.Count == 0)
            {
                throw new InvalidOperationException("The linked list is empty.");
            }

            Random random = new Random();
            int index = random.Next(list.Count);

            var currentNode = list.First;
            for (int i = 0; i < index; i++)
            {
                currentNode = currentNode.Next;
            }

            return currentNode.Value;
        }

        public static LinkedList<T> Clone<T>(this LinkedList<T> list)
        {
            LinkedList<T> clonedList = new LinkedList<T>();

            foreach (var item in list)
            {
                clonedList.AddLast(item);
            }

            return clonedList;
        }
    }
}
