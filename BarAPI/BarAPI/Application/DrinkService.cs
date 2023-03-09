using Application.Interfaces;
using Domain;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    public class DrinkService : IDrinkService
    {
        private IDrinkRepository _drinkRepository;
        private IValidator<drink> validator;

        public DrinkService(IDrinkRepository drinkRepository, IValidator<drink> validator)
        {
            _drinkRepository = drinkRepository;
            this.validator = validator;
        }

        public void DespenseDrink(drink drink)
        {
            var validation = validator.Validate(drink);
            if (!validation.IsValid)
                throw new ValidationException(validation.ToString());

            _drinkRepository.DespenseDrink(drink);
        }
    }
}
