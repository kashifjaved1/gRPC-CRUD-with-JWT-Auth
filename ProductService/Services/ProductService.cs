using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using GrpcProductService.Protos;
using Microsoft.AspNetCore.Authorization;

namespace GrpcProductService.Services
{
    [Authorize]
    public class ProductService : ProductProtoService.ProductProtoServiceBase // ProductProtoService is the service, that we defined in product.proto file and by which this abstract ProductProtoServiceBase gets generated along with request and response objects we defined in proto file e.g. GetProductRequest, Empty, ProductListResponse, etc., and we'll be implementing ProductProtoServiceBase abstract class in real over here.
    {
        private static readonly List<Product> _products = new();

        public override Task<Product?> GetProduct(GetProductRequest request, ServerCallContext context)
        {
            var product = _products.FirstOrDefault(p => p.Id == request.Id);
            if (product == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, "Requested product not found."));
            }
            else
            {
                return Task.FromResult(product)!; // ! use to suppressing the nullability warning.
            }
        }

        public override Task<ProductListResponse> GetProducts(Empty request, ServerCallContext context)
        {
            var response = new ProductListResponse();
            response.Products.AddRange(_products);
            if (response.Products.Count == 0)
            {
                throw new RpcException(new Status(StatusCode.NotFound, "Requested product not found"));
            }

            return Task.FromResult(response);
        }

        public override Task<Product> CreateProduct(CreateProductRequest request, ServerCallContext context)
        {
            var response = new Product()
            {
                Id = _products.Count + 1,
                Name = request.Name,
                Description = request.Description,
                Price = request.Price
            };

            _products.Add(response);
            return Task.FromResult(response);
        }

        public override Task<Product> UpdateProduct(UpdateProductRequest request, ServerCallContext context)
        {
            var product = _products.FirstOrDefault(p => p.Id == request.Id);
            if (product == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, "Requested product not found"));
            }

            product.Name = request.Name;
            product.Description = request.Description;
            product.Price = request.Price;

            return Task.FromResult(product);
        }

        public override Task<Empty> DeleteProduct(DeleteProductRequest request, ServerCallContext context)
        {
            var product = _products.FirstOrDefault(p => p.Id == request.Id);
            if (product == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, "Requested product not found"));
            }

            _products.Remove(product);
            return Task.FromResult(new Empty());
        }

    }
}
