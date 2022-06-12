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
    public interface IRepositoryService<T> where T : class, IEntityService
    {
        IDatabaseService Database { get; set; }

        /// <summary>
        /// Reserve to create an entity. Call SaveChanges to save any creating items.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        T Add<T>(T entity) where T : class, IEntityService;

        /// <summary>
        /// Reserve to create a array of entities. Call SaveChanges to save any creating items.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entities"></param>
        /// <returns></returns>
        List<T> Add<T>(params T[] entities) where T : class, IEntityService;

        T Find<T>(Guid Id) where T : class, IEntityService;

        T Find<T>(Expression<Func<T, bool>> where) where T : class, IEntityService;

        List<T> List<T>(Guid[] Ids) where T : class, IEntityService;

        List<T> List<T>(Expression<Func<T, bool>> where) where T : class, IEntityService;

        IQueryable<T> ListAsQueryable<T>(Expression<Func<T, bool>> where) where T : class, IEntityService;

        IQueryable<T> ListAsQueryable<T>(Guid[] Ids) where T : class, IEntityService;

        List<T> List<T>(int pageIndex, int pageSize) where T : class, IEntityService;

        List<T> List<T>(int pageIndex, int pageSize, Expression<Func<T, bool>> where) where T : class, IEntityService;

        List<T> List<T>(int pageIndex, int pageSize, Expression<Func<T, bool>> where, out int totalPage) where T : class, IEntityService;

        IQueryable<T> ListAsQueryable<T>(int pageIndex, int pageSize, Expression<Func<T, bool>> where) where T : class, IEntityService;

        /// <summary>
        /// Reserve to update a list of entities. Call SaveChanges to save any updating items.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entities"></param>
        /// <returns></returns>
        List<T> Update<T>(params T[] entities) where T : class, IEntityService;

        /// <summary>
        /// Reserve to delete entity. Call SaveChanges to save any deleting items.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool Delete<T>(T entity) where T : class, IEntityService;

        /// <summary>
        /// Reserve to delete list of entities. Call SaveChanges to save any deleting items.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entities"></param>
        /// <returns></returns>
        bool Delete<T>(T[] entities) where T : class, IEntityService;

        /// <summary>
        /// Reserve to delete entity. Call SaveChanges to save any deleting items.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Id"></param>
        /// <returns></returns>
        bool Delete<T>(Guid Id) where T : class, IEntityService;

        /// <summary>
        /// Reserve to delete any entity match with condition. Call SaveChanges to save any deleting items.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="where"></param>
        /// <returns></returns>
        bool Delete<T>(Expression<Func<T, bool>> where) where T : class, IEntityService;

        int CountWhere<T>(Expression<Func<T, bool>> @where) where T : class, IEntityService;

        int CountAll<T>() where T : class, IEntityService;

        IEnumerable<T> Find<T>(SpecificationBase<T> specification) where T : class, IEntityService;
        IEnumerable<T> Find<T>(int pageIndex, int pageSize, SpecificationBase<T> specification, out int totalPage) where T : class, IEntityService;

        /// <summary>
        /// Save all changes of any reservations
        /// </summary>
        /// <returns></returns>
        bool SaveChanges();
    }
}
