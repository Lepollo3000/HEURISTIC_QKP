using HEURISTIC_QKP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Environment;

namespace HEURISTIC_QKP.Utils
{
    public class ReadingInstanceService : IReadingInstanceService
    {
        public Instance GetInstanceData(string fileName)
        {
            TryGetFile(fileName);

            return null;
        }

        private FileInfo[] TryGetFile(string fileName)
        {
            string folderPath = System.AppContext.BaseDirectory + "/FileInstances";
            DirectoryInfo directory = new DirectoryInfo(folderPath);
            FileInfo[] files = directory.GetFiles("jeu_100_25_1*", SearchOption.TopDirectoryOnly);

            foreach (var file in files)
            {
                // do something here
            }

            return null;
        }
    }
}
