using Api.Application.Common;
using Api.Domain.Entities;
using MediatR;

namespace Api.Application.Products.Command.Create
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDateTimeProvider _dateTimeProvider;

        public CreateProductCommandHandler(IUnitOfWork unitOfWork, IDateTimeProvider dateTimeProvider)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _dateTimeProvider = dateTimeProvider ?? throw new ArgumentNullException(nameof(dateTimeProvider));
        }

        public async Task<int> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            // Begin the transaction here
            _unitOfWork.BeginTransaction();

            try
            {
                // Create a new product instance
                var product = new Product
                {
                    Name = request.Name,
                    Price = request.Price,
                    Stock = request.Stock,
                    CreatedAt = _dateTimeProvider.UtcNow,
                    UpdatedAt = _dateTimeProvider.UtcNow,
                };

                var productRepo = _unitOfWork.GetRepository<Product>();

                // Add the product to the repository
                int productId = await productRepo.AddAsync(product);

                // Commit the transaction
                await _unitOfWork.CompleteAsync();

                // Return the ID of the newly created product
                return productId;
            }
            catch (Exception ex)
            {
                // Rollback the transaction if an error occurs
                _unitOfWork.Dispose(); // Dispose will rollback the transaction
                                       // Log the exception (use your preferred logging framework)
                                       // e.g., _logger.LogError(ex, "Error creating product: {Product}", product);
                throw new Exception("An error occurred while creating the product.", ex);
            }
        }
    }
}
