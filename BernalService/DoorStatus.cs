namespace BernalService
{
    public class DoorStatus
    {
        public int status;
        public byte D0802;
        public byte[] D0902 = new byte[2];
        public byte[] D0903 = new byte[2];
        public byte[] D0904 = new byte[2];
        public byte P0501;
        public byte P0604;
        public byte P0401;
        public byte[] D0503 = new byte[4];
        public byte P0404;
        public byte[] D0504 = new byte[4];
    };
}