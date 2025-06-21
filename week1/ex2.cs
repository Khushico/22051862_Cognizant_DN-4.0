using System;

namespace FactoryMethodPatternExample
{
    // Step 1: Document interface
    public interface IDocument
    {
        void Open();
    }

    // Step 2: Concrete document classes
    public class WordDocument : IDocument
    {
        public void Open()
        {
            Console.WriteLine("Opening Word document...");
        }
    }

    public class PdfDocument : IDocument
    {
        public void Open()
        {
            Console.WriteLine("Opening PDF document...");
        }
    }

    public class ExcelDocument : IDocument
    {
        public void Open()
        {
            Console.WriteLine("Opening Excel document...");
        }
    }

    // Step 3: Abstract factory
    public abstract class DocumentFactory
    {
        public abstract IDocument CreateDocument();
    }

    // Step 4: Concrete factories
    public class WordDocumentFactory : DocumentFactory
    {
        public override IDocument CreateDocument()
        {
            return new WordDocument();
        }
    }

    public class PdfDocumentFactory : DocumentFactory
    {
        public override IDocument CreateDocument()
        {
            return new PdfDocument();
        }
    }

    public class ExcelDocumentFactory : DocumentFactory
    {
        public override IDocument CreateDocument()
        {
            return new ExcelDocument();
        }
    }

    // Step 5: Test class
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Factory Method Pattern Demo");
            Console.WriteLine("----------------------------");

            DocumentFactory wordFactory = new WordDocumentFactory();
            IDocument wordDoc = wordFactory.CreateDocument();
            wordDoc.Open();

            DocumentFactory pdfFactory = new PdfDocumentFactory();
            IDocument pdfDoc = pdfFactory.CreateDocument();
            pdfDoc.Open();

            DocumentFactory excelFactory = new ExcelDocumentFactory();
            IDocument excelDoc = excelFactory.CreateDocument();
            excelDoc.Open();

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }
    }
}
