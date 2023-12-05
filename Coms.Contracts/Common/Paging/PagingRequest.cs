namespace Coms.Contracts.Common.Paging
{
    public class PagingRequest
    {
        const int MAX_PAGE_SIZE = 50;
        private int _pageSize = 10;

        public int CurrentPage { get; set; } = 1;

        public int PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                _pageSize = (value > MAX_PAGE_SIZE) ? MAX_PAGE_SIZE : value;
            }
        }
    }
}
