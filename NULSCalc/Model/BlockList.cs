using System.Runtime.Serialization;

namespace NULSCalc.Model
{
    [DataContract]
    public class BlockList
    {
        /// <summary>
        /// pages
        /// </summary>
        [DataMember]
        public int pages { get; set; }

        /// <summary>
        /// pages
        /// </summary>
        [DataMember]
        public int total { get; set; }

        [DataMember]
        public BlockListItem[] list { get; set; }
    }
}
