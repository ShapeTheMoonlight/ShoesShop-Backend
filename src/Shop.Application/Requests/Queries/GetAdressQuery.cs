﻿using AutoMapper;
using MediatR;
using ShoesShop.Application.Exceptions;
using ShoesShop.Application.Interfaces;
using ShoesShop.Application.Requests.Base;
using ShoesShop.Application.Requests.Queries.OutputVMs;
using ShoesShop.Entities;

namespace ShoesShop.Application.Requests.Queries
{
    public record GetAdressQuery : IRequest<AdressVm>
    {
        public Guid AdressId { get; set; }
    }

    public class GetAdressQueryHandler : AbstractQueryHandler, IRequestHandler<GetAdressQuery, AdressVm>
    {
        public GetAdressQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper) { }

        public async Task<AdressVm> Handle(GetAdressQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var adressRepository = unitOfWork.GetRepositoryOf<Adress>(true);
                var adress = await adressRepository.FindAllAsync(x => x.Id == request.AdressId, cancellationToken);
                return mapper.Map<AdressVm>(adress.First());
            }
            catch (NotFoundException ex)
            {
                throw ex;
            }
        }
    }
}