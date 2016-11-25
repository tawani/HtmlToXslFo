namespace HtmlToXmlFo.Tests
{
    using System.IO;

    public class BaseTest
    {
        private readonly string _directoryWithFiles;

        public BaseTest()
        {
            _directoryWithFiles = TestHelper.DirectoryWithFiles;
        }

        protected string ReadFile(string fileName)
        {
            return File.ReadAllText(Path.Combine(_directoryWithFiles, fileName));
        }
    }
}