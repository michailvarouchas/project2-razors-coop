using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project2_Cooperation.Models
{
    public class SessionCart : Cart
    {
        private static ISession _session;
        private const string _sessionKeyCart = "cart";

        public static Cart GetSessionCart(IServiceProvider services)
        {
            _session = services.GetRequiredService<IHttpContextAccessor>().HttpContext.Session;

            var returnData = _session.GetString(_sessionKeyCart);

            if (returnData == null)
            {
                return new SessionCart();
            }
            else
            {
                return JsonConvert.DeserializeObject<SessionCart>(returnData);
            }
        }

        public override void ClearCart()
        {
            base.ClearCart();
            _session.Remove(_sessionKeyCart);
        }

        public override void AddToCart(Product product, int quantity)
        {
            base.AddToCart(product, quantity);

            _session.SetString(_sessionKeyCart, JsonConvert.SerializeObject(this));
        }

        public override void RemoveFromCart(Product product, int quantity)
        {
            base.RemoveFromCart(product, quantity);

            _session.Remove(_sessionKeyCart);
            _session.SetString(_sessionKeyCart, JsonConvert.SerializeObject(this));
        }
    }
}
