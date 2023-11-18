﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShoesShop.Application.Common.Exceptions;
using ShoesShop.Application.Requests.Images.Commands;
using ShoesShop.Application.Requests.Models.Commands;
using ShoesShop.Entities;
using ShoesShop.Tests.Core;
using Shouldly;
using Xunit;

namespace ShoesShop.Tests.Models.Commands
{
    public class CreateModelImageCommandTests : AbstractCommandTests
    {
        [Fact]
        public async void SHould_CreateImage_WhenModeExists()
        {
            // Arrange
            var imageToCreate = new Image()
            {
                ModelId = TestData.UpdateModelId,
                Url = "add test url",
                IsPreview = true,
            };
            var command = new CreateModelImageCommand()
            {
                ModelId = imageToCreate.ModelId,
                Url = imageToCreate.Url,
                IsPreview = imageToCreate.IsPreview,
            };
            var handler = new CreateModelImageCommandHandler(UnitOfWork);

            // Act
            var createdImageId = await handler.Handle(command, CancellationToken.None);

            // Assert
            DbContext.Images.SingleOrDefault(x => x.ImageId == createdImageId
                                                  && x.Url == imageToCreate.Url
                                                  && x.ModelId == x.ModelId
                                                  && x.IsPreview == imageToCreate.IsPreview).ShouldNotBeNull();
        }

        [Fact]
        public async Task Should_ThrowException_WhenModelNotExists()
        {
            // Arrange
            var imageToCreate = new Image()
            {
                ModelId = Guid.NewGuid(),
                Url = "add test url",
                IsPreview = true,
            };
            var command = new CreateModelImageCommand()
            {
                ModelId = imageToCreate.ModelId,
                Url = imageToCreate.Url,
                IsPreview = imageToCreate.IsPreview,
            };
            var handler = new CreateModelImageCommandHandler(UnitOfWork);
            // Act
            // Assert
            await Should.ThrowAsync<NotFoundException>(async () => await handler.Handle(command, CancellationToken.None));
        }
    }
}
