using System;

namespace BioinfAlgorithms
{
    public struct AlignmentResult
    {
        public double Score { get; set; }
        public string S1 { get; set; }
        public string S2 { get; set; }

        public override string ToString()
        {
            return $"Score: {Score}{ Environment.NewLine}{S1}{Environment.NewLine}{S2}";
        }
    }
}
