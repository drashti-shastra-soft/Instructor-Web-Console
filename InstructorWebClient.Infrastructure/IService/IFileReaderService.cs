using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstructorWebClient.Infrastructure.IService
{
    public interface IFileReaderService
    {
        string ReadFileContent(string filePath);
    }
}
