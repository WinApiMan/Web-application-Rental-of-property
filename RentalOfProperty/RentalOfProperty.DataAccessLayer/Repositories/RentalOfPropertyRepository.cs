// <copyright file="RentalOfPropertyRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RentalOfProperty.DataAccessLayer.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using RentalOfProperty.DataAccessLayer.Context;
    using RentalOfProperty.DataAccessLayer.Interfaces;

    /// <summary>
    /// Generic repository for data base.
    /// </summary>
    /// <typeparam name="TEntity">Entity type.</typeparam>
    public class RentalOfPropertyRepository<TEntity> : IRepository<TEntity>
        where TEntity : class
    {
        /// <summary>
        /// Db set.
        /// </summary>
        private readonly DbSet<TEntity> _dbSet;

        /// <summary>
        /// Db context.
        /// </summary>
        private RentalOfPropertyContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="RentalOfPropertyRepository{TEntity}"/> class.
        /// </summary>
        /// <param name="context">Db context.</param>
        public RentalOfPropertyRepository(RentalOfPropertyContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        /// <summary>
        /// Method for getting all items from table.
        /// </summary>
        /// <returns>All items.</returns>
        public async Task<IEnumerable<TEntity>> Get()
        {
            return await _dbSet.AsNoTracking()
                .ToListAsync();
        }

        /// <summary>
        /// Get using predicate.
        /// </summary>
        /// <param name="predicate">Predicate object.</param>
        /// <returns>Models list.</returns>
        public async Task<IEnumerable<TEntity>> Get(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbSet.AsNoTracking()
                .Where(predicate)
                .ToListAsync();
        }

        /// <summary>
        /// Add model.
        /// </summary>
        /// <param name="item">Adding model.</param>
        /// <returns>Task result.</returns>
        public async Task Create(TEntity item)
        {
            await _dbSet.AddAsync(item);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Update model.
        /// </summary>
        /// <param name="item">Updating model.</param>
        /// <returns>Task result.</returns>
        public async Task Update(TEntity item)
        {
            _context.Entry(item).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Remove model.
        /// </summary>
        /// <param name="item">Removing model.</param>
        /// <returns>Task result.</returns>
        public async Task Remove(TEntity item)
        {
            _dbSet.Remove(item);
            await _context.SaveChangesAsync();
        }
    }
}
