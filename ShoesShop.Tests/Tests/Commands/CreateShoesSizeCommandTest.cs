﻿using ShoesShop.Application.Exceptions;
using ShoesShop.Application.Interfaces;
using ShoesShop.Application.Requests.Commands;
using ShoesShop.Entities;
using ShoesShop.Persistence.Repository;
using ShoesShop.Tests.Core;
using Shouldly;
using Xunit;

namespace ShoesShop.Tests.Tests.Commands
{
    public class CreateShoesSizeCommandTest : CommandTestAbstract
    {
        public readonly IShoesSizeRepository shoesSizeRepository;

        public CreateShoesSizeCommandTest() => shoesSizeRepository = new ShoesSizeRepository(dbContext);

        [Fact]
        public async Task Should_CreateShoesSize_WhenShoesSizeNotExists()
        {
            // Arrange
            var shoesSizeToCreate = new ShoesSize()
            {
                Size = 1,
                Price = 1,
                ItemsLeft = 1,
            };
            var command = new CreateShoesSizeCommand()
            {
                ShoesId = ShoesShopTextContext.EmptyShoes,
                Size = shoesSizeToCreate.Size,
                Price = shoesSizeToCreate.Price,
                ItemsLeft = shoesSizeToCreate.ItemsLeft,
            };
            var handler = new CreateShoesSizeCommandHandler(shoesSizeRepository);

            // Act
            var shoesSizeId = await handler.Handle(command, CancellationToken.None);

            // Assert
            dbContext.Sizes.SingleOrDefault(x => x.Id == shoesSizeId
                                              && x.ShoesId == ShoesShopTextContext.EmptyShoes
                                              && x.Size == shoesSizeToCreate.Size
                                              && x.Price == shoesSizeToCreate.Price
                                              && x.ItemsLeft == shoesSizeToCreate.ItemsLeft).ShouldNotBeNull();
        }

        [Fact]
        public async Task Should_ThrowException_WhenShoesNotExists()
        {
            // Arrange
            var shoesSizeToCreate = new ShoesSize()
            {
                Size = 1,
                Price = 1,
                ItemsLeft = 1,
            };
            var command = new CreateShoesSizeCommand()
            {
                ShoesId = Guid.NewGuid(),
                Size = shoesSizeToCreate.Size,
                Price = shoesSizeToCreate.Price,
                ItemsLeft = shoesSizeToCreate.ItemsLeft,
            };
            var handler = new CreateShoesSizeCommandHandler(shoesSizeRepository);

            // Act
            // Assert
            await Should.ThrowAsync<NotFoundException>(async () => await handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Should_ThrowException_WhenSameSizeAlreadyExists()
        {
            // Arrange
            var shoesSizeToCreate = new ShoesSize()
            {
                Size = ShoesShopTextContext.ExistedSize,
                Price = 1,
                ItemsLeft = 1,
            };
            var command = new CreateShoesSizeCommand()
            {
                ShoesId = ShoesShopTextContext.FullShoes,
                Size = shoesSizeToCreate.Size,
                Price = shoesSizeToCreate.Price,
                ItemsLeft = shoesSizeToCreate.ItemsLeft,
            };
            var handler = new CreateShoesSizeCommandHandler(shoesSizeRepository);

            // Act
            // Assert
            await Should.ThrowAsync<AlreadyExistsException>(async () => await handler.Handle(command, CancellationToken.None));
        }
    }
}
