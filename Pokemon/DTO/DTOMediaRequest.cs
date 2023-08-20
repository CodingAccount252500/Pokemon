using Pokemon.Models;

namespace Pokemon.DTO
{
    public class DTOMediaRequest
    {

        public string? MediaPath { get; set; }
        public string MediaType { get; set; }
        public bool? IsMainMedia { get; set; }
        public int? MediatypeId { get; set; }
        public int? ProductId { get; set; }



       /* public virtual MediaType? Mediatype { get; set; }
        public virtual Product? Product { get; set; }*/

    }
}
