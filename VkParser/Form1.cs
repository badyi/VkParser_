using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VkParser
{
    public class Locker
    {
        public bool isLock { get; set; }
        public Locker()
        {
            isLock = false;
        }
        public void Lock()
        {
            isLock = true;
        }
        public void unlock()
        {
            isLock = false;
        }
    }
    public partial class Form1 : Form
    {
        Parser data;
        WorkWithJson wj = new WorkWithJson();
        System.Windows.Forms.Timer timer;
        Planner planner = new Planner();

        Locker locker1 = new Locker();
        Locker locker2 = new Locker();
        Locker locker3 = new Locker();

        static bool completed1 = false;
        static bool completed2 = false;
        static bool completed3 = false;

        public Form1()
        {
            InitializeComponent();
        }

        public class queueMember
        {
            public int f1;
            public int f2;
            public int f3;

            public queueMember(int i1, int i2, int i3)
            {
                f1 = i1;
                f2 = i2;
                f3 = i3;
            }

            public bool Exist4()
            {
                return f1 == 4 || f2 == 4 || f3 == 4;
            }
        }

        class Planner
        {
            private int current;
            public List<queueMember> queue;

            public Planner()
            {
                current = 0;

                queue = new List<queueMember>();
                queueMember q = new queueMember(1, 2, 3);

                queue.Add(q);
                GenerateSteps(100);
            }

            public bool checkWPlock(int i)
            {
                string spath = i.ToString() + ".WPlocker";
                return System.IO.File.Exists(spath);
            }

            public bool MonitorOS()
            {
                return checkWPlock(1) && checkWPlock(2) && checkWPlock(3);
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

            private bool check(int i)
            {
                string path = i.ToString() + ".locker";

                return System.IO.File.Exists(path);
            }

            public bool check()
            {
                return (check(1) && check(2) && check(3));
            }

            public void indexControl()
            {
                if (current == queue.Count)
                    GenerateSteps(100);
            }

            public int GetCurrent()
            {
                return current;
            }

            public void CurrentIncrement()
            {
                current++;
            }
        }

        public void interferingFunc(int i, ref Locker locker)
        {
            string nameFile = "f" + i.ToString() + ".json";
            locker.Lock();
            //FileStream setlock = new FileStream(i.ToString() + ".locker", FileMode.Create);
            
            using (FileStream f = new FileStream(nameFile, FileMode.OpenOrCreate))
            {
            }
            locker.unlock();
            //setlock.Close();
            // File.Delete(i.ToString() + ".locker");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            data = new Parser();
            timer = new System.Windows.Forms.Timer();
            timer.Enabled = true;
            timer.Interval = 2000;
            timer.Tick += timer_tick;
            timer.Start();
        }

        private void timer_tick(object sender, EventArgs e)
        {
            Thread t1;
            Thread t2;
            Thread t3;
            Thread.Sleep(100);
            //if (!planner.MonitorOS())
                if (!locker1.isLock && !locker2.isLock && !locker3.isLock) // checks existing of locker files
                {
                    if (completed1 && completed2 && completed3) { 
                        data.RefreshAndParse();
                        completed1 = false;
                        completed2 = false;
                        completed3 = false;
                    }

                    completed1 = data.get_list_texts().Count == 0 ? true : completed1;
                    completed2 = data.get_list_links().Count == 0 ? true : completed2;
                    completed3 = data.get_list_imgs().Count == 0 ? true : completed3;

                    if (planner.queue[planner.GetCurrent()].f1 == 1 && !completed1) // block for statring first thread
                    {
                        t1 = new Thread(() => WorkWithJson.save(data.get_list_texts(), 1, ref completed1,ref locker1));
                        t1.Start();
                    }
                    else if (planner.queue[planner.GetCurrent()].f1 == 4)
                    {
                        t1 = new Thread(() => interferingFunc(1,ref locker1));
                        t1.Start();
                    }

                    if (planner.queue[planner.GetCurrent()].f2 == 2 && !completed2) // block for statring second thread
                    { 
                        t2 = new Thread(() => WorkWithJson.save(data.get_list_links(), 2, ref completed2, ref locker2));
                        t2.Start();
                    }
                    else if (planner.queue[planner.GetCurrent()].f2 == 4)
                    {
                        t2 = new Thread(() => interferingFunc(2,ref locker2));
                        t2.Start();
                    }

                    if (planner.queue[planner.GetCurrent()].f3 == 3 && !completed3) // block for statring third thread
                    {
                        t3 = new Thread(() => WorkWithJson.save(data.get_list_imgs(), 3, ref completed3, ref locker3));
                        t3.Start();
                    }
                    else if (planner.queue[planner.GetCurrent()].f3 == 4)
                    {
                        t3 = new Thread(() => interferingFunc(3,ref locker3));
                        t3.Start();
                    }
                }
            planner.CurrentIncrement(); // increments current state of planned step
            planner.indexControl(); // generates new 100 steps if сurrent steps are over
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (completed1 && completed2 && completed3) {
                timer.Enabled = false;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }
    }
}
