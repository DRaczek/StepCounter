using System;
using SQLite;

namespace StepCounter.Models
{
    public class DailyStep
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [Indexed, NotNull]
        public DateTime Date { get; set; }

        [NotNull]
        public int Steps { get; set; }
    }
}