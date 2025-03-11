using ETicaretDal.Abstract;
using ETicaretData.Context;
using Microsoft.AspNetCore.Mvc;

namespace ETicaretUI.Controllers
{
    public class ProductController : Controller
    {
        private readonly ETicaretContext _contex;
        private readonly IProductDal _productDal;

        public ProductController(ETicaretContext contex, IProductDal productDal)
        {
            _contex = contex;
            _productDal = productDal;
        }

        public async Task<IActionResult> Index()
        {
            return View(_productDal.GetAll());
        }
    }
}
