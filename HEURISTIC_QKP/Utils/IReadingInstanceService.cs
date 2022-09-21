using HEURISTIC_QKP.Models;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HEURISTIC_QKP.Utils
{
    public interface IReadingInstanceService
    {
        Instance GetInstanceData(string directory);
    }
}
