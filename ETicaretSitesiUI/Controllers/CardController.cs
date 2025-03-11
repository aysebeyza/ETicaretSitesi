using ETicaretDal.Abstract;
using ETicaretData.Entities;
using ETicaretData.Helpers;
using ETicaretData.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;

namespace ETicaretSitesiUI.Controllers
{
    public class CardController : Controller
    {

        private readonly IOrderDal _orderDal;
        private readonly IProductDal _productDal;

        public CardController(IOrderDal orderDal, IProductDal productDal)
        {
            _orderDal = orderDal;
            _productDal = productDal;
        }

        public IActionResult Index()
        {
            var card = SessionHelper.GetObjectFromJson<List<CardItem>>(HttpContext.Session, "Card");
            if(card==null)
            { return View(); }

            ViewBag.Total = card.Sum(x => x.Product.Price * x.Quantity).ToString("c");
            SessionHelper.Count = card.Count;
            return View(card);

        }
    
        public IActionResult Buy(int id)
        {
            if (SessionHelper.GetObjectFromJson<List<CardItem>>
                (HttpContext.Session, "Card") == null)
            {
                var cart = new List<CardItem>();
                cart.Add(new CardItem { Product = _productDal.Get(id), Quantity = 1 });
                SessionHelper.SetObjectAsJson(HttpContext.Session, "Card", cart);
            }

            else
            {
                var cart = SessionHelper.GetObjectFromJson<List<CardItem>>(HttpContext.Session, "Card");
                int index = iSExits(cart, id);   // ürünün olup olmadığını kontrol ediyoruz id üzerinden. ürünün ne olduğunu çıkarttık 

                if (index < 0)
                {
                    cart.Add(new CardItem
                    {
                        Product = _productDal.Get(id), 
                        Quantity = 1 
                    });
                }
                else
                {
                    cart[index].Quantity++;
                }
                SessionHelper.SetObjectAsJson(HttpContext.Session, "Card", cart);
             
                                                                           

            }
            return RedirectToAction("Index");

        }

        public IActionResult CheckOut()//Alışverişi tamamlaya tıkladıktan sonra kullanıcı bilgilerini isteyecek kısım
        {
            return View(new ShippingDetails());

        }

        [HttpPost]
        public IActionResult CheckOut(ShippingDetails detail)
        {
            var Cart = SessionHelper.GetObjectFromJson<List<CardItem>>(HttpContext.Session, "Card");
           if(Cart == null)
            {
                ModelState.AddModelError("Urun Yok", "Sepetinizde ürğn yok");

            }
            if (ModelState.IsValid)
            {
                SaveOrder(Cart, detail);
                Cart.Clear();
                SessionHelper.SetObjectAsJson(HttpContext.Session, "Card", Cart);
                //ürün kodunu otomatik oluşturup ,title ları diğer taraftaki title ile kaydetmeli

            }

            return View(detail);

        }

        private void SaveOrder(List<CardItem>? cart,ShippingDetails details)
        {
            var quint = new Guid().ToString("n");
            var order = new Order();
            order.OrderNumber = quint;
            //order.OrderNumber = "A" + (new Random()).Next(1111, 9999).ToString();
            order.OrderDate = DateTime.Now;
            order.orderState = EnumOrderState.Waiting;
            order.UserName=details.UserName;
            order.Address = details.Address;
            order.AddressTitle = details.AddressTitle;
            order.City = details.City;
            order.OrderLines = new List<OrderLine>();
            foreach(var item in cart)
            {
                var orderline = new OrderLine();
                orderline.Quantity = item.Quantity;
                orderline.Price = item.Quantity * item.Product.Price;
                orderline.ProductId = item.Product.Id;
               order.OrderLines.Add(orderline);

            }
            _orderDal.Add(order);

           
        }

        private int iSExits(List<CardItem> cart, int id) // bu metotta cart ve idyi paramterre olarak gönderdik sepet varsa card bilgilerini aldık kaçıncı ürün olduğunu hangi ürün olduğunu bulup döndürüyoruz.
        {
            for (int i = 0; i < cart.Count; i++)
            {

                if (cart[i].Product.Id.Equals(id))
                {
                    return i;
                }
            }
            return -1;
               
        }



    }
}
