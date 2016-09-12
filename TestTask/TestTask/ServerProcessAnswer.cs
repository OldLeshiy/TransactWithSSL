using System.Runtime.Serialization;


namespace TestTask
{
    [DataContract]
    class ServerProcessAnswer
    {
        [DataMember]
        public DataAnswer single_process_answer;

    }
    [DataContract]
    class DataAnswer
    {
        [DataMember]
        public string id;
        [DataMember]
        public string point_id;
        [DataMember]
        public string external_transaction_id;
        [DataMember]
        public string code;
        [DataMember]
        public string description;
        [DataMember]
        public string datetime;

    }
}
