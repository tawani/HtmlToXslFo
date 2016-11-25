namespace HtmlToXmlFo.Tests
{
    using System;
    using System.IO;

    public static class TestHelper
    {
        static TestHelper()
        {
            var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

            DirectoryWithFiles = Path.Combine(baseDirectory, "data");
        }

        public static string DirectoryWithFiles { get; private set; }
    }
}