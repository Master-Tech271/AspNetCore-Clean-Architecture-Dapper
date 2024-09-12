using MediatR;

namespace Api.Application.Products.Command.Create
{
    public class CreateProductCommand : IRequest<int>
    {
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Stock { get; set; }
    }
}
