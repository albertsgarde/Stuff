using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stuff
{
    public class Data
    {
        private DataSet[] data;

        public Data(string fileName)
        {

            StreamReader file = new StreamReader(fileName);
            var dataList = new List<DataSet>();
            while (!file.EndOfStream)
                dataList.Add(new DataSet(file));
            file.Close();
            data = dataList.ToArray();
        }

        public DataSet this[int i]
        {
            get
            {
                return data[i];
            }
        }
    }
}
