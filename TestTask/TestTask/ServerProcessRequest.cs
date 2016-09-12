using System;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;


namespace TestTask
{
    [DataContract]
    class ServerProcessRequest
    {
        [DataMember]
        public Data single_process;

        public void UpToJson(Stream out_stream)
        {
            DataContractJsonSerializer json = new DataContractJsonSerializer(typeof(ServerProcessRequest));
            json.WriteObject(out_stream, this);
            out_stream.Flush();
        }

    }
    [DataContract]
    class Data
    {
        [DataMember]
        public string point_id;
        [DataMember]
        public string datetime;
        [DataMember]
        public string external_transaction_id;
        [DataMember]
        public string service_id;
        [DataMember]
        public string service_key;
        [DataMember]
        public string amount;

        public Data(string key, string amount)
        {
            point_id = "1001470048738391192";
            datetime = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:sszzz");
            external_transaction_id = (RandomUlong(new Random())).ToString();
            service_id = "1001351861392575516";
            service_key = key;
            this.amount = amount;
        }

        private ulong RandomUlong(Random rnd)
        {
            var buffer = new byte[sizeof(ulong)];
            rnd.NextBytes(buffer);
            return BitConverter.ToUInt64(buffer, 0);
        }


    }
}