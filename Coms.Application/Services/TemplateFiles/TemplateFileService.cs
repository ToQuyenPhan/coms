using Coms.Application.Common.Intefaces.Persistence;
using Coms.Domain.Entities;
using ErrorOr;
using Firebase.Storage;
using Microsoft.Office.Interop.Word;
using SkiaSharp;
using Syncfusion.DocIORenderer;
using Syncfusion.EJ2.DocumentEditor;
using Syncfusion.Pdf;
using System.Collections;
using System.IO;

namespace Coms.Application.Services.TemplateFiles
{
    public class TemplateFileService : ITemplateFileService
    {
        private readonly ITemplateFileRepository _templateFileRepository;
        private readonly ITemplateRepository _templateRepository;
        private readonly ITemplateFieldRepository _templateFieldRepository;
        private string Bucket = "coms-64e4a.appspot.com";
        private string[] ValidFieldNames = { "Contract Title", "Contract Code", "Created Date",
            "Contract Duration", "Execution Time", "Company Name", "Company Address",
            "Company Tax Code", "Company Email", "Company Code", "Signer Name", "Signer Position",
            "Partner Name", "Partner Address", "Partner Tax Code", "Partner Email", "Partner Code",
            "Partner Signer Name", "Partner Signer Position"};

        public TemplateFileService(ITemplateFileRepository templateFileRepository, 
            ITemplateRepository templateRepository, ITemplateFieldRepository templateFieldRepository)
        {
            _templateFileRepository = templateFileRepository;
            _templateRepository = templateRepository;
            _templateFieldRepository = templateFieldRepository;
        }

        public async Task<ErrorOr<TemplateFileResult>> Add(string name, string extension, string contenType, byte[] document,
                int size, int templateId)
        {
            try
            {
                string filePath = Path.Combine(Environment.CurrentDirectory, "Templates");
                bool folderExists = Directory.Exists(filePath);
                if (!folderExists)
                    Directory.CreateDirectory(filePath);
                if (name == null)
                {
                    name = "Untitled";
                }
                var templateFile = new TemplateFile()
                {
                    FileName = name,
                    Extension = extension,
                    ContentType = contenType,
                    FileData = document,
                    FileSize = size,
                    UploadedDate = DateTime.UtcNow,
                    TemplateId = templateId
                };
                await _templateFileRepository.Add(templateFile);
                MemoryStream stream = new MemoryStream();
                stream.Write(document, 0, (int)document.Length);
                filePath = Path.Combine(Environment.CurrentDirectory, "Templates", templateId + ".docx");
                File.WriteAllBytes(filePath, stream.ToArray());
                Spire.Doc.Document checkingDocument = new Spire.Doc.Document();
                checkingDocument.LoadFromFile(filePath);
                bool isValid = true;
                string[] mailMergeFieldNames = checkingDocument.MailMerge.GetMergeFieldNames();
                IList<string> fieldNames = new List<string>();
                foreach(var fieldName in mailMergeFieldNames)
                {
                    if (!fieldNames.Contains(fieldName))
                    {
                        fieldNames.Add(fieldName);
                    }
                }
                if(fieldNames.Count() > 0)
                {
                    var contractTitle = fieldNames.FirstOrDefault(fn => fn.Equals("Contract Title"));
                    if (contractTitle is null)
                    {
                        return Error.Conflict("409", "Contract Title not found");
                    }
                    var contractCode = fieldNames.FirstOrDefault(fn => fn.Equals("Contract Code"));
                    if(contractCode is null)
                    {
                        return Error.Conflict("409", "Contract Code not found");
                    }
                    List<TemplateField> templateFields = new List<TemplateField>();
                    foreach (var fieldName in fieldNames)
                    {
                        var templateField = new TemplateField
                        {
                            FieldName = fieldName,
                            TemplateId = templateId
                        };
                        if (!templateFields.Contains(templateField))
                        {
                            templateFields.Add(templateField);
                        }
                    }
                    if (templateFields.Count() > 0)
                    {
                        await _templateFieldRepository.AddRangeAsync(templateFields);
                    }
                }
                return new TemplateFileResult()
                {
                    Result = "OK"
                };
            }
            catch (Exception ex)
            {
                return Error.Failure("500", ex.Message);
            }
        }

        public async Task<ErrorOr<TemplateFileResult>> ExportPDf(string content, int id)
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
                string filePath = Path.Combine(Environment.CurrentDirectory, "Templates", id + ".pdf");
                // Saves the document to server machine file system, you can customize here to save into databases or file servers based on requirement.
                FileStream fileStream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read);

                //Saves the PDF file
                pdfDocument.Save(fileStream);
                pdfDocument.Close();
                fileStream.Close();
                document.Close();
                var stream = File.Open(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read);
                var task = new FirebaseStorage(Bucket)
                    .Child("templates")
                    .Child(fileName)
                    .PutAsync(stream);
                var template = await _templateRepository.GetTemplate(id);
                string link = "https://firebasestorage.googleapis.com/v0/b/coms-64e4a.appspot.com/o/templates%2F" + id
                    + ".pdf?alt=media&token=451cd9c9-b548-48f3-b69c-0129a0c0836c";
                template.TemplateLink = link;
                await _templateRepository.UpdateTemplate(template);
                return new TemplateFileResult() { Result = "OK" };
            }
            catch (Exception ex)
            {
                return Error.Failure("500", ex.Message);
            }
        }

        public async Task<ErrorOr<TemplateFileResult>> Update(int templateId, string templateName,
                byte[] document)
        {
            try
            {
                var templateFile = await _templateFileRepository.GetTemplateFileByTemplateId(templateId);
                if (templateFile is not null)
                {
                    string filePath = Path.Combine(Environment.CurrentDirectory, "Templates", templateId + ".docx");
                    File.WriteAllBytes(filePath, document);
                    Spire.Doc.Document checkingDocument = new Spire.Doc.Document();
                    checkingDocument.LoadFromFile(filePath);
                    string[] mailMergeFieldNames = checkingDocument.MailMerge.GetMergeFieldNames();
                    IList<string> fieldNames = new List<string>();
                    foreach (var fieldName in mailMergeFieldNames)
                    {
                        if (!fieldNames.Contains(fieldName))
                        {
                            fieldNames.Add(fieldName);
                        }
                    }
                    if (fieldNames.Count() > 0)
                    {
                        var templateFields = await _templateFieldRepository.GetTemplateFieldsByTemplateId(templateId);
                        if(templateFields is not null) {
                            await _templateFieldRepository.DeleteRangeAsync(new List<TemplateField>(templateFields));
                            List<TemplateField> newFields = new List<TemplateField>();
                            foreach (var fieldName in fieldNames)
                            {
                                var newField = new TemplateField
                                {
                                    FieldName = fieldName,
                                    TemplateId = templateId
                                };
                                if (!templateFields.Contains(newField))
                                {
                                    newFields.Add(newField);
                                }
                            }
                            if (templateFields.Count() > 0)
                            {
                                await _templateFieldRepository.AddRangeAsync(newFields);
                            }
                        }
                    }
                    templateFile.FileName = templateName;
                    templateFile.FileData = document;
                    templateFile.UpdatedDate = DateTime.Now;
                    await _templateFileRepository.Update(templateFile);
                    return new TemplateFileResult() { Result = "OK" };
                }
                else
                {
                    return Error.NotFound("404", "Template is not exist");
                }

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
