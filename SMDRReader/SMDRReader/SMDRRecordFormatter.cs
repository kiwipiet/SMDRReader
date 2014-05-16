using System.Text;

namespace SMDRReader
{
    public class SMDRRecordFormatter
    {
        readonly SMDRRecord _record;
        public SMDRRecordFormatter(SMDRRecord record)
        {
            _record = record;
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(string.Format("CallStart:              {0}", _record.CallStart));
            sb.AppendLine(string.Format("ConnectedTime:          {0}", _record.ConnectedTime));
            sb.AppendLine(string.Format("RingTime:               {0}", _record.RingTime));
            sb.AppendLine(string.Format("Caller:                 {0}", _record.Caller));
            sb.AppendLine(string.Format("CallDirection:          {0}", _record.CallDirection));
            sb.AppendLine(string.Format("CalledNumber:           {0}", _record.CalledNumber));
            sb.AppendLine(string.Format("DialedNumber:           {0}", _record.DialedNumber));
            sb.AppendLine(string.Format("Account:                {0}", _record.Account));
            sb.AppendLine(string.Format("IsInternal:             {0}", _record.IsInternal));
            sb.AppendLine(string.Format("CallId:                 {0}", _record.CallId));
            sb.AppendLine(string.Format("Continuation:           {0}", _record.Continuation));
            sb.AppendLine(string.Format("Party1Device:           {0}", _record.Party1Device));
            sb.AppendLine(string.Format("Party1Name:             {0}", _record.Party1Name));
            sb.AppendLine(string.Format("Party2Device:           {0}", _record.Party2Device));
            sb.AppendLine(string.Format("Party2Name:             {0}", _record.Party2Name));
            sb.AppendLine(string.Format("ExternalTargeterId:     {0}", _record.ExternalTargeterId));
            sb.AppendLine(string.Format("HoldTime:               {0}", _record.HoldTime));
            sb.AppendLine(string.Format("ParkTime:               {0}", _record.ParkTime));
            sb.AppendLine(string.Format("AuthValid:              {0}", _record.AuthValid));
            sb.AppendLine(string.Format("AuthCode:               {0}", _record.AuthCode));
            sb.AppendLine(string.Format("UserCharged:            {0}", _record.UserCharged));
            sb.AppendLine(string.Format("CallCharge:             {0}", _record.CallCharge));
            sb.AppendLine(string.Format("Currency:               {0}", _record.Currency));
            sb.AppendLine(string.Format("AmountatLastUserChange: {0}", _record.AmountatLastUserChange));
            sb.AppendLine(string.Format("CallUnits:              {0}", _record.CallUnits));
            sb.AppendLine(string.Format("UnitsatLastUserChange:  {0}", _record.UnitsatLastUserChange));
            sb.AppendLine(string.Format("CostperUnit:            {0}", _record.CostperUnit));
            sb.AppendLine(string.Format("MarkUp:                 {0}", _record.MarkUp));
            sb.AppendLine(string.Format("ExternalTargetingCause: {0}", _record.ExternalTargetingCause));
            sb.AppendLine(string.Format("ExternalTargetedNumber: {0}", _record.ExternalTargetedNumber));
            return sb.ToString();
        }
    }
}
