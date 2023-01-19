using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BulkyBookWeb.Areas.Customer.Controllers
{
	[Area("Customer")]
	[Authorize]
	public class CartController : Controller
	{
		private readonly IUnitOfWork _unitOfWork;
		public ShoppingCartVM ShoppingCartVM { get; set; }
		public CartController(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		//[AllowAnonymous]
		public IActionResult Index()
		{
			var claimsIdentity = (ClaimsIdentity)User.Identity;
			var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
			ShoppingCartVM = new ShoppingCartVM()
			{
				ListCart = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == claim.Value, includeProperties:"Product")
			};
			foreach(var cart in ShoppingCartVM.ListCart)
			{
				cart.Price = GetPriceBasedOnQuantiity(cart.Count, cart.Product.Price, cart.Product.Price50, cart.Product.Price100);
				ShoppingCartVM.CartTotal += cart.Price * cart.Count;
			}
			return View(ShoppingCartVM);
		}
		public IActionResult Summary()
		{
			//var claimsIdentity = (ClaimsIdentity)User.Identity;
			//var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
			//ShoppingCartVM = new ShoppingCartVM()
			//{
			//	ListCart = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == claim.Value, includeProperties:"Product")
			//};
			//foreach(var cart in ShoppingCartVM.ListCart)
			//{
			//	cart.Price = GetPriceBasedOnQuantiity(cart.Count, cart.Product.Price, cart.Product.Price50, cart.Product.Price100);
			//	ShoppingCartVM.CartTotal += cart.Price * cart.Count;
			//}
			return View();
		}

		public double GetPriceBasedOnQuantiity(double quantity , double price , double price50 , double price100)
		{
			if (quantity <= 50)
			{
				return price;
			}
			else
			{
				if (quantity < 100)
				{
					return price50;
				}
				return price100;
			}
			
		}

		public IActionResult Plus(int CartId)
		{
			var Cart = _unitOfWork.ShoppingCart.GetFirstOrDefault(u=>u.Id == CartId);
			_unitOfWork.ShoppingCart.IncrementCount(Cart , 1);
			_unitOfWork.Save();
			return RedirectToAction(nameof(Index));
		}
		
		public IActionResult Minus(int CartId)
		{
			var Cart = _unitOfWork.ShoppingCart.GetFirstOrDefault(u=>u.Id == CartId);
			_unitOfWork.ShoppingCart.DecrementCount(Cart , 1);
			_unitOfWork.Save();
			return RedirectToAction(nameof(Index));
		}

		public IActionResult Remove(int CartId)
		{
			var Cart = _unitOfWork.ShoppingCart.GetFirstOrDefault(u=>u.Id == CartId);
			_unitOfWork.ShoppingCart.Remove(Cart);
			_unitOfWork.Save();
			return RedirectToAction(nameof(Index));
		}

	}
}
