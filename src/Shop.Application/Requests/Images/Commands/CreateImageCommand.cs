﻿using MediatR;
using ShoesShop.Application.Common.Interfaces;
using ShoesShop.Application.Requests.Abstraction;
using ShoesShop.Entities;

namespace ShoesShop.Application.Requests.Images.Commands
{
    public record CreateImageCommand : IRequest<Guid>
    {
        public string Url { get; set; }
        public bool IsPreview { get; set; }
    }

    public class CreateImageCommandHandler : AbstractCommandHandler<CreateImageCommand, Guid>
    {
        public CreateImageCommandHandler(IUnitOfWork unitOfWork) : base(unitOfWork) { }

        public override async Task<Guid> Handle(CreateImageCommand request, CancellationToken cancellationToken)
        {
            var imageRepository = UnitOfWork.GetRepositoryOf<Image>();
            var imageToAdd = new Image()
            {
                Url = request.Url,
                IsPreview = request.IsPreview
            };

            await imageRepository.AddAsync(imageToAdd, cancellationToken);
            await UnitOfWork.SaveChangesAsync(cancellationToken);
            var createdImage = await imageRepository.FindAllAsync(x => x.Url == imageToAdd.Url
                                                                       && x.IsPreview == imageToAdd.IsPreview, cancellationToken);
            return createdImage.First().ImageId;
        }
    }
}
