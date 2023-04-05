using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaynotPersistence
{
    public class Person
    {
        public int Satisfaction { get; set; }
        public int Age { get; set; }
        public Tile Residency { get; set; }
        public Tile WorkPlace { get; set; }
        public Level Education { get; set; }
        public Person(int satisfaction,
            int age,
            Tile residency,
            Tile workPlace,
            Level education)
        {
            Satisfaction = satisfaction;
            Age = age;
            Residency = residency;
            WorkPlace = workPlace;
            Education = education;

        }
    }

}
