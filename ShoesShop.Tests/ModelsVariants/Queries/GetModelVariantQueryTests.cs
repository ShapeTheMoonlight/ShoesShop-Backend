﻿using ShoesShop.Application.Common.Exceptions;
using ShoesShop.Application.Requests.Queries;
using ShoesShop.Application.Requests.Queries.OutputVMs;
using ShoesShop.Tests.Core;
using Shouldly;
using Xunit;

namespace ShoesShop.Tests.ModelsVariants.Queries
{
    public class GetModelVariantQueryTests : AbstractQueryTests
    {
        public GetModelVariantQueryTests(QueryFixture fixture) : base(fixture) { }

        [Fact]
        public async Task Should_ReturnModeVariant_WhenModelVariantExists()
        {
            // Arrange
            var query = new GetModelVariantQuery()
            {
                ModelVariantId = TestData.UpdateModelVariantId,
            };
            var handler = new GetModelVariantQueryHandler(UnitOfWork, Mapper);

            // Act
            var address = await handler.Handle(query, CancellationToken.None);

            // Assert
            address.ShouldBeOfType<ModelVariantVm>();
        }

        [Fact]
        public async Task Should_ThrowException_WhenModelVariantNotExists()
        {
            // Arrange
            var query = new GetModelVariantQuery()
            {
                ModelVariantId = Guid.NewGuid(),
            };
            var handler = new GetModelVariantQueryHandler(UnitOfWork, Mapper);

            // Act
            // Assert
            await Should.ThrowAsync<NotFoundException>(async () => await handler.Handle(query, CancellationToken.None));
        }
    }
}