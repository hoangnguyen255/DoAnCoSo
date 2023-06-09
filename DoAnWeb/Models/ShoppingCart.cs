using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoAnCoSo.Models
{
    public class ShoppingCart
    {
        public List<ShoppingCartItem> items { get; set; }
        public List<ShoppingCartTableItem> itemstable { get; set; }
        public ShoppingCart()
        {
            this.items = new List<ShoppingCartItem>();
            this.itemstable = new List<ShoppingCartTableItem>();
        }
        public void AddToCart(ShoppingCartItem item, int Quantity)
        {
            var checkExits = items.FirstOrDefault(x => x.ProductId == item.ProductId);
            if(checkExits != null)
            {
                checkExits.Quantity += Quantity;
                checkExits.TotalPrice = checkExits.Price * checkExits.Quantity;
            }
            else
            {
                items.Add(item);
            }
        }

        public void AddTableToCart(ShoppingCartTableItem item, int Quantity)
        {
            var checkExits = itemstable.FirstOrDefault(x => x.TableId == item.TableId);
            if (checkExits != null)
            {
                checkExits.Quantity += Quantity;
            }
            else
            {
                itemstable.Add(item);
            }
        }
        public void Remove(int id)
        {
            var checkExits = items.SingleOrDefault(x => x.ProductId == id);
            if (checkExits != null)
            {
                items.Remove(checkExits);
            }
        }
        public void RemoveTable(int id)
        {
            var checkExitsTable = itemstable.SingleOrDefault(x => x.TableId == id);
            if (checkExitsTable != null)
            {
                itemstable.Remove(checkExitsTable);
            }
        }
        public void UpdateQuantity(int id, int quantity)
        {
            var checkExits = items.SingleOrDefault(x => x.ProductId == id);
            if (checkExits != null)
            {
                checkExits.Quantity = quantity;
                checkExits.TotalPrice = checkExits.Price * checkExits.Quantity;
            }

            var checkExitsTable = itemstable.SingleOrDefault(x => x.TableId == id);
            if (checkExitsTable != null)
            {
                checkExitsTable.Quantity = quantity;
            }
        }
        public decimal GetTotalPrice()
        {
            return items.Sum(x => x.TotalPrice);
        }
        public int GetTotalQuantity()
        {
            return items.Sum(x => x.Quantity);
        }
        public void clearCart()
        {
            items.Clear();
            itemstable.Clear();
        }
    }
    public class ShoppingCartItem 
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string CategoryName { get; set; }
        public string Alias { get; set; }
        public string ProductImg { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal TotalPrice { get; set; }
    }

    public class ShoppingCartNew
    {
        public List<ShoppingCartTableItem> Tables { get; set; }
        public decimal Total { get; set; }
    }

    public class ShoppingCartTableItem
    {
        public int TableId { get; set; }
        public string TableName { get; set; }
        public string SpaceName { get; set; }
        public string Alias { get; set; }
        public string TableImg { get; set; }
        public int Quantity { get; set; }
    }
}