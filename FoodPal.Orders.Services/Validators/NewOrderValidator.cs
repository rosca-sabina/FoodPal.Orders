using FluentValidation;
using FoodPal.Orders.DTOs;

namespace FoodPal.Orders.Services.Validators
{
    public class NewOrderValidator: InternalValidator<NewOrderDTO>
    {
        public NewOrderValidator()
        {
            RuleFor(x => x.CustomerId).NotEmpty();
            RuleFor(x => x.CustomerName).NotEmpty();
            RuleFor(x => x.CustomerEmail).EmailAddress();
            RuleFor(x => x.Items).NotEmpty();

            RuleFor(x => x.DeliveryDetails).SetValidator(new DeliveryDetailsValidator());
        }
    }
}
