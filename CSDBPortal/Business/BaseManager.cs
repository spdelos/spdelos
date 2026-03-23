using CSDBPortal.Data;
using Microsoft.EntityFrameworkCore;

namespace CSDBPortal.Business
{
    public class BaseManager
    {
        private readonly ApplicationDbContext _db;

        public BaseManager(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<bool> CreateOrUpdateRecordAsync<T>(T tableObj, string mode) where T : class
        {
            try
            {
                _db.Entry(tableObj).State = mode == "Add" ? EntityState.Added : EntityState.Modified;
                await _db.SaveChangesAsync();
            }
            catch
            {
                return false;
            }
            return true;
        }

        public async Task<bool> DeleteRecordAsync<T>(T tableObj, string deleteType) where T : class
        {
            try
            {
                _db.Entry(tableObj).State = deleteType == "Logical" ? EntityState.Modified : EntityState.Deleted;
                await _db.SaveChangesAsync();
            }
            catch
            {
                return false;
            }
            return true;
        }
    }
}
