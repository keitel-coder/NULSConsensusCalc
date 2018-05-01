using NULSCalc.Model;
using NULSCalc.Uitls;
using System;
using System.IO;
using System.Text;

namespace NULSCalc
{
    public class GetNulsNodeInfo
    {
        /// <summary>
        /// 获取共识信息
        /// </summary>
        /// <returns></returns>
        public static ConsensusInfo GetConsensusInfo()
        {
            try
            {
                string json = Uitls.HttpUitls.Get("http://data.nuls.io/nuls/consensus?t=" + DateTime.Now.GetTimeStamp());
                if (json == null) return null;
                System.Runtime.Serialization.Json.DataContractJsonSerializer serializer = new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(NulsResponse<ConsensusInfo>));
                using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(json)))
                {
                    var response = (NulsResponse<ConsensusInfo>)serializer.ReadObject(ms);
                    if (response.success) return response.data;
                    else return null;
                }
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 获取区块信息
        /// </summary>
        /// <returns></returns>
        public static BlockList GetBlockInfo()
        {
            try
            {
                string json = Uitls.HttpUitls.Get("http://data.nuls.io/nuls/block/list?pageNumber=1&pageSize=1&t=1525128002869&t=" + DateTime.Now.GetTimeStamp());
                if (json == null) return null;
                System.Runtime.Serialization.Json.DataContractJsonSerializer serializer = new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(NulsResponse<BlockList>));
                using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(json)))
                {
                    var response = (NulsResponse<BlockList>)serializer.ReadObject(ms);
                    if (response.success) return response.data;
                    else return null;
                }
            }
            catch
            {
                return null;
            }
        }
    }
}
