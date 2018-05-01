using System.Runtime.Serialization;

namespace NULSCalc.Model
{
    [DataContract]
    public class BlockListItem
    {
        /// <summary>
        /// 当前hash
        /// </summary>
        [DataMember]
        public string hash { get; set; }

        /// <summary>
        /// 区块高度
        /// </summary>
        [DataMember]
        public int height { get; set; }

        /// <summary>
        /// 出块时间
        /// </summary>
        [DataMember]
        public long time { get; set; }
    }
}
