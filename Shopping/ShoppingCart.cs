using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using XPGroup.Models;

public static class ShoppingCart
{
    public static List<Product> Cart = new List<Product>();

    public static void Add(Product product)
    {
        Cart.Add(product);
    }
}