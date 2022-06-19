using AppShareServices.DataAccess.Persistences;
using AppShareServices.Queries.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AppShareServices.DataAccess.Repository
{
    public class RepositoryService : IRepositoryService
    {
        public IDatabaseService Database { get; set; }

        public RepositoryService(IDatabaseService database)
        {
            Database = database;
        }

        public T Add<T>(T entity) where T : class, IEntityService
        {
            try
            {
                var dbset = Database.GetDbSet<T>();
                entity.DateCreated = DateTime.UtcNow;
                dbset.Add(entity);
            }
            catch (Exception)
            {
                throw;
            }

            return entity;
        }

        public List<T> Add<T>(params T[] entities) where T : class, IEntityService
        {
            try
            {
                var dbset = Database.GetDbSet<T>();
                foreach (var entity in entities)
                {
                    entity.DateCreated = DateTime.UtcNow;
                    dbset.Add(entity);
                }
            }
            catch (Exception)
            {
                throw;
            }

            return entities.ToList();
        }

        public T Find<T>(int Id) where T : class, IEntityService
        {
            return Database.GetDbSet<T>().FirstOrDefault(x => x.Id.Equals(Id));
        }

        public T Find<T>(Expression<Func<T, bool>> @where) where T : class, IEntityService
        {
            var dbset = Database.GetDbSet<T>();
            return dbset.FirstOrDefault(@where);
        }

        public List<T> List<T>(int[] Ids) where T : class, IEntityService
        {
            return Database.GetDbSet<T>().Where(x => Ids.Contains(x.Id)).ToList();
        }

        public List<T> List<T>(Expression<Func<T, bool>> @where) where T : class, IEntityService
        {
            return Database.GetDbSet<T>().Where(@where).ToList();
        }

        public IQueryable<T> ListAsQueryable<T>(Expression<Func<T, bool>> @where) where T : class, IEntityService
        {
            var dbset = Database.GetDbSet<T>();
            if (@where == null)
                return dbset;

            return dbset.Where(@where);
        }

        public IQueryable<T> ListAsQueryable<T>(int[] Ids) where T : class, IEntityService
        {
            return Database.GetDbSet<T>().Where(x => Ids.Contains(x.Id));
        }

        public List<T> List<T>(int pageIndex, int pageSize) where T : class, IEntityService
        {
            var dbset = Database.GetDbSet<T>();
            return dbset.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
        }

        public List<T> List<T>(int pageIndex, int pageSize, Expression<Func<T, bool>> @where) where T : class, IEntityService
        {
            return Database.GetDbSet<T>().Where(@where).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
        }

        public List<T> List<T>(int pageIndex, int pageSize, Expression<Func<T, bool>> @where, out int totalPage) where T : class, IEntityService
        {
            var query = Database.GetDbSet<T>().Where(@where);
            totalPage = query.Count();

            return query.OrderByDescending(x => x.DateCreated).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
        }

        public IQueryable<T> ListAsQueryable<T>(int pageIndex, int pageSize, Expression<Func<T, bool>> @where) where T : class, IEntityService
        {
            return Database.GetDbSet<T>().Where(@where).Skip((pageIndex - 1) * pageSize).Take(pageSize);
        }

        public List<T> Update<T>(params T[] entities) where T : class, IEntityService
        {
            try
            {
                var dbset = Database.GetDbSet<T>();
                foreach (var entity in entities)
                {
                    var item = dbset.FirstOrDefault(x => x.Id == entity.Id);
                    if (item != null)
                    {
                        // mapping entity vs item
                        item.DateModified = DateTime.Now.ToUniversalTime();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return entities.ToList();
        }

        public bool Delete<T>(T entity) where T : class, IEntityService
        {
            try
            {
                var dbset = Database.GetDbSet<T>();
                var item = dbset.FirstOrDefault(x => x.Id == entity.Id);
                if (item != null)
                {
                    dbset.Remove(item);
                }
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool Delete<T>(T[] entities) where T : class, IEntityService
        {
            try
            {
                var dbset = Database.GetDbSet<T>();
                foreach (var entity in entities)
                {
                    dbset.Remove(entity);
                }
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool Delete<T>(int Id) where T : class, IEntityService
        {
            try
            {
                var dbset = Database.GetDbSet<T>();
                var item = dbset.FirstOrDefault(x => x.Id == Id);
                if (item != null)
                {
                    dbset.Remove(item);
                }
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool Delete<T>(Expression<Func<T, bool>> @where) where T : class, IEntityService
        {
            try
            {
                var dbset = Database.GetDbSet<T>();
                var items = dbset.Where(@where);
                if (items != null && items.Count() > 0)
                {
                    dbset.RemoveRange(items);
                }
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int CountWhere<T>(Expression<Func<T, bool>> @where) where T : class, IEntityService
        {
            return Database.GetDbSet<T>().Count(@where);
        }

        public int CountAll<T>() where T : class, IEntityService
        {
            return Database.GetDbSet<T>().Count();
        }

        public IEnumerable<T> Find<T>(SpecificationBase<T> specification) where T : class, IEntityService
        {
            return Database.GetDbSet<T>().AsQueryable().Where(specification.Criteria);
        }

        public IEnumerable<T> Find<T>(int pageIndex, int pageSize, SpecificationBase<T> specification, out int totalPage) where T : class, IEntityService
        {
            var query = Database.GetDbSet<T>().Where(specification.Criteria);
            totalPage = query.Count();

            return query.OrderByDescending(x => x.DateCreated).Skip((pageIndex - 1) * pageSize).Take(pageSize).AsEnumerable();
        }


        public bool SaveChanges()
        {
            try
            {
                var result = Task.FromResult(Database.SaveChanges());
                return result.Result.IsCompletedSuccessfully;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
