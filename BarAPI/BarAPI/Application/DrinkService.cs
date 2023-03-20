using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Domain;
using FluentValidation;
using System.Globalization;

namespace Application
{
    public class DrinkService : IDrinkService
    {
        private IDrinkRepository _drinkRepository;
        private IValidator<DrinkDTO> _validator;
        private IMapper _mapper;

        public DrinkService(IDrinkRepository drinkRepository, IValidator<DrinkDTO> validator, IMapper mapper)
        {
            _drinkRepository = drinkRepository;
            _validator = validator;
            _mapper = mapper;
        }

        public void DespenseDrink(DrinkDTO drinkDTO)
        {
            var validation = _validator.Validate(drinkDTO);
            if (!validation.IsValid)
            {
                throw new ValidationException(validation.ToString());
            }
            drink drinkMap = _mapper.Map<drink>(drinkDTO);
            drinkMap.AlcCL = CalculateDrink(drinkMap.AlcCL);
            if (drinkMap.SodaCL > 0)
            {
                drinkMap.SodaCL = CalculateDrink(drinkMap.SodaCL);
            }
            Console.WriteLine(drinkMap.AlcCL.ToString(CultureInfo.GetCultureInfo("en-GB")));
            Console.WriteLine(drinkMap.SodaCL.ToString(CultureInfo.GetCultureInfo("en-GB")));
            _drinkRepository.DespenseDrink(drinkMap);
        }
        
        public double CalculateDrink(double x)
        {
            double y = 0.445 * x + 0.0667;
            return y;
        }
    }
}
