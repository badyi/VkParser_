using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VkParser
{
    class ProcessPlanner
    {
        private int current;
        private List<queueMember> queue;

        public ProcessPlanner()
        {
            current = 0;

            queue = new List<queueMember>();
            queueMember q = new queueMember(1, 1, 1);

            queue.Add(q);
            GenerateSteps(100);
        }
        
        private void GenerateSteps(int k)
        {
            int n = 1;

            for (int i = 0; i < k; i++)
            {
                if (n == 1)
                    queue.Add(new queueMember(2, 1, 1));
                else if (n == 2)
                    queue.Add(new queueMember(1, 2, 1));
                else if (n == 3)
                    queue.Add(new queueMember(1, 1, 2));
                else if (n == 4)
                    queue.Add(new queueMember(2, 2, 1));
                else 
                    queue.Add(new queueMember(1, 1, 1));
                n++;
                if (n == 6)
                    n = 1;
            }
        }

        public bool checkWPlock(int i)
        {
            string spath = i.ToString() + ".WPlocker";
            return System.IO.File.Exists(spath);
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
