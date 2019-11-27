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
    public partial class Form1 : Form
    {
        private Parser data;
        private WorkWithJson wj = new WorkWithJson();

        private System.Windows.Forms.Timer timer;

        private Planner planner = new Planner();
        private ProcessPlanner processPlanner = new ProcessPlanner();

        private Locker locker1 = new Locker();
        private Locker locker2 = new Locker();
        private Locker locker3 = new Locker();

        private static bool completed1 = false;
        private static bool completed2 = false;
        private static bool completed3 = false;

        public Form1()
        {
            InitializeComponent();
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

            Thread.Sleep(200);
            if (!processPlanner.checkWPlock(1) && !processPlanner.checkWPlock(2) && !processPlanner.checkWPlock(3) && !planner.check(1) && !planner.check(2) && !planner.check(3))
                if (!locker1.isLock() && !locker2.isLock() && !locker3.isLock()) // checks existing of locker files
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


                    bool processTurn1 = processPlanner.GetCurrent().f1 == 2 ? true : false; // true if its the turn of 2nd process
                    bool processTurn2 = processPlanner.GetCurrent().f2 == 2 ? true : false; // true if its the turn of 2nd process
                    bool processTurn3 = processPlanner.GetCurrent().f3 == 2 ? true : false; // true if its the turn of 2nd process

                    if (planner.GetCurrent().f1 == 1 && !completed1) // block for statring first thread
                    {
                        t1 = new Thread(() => WorkWithJson.save(data.get_list_texts(), 1, ref completed1,ref locker1 , ref processTurn1));
                        t1.Start();
                    }
                    else if (planner.GetCurrent().f1 == 4)
                    {
                        t1 = new Thread(() => WorkWithJson.interferingFunc(1,ref locker1, ref processTurn1));
                        t1.Start();
                    }

                    if (planner.GetCurrent().f2 == 2 && !completed2) // block for statring second thread
                    { 
                        t2 = new Thread(() => WorkWithJson.save(data.get_list_links(), 2, ref completed2, ref locker2, ref processTurn2));
                        t2.Start();
                    }
                    else if (planner.GetCurrent().f2 == 4)
                    {
                        t2 = new Thread(() => WorkWithJson.interferingFunc(2,ref locker2, ref processTurn2));
                        t2.Start();
                    }

                    if (planner.GetCurrent().f3 == 3 && !completed3) // block for statring third thread
                    {
                        t3 = new Thread(() => WorkWithJson.save(data.get_list_imgs(), 3, ref completed3, ref locker3, ref processTurn3));
                        t3.Start();
                    }
                    else if (planner.GetCurrent().f3 == 4)
                    {
                        t3 = new Thread(() => WorkWithJson.interferingFunc(3,ref locker3, ref processTurn3));
                        t3.Start();
                    }
                }
            planner.CurrentIncrement(); // increments current state of planned steps list
            planner.indexControl(); // generates new 100 steps if сurrent steps are over
            processPlanner.CurrentIncrement();
            processPlanner.indexControl();
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

    public class Locker
    {
        private bool lockState { get; set; }

        public Locker()
        {
            lockState = false;
        }
        public void Lock()
        {
            lockState = true;
        }
        public void Unlock()
        {
            lockState = false;
        }

        public bool isLock()
        {
            return lockState;
        }
    }
}
