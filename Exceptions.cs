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

    public class WrongFormat: Exception
    {
        public WrongFormat() { }
        public WrongFormat(string message) : base(String.Format("Import file has wrong format: {0}", message)) { }
    }
}
