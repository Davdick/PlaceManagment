using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GestionLugaresFacilities.Models
{
    public class Validators
    {
        public bool IsEqual(DataJSON dato1, DataJSON dato2)
        {
            return dato1.x == dato2.x && dato1.y == dato2.y;
        }
        public List<DataJSON> AddCells(List<DataJSON> currentList, List<DataJSON> newObjects)
        {
            var list = currentList.Concat(newObjects).ToList();

            return list;
        }

        //Sobrecarga
        public List<DataJSON> AddCells(List<DataJSON> currentList, DataJSON newObject)
        {
            currentList.Add(newObject);
            return currentList;
        }
    }
}