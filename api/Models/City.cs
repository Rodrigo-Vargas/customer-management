namespace api.Models
{
   public class City
   {
      public int Id { get; set; }
      public string Name { get; set; }
      public Region Region { get; set; }
   }
}