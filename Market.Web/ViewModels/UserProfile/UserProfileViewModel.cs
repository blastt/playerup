using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Market.Web.ViewModels
{
    public class UserProfileViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public int Rating { get; set; }
        public double PositiveFeedbackProcent { get; set; }
        public double NegativeFeedbackProcent { get; set; }

        public int Positive { get; set; }
        public int Negative { get; set; }
    }
}