namespace Store.Core.Entities.Order
{
    public class ProductItemOrder
    {
        public ProductItemOrder()
        {
            
        }
        public ProductItemOrder(int id, string name, string pictureUrl)
        {
            Id = id;
            Name = name;
            PictureUrl = pictureUrl;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string PictureUrl { get; set; }
    }
}