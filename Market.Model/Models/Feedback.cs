using System;


namespace Market.Model.Models
{
    public enum Emotions
    {
        Bad,
        Good
    }

    public class Feedback
    {
        public int Id { get; set; }
        public Emotions Grade { get; set; }
        public string Comment { get; set; }
        public DateTime DateLeft { get; set; } = DateTime.Now;

        public Order Order { get; set; }

        public string UserToId { get; set; }
        public UserProfile UserTo { get; set; }

        public string UserFromId { get; set; }
        public UserProfile UserFrom { get; set; }

    }
}
