using BuildingBlocks.Exceptions;

namespace Catalog.API.Exceptions
{
    public class ProductNotFoundException(Guid guid) : NotFoundException("Product", guid)
    {
    }
}
