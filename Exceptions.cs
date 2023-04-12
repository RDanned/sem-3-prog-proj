using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemestralProject
{
    public class FileAlreadyExists: Exception
    {
        public FileAlreadyExists() { }
        public FileAlreadyExists(string path): base(String.Format("File already exists: {0}", path)) { }
    }
}
