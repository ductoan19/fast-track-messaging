namespace my_app_backend.Models
{
    public class UpdateBookQuantityReq
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public int Direction { get; set; }
    }
}
