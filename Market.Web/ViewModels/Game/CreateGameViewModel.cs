﻿namespace Market.Web.ViewModels
{
    public class CreateGameViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }

        public int Rank { get; set; }

        public string ImagePath { get; set; }
    }
}