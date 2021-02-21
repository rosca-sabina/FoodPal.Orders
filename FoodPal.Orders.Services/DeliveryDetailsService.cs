using AutoMapper;
using FoodPal.Orders.Data;
using FoodPal.Orders.DTOs;
using FoodPal.Orders.Enums;
using FoodPal.Orders.Exceptions;
using FoodPal.Orders.Models;
using FoodPal.Orders.Services.Contracts;
using System;
using System.Threading.Tasks;

namespace FoodPal.Orders.Services
{
    public class DeliveryDetailsService : BaseService, IDeliveryDetailsService
    {
        private readonly IOrdersUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public DeliveryDetailsService(IOrdersUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<DeliveryDetailsDTO> GetOrderDeliveryDetailsAsync(int orderId)
        {
            ParameterChecks(new (Func<bool>, Exception)[]
            {
                ( () => orderId > 0, new ArgumentOutOfRangeException(nameof(orderId), $"{nameof(orderId)} must be positive."))
            });

            var deliveryDetails = await _unitOfWork.DeliveryDetailsRepository.GetOrderDeliveryDetailsAsync(orderId);
            if (deliveryDetails == null)
            {
                throw new FoodPalNotFoundException(new ErrorInfoDTO
                {
                    InfoType = ErrorInfoType.Error,
                    Message = $"No delivery details were found for order {orderId}."
                });
            }

            return _mapper.Map<DeliveryDetails, DeliveryDetailsDTO>(deliveryDetails);
        }
    }
}
