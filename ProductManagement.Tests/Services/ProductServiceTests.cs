using FakeItEasy;
using FluentAssertions;
using MockQueryable.FakeItEasy;
using ProductManagement.Api.Services;
using ProductManagement.Api.Models;
using ProductManagement.Api.DTOs;
using ProductManagement.Api.Repositories.Interfaces;
using AutoMapper;
using ProductManagement.Api.Utilities.Helper.AutoMapper;

namespace ProductManagement.Tests.Services
{
    public class ProductServiceTests
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public ProductServiceTests()
        {
            this._productRepository = A.Fake<IProductRepository>();
            this._mapper = A.Fake<IMapper>();
        }

        [Fact]
        public async Task ProductService_GetAllPaginationAsync_Returns_ProductPaginationResponseDto()
        {
            // Arrange
            var productPaginationRequestDto = new ProductPaginationRequestDto { PageNumber = 1, PageSize = 10 };

            var products = A.Fake<IEnumerable<Product>>();
            var totalCount = 10;
            
            var productDtos = A.Fake<IEnumerable<ProductDto>>();

            A.CallTo(() => _productRepository.GetAllPaginationAsync(A<ProductPaginationRequestDto>.Ignored)).Returns((products, totalCount));
            A.CallTo(() => _mapper.Map<IEnumerable<ProductDto>>(A<IEnumerable<Product>>.Ignored)).Returns(productDtos);

            var service = new ProductService(_productRepository, _mapper);

            // Act
            var result = await service.GetAllPaginationAsync(productPaginationRequestDto);

            // Assert
            result.Should().BeOfType<ProductPaginationResponseDto>();
            result.Products.Should().BeEquivalentTo(productDtos);
            result.TotalCount.Should().Be(totalCount);
            result.PageNumber.Should().Be(productPaginationRequestDto.PageNumber);
            result.PageSize.Should().Be(productPaginationRequestDto.PageSize);
        }

        [Fact]
        public async Task ProductService_GetAllPaginationAsync_Returns_Empty_If_No_Products_Found()
        {
            // Arrange
            var productPaginationRequestDto = new ProductPaginationRequestDto { PageNumber = 1, PageSize = 10 };

            var products = A.Fake<IEnumerable<Product>>();
            var totalCount = 0;

            var productDtos = new List<ProductDto>();

            A.CallTo(() => _productRepository.GetAllPaginationAsync(A<ProductPaginationRequestDto>.Ignored)).Returns((products, totalCount));
            A.CallTo(() => _mapper.Map<IEnumerable<ProductDto>>(A<IEnumerable<Product>>.Ignored)).Returns(productDtos);

            var service = new ProductService(_productRepository, _mapper);

            // Act
            var result = await service.GetAllPaginationAsync(productPaginationRequestDto);

            // Assert
            result.Should().BeOfType<ProductPaginationResponseDto>();
            result.Products.Should().BeEmpty();
            result.TotalCount.Should().Be(0);
            result.PageNumber.Should().Be(1);
            result.PageSize.Should().Be(10);
        }

        [Fact]
        public async Task ProductService_GetByIdAsync_Returns_ProductDto_If_Product_Found()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var product = A.Fake<Product>();
            product.Id = productId;
            
            var productDto = A.Fake<ProductDto>();
            productDto.Id = product.Id;

            A.CallTo(() => _productRepository.GetByIdAsync(productId)).Returns(product);
            A.CallTo(() => _mapper.Map<ProductDto>(product)).Returns(productDto);

            var service = new ProductService(_productRepository, _mapper);

            // Act
            var result = await service.GetByIdAsync(productId);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<ProductDto>();
            result.Id.Should().Be(productId);
        }

        [Fact]
        public async Task ProductService_GetByIdAsync_Returns_Null_If_Product_Not_Found()
        {
            // Arrange
            var productId = Guid.NewGuid();
            Product? product = null;

            A.CallTo(() => _productRepository.GetByIdAsync(productId)).Returns(product);

            var service = new ProductService(_productRepository, _mapper);

            // Act
            var result = await service.GetByIdAsync(productId);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task ProductService_CreateAsync_Returns_ProductDto()
        {
            // Arrange
            var productRequestDto = A.Fake<ProductRequestDto>();
            productRequestDto.Name = "Test Product";
            productRequestDto.Price = 100;
            productRequestDto.Stock = 10;

            var product = A.Fake<Product>();
            product.Id = Guid.NewGuid();
            product.Name = productRequestDto.Name;
            product.Price = productRequestDto.Price;
            product.Stock = productRequestDto.Stock;
            
            var productDto = A.Fake<ProductDto>();
            productDto.Name = product.Name;
            productDto.Price = product.Price;
            productDto.Stock = product.Stock;

            A.CallTo(() => _mapper.Map<Product>(productRequestDto)).Returns(product);
            A.CallTo(() => _productRepository.CreateAsync(product)).Returns(product);
            A.CallTo(() => _productRepository.GetByIdAsync(product.Id)).Returns(product);
            A.CallTo(() => _mapper.Map<ProductDto>(product)).Returns(productDto);

            var service = new ProductService(_productRepository, _mapper);

            // Act
            var result = await service.CreateAsync(productRequestDto);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<ProductDto>();
            result.Name.Should().Be(productRequestDto.Name);
            result.Price.Should().Be(productRequestDto.Price);
            result.Stock.Should().Be(productRequestDto.Stock);
        }

        [Fact]
        public async Task ProductService_DeleteAsync_Returns_True_If_Product_Deleted_Successfully()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var product = A.Fake<Product>();
            product.Id = productId;

            A.CallTo(() => _productRepository.GetByIdAsync(productId)).Returns(product);
            A.CallTo(() => _productRepository.DeleteAsync(productId)).Returns(Task.CompletedTask);

            var service = new ProductService(_productRepository, _mapper);

            // Act
            var result = await service.DeleteAsync(productId);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task ProductService_DeleteAsync_Returns_False_If_Product_Not_Found()
        {
            // Arrange
            var productId = Guid.NewGuid();
            Product? product = null;

            A.CallTo(() => _productRepository.GetByIdAsync(productId)).Returns(product);

            var service = new ProductService(_productRepository, _mapper);

            // Act
            var result = await service.DeleteAsync(productId);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task ProductService_UpdateAsync_Returns_True_If_Product_Updated_Successfully()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var productRequestDto = A.Fake<ProductRequestDto>();
            productRequestDto.Name = "Test Product";
            productRequestDto.Price = 100;
            productRequestDto.Stock = 10;

            var product = A.Fake<Product>();
            product.Id = productId;
            product.Name = productRequestDto.Name;
            product.Price = productRequestDto.Price;
            product.Stock = productRequestDto.Stock;

            A.CallTo(() => _mapper.Map(productRequestDto, product)).Returns(product);
            A.CallTo(() => _productRepository.GetByIdAsync(productId)).Returns(product);
            A.CallTo(() => _productRepository.UpdateAsync(product)).Returns(Task.CompletedTask);

            var service = new ProductService(_productRepository, _mapper);

            // Act
            var result = await service.UpdateAsync(productId, productRequestDto);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task ProductService_UpdateAsync_Returns_False_If_Product_Not_Found()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var productRequestDto = A.Fake<ProductRequestDto>();
            Product? product = null;

            A.CallTo(() => _productRepository.GetByIdAsync(productId)).Returns(product);

            var service = new ProductService(_productRepository, _mapper);

            // Act
            var result = await service.UpdateAsync(productId, productRequestDto);

            // Assert
            result.Should().BeFalse();
        }
    }
}