using ErrorOr;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using static iTextSharp.text.pdf.parser.LocationTextExtractionStrategy;

namespace Coms.Application.Services.Coordinates
{
    public class CoordinateService : LocationTextExtractionStrategy, ICoordinateService
    {
        private readonly List<TextChunk> locationalResult = new List<TextChunk>();

        private readonly ITextChunkLocationStrategy tclStrat;

        public CoordinateService() : this(new TextChunkLocationStrategyDefaultImp())
        {
        }
        public CoordinateService(ITextChunkLocationStrategy strat)
        {
            tclStrat = strat;
        }

        public IList<CoordianteResult> GetCoordinates(int contractId, string searchText)
        {
            List<CoordianteResult> result = new List<CoordianteResult>();
            string filePath = System.IO.Path.Combine(Environment.CurrentDirectory, "Contracts", contractId + ".pdf");
            var reader = new PdfReader(filePath);
            int numberOfPages = reader.NumberOfPages;
            for (int pageIndex = numberOfPages; pageIndex >= 0; pageIndex--)
            {
                var parser = new PdfReaderContentParser(reader);
                var strategy = parser.ProcessContent(pageIndex, new CoordinateService());
                var res = strategy.GetLocations(pageIndex);
                result = res.Where(p => p.searchText.Contains(searchText)).OrderBy(p => p.Y).Reverse().ToList();
                if (result.Count > 0)
                {
                    reader.Close();
                    break;
                }
            }
            return result;
        }

        private bool StartsWithSpace(string str)
        {
            if (str.Length == 0) return false;
            return str[0] == ' ';
        }


        private bool EndsWithSpace(string str)
        {
            if (str.Length == 0) return false;
            return str[str.Length - 1] == ' ';
        }


        private List<TextChunk> filterTextChunks(List<TextChunk> textChunks, ITextChunkFilter filter)
        {
            if (filter == null)
            {
                return textChunks;
            }

            var filtered = new List<TextChunk>();

            foreach (var textChunk in textChunks)
            {
                if (filter.Accept(textChunk))
                {
                    filtered.Add(textChunk);
                }
            }

            return filtered;
        }

        public override void RenderText(TextRenderInfo renderInfo)
        {
            LineSegment segment = renderInfo.GetBaseline();
            if (renderInfo.GetRise() != 0)
            { // remove the rise from the baseline - we do this because the text from a super/subscript render operations should probably be considered as part of the baseline of the text the super/sub is relative to 
                Matrix riseOffsetTransform = new Matrix(0, -renderInfo.GetRise());
                segment = segment.TransformBy(riseOffsetTransform);
            }
            TextChunk tc = new TextChunk(renderInfo.GetText(), tclStrat.CreateLocation(renderInfo, segment));
            locationalResult.Add(tc);
        }


        public IList<CoordianteResult> GetLocations(int pageNumber)
        {
            var filteredTextChunks = filterTextChunks(locationalResult, null);
            filteredTextChunks.Sort();

            TextChunk lastChunk = null;

            var textLocations = new List<CoordianteResult>();

            foreach (var chunk in filteredTextChunks)
            {

                if (lastChunk == null)
                {
                    //initial
                    textLocations.Add(new CoordianteResult
                    {
                        searchText = chunk.Text,
                        X = (int)iTextSharp.text.Utilities.PointsToMillimeters(chunk.Location.StartLocation[0]),
                        Y = (int)iTextSharp.text.Utilities.PointsToMillimeters(chunk.Location.StartLocation[1]),
                        PageNumber = pageNumber
                    });
                }
                else
                {
                    if (chunk.SameLine(lastChunk))
                    {
                        var text = "";
                        // we only insert a blank space if the trailing character of the previous string wasn't a space, and the leading character of the current string isn't a space
                        if (IsChunkAtWordBoundary(chunk, lastChunk) && !StartsWithSpace(chunk.Text) && !EndsWithSpace(lastChunk.Text))
                            text += ' ';
                        text += chunk.Text;
                        textLocations[textLocations.Count - 1].searchText += text;
                    }
                    else
                    {
                        textLocations.Add(new CoordianteResult
                        {
                            searchText = chunk.Text,
                            X = (int)iTextSharp.text.Utilities.PointsToMillimeters(chunk.Location.StartLocation[0]),
                            Y = (int)iTextSharp.text.Utilities.PointsToMillimeters(chunk.Location.StartLocation[1]),
                            PageNumber = pageNumber

                        });
                    }
                }
                lastChunk = chunk;
            }
            return textLocations;
        }
    }
}