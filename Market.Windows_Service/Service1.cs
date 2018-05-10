using Market.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace Market.Windows_Service
{
    public partial class Service2 : ServiceBase
    {


        private System.Timers.Timer timer;
        IOrderService _orderService;
        public Service2()
        {
            InitializeComponent();
            _orderService = orderService;
            
        }


        protected override void OnStart(string[] args)
        {
            Deletefile(); // This Method for Deleting files in DeleteData after 7 days. 
        }
        protected void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            Deletefile();
        }


        public void Deletefile()
        {
            try
            {
                timer = new System.Timers.Timer();
                timer.Interval = 60000;
                timer.Elapsed += new System.Timers.ElapsedEventHandler(timer_Elapsed);
                foreach (var order in _orderService.GetOrders())
                {
                    order.Sum = 111;
                }
                timer.Enabled = true;
                timer.Start();
            }
            catch
            {

            }

        }
        protected override void OnStop()
        {

        }
    }
}
