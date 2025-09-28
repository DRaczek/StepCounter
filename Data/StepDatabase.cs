using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SQLite;
using StepCounter.Models;

namespace StepCounter.Data
{
    public class StepDatabase
    {
        private readonly SQLiteAsyncConnection _database;

        public StepDatabase(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<DailyStep>().Wait();
        }

        public Task<List<DailyStep>> GetStepsAsync()
            => _database.Table<DailyStep>().OrderByDescending(s => s.Date).ToListAsync();

        public Task<DailyStep?> GetStepForDateAsync(DateTime date)
            => _database.Table<DailyStep>().FirstOrDefaultAsync(s => s.Date == date.Date);

        public Task<int> SaveStepAsync(DailyStep step)
            => _database.InsertOrReplaceAsync(step);

        public Task<int> DeleteStepAsync(DailyStep step)
            => _database.DeleteAsync(step);
    }
}