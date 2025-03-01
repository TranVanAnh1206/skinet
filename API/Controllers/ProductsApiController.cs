
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Core.Specification;
using API.Dtos;
using AutoMapper;
using API.Errors;
using API.Helpers;

namespace API.Controllers
{
    public class ProductsApiController : BaseApiController
    {
        private readonly IGenericRepository<ProductBrand> _productBrandRepo;
        private readonly IGenericRepository<Product> _productRepo;
        private readonly IGenericRepository<ProductType> _productTypeRepo;
        private readonly IMapper _mapper;

        public ProductsApiController(IGenericRepository<Product> productRepo,
                                    IGenericRepository<ProductBrand> productBrandRepo,
                                    IGenericRepository<ProductType> productTypeRepo,
                                    IMapper mapper)
        {
            _productTypeRepo = productTypeRepo;
            _mapper = mapper;
            _productRepo = productRepo;
            _productBrandRepo = productBrandRepo;
        }

        [HttpGet]
        [Route("get-products")]
        public async Task<ActionResult<Pagination<ProductToReturnDto>>> GetProducts([FromQuery] ProductSpecParams productParams)
        {
            var spec = new ProductWithTypesAndBrandsSpecification(productParams);

            var countSpec = new ProductFilterWithCountSpecification(productParams);

            var totalItems = await _productRepo.CountAsync(countSpec);

            var entities = await _productRepo.ListAsync(spec);

            var data = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(entities);


            return Ok(new Pagination<ProductToReturnDto>(productParams.PageIndex, productParams.PageSize, totalItems, data));
        }

        [HttpGet]
        [Route("get-product/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
        {
            var spec = new ProductWithTypesAndBrandsSpecification(id);

            var entity = await _productRepo.GetEntityWithSpec(spec);

            if (entity == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<Product, ProductToReturnDto>(entity));
        }

        [HttpGet]
        [Route("get-product-brands")]
        public async Task<ActionResult<List<ProductBrand>>> GetProductBrands()
        {
            return Ok(await _productBrandRepo.GetAllAsync());
        }

        [HttpGet]
        [Route("get-product-types")]
        public async Task<ActionResult<List<ProductType>>> GetProductTypes()
        {
            return Ok(await _productTypeRepo.GetAllAsync());
        }
    }
}