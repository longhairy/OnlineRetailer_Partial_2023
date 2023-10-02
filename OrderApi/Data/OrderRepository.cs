using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using OrderApi.Models;
using System;

namespace OrderApi.Data
{
    public class OrderRepository : IRepository<Order>
    {
        private readonly OrderApiContext db;

        public OrderRepository(OrderApiContext context)
        {
            db = context;
        }

         public async Task<Order> AddAsync(Order entity)
        {
            if (entity.Date == null)
                entity.Date = DateTime.Now;
            
            var newOrder = db.Orders.Add(entity).Entity;
            await db.SaveChangesAsync();
            return newOrder;
        }

        public async Task EditAsync(Order entity)
        {
            db.Entry(entity).State = EntityState.Modified;
            await db.SaveChangesAsync();
        }

        public async Task<Order> GetAsync(int id)
        {
            return await db.Orders.FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            return await db.Orders.ToListAsync();
        }

        public async Task RemoveAsync(int id)
        {
            var order = await db.Orders.FirstOrDefaultAsync(p => p.Id == id);
            db.Orders.Remove(order);
            await db.SaveChangesAsync();
        }

      
    }
}
