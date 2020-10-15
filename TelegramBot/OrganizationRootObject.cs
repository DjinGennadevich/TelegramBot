using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBot
{
    [DataContract]
    public class RootObject : IEnumerable
    {
        [DataMember(Order = 0)]
        public string type { get; set; }
        [DataMember(Order = 1)]
        public Properties properties { get; set; }
        [DataMember(Order = 2)]
        public List<Feature> features;

        public RootObject()
        {
            features = new List<Feature>();
        }

        public int Count
        {
            get
            {
                return features.Count;
            }
        }
        public IEnumerator GetEnumerator()
        {
            return features.GetEnumerator();
        }
        public object this[int index]
        {
            get
            {
                if (index < features.Count)
                    return features[index];
                else
                {
                    return null;
                }
            }
        }
        [DataContract]
        public class SearchResponse
        {
            [DataMember(Order = 0)]
            public int found { get; set; }
            [DataMember(Order = 1)]
            public string display { get; set; }
        }
        [DataContract]
        public class SearchRequest
        {
            [DataMember(Order = 0)]
            public string request { get; set; }
            [DataMember(Order = 1)]
            public int skip { get; set; }
            [DataMember(Order = 2)]
            public int results { get; set; }
            [DataMember(Order = 3)]
            public List<List<double>> boundedBy { get; set; }
        }
        [DataContract]
        public class ResponseMetaData
        {
            [DataMember(Order = 0)]
            public SearchResponse SearchResponse { get; set; }
            [DataMember(Order = 1)]
            public SearchRequest SearchRequest { get; set; }
        }
        [DataContract]

        public class Properties
        {
            [DataMember]
            public ResponseMetaData ResponseMetaData { get; set; }
        }

        [DataContract]
        public class Feature
        {
            [DataMember(Order = 0)]
            public string type { get; set; }
            [DataMember(Order = 1)]
            public Geometry geometry { get; set; }
            [DataMember(Order = 2)]
            public Properties properties { get; set; }

            [DataContract]
            public class Geometry
            {
                [DataMember(Order = 0)]
                public string type { get; set; }
                [DataMember(Order = 1)]
                public List<double> coordinates { get; set; }
            }
            [DataContract]
            public class Phone
            {
                [DataMember(Order = 0)]
                public string type { get; set; }
                [DataMember(Order = 1)]
                public string formatted { get; set; }
            }
            [DataContract]
            public class Category
            {
                [DataMember(Order = 0)]
                public string @class { get; set; }
                [DataMember(Order = 1)]
                public string name { get; set; }
            }
            [DataContract]
            public class Interval
            {
                [DataMember(Order = 0)]
                public string from { get; set; }
                [DataMember(Order = 1)]
                public string to { get; set; }
            }
            [DataContract]
            public class Availability
            {
                [DataMember(Order = 0)]
                public List<Interval> Intervals { get; set; }
                [DataMember(Order = 1)]
                public bool Monday { get; set; }
                [DataMember(Order = 2)]
                public bool Tuesday { get; set; }
                [DataMember(Order = 3)]
                public bool Wednesday { get; set; }
                [DataMember(Order = 4)]
                public bool Thursday { get; set; }
                [DataMember(Order = 5)]
                public bool? Friday { get; set; }
                [DataMember(Order = 6)]
                public bool? Saturday { get; set; }
                [DataMember(Order = 7)]
                public bool? Sunday { get; set; }
                [DataMember(Order = 8)]
                public bool? Everyday { get; set; }
                [DataMember(Order = 9)]
                public bool? TwentyFourHours { get; set; }
            }
            [DataContract]
            public class Hours
            {
                [DataMember(Order = 0)]
                public string text { get; set; }
                [DataMember(Order = 1)]
                public List<Availability> Availabilities { get; set; }
            }
            [DataContract]
            public class CompanyMetaData
            {
                [DataMember(Order = 0)]
                public string id { get; set; }
                [DataMember(Order = 1)]
                public string name { get; set; }
                [DataMember(Order = 2)]
                public string address { get; set; }
                [DataMember(Order = 3)]
                public string url { get; set; }
                [DataMember(Order = 4)]
                public List<Phone> Phones { get; set; }
                [DataMember(Order = 5)]
                public List<Category> Categories { get; set; }
                [DataMember(Order = 6)]
                public Hours Hours { get; set; }
            }
            [DataContract]
            public class Properties
            {
                [DataMember(Order = 3)]
                public CompanyMetaData CompanyMetaData { get; set; }
                [DataMember(Order = 2)]
                public List<List<double>> boundedBy { get; set; }
                [DataMember(Order = 1)]
                public string description { get; set; }
                [DataMember(Order = 0)]

                public string name { get; set; }
            }
            public override string ToString()
            {
                var temp = "";
                try
                {
                    temp = $"Title: {properties.name}\nAdress: {properties.description}\nSite: {properties.CompanyMetaData.url}\nWorking Hours: {properties.CompanyMetaData.Hours.text}\nPhone Numbers:\n";
                    foreach (Phone phone in properties.CompanyMetaData.Phones)
                    {
                        temp += $" {phone.formatted}\n";
                    }
                    temp += $"Categories:\n";
                    foreach (Category category in properties.CompanyMetaData.Categories)
                    {
                        temp += $" {category.name}\n";
                    }
                }
                catch
                {

                }
                return temp;
            }
        }
    }
}
