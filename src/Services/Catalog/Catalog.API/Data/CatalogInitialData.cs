using Marten.Schema;

namespace Catalog.API.Data
{
    public class CatalogInitialData : IInitialData
    {
        public async Task Populate(IDocumentStore store, CancellationToken cancellation)
        {
            using var session = store.LightweightSession();

            if (await session.Query<Product>().AnyAsync()) return;

            // Marten UPSERT will cater for existing records
            session.Store<Product>(GetPreconfiguredProducts());
            await session.SaveChangesAsync();
        }

        private static IEnumerable<Product> GetPreconfiguredProducts() => new List<Product>
        {
            new Product
            {
                Id = Guid.Parse("e2b6f5a0-1a9f-4b3a-9c5a-1a2b3c4d5e6f"),
                Name = "Wireless Headphones",
                Category = new List<string>{ "Electronics", "Audio" },
                Description = "Comfortable wireless headphones with active noise cancellation.",
                ImageFile = "wireless-headphones.jpg",
                Price = 199.99m
            },
            new Product
            {
                Id = Guid.Parse("a1f9c3d2-4b6e-47f8-9d2c-7b8a9c0d1e2f"),
                Name = "Gaming Mouse",
                Category = new List<string>{ "Electronics", "Gaming", "Accessories" },
                Description = "Ergonomic gaming mouse with programmable buttons and RGB lighting.",
                ImageFile = "gaming-mouse.jpg",
                Price = 59.50m
            },
            new Product
            {
                Id = Guid.Parse("b3d2e1f4-6c7a-48b9-8f3d-0a1b2c3d4e5f"),
                Name = "Mechanical Keyboard",
                Category = new List<string>{ "Electronics", "Computers", "Accessories" },
                Description = "Durable mechanical keyboard with tactile switches and backlight.",
                ImageFile = "mechanical-keyboard.jpg",
                Price = 129.00m
            },
            new Product
            {
                Id = Guid.Parse("c4e3f2a1-7d8b-49c0-9e4f-1b2c3d4e5f6a"),
                Name = "Smart Watch",
                Category = new List<string>{ "Electronics", "Wearables" },
                Description = "Water-resistant smart watch with heart-rate monitor and GPS.",
                ImageFile = "smart-watch.jpg",
                Price = 249.99m
            },
            new Product
            {
                Id = Guid.Parse("d5f4a3b2-8e9c-40d1-af5e-2c3d4e5f6a7b"),
                Name = "Coffee Maker",
                Category = new List<string>{ "Home", "Kitchen" },
                Description = "Automatic drip coffee maker with programmable timer.",
                ImageFile = "coffee-maker.jpg",
                Price = 89.75m
            },
            new Product
            {
                Id = Guid.Parse("f6a5b4c3-9f0d-41e2-bf6a-3d4e5f6a7b8c"),
                Name = "Running Shoes",
                Category = new List<string>{ "Sports", "Footwear" },
                Description = "Lightweight running shoes with breathable mesh upper.",
                ImageFile = "running-shoes.jpg",
                Price = 74.99m
            }
        };
    }
}
