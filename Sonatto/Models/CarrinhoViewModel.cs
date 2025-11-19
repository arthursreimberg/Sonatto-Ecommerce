using System.Collections.Generic;

namespace Sonatto.Models
{
    public class CarrinhoViewModel
    {
        public Carrinho? Carrinho { get; set; }
        public IEnumerable<ItemCarrinho>? Items { get; set; }
    }
}
