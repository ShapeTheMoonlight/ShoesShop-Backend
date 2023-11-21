﻿using FluentValidation;
using MediatR;
using ShoesShop.Application.Common.Exceptions;
using ShoesShop.Application.Common.Interfaces;
using ShoesShop.Application.Requests.Abstraction;
using ShoesShop.Entities;

namespace ShoesShop.Application.Requests.ModelsVariants.Commands
{
    public record UpdateModelVariantCommand : IRequest<Unit>
    {
        public Guid ModelVariantId { get; set; }
        public Guid? ModelId { get; set; }
        public Guid? ModelSizeId { get; set; }
        public int ItemsLeft { get; set; }
        public decimal Price { get; set; }
    }

    public class UpdateModelVariantCommandValidator : AbstractValidator<UpdateModelVariantCommand>
    {
        public UpdateModelVariantCommandValidator()
        {
            RuleFor(x => x.ModelVariantId).NotEqual(Guid.Empty);
            RuleFor(x => x.ModelId).NotEqual(Guid.Empty);
            RuleFor(x => x.ModelSizeId).NotEqual(Guid.Empty);
            RuleFor(x => x.ItemsLeft).GreaterThanOrEqualTo(0);
            RuleFor(x => x.Price).GreaterThan(0);
        }
    }

    public class UpdateModelVariantCommandHandler : AbstractCommandHandler<UpdateModelVariantCommand, Unit>
    {
        public UpdateModelVariantCommandHandler(IUnitOfWork unitOfWork) : base(unitOfWork) { }

        public override async Task<Unit> Handle(UpdateModelVariantCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var modelVariantRepository = UnitOfWork.GetRepositoryOf<ModelVariant>();
                var modelRepository = UnitOfWork.GetRepositoryOf<Model>();
                var modelSizeRepository = UnitOfWork.GetRepositoryOf<ModelSize>();
                var newModelVariant = new ModelVariant()
                {
                    ModelVariantId = request.ModelVariantId,
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
                if (request.ModelId is not null && request.ModelSizeId is not null)
                {
                    var sameModelVariants = await modelVariantRepository.FindAllAsync(x => x.ModelSizeId == request.ModelSizeId && x.ModelSizeId == request.ModelId, cancellationToken);
                    if (sameModelVariants.Any()) throw new AlreadyExistsException($"(Model:{request.ModelId}, Size:{request.ModelSizeId})", typeof(ModelVariant));
                }
                await modelVariantRepository.EditAsync(newModelVariant, cancellationToken);
                await UnitOfWork.SaveChangesAsync(cancellationToken);
                return Unit.Value;
            }
            catch (NotFoundException) { throw; }
        }
    }
}
