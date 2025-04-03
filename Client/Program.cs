using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Grpc.Net.Client;
using GrpcProductService.Protos;

var channel = GrpcChannel.ForAddress("https://localhost:5000");
var authClienet = new AuthProtoService.AuthProtoServiceClient(channel);

var token = await authClienet.GenerateTokenAsync(new Empty());
var headers = new Metadata()
{
    { "Authorization", $"Bearer {token.Token}" }
};

var serviceClient = new ProductProtoService.ProductProtoServiceClient(channel);

// Create Product
Console.WriteLine("------------------------------- Create -------------------------------\n");
var product = await serviceClient.CreateProductAsync(new CreateProductRequest
{
    Name = "Product 1",
    Description = "Product 1 description.",
    Price = 100
}, headers);

Console.WriteLine(product);

product = await serviceClient.CreateProductAsync(new CreateProductRequest
{
    Name = "Product 2",
    Description = "Product 2 description.",
    Price = 200
}, headers);

Console.WriteLine(product);
Console.WriteLine("\n------------------------------- Create -------------------------------\n");

// Update Product
Console.WriteLine("------------------------------- Update -------------------------------\n");
product = await serviceClient.UpdateProductAsync(new UpdateProductRequest
{
    Id = 2,
    Name = "Product 2",
    Description = "Product 2 updated description.",
    Price = 200
}, headers);

Console.WriteLine(product);
Console.WriteLine("\n------------------------------- Update -------------------------------\n");

// Get All
Console.WriteLine("------------------------------- Get All Products -------------------------------\n");
var list = await serviceClient.GetProductsAsync(new Empty(), headers);
foreach (var item in list.Products)
{
    Console.WriteLine(item);
}

Console.WriteLine("\n------------------------------- Get All Products -------------------------------\n");

// Delete Product
Console.WriteLine("------------------------------- Delete -------------------------------\n");
await serviceClient.DeleteProductAsync(new DeleteProductRequest()
{
    Id = 1
}, headers);

Console.WriteLine("Product deleted successfully.");
Console.WriteLine("\n------------------------------- Delete -------------------------------\n");

// Get All
Console.WriteLine("------------------------------- Get All Products -------------------------------\n");
list = await serviceClient.GetProductsAsync(new Empty(), headers);
foreach (var item in list.Products)
{
    Console.WriteLine(item);
}

Console.WriteLine("\n------------------------------- Get All Products -------------------------------\n");


Console.ReadLine();