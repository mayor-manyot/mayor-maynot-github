using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaynotPersistence
{
    public interface Facility : Tile
    {}

    public class PoliceStation: Facility
    {
        /// <summary>
        /// biztonságot adó hatáskör
        /// </summary>
        public static Int32 safetyEffectRadius;

        /// <summary>
        /// épitési költésg a rendőrségeknek
        /// </summary>
        public static Int32 buildCost;

        /// <summary>
        /// fenntartási költség a rendőrségeknek
        /// </summary>
        public static Int32 maintenanceFee;
        public PoliceStation()
        {}
    }

    public class Stadium: Facility
    {
        /// <summary>
        /// boldogító hatáskör
        /// </summary>
        public static Int32 happinessEffectRadius;

        /// <summary>
        /// épitési költésg a stadionoknak
        /// </summary>
        public static Int32 buildCost;

        /// <summary>
        /// fenntartási költség a stadionoknak
        /// </summary>
        public static Int32 maintenanceFee;
        public Int32 Id { get; set; }
        public Stadium()
        {}
    }

    public class School : Facility
    {
        /// <summary>
        /// épitési költésg az iskoláknak
        /// </summary>
        public static Int32 buildCost;

        /// <summary>
        /// fenntartási költség az iskoláknak
        /// </summary>
        public static Int32 maintenanceFee;
        public Int32 Id { get; set; }
        public List<Person> People { get; set; }
        public School(Int32 id)
        { 
            Id = id;
            People = new List<Person>();

        }
    }

    public class University: Facility
    {
        /// <summary>
        /// épitési költésg az egyetemeknek
        /// </summary>
        public static Int32 buildCost;

        /// <summary>
        /// fenntartási költség az egyetemeknek
        /// </summary>
        public static Int32 maintenanceFee;
        public Int32 Id { get; set; }
        public List<Person> People { get; set; }
        public University(Int32 id)
        {
            Id = id;
            People = new List<Person>();
        }
    }
}
