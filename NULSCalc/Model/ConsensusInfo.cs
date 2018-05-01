using System.Runtime.Serialization;

namespace NULSCalc.Model
{
    [DataContract]
    public class ConsensusInfo
    {
        [DataMember]
        public int agentCount { get; set; }

        [DataMember]
        public long totalDeposit { get; set; }

        [DataMember]
        public long rewardOfDay { get; set; }

        [DataMember]
        public int consensusAccountNumber { get; set; }
    }
}
