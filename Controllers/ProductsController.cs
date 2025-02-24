using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Talabat.APIs.Dtos;
using Talabat.APIs.Errors;
using Talabat.APIs.Helpers;
using Talabat.Core.Entities;
using Talabat.Core.Repositories;
using Talabat.Core.Specification;

namespace Talabat.APIs.Controllers
{

    public class ProductsController : BaseApiController
    {
        
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public ProductsController(IMapper mapper
            ,IUnitOfWork unitOfWork)
        {
            
            _mapper = mapper;
            _unitOfWork = unitOfWork;
           
        }
        [CachedAttribute(600)]
        [HttpGet]

        public async Task<ActionResult<Pagination<ProductToReturnDto>>> GetProducts([FromQuery] ProductSpecParams productParams)
        {
            var spec = new ProductWithBrandAndTypeSpecification(productParams);
            var products = await _unitOfWork.Repository<Product>().GetAllWithSpecAsync(spec);
            var Data = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products);
            var countSpec = new ProductWithFiltersForCountSpecification(productParams);
            var count = await _unitOfWork.Repository<Product>().GetCountAsync(countSpec);
            return Ok(new Pagination<ProductToReturnDto>(productParams.PageIndex,productParams.PageSize,count,Data));
        }
        [CachedAttribute(600)]
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductToReturnDto>> GetProductsById(int id)
        {
            var spec=new ProductWithBrandAndTypeSpecification(id);
            var product = await _unitOfWork.Repository<Product>().GetByIdWithSpecAsync(spec);
            if (product == null) return NotFound(new ApiResponse(404));
            return Ok(_mapper.Map<Product,ProductToReturnDto>(product));
        }
        [CachedAttribute(600)]
        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetBrands()
        {
            var brands = await _unitOfWork.Repository<ProductBrand>().GetAllAsync();
            return Ok(brands);
        }
        [CachedAttribute(600)]
        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetTypes()
        {
            var types=await _unitOfWork.Repository<ProductType>().GetAllAsync();
            return Ok(types);
        }
    }
}
