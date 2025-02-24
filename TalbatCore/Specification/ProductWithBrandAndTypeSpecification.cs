using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Specification
{
    public class ProductWithBrandAndTypeSpecification : BaseSpecification<Product>
    {
        public ProductWithBrandAndTypeSpecification(ProductSpecParams productParams)
            :base(p=>
            (string.IsNullOrEmpty(productParams.Search)||p.Name.ToLower().Contains(productParams.Search))&&
            (!productParams.BrandId.HasValue || p.ProductBrandId==productParams.BrandId.Value)&&
            (!productParams.TypeId.HasValue  || p.ProductTypeId==productParams.TypeId.Value)
            )
        {
            Includes.Add(P => P.ProductBrand);
            Includes.Add(P => P.ProductType);

            
            ApplyPagination(productParams.PageSize*(productParams.PageIndex-1),productParams.PageSize);
            if(!string.IsNullOrEmpty(productParams.Sort))
            {
                switch (productParams.Sort)
                {
                    case "priceAsc":
                        AddOrderBy(p => p.Price);
                        break;
                    case "priceDesc":
                        AddOrderByDescending(p => p.Price);
                        break;

                    default:
                        AddOrderBy(p => p.Name);
                        break;
                }
            }

        }
        public ProductWithBrandAndTypeSpecification(int id) : base(P => P.Id == id)
        {
            Includes.Add(P => P.ProductBrand);
            Includes.Add(P => P.ProductType);
        }
    }
}
