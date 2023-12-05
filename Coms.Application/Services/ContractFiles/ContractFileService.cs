using Coms.Application.Common.Intefaces.Persistence;
using Coms.Application.Services.TemplateFiles;
using Coms.Domain.Entities;
using ErrorOr;
using Firebase.Storage;
using Syncfusion.DocIORenderer;
using Syncfusion.EJ2.DocumentEditor;
using Syncfusion.Pdf;

namespace Coms.Application.Services.ContractFiles
{
    public class ContractFileService :IContractFileService
    {
        private readonly IContractFileRepository _contractFileRepository;
        private readonly IContractRepository _contractRepository;
        private string Bucket = "coms-64e4a.appspot.com";

        public ContractFileService(IContractFileRepository contractFileRepository, IContractRepository contractRepository)
        {
            _contractFileRepository = contractFileRepository;
            _contractRepository = contractRepository;
        }
        public async Task<ErrorOr<ContractFileResult>> Add(string name, string extension, string contenType, byte[] document,
               int size, int contractId)
        {
            try
            {
                if (name == null)
                {
                    name = "Untitled";
                }
                var contractFile = new ContractFile()
                {
                    FileName = name,
                    Extension = extension,
                    ContentType = contenType,
                    FileData = document,
                    FileSize = size,
                    UploadedDate = DateTime.UtcNow,
                    ContractId = contractId
                };
                await _contractFileRepository.Add(contractFile);
                return new ContractFileResult() { Result = "OK" };
            }
            catch (Exception ex)
            {
                return Error.Failure("500", ex.Message);
            }
        }

        public async Task<ErrorOr<ContractFileResult>> ExportPDf(string content, int id)
        {
            try
            {
                // Converts the sfdt to stream
                Stream document = Syncfusion.EJ2.DocumentEditor.WordDocument.Save(content, FormatType.Docx);
                Syncfusion.DocIO.DLS.WordDocument doc = new Syncfusion.DocIO.DLS.WordDocument(document, Syncfusion.DocIO.FormatType.Docx);
                //Instantiation of DocIORenderer for Word to PDF conversion
                DocIORenderer render = new DocIORenderer();
                //Converts Word document into PDF document
                PdfDocument pdfDocument = render.ConvertToPDF(doc);
                string fileName = id + ".pdf";
                string filePath =
                      Path.Combine(Environment.CurrentDirectory, "Data",
                        fileName);
                // Saves the document to server machine file system, you can customize here to save into databases or file servers based on requirement.
                FileStream fileStream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);

                //Saves the PDF file
                pdfDocument.Save(fileStream);
                pdfDocument.Close();
                fileStream.Close();
                document.Close();
                var stream = File.Open(filePath, FileMode.Open);
                var task = new FirebaseStorage(Bucket)
                    .Child("files")
                    .Child(fileName)
                    .PutAsync(stream);
                //var contract = await _contractRepository.GetContract(id);
                string link = "https://firebasestorage.googleapis.com/v0/b/coms-64e4a.appspot.com/o/files%2F" + id
                    + ".pdf?alt=media&token=451cd9c9-b548-48f3-b69c-0129a0c0836c";
                //contract.Link = link;
                //await _contractRepository.UpdateContract(contract);
                return new ContractFileResult() { Result = "OK" };
            }
            catch (Exception ex)
            {
                return Error.Failure("500", ex.Message);
            }
        }

        private FormatType GetFormatType(string format)
        {
            if (string.IsNullOrEmpty(format))
                throw new NotSupportedException("This file format is not supported!");
            switch (format.ToLower())
            {
                case ".dotx":
                case ".docx":
                case ".docm":
                case ".dotm":
                    return FormatType.Docx;
                case ".dot":
                case ".doc":
                    return FormatType.Doc;
                case ".rtf":
                    return FormatType.Rtf;
                case ".txt":
                    return FormatType.Txt;
                case ".xml":
                    return FormatType.WordML;
                default:
                    throw new NotSupportedException("This file format is not supported!");
            }
        }
    }
}
