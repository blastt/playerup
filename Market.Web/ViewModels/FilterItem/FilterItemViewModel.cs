﻿namespace Market.Web.ViewModels
{
    public class FilterItemViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public string FilterName { get; set; }
        public string FilterValue { get; set; }
        public string GameName { get; set; }
        public int Rank { get; set; }

        public string ImagePath { get; set; }
    }
}