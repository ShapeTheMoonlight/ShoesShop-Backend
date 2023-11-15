﻿using MediatR;
using ShoesShop.Application.Exceptions;
using ShoesShop.Application.Interfaces;
using ShoesShop.Application.Requests.Base;
using ShoesShop.Entities;

namespace ShoesShop.Application.Requests.Commands
{
    public record UpdateModelVariantCommand : IRequest<Unit>
    {
        public Guid ModelVariantId { get; set; }
        public Guid? ModelId { get; set; }
        public Guid? ModelSizeId { get; set; }
        public int ItemsLeft { get; set; }
    }

    public class UpdateModelVariantCommandHandler : AbstractCommandHandler<UpdateModelVariantCommand, Unit>
    {
        public UpdateModelVariantCommandHandler(IUnitOfWork unitOfWork) : base(unitOfWork) { }

        public override async Task<Unit> Handle(UpdateModelVariantCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var modelVariantRepository = unitOfWork.GetRepositoryOf<ModelVariant>();
                var modelRepository = unitOfWork.GetRepositoryOf<Model>();
                var modelSizeRepository = unitOfWork.GetRepositoryOf<ModelSize>();
                var newModelVariant = new ModelVariant()
                {
                    Id = request.ModelVariantId,
                    ItemsLeft = request.ItemsLeft,
                };
                if (request.ModelId is not null)
                {
                    newModelVariant.Model = await modelRepository.GetAsync((Guid)request.ModelId, cancellationToken);
                }
                if (request.ModelSizeId is not null)
                {
                    newModelVariant.ModelSize = await modelSizeRepository.GetAsync((Guid)request.ModelSizeId, cancellationToken);
                }
                if(request.ModelId is not null && request.ModelSizeId is not null)
                {
                    var sameModelvariants = await modelVariantRepository.FindAllAsync(x => x.ModelSizeId == request.ModelSizeId && x.ModelSizeId == request.ModelId, cancellationToken);
                    if (sameModelvariants.Any()) throw new AlreadyExistsException($"(Model:{request.ModelId}, Size:{request.ModelSizeId})", typeof(ModelVariant));
                }
                await modelVariantRepository.EditAsync(newModelVariant, cancellationToken);
                await unitOfWork.SaveChangesAsync(cancellationToken);
                return Unit.Value;
            }
            catch (NotFoundException ex)
            {
                throw ex;
            }
        }
    }
}
