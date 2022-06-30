namespace Task2_Kapserskiy
{
    [Serializable]
    class ScanRequest
    {
        public ScanRequest(string? argument)
        {
            Argument = argument;
        }
        public string? Argument { get; private set; }
    }
}
