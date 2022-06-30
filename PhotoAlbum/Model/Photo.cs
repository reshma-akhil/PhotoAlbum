using System.Runtime.Serialization;

namespace PhotoAlbum.Model
{
    [DataContract]
    public class Photo
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int AlbumId { get; set; }
        [DataMember]
        public string Title { get; set; }
        [DataMember]
        public string Url { get; set; }
        [DataMember]
        public string ThumbnailUrl { get; set; }
    }
}
