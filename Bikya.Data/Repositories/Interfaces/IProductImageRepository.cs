using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bikya.Data.Models;
namespace Bikya.Data.Repositories.Interfaces
{
    public interface IProductImageRepository:IRepository<ProductImage>
    {
      Task<IEnumerable<ProductImage>> GetByProductIdAsync(int productId);
    }
}
