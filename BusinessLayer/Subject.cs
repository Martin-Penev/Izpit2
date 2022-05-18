using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    [SerializableAttribute]
    public class Subject
    {
        public string ID { get; private set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public List<Teacher> Teachers { get; set; }

        private Subject()
        {

        }
        public Subject(string name, string type)
        {
            ID = Guid.NewGuid().ToString();
            Name = name;
            Type = type;
            
        }
    }
}
