﻿using MediatR;
using ShoesShop.Application.Exceptions;
using ShoesShop.Application.Interfaces;
using ShoesShop.Application.Requests.Abstraction;
using ShoesShop.Application.Requests.Queries.OutputVMs;
using ShoesShop.Entities;

namespace ShoesShop.Application.Requests.Commands
{
    public record DeleteAddressCommand : IRequest<Unit>
    {
        public Guid AddressId { get; set; }
    }

    public class DeleteAddressCommandHandler : AbstractCommandHandler<DeleteAddressCommand, Unit>
    {
        public DeleteAddressCommandHandler(IUnitOfWork unitOfWork) : base(unitOfWork) { }

        public override async Task<Unit> Handle(DeleteAddressCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var addressRepository = UnitOfWork.GetRepositoryOf<Address>(true);
                await addressRepository.RemoveAsync(request.AddressId, cancellationToken);
                await UnitOfWork.SaveChangesAsync(cancellationToken);
                return Unit.Value;
            }
            catch (NotFoundException ex)
            {
                throw ex;
            }
        }
    }
}
