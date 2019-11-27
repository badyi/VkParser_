using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VkParser
{
    class Planner
    {
        private int current;
        private List<queueMember> queue;

        public Planner()
        {
            current = 0;

            queue = new List<queueMember>();
            queueMember q = new queueMember(1, 2, 3);

            queue.Add(q);
            GenerateSteps(100);
        }

        private void GenerateSteps(int k)
        {
            int n = 1;

            for (int i = 0; i < k; i++)
            {
                if (n == 1)
                    queue.Add(new queueMember(4, 2, 3));
                else if (n == 2)
                    queue.Add(new queueMember(1, 4, 3));
                else if (n == 3)
                    queue.Add(new queueMember(1, 2, 4));
                else
                    queue.Add(new queueMember(1, 2, 3));
                n++;
                if (n == 5)
                    n = 1;
            }
        }

        public bool check(int i)
        {
            string path = i.ToString() + ".locker";

            return System.IO.File.Exists(path);
        }


        public void indexControl()
        {
            if (current == queue.Count)
                GenerateSteps(100);
        }

        public queueMember GetCurrent()
        {
            return queue[current];
        }

        public void CurrentIncrement()
        {
            current++;
        }
    }
}
