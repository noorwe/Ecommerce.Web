using AutoMapper;
using Ecommerce.Domain.Contracts;
using Ecommerce.Domain.Entities.ProductModule;
using Ecommerce.Domain.Exceptions;
using Ecommerce.Domain.OrderModule;
using Ecommerce.Service.Specification.OrderModuleSpecifications;
using Ecommerce.ServiceAbstractions;
using Ecommerce.Shared.DTOS.IdentityDtos;
using Ecommerce.Shared.DTOS.OrderDtos;

namespace Ecommerce.Service
{
    internal class OrderService : IOrderService
    {
        private readonly IMapper _mapper;
        private readonly IBasketRepository _basketRepository;
        private readonly IUnitOfWork _unitOfWork;

        public OrderService(
            IMapper mapper,
            IBasketRepository basketRepository,
            IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _basketRepository = basketRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<OrderToReturnDto> CreateOrderAsync(OrderDto orderDto, string email)
        {
            // Map Address
            var orderAddress = _mapper.Map<AddressDto, OrderAddress>(orderDto.Address);

            // Get Basket
            var basket = await _basketRepository.GetBasketAsync(orderDto.BasketId)
                ?? throw new BasketNotFoundException(orderDto.BasketId);

            ArgumentNullException.ThrowIfNull(basket.PaymentIntentId);

            var orderRepo = _unitOfWork.GetRepository<Order, Guid>();

            var existingOrder = await orderRepo.GetByIdAsync(
                new OrderWithPaymentIntentIdSpecification(basket.PaymentIntentId)
            );
            if (existingOrder is not null)
            {
                orderRepo.Remove(existingOrder);
            }

            // Create Order Items
            var orderItems = new List<OrderItem>();
            var productRepo = _unitOfWork.GetRepository<Product, int>();

            foreach (var basketItem in basket.Items)
            {
                var product = await productRepo.GetByIdAsync(basketItem.Id)
                    ?? throw new ProductNotFoundException(basketItem.Id);

                var orderItem = new OrderItem
                {
                    Product = new ProductItemOrdered
                    {
                        ProductId = product.Id,
                        ProductName = product.Name,
                        PictureUrl = product.PictureUrl
                    },
                    Price = product.Price,
                    Quantity = basketItem.Quantity
                };

                orderItems.Add(orderItem);
            }

            // Get Delivery Method
            var deliveryMethod = await _unitOfWork
                .GetRepository<DeliveryMethod, int>()
                .GetByIdAsync(orderDto.DeliveryMethodId)
                ?? throw new DeliveryMethodNotFoundException(orderDto.DeliveryMethodId);

            // Calculate Subtotal
            var subtotal = orderItems.Sum(i => i.Price * i.Quantity);

            // Create Order
            var order = new Order(
                email,
                orderAddress,
                deliveryMethod,
                deliveryMethod.Id,
                subtotal,
                orderItems,
                basket.PaymentIntentId

            );

            await orderRepo.AddAsync(order);
            await _unitOfWork.SaveChangesAsync();

            // Map To DTO & Return
            return _mapper.Map<OrderToReturnDto>(order);
        }

        public async Task<IEnumerable<DeliveryMethodDto>> GetDeliveryMethodsAsync()
        {
            var deliveryMethods = await _unitOfWork
                .GetRepository<DeliveryMethod, int>()
                .GetAllAsync();
            return _mapper.Map<IEnumerable<DeliveryMethodDto>>(deliveryMethods);
        }

        public async Task<OrderToReturnDto> GetOrderAsync(Guid id)
        {
            var spec = new OrderSpecification(id);
            var order = await _unitOfWork
               .GetRepository<Order, Guid>().GetByIdAsync(spec);
            return _mapper.Map<OrderToReturnDto>(order);

        }

        public async Task<IEnumerable<OrderToReturnDto>> GetOrdersForUserAsync(string email)
        {
            var spec = new OrderSpecification(email);
            var orders = await _unitOfWork
                .GetRepository<Order, Guid>().GetAllAsync(spec);
            return _mapper.Map<IEnumerable<OrderToReturnDto>>(orders);
        }
    }
}
