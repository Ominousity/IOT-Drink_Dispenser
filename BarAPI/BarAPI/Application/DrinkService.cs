using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
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
                throw new ValidationException(validation.ToString());
            drink drinkMap = _mapper.Map<drink>(drinkDTO);
            _drinkRepository.DespenseDrink(drinkMap);
        }
    }
}
