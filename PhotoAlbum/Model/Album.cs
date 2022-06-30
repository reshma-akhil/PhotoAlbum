using System.Collections.Generic;
using System.Runtime.Serialization;

namespace PhotoAlbum.Model
{
    [DataContract]
    public class Album
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int UserId { get; set; }
        [DataMember]
        public string Title { get; set; }
        [DataMember]
        public List<Photo> Photos { get; set; }
        public Album()
        {
            Photos = new List<Photo>();
        }

    }
}
