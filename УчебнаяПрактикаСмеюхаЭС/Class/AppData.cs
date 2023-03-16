using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using УчебнаяПрактикаСмеюхаЭС.DataBase;

namespace УчебнаяПрактикаСмеюхаЭС.Class
{
    public static class AppData
    {
        public static DBEntities1 db = new DBEntities1();
    }
}
