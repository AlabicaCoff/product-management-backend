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
    public class CategoryServiceTests
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryServiceTests()
        {
            this._categoryRepository = A.Fake<ICategoryRepository>();
            this._mapper = A.Fake<IMapper>();
        }

        [Fact]
        public async Task CategoryService_GetAllAsync_Returns_CategoriesDtoList()
        {
            // Arrange
            var categories = new List<Category>();
            var categoryDtos = new List<CategoryDto>();
            
            A.CallTo(() => _categoryRepository.GetAllAsync())
                .Returns(Task.FromResult<IEnumerable<Category>>(categories));
            A.CallTo(() => _mapper.Map<IEnumerable<CategoryDto>>(categories))
                .Returns(categoryDtos);
            var service = new CategoryService(_categoryRepository, _mapper);
            
            // Act
            var result = await service.GetAllAsync();
            
            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<IEnumerable<CategoryDto>>();
        }

        [Fact]
        public async Task CategoryService_GetByIdAsync_Returns_CategoryDto()
        {
            // Arrange
            var id = Guid.NewGuid();
            var category = A.Fake<Category>();
            category.Id = id;
            
            var categoryDto = A.Fake<CategoryDto>();
            categoryDto.Id = category.Id;
            
            A.CallTo(() => _categoryRepository.GetByIdAsync(id))
                .Returns(Task.FromResult<Category?>(category));
            A.CallTo(() => _mapper.Map<CategoryDto>(category))
                .Returns(categoryDto);
            var service = new CategoryService(_categoryRepository, _mapper);
            
            // Act
            var result = await service.GetByIdAsync(id);
            
            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<CategoryDto>();
            result.Id.Should().Be(id);
        }

        [Fact]
        public async Task CategoryService_GetByIdAsync_Returns_Null_WhenCategoryNotFound()
        {
            // Arrange
            var id = Guid.NewGuid();
            Category? category = null;
            
            A.CallTo(() => _categoryRepository.GetByIdAsync(id))
                .Returns(Task.FromResult<Category?>(category));
            var service = new CategoryService(_categoryRepository, _mapper);
            
            // Act
            var result = await service.GetByIdAsync(id);
            
            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task CategoryService_CreateAsync_Returns_CreatedCategoryDto()
        {
            // Arrange
            var categoryRequestDto = new CategoryRequestDto { Name = "Test"};
            var category = A.Fake<Category>();
            category.Name = categoryRequestDto.Name;
            var categoryDto = A.Fake<CategoryDto>();
            categoryDto.Name = category.Name;
            
            A.CallTo(() => _mapper.Map<Category>(categoryRequestDto))
                .Returns(category);
            A.CallTo(() => _categoryRepository.CreateAsync(category))
                .Returns(Task.FromResult<Category>(category));
            A.CallTo(() => _mapper.Map<CategoryDto>(category))
                .Returns(categoryDto);
            var service = new CategoryService(_categoryRepository, _mapper);
            
            // Act
            var result = await service.CreateAsync(categoryRequestDto);
            
            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<CategoryDto>();
            result.Name.Should().Be(categoryRequestDto.Name);
        }

        [Fact]
        public async Task CategoryService_UpdateAsync_Returns_True()
        {
            // Arrange
            var id = Guid.NewGuid();
            
            var categoryRequestDto = A.Fake<CategoryRequestDto>();
            categoryRequestDto.Name = "Test Category";

            var category = A.Fake<Category>();
            category.Id = id;
            category.Name = categoryRequestDto.Name;
            
            A.CallTo(() => _categoryRepository.GetByIdAsync(id))
                .Returns(Task.FromResult<Category?>(category));
            A.CallTo(() => _mapper.Map(categoryRequestDto, category))
                .Returns(category);
            A.CallTo(() => _categoryRepository.UpdateAsync(category))
                .Returns(Task.CompletedTask);
            var service = new CategoryService(_categoryRepository, _mapper);
            
            // Act
            var result = await service.UpdateAsync(id, categoryRequestDto);
            
            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task CategoryService_UpdateAsync_Returns_False_WhenCategoryNotFound()
        {
            // Arrange
            var id = Guid.NewGuid();
            var categoryRequestDto = A.Fake<CategoryRequestDto>();
            Category? category = null;
            
            A.CallTo(() => _categoryRepository.GetByIdAsync(id))
                .Returns(Task.FromResult<Category?>(category));

            var service = new CategoryService(_categoryRepository, _mapper);
            
            // Act
            var result = await service.UpdateAsync(id, categoryRequestDto);
            
            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task CategoryService_DeleteAsync_Returns_True()
        {
            // Arrange
            var id = Guid.NewGuid();
            var category = A.Fake<Category>();
            category.Id = id;
            
            A.CallTo(() => _categoryRepository.GetByIdAsync(id))
                .Returns(Task.FromResult<Category?>(category));
            A.CallTo(() => _categoryRepository.DeleteAsync(id))
                .Returns(Task.CompletedTask);
            var service = new CategoryService(_categoryRepository, _mapper);
            
            // Act
            var result = await service.DeleteAsync(id);
            
            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task CategoryService_DeleteAsync_Returns_False_WhenCategoryNotFound()
        {
            // Arrange
            var id = Guid.NewGuid();
            Category? category = null;
            
            A.CallTo(() => _categoryRepository.GetByIdAsync(id))
                .Returns(Task.FromResult<Category?>(category));

            var service = new CategoryService(_categoryRepository, _mapper);
            
            // Act
            var result = await service.DeleteAsync(id);
            
            // Assert
            result.Should().BeFalse();
        }
    }
}