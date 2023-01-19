using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAccess.Repository
{
    public class ShoppingCartRepository : Repository<ShoppingCart>, IShoppingCartRepository
    {
        private ApplicationDbContext _db;

        public ShoppingCartRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

		public int DecrementCount(ShoppingCart shoppingcart, int count)
		{
			shoppingcart.Count -= count;
			return shoppingcart.Count;
		}

		public int IncrementCount(ShoppingCart shoppingcart, int count)
		{
			shoppingcart.Count += count;
			return shoppingcart.Count;
		}
	}
}
