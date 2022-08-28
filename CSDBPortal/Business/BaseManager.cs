using CSDBPortal.Data;
using Microsoft.EntityFrameworkCore;

namespace CSDBPortal.Business
{
    public class BaseManager
    {
        public bool CreateOrUpdateRecord<T>(T tableObj, string Mode)
        {
            try
            {
                using (ApplicationDbContext _db = new())
                {
                    if (Mode == "Add")
                    {
                        _db.Entry(tableObj).State = EntityState.Added;
                    }
                    else
                    {
                        _db.Entry(tableObj).State = EntityState.Modified;
                    }
                    _db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        public bool DeleteRecord<T>(T tableObj, string deleteType)
        {
            try
            {
                using (ApplicationDbContext _db = new())
                {
                    if (deleteType == "Logical")
                    {
                        _db.Entry(tableObj).State = EntityState.Modified;
                    }
                    else
                    {
                        _db.Entry(tableObj).State = EntityState.Deleted;
                    }
                    _db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;

        }
    }
}
