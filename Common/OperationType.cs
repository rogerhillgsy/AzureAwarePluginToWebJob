using System;

namespace Arup.Common
{
    public class OperationType
    {
        public const string GetCJNSuffices = "GetCJNSuffices";
        [Obsolete] public const string PopulatePJNSimple = "PopulatePJNJNSimple";

        public const string CommitJN = "CommitJN";
        public const string GetJNForCJNA = "GetJNForCJNA";
        public const string GetJNForOpp = "GetJNForOpp";
        public const string GetPJNForOpp = "GetPJNForOpp";
        public const string ReleaseJN = "ReleaseJN";
        public const string ReserveJN = "ReserveJN";
        public const string WriteCJNAToIPP = "WriteCJNAToIPP";
        public const string WriteOppToIPP = "WriteOppToIPP";
        public const string WriteCJNAToOvabaseForJN = "WriteCJNAToOvabaseForJN";
        public const string WriteOPPToOvabaseForJN = "WriteOPPToOvabaseForJN";
        public const string WriteOPPToOvabaseForPJN = "WriteOPPToOvabaseForPJN";
        public const string Ping = "Ping";
    }
}
