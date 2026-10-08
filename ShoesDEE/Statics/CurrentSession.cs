using ShoesDEE.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoesDEE.Statics
{
    public static class CurrentSession
    {
        public static User CurrentUser { get; set; }
    }
}
