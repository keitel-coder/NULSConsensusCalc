using System.Runtime.Serialization;

namespace NULSCalc.Model
{
    [DataContract]
    public class NulsResponse<T>
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        [DataMember]
        public bool success { get; set; }

        /// <summary>
        /// 状态码
        /// </summary>
        [DataMember]
        public string code { get; set; }

        /// <summary>
        /// 消息
        /// </summary>
        [DataMember]
        public string msg { get; set; }

        /// <summary>
        /// 数据
        /// </summary>
        [DataMember]
        public T data { get; set; }
    }
}
