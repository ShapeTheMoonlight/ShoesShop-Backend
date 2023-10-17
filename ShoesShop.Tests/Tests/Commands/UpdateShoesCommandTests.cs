﻿
using ShoesShop.Application.Exceptions;
using ShoesShop.Application.Interfaces;
using ShoesShop.Application.Requests.Commands;
using ShoesShop.Persistence.Repository;
using ShoesShop.Tests.Core;
using Shouldly;
using Xunit;

namespace ShoesShop.Tests.Tests.Commands
{
    public class UpdateShoesCommandTests : CommandTestAbstract
    {
        private readonly IShoesRepository shoesRepository;

        public UpdateShoesCommandTests() => shoesRepository = new ShoesRepository(dbContext);

        [Fact]
        public async Task Should_UpdateShoes_WhenShoesExists()
        {
            // Arrange 
            var newShoesName = "UpdateShoesName";
            var command = new UpdateShoesCommand()
            {
                ShoesId = ShoesShopTextContext.FullShoes,
                Name = newShoesName

            };
            var handler = new UpdateShoesNameCommandHandler(shoesRepository);

            // Act 
            await handler.Handle(command, CancellationToken.None);

            // Assert
            dbContext.Shoes.SingleOrDefault(x => x.Id == ShoesShopTextContext.FullShoes
                                                                && x.Name == newShoesName).ShouldNotBeNull();
        }

        [Fact]
        public async Task Should_ThrowException_WhenShoesNotExists()
        {
            // Arrange 
            var newShoesName = "UpdateShoesName";
            var command = new UpdateShoesCommand()
            {
                ShoesId = Guid.NewGuid(),
                Name = newShoesName
            };
            var handler = new UpdateShoesNameCommandHandler(shoesRepository);

            // Act 
            // Assert
            await Should.ThrowAsync<NotFoundException>(async () => await handler.Handle(command, CancellationToken.None));
        }
    }
}


