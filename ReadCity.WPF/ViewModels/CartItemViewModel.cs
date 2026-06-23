using ReadCity.DAL.Models;

namespace ReadCity.WPF.ViewModels
{
    /// <summary>
    /// Presentation wrapper for a single <see cref="OrderItem"/> in the cart.
    /// </summary>
    public class CartItemViewModel
    {
        /// <summary>The underlying order item.</summary>
        public OrderItem Item { get; }

        /// <summary>Initializes the view-model.</summary>
        /// <param name="item">Source order item.</param>
        public CartItemViewModel(OrderItem item)
        {
            Item = item;
        }

        /// <summary>Quantity of this item.</summary>
        public int Quantity => Item.Quantity;

        /// <summary>The associated book.</summary>
        public Book? Book => Item.Book;

        /// <summary>Total price for this line (quantity × unit price).</summary>
        public decimal LineTotal => Item.Quantity * (Item.Book?.Price ?? 0);
    }
}
