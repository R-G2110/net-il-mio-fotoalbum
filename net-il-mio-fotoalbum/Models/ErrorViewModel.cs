namespace net_il_mio_fotoalbum.Models
{
    public class ErrorViewModel
    {
        public string? RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
        public string Message { get; set; }

        public ErrorViewModel() { }

        public ErrorViewModel(string msg)
        {
            Message = msg;
        }
    }
}
