using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DashBoard.Models
{
    [DataContract]
    public class MessageModel
    {
        [DataMember(Name = "Id")]
        public int Id { get; set; }

        [DataMember(Name = "ParentId")]
        public int ParentId { get; set; }

        [DataMember(Name = "Question")]
        public string Question { get; set; }

        [DataMember(Name = "Answer")]
        public string Answer { get; set; }

        [DataMember(Name = "Type")]
        public bool Type { get; set; }

        [DataMember(Name = "UserName")]
        public string UserName { get; set; }

        [DataMember(Name = "IsProcessed")]
        public bool IsProcessed { get; set; }

        [DataMember(Name = "IsRead")]
        public bool IsRead { get; set; }

        [DataMember(Name = "DateTime")]
        public string DateTime { get; set; }

        [DataMember(Name = "AnswerDateTime")]
        public string AnswerDateTime { get; set; }

        public string children { get; set; }
    }
}