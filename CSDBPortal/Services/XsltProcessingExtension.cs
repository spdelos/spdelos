namespace CSDBPortal.Services
{
    /// <summary>
    /// XSLT extension object registered under the namespace "com.delos.model.Processing".
    /// The print_all_pages.xslt stylesheet calls these methods during transformation to track
    /// figure numbers, hot-spot references, and the current date.
    /// </summary>
    public class XsltProcessingExtension
    {
        private float _fileNum;
        private float _randomList;
        private readonly Dictionary<float, float>  _figs = new();
        private readonly Dictionary<float, string> _hots = new();

        public float getFileNum()  => _fileNum;
        public void  setFileNum(float n)  { _fileNum = n; }

        public float getRandomList()  => _randomList;
        public void  setRandomList(float n) { _randomList = n; }

        public float getFigs(float n)
        {
            return _figs.TryGetValue(n - 1, out var v) ? v : 0f;
        }

        public void setFigs(float n, float a)
        {
            _figs.TryAdd(n - 1, a);
        }

        public string getHots(float n)
        {
            return _hots.TryGetValue(n - 1, out var v) ? v : string.Empty;
        }

        public void setHots(float n, string a)
        {
            _hots.TryAdd(n - 1, a);
        }

        public string currentDate() => DateTime.Now.ToString("d");
    }
}
