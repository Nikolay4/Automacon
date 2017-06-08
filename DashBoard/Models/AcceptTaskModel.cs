using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace DashBoard.Models
{
    //принять или не принять задание 
    [DataContract]
    public class AcceptTaskModel
    {
        [DataMember(Name = "Number")]
        public string Number { get; set; }

        [DataMember(Name = "Reason")]
        public string Reason { get; set; }
    }
}